using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSAddressableAssets class is a singleton responsible for handling scene transitions within a system.
    /// It implements the STSTransitionInterface and STSIntermissionInterface interfaces.
    /// </summary>
    public partial class STSAddressableAssets : STSSingletonUnity<STSAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Stores the history of scene transitions as a list of STSScenesPackage instances.
        /// </summary>
        private List<STSScenesPackage> Historic = new List<STSScenesPackage>();

        /// <summary>
        /// The DefaultScenesPackage holds information about the default scenes package used within the Scene Transition System.
        /// It encapsulates details such as the active scene name, list of scene names, intermission scene, and transition data.
        /// </summary>
        /// <remarks>
        /// This is a private variable in the STSAddressableAssets class, used to manage default scenes behavior when performing transitions.
        /// </remarks>
        private STSScenesPackage DefaultScenesPackage;

        /// <summary>
        /// Clears the historic stack of recorded scene transitions.
        /// This operation resets the navigation history, effectively removing all previous states.
        /// </summary>
        public static void ResetHistoric()
        {
            Singleton().INTERNAL_Reset();
        }

        /// <summary>
        /// Navigates back in the scene history by the default step of 1.
        /// </summary>
        public static void GoBack()
        {
            GoBack(1, null);
        }

        /// <summary>
        /// Navigates back to a previous scene or state.
        /// </summary>
        /// <param name="sNewData">New transition data to apply when going back, can be null.</param>
        public static void GoBack(STSTransitionData sNewData)
        {
            GoBack(1, sNewData);
        }

        /// <summary>
        /// Navigates back a specified number of steps in the scene history.
        /// </summary>
        /// <param name="sUnstack">The number of steps to go back in the scene history.</param>
        public static void GoBack(int sUnstack)
        {
            GoBack(sUnstack, null);
        }

        /// <summary>
        /// Navigates back through the scene history by a specified number of steps, optionally using new transition data.
        /// </summary>
        /// <param name="sUnstack">The number of steps to go back in the scene history.</param>
        /// <param name="sNewData">Optional transition data for the navigation.</param>
        public static void GoBack(int sUnstack, STSTransitionData sNewData)
        {
            for (int ti = 0; ti < sUnstack; ti++)
            {
                if (Singleton().Historic.Count > 0)
                {
                    Singleton().Historic.RemoveAt(Singleton().Historic.Count - 1);
                }
            }

            GoTo(Singleton().Historic.Count - 1, sNewData);
        }

        /// <summary>
        /// Navigates to a specific historic index in the navigation history.
        /// </summary>
        /// <param name="sHistoricIndex">The index in the history stack to navigate to.</param>
        public static void GoTo(int sHistoricIndex)
        {
            GoTo(sHistoricIndex, null);
        }

        /// <summary>
        /// Transitions to the specified scene in the historic list.
        /// </summary>
        /// <param name="sHistoricIndex">The index of the scene in the historic list to transition to.</param>
        /// <param name="sNewData">Optional. The transition data for the new scene.</param>
        public static void GoTo(int sHistoricIndex, STSTransitionData sNewData)
        {
            if (sHistoricIndex >= 0 && sHistoricIndex < Singleton().Historic.Count)
            {
                Singleton().INTERNAL_Go(Singleton().Historic[sHistoricIndex], sNewData);
            }
            else
            {
                Debug.LogWarning("No scene in historic");
                Singleton().INTERNAL_Go(null, null);
            }
        }

        /// <summary>
        /// Resets the historic list of scenes to an empty state.
        /// </summary>
        private void INTERNAL_Reset()
        {
            Historic.Clear();
        }

        /// <summary>
        /// Adds a new navigation entry to the historic list with the provided scene data.
        /// </summary>
        /// <param name="sActiveSceneName">The name of the currently active scene.</param>
        /// <param name="sScenesNameList">A list of scene names to navigate through.</param>
        /// <param name="sIntermissionScene">The intermission scene name, if any.</param>
        /// <param name="sDatas">The transition data associated with the scene transition.</param>
        private void INTERNAL_AddNavigation(string sActiveSceneName, List<string> sScenesNameList, string sIntermissionScene, STSTransitionData sDatas)
        {
            INTERNAL_GetDefaultScenesPackage(); // create default
            STSScenesPackage tScenePackage = new STSScenesPackage(sActiveSceneName, sScenesNameList, sIntermissionScene, sDatas);
            Historic.Add(tScenePackage);
        }

        /// <summary>
        /// Retrieves the default scenes package. If the default scenes package is not already initialized,
        /// it creates a new instance with the original scene's short name and initializes optional fields to null.
        /// </summary>
        /// <returns>The default <see cref="STSScenesPackage"/> instance.</returns>
        private STSScenesPackage INTERNAL_GetDefaultScenesPackage()
        {
            if (DefaultScenesPackage == null)
            {
                if (OriginalScene == null)
                {
                    OriginalScene = new STSScene();
                    Scene tScene = SceneManager.GetActiveScene();
                    if (tScene.path != null)
                    {
                        OriginalScene.ScenePath = tScene.path;
                    }
                }

                DefaultScenesPackage = new STSScenesPackage(OriginalScene.GetSceneShortName(), null, null, null);
            }

            return DefaultScenesPackage;
        }

        /// <summary>
        /// Manages the transition to a new scene package, ensuring the correct scenes are loaded and unloaded.
        /// </summary>
        /// <param name="sPackage">The scene package to transition to.</param>
        /// <param name="sNewData">Optional transition data to be used during the scene transition.</param>
        private void INTERNAL_Go(STSScenesPackage sPackage, STSTransitionData sNewData)
        {
            if (sPackage == null)
            {
                sPackage = INTERNAL_GetDefaultScenesPackage();
            }

            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Add(tScene.name);
            }

            if (sNewData == null)
            {
                sNewData = sPackage.Datas;
            }

            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name,
                sPackage.ActiveSceneName,
                sPackage.ScenesNameList,
                tScenesToRemove,
                sPackage.IntermissionScene,
                sNewData,
                false);
        }
    }
}