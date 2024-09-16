using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;

namespace SceneTransitionSystem
{
    /// <summary>
    /// A class responsible for managing addressable assets within the Scene Transition System (STS).
    /// Implements the Singleton pattern and interfaces for transitions and intermissions.
    /// </summary>
    public partial class STSAddressableAssets : STSSingletonUnity<STSAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// A constant string message indicating that a scene must be loaded.
        /// </summary>
        const string K_SCENE_MUST_BY_LOADED = "Scene must be loaded!";

        /// <summary>
        /// Constant string that indicates the presence of scenes that are not included
        /// in the build configuration. This message is used to inform about the missing scenes
        /// during the scene transition process in the SceneTransitionSystem.
        /// </summary>
        const string K_SCENE_UNKNOW = "Some scenes are not in build!";

        /// <summary>
        /// Constant string message indicating that a transition is currently in progress.
        /// </summary>
        const string K_TRANSITION_IN_PROGRESS = "Transition in progress";

        /// <summary>
        /// Represents the current state of a transition process.
        /// </summary>
        /// <remarks>
        /// This variable is used to determine whether a scene transition is actively in progress within the Scene Transition System.
        /// </remarks>
        private bool TransitionInProgress = false;

        /// <summary>
        /// A dictionary that maps scene names to their respective <see cref="SceneInstance"/> objects,
        /// representing the currently loaded scenes.
        /// </summary>
        static private Dictionary<string, SceneInstance> SceneInstanceLoaded = new Dictionary<string, SceneInstance>();

        /// <summary>
        /// Checks if a scene transition is currently in progress.
        /// </summary>
        /// <returns>True if a transition is in progress, otherwise false.</returns>
        static public bool InTransition()
        {
            bool rReturn = Singleton().TransitionInProgress;
            if (rReturn == false)
            {
                Debug.Log("#### STSSceneManager NOT IN TRANSITION");
            }
            else
            {
                Debug.Log("#### STSSceneManager IN TRANSITION");
            }

            return rReturn;
        }

        /// <summary>
        /// Gets a value indicating whether a scene transition is currently in progress.
        /// </summary>
        /// <value>
        /// A boolean value: <c>true</c> if a transition is in progress; otherwise, <c>false</c>.
        /// </value>
        static public bool inTransition => Singleton().TransitionInProgress;

        /// <summary>
        /// Represents the default effect to be applied when entering a scene.
        /// </summary>
        public STSEffectType DefaultEffectOnEnter;

        /// <summary>
        /// Specifies the default effect to be executed when transitioning out of a scene.
        /// Used to determine the exit effect in cases where a specific effect is not explicitly defined.
        /// </summary>
        public STSEffectType DefaultEffectOnExit;

        /// <summary>
        /// Represents the initial or baseline scene in the scene transition system.
        /// This scene is used as the starting point for transitions and can be updated
        /// based on the active scene when the system initializes.
        /// </summary>
        public STSScene OriginalScene;

        /// <summary>
        /// Represents the type of effect used during scene transitions.
        /// </summary>
        private STSEffect EffectType;

        /// <summary>
        /// Represents the timer for the standby duration between scene transitions.
        /// This timer is used to track the elapsed time during the standby phase.
        /// </summary>
        private float StandByTimer;

        /// <summary>
        /// Represents a flag indicating whether the system should launch the next scene.
        /// </summary>
        private bool LauchNextScene = false;

        /// <summary>
        /// Indicates whether a stand-by phase is currently in progress during a scene transition.
        /// </summary>
        private bool StandByInProgress = false;

        /// <summary>
        /// Controls whether user interactions are allowed during the scene transition process.
        /// When set to true, the system prevents any user interactions such as input events
        /// until the transition is complete.
        /// </summary>
        private bool PreventUserInteractions = true;

        /// <summary>
        /// Retrieves the transition parameters script for the specified scene. If it does not already exist,
        /// creates a new instance and assigns default enter and exit effects if available.
        /// </summary>
        /// <param name="sScene">The scene for which to retrieve or create the transition parameters script.</param>
        /// <returns>The transition parameters script for the specified scene.</returns>
        public STSTransition GetTransitionsParams(Scene sScene)
        {
            STSTransition tTransitionParametersScript;
            if (STSTransition.SharedInstanceExists(sScene))
            {
                //Debug.LogWarning("tTransitionParametersScript exists");
                tTransitionParametersScript = STSTransition.SharedInstance(sScene);
            }
            else
            {
                //Debug.LogWarning("tTransitionParametersScript not exists");
                tTransitionParametersScript = STSTransition.SharedInstance(sScene);
                if (DefaultEffectOnEnter != null)
                {
                    tTransitionParametersScript.EffectOnEnter = DefaultEffectOnEnter.Dupplicate();
                }
                else
                {
                    tTransitionParametersScript.EffectOnEnter = STSEffectType.Default.Dupplicate();
                }

                if (DefaultEffectOnExit != null)
                {
                    tTransitionParametersScript.EffectOnExit = DefaultEffectOnExit.Dupplicate();
                }
                else
                {
                    tTransitionParametersScript.EffectOnExit = STSEffectType.Default.Dupplicate();
                }
            }

            return tTransitionParametersScript;
        }

