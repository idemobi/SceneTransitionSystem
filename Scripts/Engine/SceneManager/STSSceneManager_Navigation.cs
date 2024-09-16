using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Manages scene transitions within the Scene Transition System framework.
    /// </summary>
    public partial class STSSceneManager : STSSingletonUnity<STSSceneManager>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Maintains a historical collection of scene transition packages.
        /// This list keeps track of the scenes and intermissions that have been navigated.
        /// </summary>
        private List<STSScenesPackage> Historic = new List<STSScenesPackage>();

        /// <summary>
        /// Holds the default package of scenes which include active scene name, a list of scenes, intermission scene,
        /// and related transition data. This package is initialized with the original scene as the active scene if
        /// not already defined.
        /// </summary>
        private STSScenesPackage DefaultScenesPackage;

        /// <summary>
        /// Resets the historic scene transition data.
        /// </summary>
        /// <remarks>
        /// This method clears all previously stored scene transition histories, effectively resetting
        /// the state of the scene transitions managed by the STSSceneManager.
        /// </remarks>
        public static void ResetHistoric()
        {
            Singleton().INTERNAL_Reset();
        }

        /// <summary>
        /// Navigates back in the scene history by one step.
        /// </summary>
        public static void GoBack()
        {
            GoBack(1, null);
        }

        /// <summary>
        /// Navigates back to the previous scene in the transition history.
        /// </summary>
        /// <param name="sNewData">Transition data to be used for the navigation.</param>
        public static void GoBack(STSTransitionData sNewData)
        {
            GoBack(1, sNewData);
        }

        /// Navigates back to the previous scene.
        /// <param name="sUnstack">The number of scenes to go back.</param>
        public static void GoBack(int sUnstack)
        {
            GoBack(sUnstack, null);
        }

        /// <summary>
        /// Navigates back in the scene history by the specified number of steps and optionally applies new transition data.
        /// </summary>
        /// <param name="sUnstack">The number of scenes to move back in the history stack.</param>
        /// <param name="sNewData">Optional parameter to specify new transition data to apply after going back.</param>
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
        /// Navigates to a specific point in the scene history.
        /// </summary>
        /// <param name="sHistoricIndex">The index in the history to navigate to.</param>
        public static void GoTo(int sHistoricIndex)
        {
            GoTo(sHistoricIndex, null);
        }

        /// <summary>
        /// Transitions to a scene specified by its historic index with optional transition data.
        /// </summary>
        /// <param name="sHistoricIndex">The index of the scene in the historic list to transition to.</param>
        /// <param name="sNewData">Optional transition data to apply during the scene transition.</param>
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
        /// Resets the historical record of scene transitions by clearing the internal list that tracks these transitions.
        /// </summary>
        private void INTERNAL_Reset()
        {
            Historic.Clear();
        }

        /// <summary>
        /// Adds a new navigation entry to the historic list of scene transitions.
        /// </summary>
        /// <param name="sActiveSceneName">The name of the currently active scene.</param>
        /// <param name="sScenesNameList">A list of scene names to be included in the navigation.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene, if any.</param>
        /// <param name="sDatas">Additional transition data associated with the navigation.</param>
        private void INTERNAL_AddNavigation(string sActiveSceneName, List<string> sScenesNameList, string sIntermissionScene, STSTransitionData sDatas)
        {
            INTERNAL_GetDefaultScenesPackage(); // create default
            STSScenesPackage tScenePackage = new STSScenesPackage(sActiveSceneName, sScenesNameList, sIntermissionScene, sDatas);
            Historic.Add(tScenePackage);
        }

        /// <summary>
        /// Returns the default scenes package containing the initial or original scene settings.
        /// If no default scenes package is set, it initializes one using the current active scene.
        /// </summary>
        /// <returns>An instance of the STSScenesPackage representing the default scenes package.</returns>
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
        /// Changes the currently active scene to the specified scene package, using the provided transition data. If the
        /// specified scene package is null, the default scene package is used. If the transition data is null, the data from
        /// the scene package is used.
        /// </summary>
        /// <param name="sPackage">The scene package to change to. If null, the default scene package is used.</param>
        /// <param name="sNewData">The transition data to use for the scene change. If null, the data from the scene package is used.</param>
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