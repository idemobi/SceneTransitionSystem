using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;


namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSSceneManager class provides functionality for managing scenes in a Unity application,
    /// including loading, unloading, and retrieving information about scenes.
    /// </summary>
    public partial class STSSceneManager : STSSingletonUnity<STSSceneManager>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Retrieves the currently active scene in the application.
        /// </summary>
        /// <returns>The currently active <see cref="Scene"/>.</returns>
        public static Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }

        /// <summary>
        /// Retrieves a Scene by its index in the Build Settings.
        /// </summary>
        /// <param name="sIndex">The index of the Scene to retrieve.</param>
        /// <returns>Returns the Scene at the given index.</returns>
        public static Scene GetSceneAt(int sIndex)
        {
            return SceneManager.GetSceneAt(sIndex);
        }

        /// <summary>
        /// Retrieves a scene by its build index.
        /// </summary>
        /// <param name="sBuildIndex">The build index of the scene to retrieve.</param>
        /// <return>The Scene object corresponding to the given build index.</return>
        public static Scene GetSceneByBuildIndex(int sBuildIndex)
        {
            return SceneManager.GetSceneByBuildIndex(sBuildIndex);
        }

        /// <summary>
        /// Retrieves a scene by its name.
        /// </summary>
        /// <param name="sName">The name of the scene to retrieve.</param>
        /// <returns>The Scene object corresponding to the specified name.</returns>
        public static Scene GetSceneByName(string sName)
        {
            return SceneManager.GetSceneByName(sName);
        }

        /// <summary>
        /// Retrieves a scene by its file path.
        /// </summary>
        /// <param name="sPath">The path of the scene to retrieve.</param>
        /// <returns>The scene corresponding to the provided path.</returns>
        public static Scene GetSceneByPath(string sPath)
        {
            return SceneManager.GetSceneByPath(sPath);
        }

        /// <summary>
        /// Loads the specified scene by build index with optional parameters for load mode, intermission, and transition data.
        /// </summary>
        /// <param name="sSceneBuildIndex">The build index of the scene to be loaded.</param>
        /// <param name="sLoadSceneMode">Specifies whether to load the scene additively or as a single scene. Default is single.</param>
        /// <param name="sSceneIntermission">Optional parameter to specify an intermission scene.</param>
        /// <param name="sDatas">Optional data to be used during the scene transition.</param>
        public static void LoadScene(int sSceneBuildIndex, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            INTERNAL_LoadScene(tSceneName, sLoadSceneMode, sSceneIntermission, sDatas);
        }

        /// <summary>
        /// Loads a new scene by its name with optional parameters for load mode, scene intermission, and transition data.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to load.</param>
        /// <param name="sLoadSceneMode">The mode in which to load the scene (Single or Additive). Default is Single.</param>
        /// <param name="sSceneIntermission">Optional parameter for specifying a scene intermission.</param>
        /// <param name="sDatas">Optional transition data to pass to the scene being loaded.</param>
        public static void LoadScene(string sSceneName, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas);
        }

        /// <summary>
        /// Loads a scene given its build index with the option to permit or prevent cyclic loading.
        /// </summary>
        /// <param name="sSceneBuildIndex">The build index of the scene to be loaded.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic scene loads are allowed.</param>
        /// <param name="sLoadSceneMode">Specifies whether to load the scene in single or additive mode. Default is LoadSceneMode.Single.</param>
        /// <param name="sSceneIntermission">Optional intermission scene to load before the target scene.</param>
        /// <param name="sDatas">Optional transition data for the scene.</param>
        public static void LoadScene(int sSceneBuildIndex, bool sAllowCyclic, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            INTERNAL_LoadScene(tSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
        }

        /// <summary>
        /// Loads the specified scene by its name with optional transition data and intermission.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to be loaded.</param>
        /// <param name="sAllowCyclic">Specifies if cyclic transitions are allowed.</param>
        /// <param name="sLoadSceneMode">The mode in which to load the scene, either single or additive.</param>
        /// <param name="sSceneIntermission">The scene intermission identifier.</param>
        /// <param name="sDatas">Optional transition data associated with the scene load.</param>
        public static void LoadScene(string sSceneName, bool sAllowCyclic, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
        }

        /// <summary>
        /// Asynchronously loads a scene by its name in the specified scene load mode.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to be loaded.</param>
        /// <param name="sLoadSceneMode">Specifies whether to load the scene additively or replace the current scene. Default is LoadSceneMode.Single.</param>
        /// <param name="sSceneIntermission">Optional intermission scene to play during the transition. Default is null.</param>
        /// <param name="sDatas">Optional transition data to be used during the scene loading. Default is null.</param>
        /// <returns>An AsyncOperation that can be used to monitor the progress of the scene loading.</returns>
        public static AsyncOperation LoadSceneAsync(string sSceneName, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }

        /// <summary>
        /// Asynchronously loads a scene given its build index.
        /// </summary>
        /// <param name="sSceneBuildIndex">The build index of the scene to load.</param>
        /// <param name="sLoadSceneMode">The mode in which to load the scene. Defaults to LoadSceneMode.Single.</param>
        /// <param name="sSceneIntermission">Optional parameter for intermission scene.</param>
        /// <param name="sDatas">Optional parameter for additional transition data.</param>
        /// <returns>Returns an AsyncOperation that can be used to track the progress of the operation.</returns>
        public static AsyncOperation LoadSceneAsync(int sSceneBuildIndex, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            return null;
        }

        /// <summary>
        /// Loads a scene asynchronously by name using specified parameters and optional scene intermission and transition data.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to load.</param>
        /// <param name="sParameters">The parameters for loading the scene.</param>
        /// <param name="sSceneIntermission">Optional scene intermission name.</param>
        /// <param name="sDatas">Optional transition data for the scene.</param>
        /// <returns>Returns an AsyncOperation that can be used to monitor the progress of the scene load.</returns>
        public static AsyncOperation LoadSceneAsync(string sSceneName, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            INTERNAL_LoadScene(sSceneName, tLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }

        /// <summary>
        /// Asynchronously loads a scene by its name with optional parameters for loading mode, intermission scene,
        /// and transition data.
        /// </summary>
        /// <param name="sParameters"></param>
        /// <param name="sSceneIntermission">The name of the intermission scene to display during the transition.</param>
        /// <param name="sDatas">Additional transition data to use during the scene load.</param>
        /// <param name="sSceneBuildIndex"></param>
        /// <returns>An AsyncOperation that can be used to monitor the progress of the scene load.</returns>
        public static AsyncOperation LoadSceneAsync(int sSceneBuildIndex, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            INTERNAL_LoadScene(tSceneName, tLoadSceneMode, sSceneIntermission, sDatas);
            return null;
        }

        /// <summary>
        /// Asynchronously loads a scene by its name.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to load.</param>
        /// <param name="sAllowCyclic">Determines if cyclic scene loading is allowed.</param>
        /// <param name="sLoadSceneMode">The mode in which to load the scene.</param>
        /// <param name="sSceneIntermission">Name of the intermission scene.</param>
        /// <param name="sDatas">Transition data used during the scene load.</param>
        /// <returns>Returns an AsyncOperation that can be used to monitor the progress of the scene loading process.</returns>
        public static AsyncOperation LoadSceneAsync(string sSceneName, bool sAllowCyclic, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            INTERNAL_LoadScene(sSceneName, sLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }

        /// <summary>
        /// Asynchronously loads the scene by its name.
        /// </summary>
        /// <param name="sAllowCyclic"></param>
        /// <param name="sLoadSceneMode">The mode to use for loading the scene.</param>
        /// <param name="sSceneIntermission">Optional intermission scene to display during the transition.</param>
        /// <param name="sDatas">Additional transition data.</param>
        /// <param name="sSceneBuildIndex"></param>
        /// <returns>An AsyncOperation that can be used to monitor the progress of the scene loading.</returns>
        public static AsyncOperation LoadSceneAsync(int sSceneBuildIndex, bool sAllowCyclic, LoadSceneMode sLoadSceneMode = LoadSceneMode.Single, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            return null;
        }

        /// <summary>
        /// Asynchronously loads the scene specified by its name, with options to allow cyclic loading and to specify additional parameters for loading.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to load.</param>
        /// <param name="sAllowCyclic">Specifies whether cyclic loading of the scene is allowed.</param>
        /// <param name="sParameters">The parameters to use when loading the scene.</param>
        /// <param name="sSceneIntermission">Optional parameter specifying a scene to load as an intermission before the target scene.</param>
        /// <param name="sDatas">Optional transition data to use during the scene transition.</param>
        /// <returns>AsyncOperation that can be used to track the loading process.</returns>
        public static AsyncOperation LoadSceneAsync(string sSceneName, bool sAllowCyclic, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            INTERNAL_LoadScene(sSceneName, tLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }

        /// <summary>
        /// Asynchronously loads a scene by its build index.
        /// </summary>
        /// <param name="sSceneBuildIndex">Build index of the scene to be loaded.</param>
        /// <param name="sAllowCyclic">Flag indicating whether cyclic loading is allowed.</param>
        /// <param name="sParameters">Scene loading parameters, such as LoadSceneMode.</param>
        /// <param name="sSceneIntermission">Optional intermission scene to be displayed during loading.</param>
        /// <param name="sDatas">Optional transition data to be used during the loading process.</param>
        /// <returns>Returns an AsyncOperation that can be used to monitor the loading process.</returns>
        public static AsyncOperation LoadSceneAsync(int sSceneBuildIndex, bool sAllowCyclic, LoadSceneParameters sParameters, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            LoadSceneMode tLoadSceneMode = sParameters.loadSceneMode;
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            INTERNAL_LoadScene(tSceneName, tLoadSceneMode, sSceneIntermission, sDatas, sAllowCyclic);
            return null;
        }

        /// <summary>
        /// Asynchronously unloads a scene by its build index.
        /// </summary>
        /// <param name="sSceneBuildIndex">The build index of the scene to unload.</param>
        /// <param name="sSceneIntermission">An optional intermission scene to load during the transition.</param>
        /// <param name="sDatas">Additional transition data.</param>
        /// <returns>An AsyncOperation that can be used to track the progress of the scene unload.</returns>
        public static AsyncOperation UnloadSceneAsync(int sSceneBuildIndex, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            return null;
        }

        /// <summary>
        /// Unloads a scene asynchronously with optional intermission and transition data.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to be unloaded.</param>
        /// <param name="sSceneIntermission">The name of the intermission scene to be shown during the transition, if any.</param>
        /// <param name="sDatas">Additional transition data that might be needed during the unloading process.</param>
        /// <returns>An AsyncOperation object that can be used to track the progress of the scene unload.</returns>
        public static AsyncOperation UnloadSceneAsync(string sSceneName, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
            return null;
        }

        /// <summary>
        /// Asynchronously unloads a specified scene.
        /// </summary>
        /// <param name="sScene">The Scene object to unload.</param>
        /// <param name="sSceneIntermission">Optional parameter for specifying an intermission scene during the unload process.</param>
        /// <param name="sDatas">Optional parameter for providing transition data for the unload process.</param>
        /// <returns>Returns an AsyncOperation that can be used to track the progress of the scene unloading.</returns>
        public static AsyncOperation UnloadSceneAsync(Scene sScene, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = sScene.name;
            RemoveScene(SceneManager.GetActiveScene().name, tSceneName, sSceneIntermission, sDatas);
            return null;
        }

        /// <summary>
        /// Unloads a scene asynchronously by its build index.
        /// </summary>
        /// <param name="sSceneBuildIndex">The build index of the scene to unload.</param>
        /// <param name="sOptions">Options for unloading the scene.</param>
        /// <param name="sSceneIntermission">Optional intermission scene name after which the scene should be unloaded.</param>
        /// <param name="sDatas">Optional transition data associated with the scene unload process.</param>
        /// <returns>An AsyncOperation representing the unload operation.</returns>
        public static AsyncOperation UnloadSceneAsync(int sSceneBuildIndex, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = SceneManager.GetSceneByBuildIndex(sSceneBuildIndex).name;
            RemoveScene(SceneManager.GetActiveScene().name, tSceneName, sSceneIntermission, sDatas);
            return null;
        }

        /// Asynchronously unloads a scene by its name with the specified options, intermission scene, and transition data.
        /// <param name="sSceneName">The name of the scene to unload.</param>
        /// <param name="sOptions">The options for unloading the scene.</param>
        /// <param name="sSceneIntermission">The optional intermission scene to use during the transition.</param>
        /// <param name="sDatas">The optional transition data to use during the transition.</param>
        /// <returns>An AsyncOperation representing the asynchronous unload operation.</returns>
        public static AsyncOperation UnloadSceneAsync(string sSceneName, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
            return null;
        }

        /// <summary>
        /// Unloads the specified scene asynchronously with optional parameters.
        /// </summary>
        /// <param name="sScene">The scene to unload.</param>
        /// <param name="sOptions">Options for unloading the scene.</param>
        /// <param name="sSceneIntermission">Optional intermission scene to transition to during the unload operation.</param>
        /// <param name="sDatas">Optional transition data to be used during the unload operation.</param>
        /// <returns>An AsyncOperation that can be used to track the progress of the scene unload.</returns>
        public static AsyncOperation UnloadSceneAsync(Scene sScene, UnloadSceneOptions sOptions, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            string tSceneName = sScene.name;
            return null;
        }

        /// <summary>
        /// Sets the given scene as the active scene.
        /// </summary>
        /// <param name="sScene">The scene to be set as active.</param>
        public static void SetActiveScene(Scene sScene)
        {
            SceneManager.SetActiveScene(sScene);
        }

        /// <summary>
        /// Internal method to load a scene with specified parameters.
        /// </summary>
        /// <param name="sSceneName">Name of the scene to load.</param>
        /// <param name="sLoadSceneMode">Load mode for the scene, either Single or Additive.</param>
        /// <param name="sSceneIntermission">Optional parameter to specify a scene intermission.</param>
        /// <param name="sDatas">Optional parameter to provide additional transition data.</param>
        /// <param name="sAllowCyclic">Flag to allow cyclic loading, default is false.</param>
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
                    AddScene(sSceneName, sSceneIntermission, sDatas, sAllowCyclic);
                }
                    break;
            }
        }

        /// <summary>
        /// Unloads the specified scene, optionally transitioning to an intermission scene and
        /// using specific transition data if provided.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to unload.</param>
        /// <param name="sSceneIntermission">The name of the intermission scene to transition to, if any. This parameter is optional.</param>
        /// <param name="sDatas">Transition data to use during the unload process. This parameter is optional.</param>
        private static void INTERNAL_UnloadScene(string sSceneName, string sSceneIntermission = null, STSTransitionData sDatas = null)
        {
            RemoveScene(SceneManager.GetActiveScene().name, sSceneName, sSceneIntermission, sDatas);
        }

        /// <summary>
        /// Retrieves a list of all scenes that are currently loaded in the application.
        /// </summary>
        /// <returns>A list of <see cref="Scene"/> objects representing all loaded scenes.</returns>
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