        /// <summary>
        /// Retrieves the stand-by parameters for a given scene transition. If the shared instance of
        /// the transition is not yet initialized, it initializes a new instance and sets the stand-by
        /// seconds to a default value of 5.0 seconds.
        /// </summary>
        /// <param name="sScene">The scene for which to retrieve the stand-by parameters.</param>
        /// <returns>A <see cref="STSIntermission"/> object containing the stand-by parameters for the specified scene.</returns>
        public STSIntermission GetStandByParams(Scene sScene)
        {
            STSIntermission tTransitionStandByScript;
            if (STSIntermission.SharedInstanceExists(sScene))
            {
                tTransitionStandByScript = STSIntermission.SharedInstance(sScene);
            }
            else
            {
                tTransitionStandByScript = STSIntermission.SharedInstance(sScene);
                tTransitionStandByScript.StandBySeconds = 5.0f;
            }

            return tTransitionStandByScript;
        }

        /// <summary>
        /// Initializes the OriginalScene property if it is null. Sets the ScenePath of the OriginalScene
        /// to the path of the currently active Unity scene.
        /// </summary>
        private void Start()
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
        }

        // toolbox method
        /// <summary>
        /// Toggles the AudioListeners for all scenes except the active scene. The active scene's AudioListener is set based on the provided parameter.
        /// </summary>
        /// <param name="sEnable">A boolean value that enables or disables the AudioListener for the active scene.</param>
        private void AudioListenerPrevent(bool sEnable)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene tScene = SceneManager.GetSceneAt(i);
                AudioListenerEnable(tScene, false);
            }

            AudioListenerEnable(SceneManager.GetActiveScene(), sEnable);
        }

        /// <summary>
        /// Enables or disables the AudioListener component in a given scene.
        /// </summary>
        /// <param name="sScene">The scene in which to find and modify the AudioListener component.</param>
        /// <param name="sEnable">A boolean value indicating whether to enable or disable the AudioListener component.</param>
        private void AudioListenerEnable(Scene sScene, bool sEnable)
        {
            AudioListener tAudioListener = null;
            GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
            foreach (GameObject tObject in tAllRootObjects)
            {
                if (tObject.GetComponent<AudioListener>() != null)
                {
                    tAudioListener = tObject.GetComponent<AudioListener>();
                }
            }

            if (tAudioListener != null)
            {
                tAudioListener.enabled = sEnable;
            }
        }

        /// <summary>
        /// Prevents the event system from sending events to the scenes other than the active scene.
        /// </summary>
        /// <param name="sEnable">Indicates if the event system should be enabled for the active scene.</param>
        private void EventSystemPrevent(bool sEnable)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene tScene = SceneManager.GetSceneAt(i);
                if (tScene.isLoaded)
                {
                    EventSystemEnable(tScene, false);
                }
            }

            EventSystemEnable(SceneManager.GetActiveScene(), sEnable);
        }

        /// <summary>
        /// Enables or disables the EventSystem in a given scene.
        /// </summary>
        /// <param name="sScene">The scene in which the EventSystem should be enabled or disabled.</param>
        /// <param name="sEnable">A boolean indicating whether to enable (true) or disable (false) the EventSystem.</param>
        private void EventSystemEnable(Scene sScene, bool sEnable)
        {
            if (PreventUserInteractions == true)
            {
                EventSystem tEventSystem = null;
                if (string.IsNullOrEmpty(sScene.name) == false)
                {
                    GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
                    foreach (GameObject tObject in tAllRootObjects)
                    {
                        if (tObject.GetComponent<EventSystem>() != null)
                        {
                            tEventSystem = tObject.GetComponent<EventSystem>();
                        }
                    }

                    if (tEventSystem != null)
                    {
                        tEventSystem.enabled = sEnable;
                    }
                }
                else
                {
                    Debug.LogWarning("EventSystemEnable() - Scene is null");
                }
            }
        }

        /// <summary>
        /// Enables or disables camera prevention for the active scene and all other currently loaded scenes.
        /// </summary>
        /// <param name="sEnable">A boolean indicating whether to enable or disable camera prevention.</param>
        private void CameraPrevent(bool sEnable)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene tScene = SceneManager.GetSceneAt(i);
                if (tScene.isLoaded)
                {
                    CameraPreventEnable(tScene, false);
                }
            }

            CameraPreventEnable(SceneManager.GetActiveScene(), sEnable);
        }

        /// <summary>
        /// Enables or disables the camera in the specified scene.
        /// </summary>
        /// <param name="sScene">The scene in which the camera should be enabled or disabled.</param>
        /// <param name="sEnable">A boolean value indicating whether to enable or disable the camera.</param>
        private void CameraPreventEnable(Scene sScene, bool sEnable)
        {
            if (PreventUserInteractions == true)
            {
                Camera tCameraSystem = null;
                if (string.IsNullOrEmpty(sScene.name) == false)
                {
                    GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
                    foreach (GameObject tObject in tAllRootObjects)
                    {
                        if (tObject.GetComponent<Camera>() != null)
                        {
                            tCameraSystem = tObject.GetComponent<Camera>();
                        }
                    }

                    if (tCameraSystem != null)
                    {
                        tCameraSystem.enabled = sEnable;
                    }
                }
                else
                {
                    Debug.LogWarning("CameraEnable() - Scene is null");
                }
            }
        }

        /// <summary>
        /// Retrieves an array of <c>STSIntermissionInterface</c> instances from the root GameObjects in the specified scene.
        /// </summary>
        /// <param name="sScene">The scene from which to retrieve the <c>STSIntermissionInterface</c> instances.</param>
        /// <returns>An array of <c>STSIntermissionInterface</c> instances found in the root GameObjects of the scene.</returns>
        private STSIntermissionInterface[] GetIntermissionInterface(Scene sScene)
        {
            List<STSIntermissionInterface> rReturn = new List<STSIntermissionInterface>();
            GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
            foreach (GameObject tObject in tAllRootObjects)
            {
                STSIntermissionInterface tScript = tObject.GetComponent<STSIntermissionInterface>();
                if (tScript != null)
                {
                    rReturn.Add(tScript);
                }
            }

            return rReturn.ToArray();
        }

        /// <summary>
        /// Retrieves an array of all objects in a scene that implement the STSTransitionInterface.
        /// </summary>
        /// <param name="sScene">The scene to search for objects implementing the STSTransitionInterface.</param>
        /// <returns>An array of objects from the scene that implement the STSTransitionInterface.</returns>
        private STSTransitionInterface[] GetTransitionInterface(Scene sScene)
        {
            List<STSTransitionInterface> rReturn = new List<STSTransitionInterface>();
            GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
            foreach (GameObject tObject in tAllRootObjects)
            {
                STSTransitionInterface tScript = tObject.GetComponent<STSTransitionInterface>();
                if (tScript != null)
                {
                    rReturn.Add(tScript);
                }
            }

            return rReturn.ToArray();
        }

        /// <summary>
        /// Finds and returns all instances of <see cref="STSTransitionInterface"/> in loaded scenes except the specified scene.
        /// </summary>
        /// <param name="sExceptScene">The scene to be excluded from the search.</param>
        /// <returns>An array of <see cref="STSTransitionInterface"/> instances from all other loaded scenes.</returns>
        private STSTransitionInterface[] GetOtherTransitionInterface(Scene sExceptScene)
        {
            List<STSTransitionInterface> rReturn = new List<STSTransitionInterface>();
            foreach (Scene tScene in GetAllLoadedScenes())
            {
                if (tScene != sExceptScene)
                {
                    GameObject[] tAllRootObjects = tScene.GetRootGameObjects();
                    foreach (GameObject tObject in tAllRootObjects)
                    {
                        STSTransitionInterface tScript = tObject.GetComponent<STSTransitionInterface>();
                        if (tScript != null)
                        {
                            rReturn.Add(tScript);
                        }
                    }
                }
            }

            return rReturn.ToArray();
        }

        /// <summary>
        /// Determines if an animation is currently in progress.
        /// </summary>
        /// <returns>
        /// True if the animation associated with the current effect type is playing; otherwise, false.
        /// </returns>
        private bool AnimationProgress()
        {
            return EffectType.AnimIsPlaying;
        }

        /// Checks if the animation has finished.
        /// <return>True if the animation is finished; otherwise, false.</return>
        private bool AnimationFinished()
        {
            return EffectType.AnimIsFinished;
        }

        /// <summary>
        /// Initiates the transition effect when entering a new scene.
        /// </summary>
        /// <param name="sThisSceneParameters">Transition settings for the current scene.</param>
        /// <param name="sDatas">Data related to the transition effect, including additional effect information.</param>
        private void AnimationTransitionIn(STSTransition sThisSceneParameters, STSTransitionData sDatas)
        {
            Color tOldColor = Color.black;
            float tInterlude = 0;
            if (EffectType != null)
            {
                // I get the old value of
                tOldColor = new Color(EffectType.TintPrimary.r, EffectType.TintPrimary.g, EffectType.TintPrimary.b, EffectType.TintPrimary.a);
                tInterlude = sThisSceneParameters.InterEffectDuration;
            }

            EffectType = sThisSceneParameters.EffectOnEnter.GetEffect();
            if (EffectType == null)
            {
                EffectType = new STSEffectFade();
            }

            STSEffectMoreInfos sMoreInfos = null;
            if (sDatas != null)
            {
                sMoreInfos = sDatas.EffectMoreInfos;
            }

            EffectType.StartEffectEnter(new Rect(0, 0, Screen.width, Screen.height), tOldColor, tInterlude, sMoreInfos);
        }

        /// <summary>
        /// Initiates the transition-out animation for the current scene using the specified transition parameters and transition data.
        /// </summary>
        /// <param name="sThisSceneParameters">The transition parameters for the current scene.</param>
        /// <param name="sDatas">Additional data for the transition.</param>
        private void AnimationTransitionOut(STSTransition sThisSceneParameters, STSTransitionData sDatas)
        {
            EffectType = sThisSceneParameters.EffectOnExit.GetEffect();
            if (EffectType == null)
            {
                EffectType = new STSEffectFade();
            }

            STSEffectMoreInfos sMoreInfos = null;
            if (sDatas != null)
            {
                sMoreInfos = sDatas.EffectMoreInfos;
            }

            EffectType.StartEffectExit(new Rect(0, 0, Screen.width, Screen.height), sMoreInfos);
        }

        /// <summary>
        /// Initializes the standby process by setting the standby timer to 0,
        /// marking the standby as in progress, and ensuring the next scene is not launched.
        /// </summary>
        private void StandBy()
        {
            StandByTimer = 0.0f;
            StandByInProgress = true;
            LauchNextScene = false;
        }

        /// <summary>
        /// Checks if the stand-by process is still progressing based on the provided intermission scene's stand-by duration.
        /// </summary>
        /// <param name="sIntermissionSceneStandBy">An instance of STSIntermission that contains the duration for the stand-by period.</param>
        /// <returns>A boolean indicating whether the stand-by process is still in progress.</returns>
        private bool StandByIsProgressing(STSIntermission sIntermissionSceneStandBy)
        {
            StandByTimer += Time.deltaTime;
            if (StandByTimer >= sIntermissionSceneStandBy.StandBySeconds)
            {
                StandByInProgress = false;
            }

            return StandByInProgress;
        }

        /// <summary>
        /// Determines if the next scene should be automatically launched based on the provided intermission scene's settings.
        /// </summary>
        /// <param name="sIntermissionSceneStandBy">An instance of STSIntermission that holds the standby scene data and settings for launching the next scene.</param>
        /// <returns>Returns <c>true</c> if the current scene should wait and not automatically launch the next scene, <c>false</c> otherwise.</returns>
        private bool WaitingToLauchNextScene(STSIntermission sIntermissionSceneStandBy)
        {
            if (sIntermissionSceneStandBy.AutoActiveNextScene == true)
            {
                LauchNextScene = true;
            }

            return !LauchNextScene;
        }

        /// <summary>
        /// Sets the status of the system to indicate that the standby process has completed
        /// and that the next scene should be launched.
        /// </summary>
        public void FinishStandBy()
        {
            LauchNextScene = true;
            StandByInProgress = false;
        }

        /// <summary>
        /// The OnGUI method is responsible for drawing the GUI elements on the screen during transitions.
        /// </summary>
        /// <remarks>
        /// If a transition effect is active, it renders the effect within the defined screen dimensions.
        /// </remarks>
        void OnGUI()
        {
            if (EffectType != null)
            {
                EffectType.DrawMaster(new Rect(0, 0, Screen.width, Screen.height));
            }
        }

        /// <summary>
        /// Handles the logic to be executed when a transition starts entering.
        /// This method sets up the transition effects, interlude duration, and the
        /// active scene status as per the provided parameters.
        /// </summary>
        /// <param name="sData">Data related to the transition.</param>
        /// <param name="sEffect">Type of effect to be applied during the transition.</param>
        /// <param name="sInterludeDuration">The duration of the interlude between transitions.</param>
        /// <param name="sActiveScene">A boolean indicating whether the transition is for the active scene.</param>
        public void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the transition into a new scene has completed.
        /// </summary>
        /// <param name="sData">The transition data associated with the transition.</param>
        /// <param name="sActiveScene">Indicates whether the scene is active or not.</param>
        public void OnTransitionEnterFinish(STSTransitionData sData, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// This method is triggered at the start of a transition exit.
        /// <param name="sData">Transition data containing details about the current transition.</param>
        /// <param name="sEffect">Effect type that will be played during the transition.</param>
        /// <param name="sActiveScene">Indicates whether the active scene is being transitioned or not.</param>
        public void OnTransitionExitStart(STSTransitionData sData, STSEffectType sEffect, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the exit transition has finished.
        /// </summary>
        /// <param name="sData">Data related to the transition.</param>
        /// <param name="sActiveScene">Indicates if the scene being transitioned to is now active.</param>
        public void OnTransitionExitFinish(STSTransitionData sData, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles actions to be performed when the transition scene has been successfully loaded.
        /// </summary>
        /// <param name="sData">An instance of STSTransitionData containing information about the current transition.</param>
        public void OnTransitionSceneLoaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the logic for when a transition scene becomes enabled.
        /// </summary>
        /// <param name="sData">The transition data associated with the current scene transition.</param>
        public void OnTransitionSceneEnable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the event when the transition scene is disabled.
        /// </summary>
        /// <param name="sData">The data associated with the transition.</param>
        public void OnTransitionSceneDisable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called just before the scene starts unloading during a transition.
        /// </summary>
        /// <param name="sData">The data related to the current transition.</param>
        public void OnTransitionSceneWillUnloaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Invoked when the process of loading the next scene starts.
        /// </summary>
        /// <param name="sData">Transition data containing information about the current transition.</param>
        /// <param name="sPercent">Percentage of the loading process completed.</param>
        public void OnLoadNextSceneStart(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// This method is called to update the loading progress of the next scene.
        /// </summary>
        /// <param name="sData">The transition data associated with the current scene transition.</param>
        /// <param name="sPercent">A float value representing the percentage of the next scene that has been loaded.</param>
        public void OnLoadingNextScenePercent(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the loading of the next scene has finished.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene loading.</param>
        /// <param name="sPercent">The percentage of the scene that was loaded when this method was called, typically should be 100%.</param>
        public void OnLoadNextSceneFinish(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the logic to be executed when a stand-by phase begins during a scene transition.
        /// </summary>
        /// <param name="sStandBy">An instance of STSIntermission that provides parameters for the stand-by phase.</param>
        public void OnStandByStart(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the standby phase has finished.
        /// </summary>
        /// <param name="sStandBy">The intermission data associated with the standby phase.</param>
        public void OnStandByFinish(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }

        /// Invoked at the start of the scene loading process.
        /// <param name="sData">The transition data associated with the scene load.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The numerical identifier of the scene being loaded.</param>
        /// <param name="sScenePercent">The loading progress percentage of the current scene.</param>
        /// <param name="sPercent">The overall loading progress percentage.</param>
        public void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the percentage progress update while loading a scene.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The number identifier for the scene.</param>
        /// <param name="sScenePercent">The percentage progress of the individual scene being loaded.</param>
        /// <param name="sPercent">The overall percentage progress of the loading process.</param>
        public void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the loading of a scene is completed.
        /// </summary>
        /// <param name="sData">Contains data related to the scene transition.</param>
        /// <param name="sSceneName">The name of the scene that has finished loading.</param>
        /// <param name="SceneNumber">The number of the scene that has finished loading.</param>
        /// <param name="sScenePercent">The percentage of the specific scene loading process that is completed.</param>
        /// <param name="sPercent">The overall percentage of all scene loading processes that are completed.</param>
        public void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when all necessary assets for the scene have been loaded and are ready to be used.
        /// </summary>
        /// <param name="sData">Data associated with the transition.</param>
        /// <param name="sSceneName">The name of the scene that has finished loading.</param>
        /// <param name="SceneNumber">The index of the scene.</param>
        /// <param name="sPercent">The loading progress percentage.</param>
        public void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the process of unloading a specified scene during a transition.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene unload process.</param>
        /// <param name="sSceneName">The name of the scene to be unloaded.</param>
        /// <param name="SceneNumber">The numerical identifier of the scene.</param>
        /// <param name="sPercent">The percentage progress of the scene unload process.</param>
        public void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
    }
}