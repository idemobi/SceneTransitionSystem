using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;
namespace SceneTransitionSystem
{
    /// <summary>
    /// STSAddressableAssets is a singleton class that provides functionalities to handle
    /// scene transitions and intermission interfaces within Unity. It extends the generic
    /// STSSingletonUnity class and implements the STSTransitionInterface and STSIntermissionInterface
    /// interfaces.
    /// </summary>
    public partial class STSAddressableAssets : STSSingletonUnity<STSAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Retrieves the currently active scene.
        /// </summary>
        /// <returns>The currently active scene.</returns>
        public static Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }

        /// <summary>
        /// Gets the scene at the specified index.
        /// </summary>
        /// <param name="sIndex">The index of the scene to retrieve.</param>
        /// <returns>The scene at the specified index.</returns>
        public static Scene GetSceneAt(int sIndex)
        {
            return SceneManager.GetSceneAt(sIndex);
        }

        /// <summary>
        /// Retrieves a scene by its name.
        /// </summary>
        /// <param name="sName">The name of the scene to retrieve.</param>
        /// <returns>The scene with the specified name.</returns>
        public static Scene GetSceneByName(string sName)
        {
            return SceneManager.GetSceneByName(sName);
        }

        /// <summary>
        /// Retrieves a scene by its path.
        /// </summary>
        /// <param name="sPath">The path of the scene to retrieve.</param>
        /// <returns>The scene located at the specified path.</returns>
        public static Scene GetSceneByPath(string sPath)
        {
            return SceneManager.GetSceneByPath(sPath);
        }

        /// <summary>
        /// Loads a new scene asynchronously with optional parameters for intermission and transition data.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to load.</param>
        /// <param name="sLoadSceneMode">The mode in which to load the scene. Default is LoadSceneMode.Single.</param>
        /// <param name="sSceneIntermission">The name of a scene to be used as an intermission during the transition.</param>
        /// <param name="sDatas">Optional data for the scene transition.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic scene loading is allowed.</param>
        public static void LoadScene(string sSceneName, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null, bool sAllowCyclic = false)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
        }
        /*public static AsyncOperation LoadSceneAsync(string sSceneName, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }
        public static AsyncOperation LoadSceneAsync(string sSceneName, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            INTERNAL_LoadScene(sSceneName, tLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }
        public static AsyncOperation LoadSceneAsync(string sSceneName, bool sAllowCyclic, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }
        public static AsyncOperation LoadSceneAsync(string sSceneName, bool sAllowCyclic, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            INTERNAL_LoadScene(sSceneName, tLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }
        public static AsyncOperation UnloadSceneAsync(string sSceneName, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
            return null;
        }
        public static AsyncOperation UnloadSceneAsync(Scene sScene, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = sScene.name;
            RemoveScene(SceneManager.GetActiveScene().name, tSceneName, sSceneIntermission, sDatas);
            return null;
        }
        public static AsyncOperation UnloadSceneAsync(string sSceneName, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
            return null;
        }
        public static AsyncOperation UnloadSceneAsync(Scene sScene, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = sScene.name;
            return null;
        }*/
        /// <summary>
        /// Sets the specified scene as the active scene.
        /// </summary>
        /// <param name="sScene">The scene to be set as active.</param>
        public static void SetActiveScene(Scene sScene)
        {
            SceneManager.SetActiveScene(sScene);
        }

        /// <summary>
        /// Loads a scene with the specified parameters, managing the transition and intermission effects.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to load.</param>
        /// <param name="sLoadSceneMode">The mode in which the scene should be loaded (Single or Additive).</param>
        /// <param name="sSceneIntermission">An optional intermission scene to display during the transition.</param>
        /// <param name="sDatas">Optional data to be used during the scene transition.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic scene loading is allowed.</param>
        private static void INTERNAL_LoadScene(string sSceneName, LoadSceneMode sLoadSceneMode, string sSceneIntermission = null, STSTransitionData sDatas = null, bool sAllowCyclic = false)
        {
            switch (sLoadSceneMode)
            {
                case LoadSceneMode.Single:
                    {
                        ReplaceAllByScene(sSceneName, sSceneIntermission, sDatas, sAllowCyclic);
                    }
                    break;
                case LoadSceneMode.Additive:
                    {
                        AddScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas, sAllowCyclic);
                    }
                    break;
            }
        }

        /// <summary>
        /// Unloads a specified scene from the active scenes in the game.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to unload.</param>
        /// <param name="sSceneIntermission">Optional intermission scene to load during transition.</param>
        /// <param name="sDatas">Optional data used for the scene transition.</param>
        private static void INTERNAL_UnloadScene(string sSceneName, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, null, sSceneIntermission, sDatas);
        }

        /// <summary>
        /// Retrieves a list of all currently loaded scenes.
        /// </summary>
        /// <returns>A list of all scenes that are currently loaded.</returns>
        public List<Scene> GetAllLoadedScenes()
        {
            List<Scene> tSceneList = new List<Scene>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                if (tScene.isLoaded == true)
                {
                    tSceneList.Add(tScene);
                }
            }
            return tSceneList;
        }
    }
}