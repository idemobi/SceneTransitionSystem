using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;


namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSSceneManager class is responsible for managing scene transitions within the Scene Transition System.
    /// This class extends STSSingletonUnity to ensure a single instance is used throughout the application.
    /// It implements the STSTransitionInterface and STSIntermissionInterface to handle scene transitions and intermissions.
    /// </summary>
    public partial class STSSceneManager : STSSingletonUnity<STSSceneManager>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Error message indicating that the scene must be loaded.
        /// </summary>
        const string K_SCENE_MUST_BY_LOADED = "Scene must be loaded!";

        /// <summary>
        /// Message indicating that some scenes are not included in the build settings.
        /// </summary>
        const string K_SCENE_UNKNOW = "Some scenes are not in build!";

        /// <summary>
        /// Constant string indicating that a transition is currently in progress.
        /// </summary>
        const string K_TRANSITION_IN_PROGRESS = "Transition in progress";

        /// <summary>
        /// Flag indicating whether a scene transition is currently in progress.
        /// </summary>
        private bool TransitionInProgress = false;


        /// <summary>
        /// Return if transition is in progress or not.
        /// This method also logs the result of the method.
        /// </summary>
        /// <returns>true if transition is running, false otherwise.</returns>
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
        /// Indicates whether a scene transition is currently in progress.
        /// </summary>
        static public bool inTransition => Singleton().TransitionInProgress;


        // prevent user actions during the transition
        /// <summary>
        /// Specifies whether user interactions should be prevented during scene transitions.
        /// </summary>
        private bool PreventUserInteractions = true;

        /// <summary>
        /// Represents the default effect applied when a scene transition starts.
        /// </summary>
        public STSEffectType DefaultEffectOnEnter;

        /// <summary>
        /// Specifies the default transition effect to be applied when exiting a scene.
        /// </summary>
        public STSEffectType DefaultEffectOnExit;

        /// <summary>
        /// Controls the various transition effects applied between scenes.
        /// </summary>
        private STSEffect EffectType;

        /// <summary>
        /// Holds the elapsed time during a standby period for scene transitions.
        /// Reset when standby period starts and incremented with deltaTime.
        /// </summary>
        private float StandByTimer;

        /// <summary>
        /// Flag indicating whether the next scene should be launched
        /// </summary>
        private bool LauchNextScene = false;

        /// <summary>
        /// Indicates whether a standby operation is currently in progress.
        /// </summary>
        private bool StandByInProgress = false;

        /// <summary>
        /// Represents the initial scene when the scene transition system starts.
        /// </summary>
        public STSScene OriginalScene;

        // Memory managment
        /// <summary>
        /// Initializes the instance of the STSSceneManager.
        /// Ensures required components are added to the game object.
        /// </summary>
        public override void InitInstance()
        {
            //Debug.Log("STSSceneManager InitInstance()");
            //if (gameObject.GetComponent<STSTransition>() == null)
            //{
            //    gameObject.AddComponent<STSTransition>();
            //}
            //if (gameObject.GetComponent<STSIntermission>() == null)
            //{
            //    gameObject.AddComponent<STSIntermission>();
            //}
        }

        /// <summary>
        /// Initializes the OriginalScene with the active scene path if it is not already set.
        /// This method is called at the start of the scene transition process.
        /// </summary>
        private void Start()
        {
            //Debug.Log("<color=red>START</color>");
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
        /// Enables or disables audio listeners in all scenes except the active scene,
        /// where the listener is enabled or disabled based on the provided parameter.
        /// </summary>
        /// <param name="sEnable">If true, enables the audio listener in the active scene. Otherwise, disables it.</param>
        private void AudioListenerPrevent(bool sEnable)
        {
            //Debug.Log("STSSceneManager AudioListenerPrevent()");
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene tScene = SceneManager.GetSceneAt(i);
                //if (tScene.isLoaded)
                {
                    AudioListenerEnable(tScene, false);
                }
            }

            AudioListenerEnable(SceneManager.GetActiveScene(), sEnable);
        }

        /// <summary>
        /// Enables or disables the AudioListener component in the root game objects of the specified scene.
        /// </summary>
        /// <param name="sScene">The scene in which to enable or disable the AudioListener component.</param>
        /// <param name="sEnable">A boolean value indicating whether to enable (true) or disable (false) the AudioListener component.</param>
        private void AudioListenerEnable(Scene sScene, bool sEnable)
        {
            //Debug.Log("STSSceneManager AudioListenerEnable()");
            // if (sScene.isLoaded)
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
                else
                {
                    //Debug.Log ("No <AudioListener> type component found in the root Objects. Becarefull!");
                }
            }
        }

        /// <summary>
        /// Enables or disables the EventSystem for all loaded scenes.
        /// This method is primarily used to prevent user input during scene transitions.
        /// </summary>
        /// <param name="sEnable">If true, the EventSystem will be enabled for the active scene; otherwise, it will be disabled for all loaded scenes.</param>
        private void EventSystemPrevent(bool sEnable)
        {
            //Debug.Log("STSSceneManager EventSystemPrevent()");
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
        /// Enables or disables the EventSystem in the specified scene.
        /// This method is used to control user interactions during scene transitions.
        /// </summary>
        /// <param name="sScene">The scene in which to enable or disable the EventSystem.</param>
        /// <param name="sEnable">A boolean indicating whether to enable (true) or disable (false) the EventSystem.</param>
        private void EventSystemEnable(Scene sScene, bool sEnable)
        {
            //Debug.Log("STSSceneManager EventSystemEnable()");
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
        /// Disables or enables camera functionality for all currently loaded scenes and the active scene.
        /// </summary>
        /// <param name="sEnable">If true, camera functionality is enabled. If false, it is disabled.</param>
        private void CameraPrevent(bool sEnable)
        {
            //Debug.Log("STSSceneManager EventSystemPrevent()");
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
        /// Enables or disables the Camera component in the given scene.
        /// </summary>
        /// <param name="sScene">The scene in which the Camera component needs to be enabled or disabled.</param>
        /// <param name="sEnable">A boolean indicating whether to enable (true) or disable (false) the Camera component.</param>
        private void CameraPreventEnable(Scene sScene, bool sEnable)
        {
            //Debug.Log("STSSceneManager EventSystemEnable()");
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
        /// Retrieves all the components in the specified scene that implement the STSIntermissionInterface.
        /// </summary>
        /// <param name="sScene">The Scene from which to retrieve the STSIntermissionInterface components.</param>
        /// <returns>An array of STSIntermissionInterface components found in the specified scene.</returns>
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
            //return FindObjectsOfType(typeof(STSIntermissionInterface)) as STSIntermissionInterface[];
        }

        /// <summary>
        /// Retrieves all the transition interfaces implemented by the root objects in the provided scene.
        /// </summary>
        /// <param name="sScene">The scene in which to look for transition interfaces.</param>
        /// <returns>An array of STSTransitionInterface objects found in the root objects of the scene.</returns>
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
            //return FindObjectsOfType(typeof(STSTransitionInterface)) as STSTransitionInterface[];
        }

        /// <summary>
        /// Retrieves an array of STSTransitionInterface components from all loaded scenes, excluding the specified scene.
        /// </summary>
        /// <param name="sExceptScene">The scene to exclude from the search.</param>
        /// <returns>An array of STSTransitionInterface components from all loaded scenes, excluding the specified scene.</returns>
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
            //return FindObjectsOfType(typeof(STSTransitionInterface)) as STSTransitionInterface[];
        }

        /// <summary>
        /// Retrieves the transition parameters associated with a given scene.
        /// If no transition parameters exist, they are created and assigned default values.
        /// </summary>
        /// <param name="sScene">The scene for which to retrieve the transition parameters.</param>
        /// <returns>The transition parameters script associated with the given scene.</returns>
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

            /*
            //Debug.Log("STSSceneManager GetTransitionsParams()");
            STSTransition tTransitionParametersScript = null;
            GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
            // quick solution?!
            foreach (GameObject tObject in tAllRootObjects)
            {
                if (tObject.GetComponent<STSTransition>() != null)
                {
                    tTransitionParametersScript = tObject.GetComponent<STSTransition>();
                    break;
                }
            }
            // slower solution?!
            if (tTransitionParametersScript == null)
            {
                foreach (GameObject tObject in tAllRootObjects)
                {
                    if (tObject.GetComponentInChildren<STSTransition>() != null)
                    {
                        tTransitionParametersScript = tObject.GetComponent<STSTransition>();
                        break;
                    }
                }
            }
            // no solution?!
            if (tTransitionParametersScript == null)
            {
                Scene tActual = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(sScene);
                // create Game Object?
                //Debug.Log ("NO PARAMS");
                GameObject tObjToSpawn = new GameObject(STSConstants.K_TRANSITION_DEFAULT_OBJECT_NAME);
                tObjToSpawn.AddComponent<STSSceneController>();
                tTransitionParametersScript = tObjToSpawn.AddComponent<STSTransition>();
                if (DefaultEffectOnEnter != null)
                {
                    tTransitionParametersScript.EffectOnEnter = DefaultEffectOnEnter.Dupplicate();
                }
                else
                {
                    tTransitionParametersScript.EffectOnEnter = STSEffectType.Default.Dupplicate();
                }
                if (DefaultEffectOnEnter != null)
                {
                    tTransitionParametersScript.EffectOnExit = DefaultEffectOnExit.Dupplicate();
                }
                else
                {
                    tTransitionParametersScript.EffectOnExit = STSEffectType.Default.Dupplicate();
                }
                SceneManager.SetActiveScene(tActual);
            }
            */
            return tTransitionParametersScript;
        }

        /// <summary>
        /// Retrieves the standby parameters for a given scene.
        /// </summary>
        /// <param name="sScene">The scene for which the standby parameters are to be fetched.</param>
        /// <returns>An instance of STSIntermission containing the standby parameters for the specified scene.</returns>
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
                tTransitionStandByScript.AutoActiveNextScene = true;
            }

            /*
            //Debug.Log("STSSceneManager GetStandByParams()");
            STSIntermission tTransitionStandByScript = null;
            GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
            // quick solution?!
            foreach (GameObject tObject in tAllRootObjects)
            {
                if (tObject.GetComponent<STSIntermission>() != null)
                {
                    tTransitionStandByScript = tObject.GetComponent<STSIntermission>();
                    break;
                }
            }
            // slower solution?!
            if (tTransitionStandByScript == null)
            {
                foreach (GameObject tObject in tAllRootObjects)
                {
                    if (tObject.GetComponentInChildren<STSIntermission>() != null)
                    {
                        tTransitionStandByScript = tObject.GetComponent<STSIntermission>();
                        break;
                    }
                }
            }
            // no solution?!
            if (tTransitionStandByScript == null)
            {
                Scene tActual = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(sScene);
                GameObject tObjToSpawn = new GameObject(STSConstants.K_TRANSITION_Intermission_OBJECT_NAME);
                tObjToSpawn.AddComponent<STSSceneIntermissionController>();
                tTransitionStandByScript = tObjToSpawn.AddComponent<STSIntermission>();
                tTransitionStandByScript.StandBySeconds = 5.0f;
                tTransitionStandByScript.AutoLoadNextScene = true;
                SceneManager.SetActiveScene(tActual);
            }
            */
            return tTransitionStandByScript;
        }
        //private int m_DrawDepth = -1000;

        /// <summary>
        /// Determines if an animation is currently in progress.
        /// </summary>
        /// <returns>true if an animation is playing; otherwise, false.</returns>
        private bool AnimationProgress()
        {
            return EffectType.AnimIsPlaying;
        }

        /// <summary>
        /// Indicates whether the current animation has finished playing.
        /// </summary>
        /// <returns>true if the animation is finished, false otherwise.</returns>
        private bool AnimationFinished()
        {
            return EffectType.AnimIsFinished;
        }

        /// <summary>
        /// Initiates the animation-based transition when entering a new scene.
        /// </summary>
        /// <param name="sThisSceneParameters">Parameters of the scene transition, including effect settings.</param>
        /// <param name="sDatas">Additional transition data, which may include more effect information.</param>
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
        /// Plays the exit animation transition based on the specified transition parameters and data.
        /// </summary>
        /// <param name="sThisSceneParameters">The transition parameters for the current scene.</param>
        /// <param name="sDatas">Additional data for the transition, such as effect information.</param>
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
        /// Initializes the standby state by resetting the standby timer,
        /// setting the standby progress flag to true, and ensuring that the next scene
        /// launch is set to false.
        /// </summary>
        private void StandBy()
        {
            StandByTimer = 0.0f;
            StandByInProgress = true;
            LauchNextScene = false;
        }

        /// <summary>
        /// Indicates whether the stand-by phase is currently in progress.
        /// Updates the stand-by timer and checks if the stand-by period has elapsed.
        /// </summary>
        /// <param name="sIntermissionSceneStandBy">The STSIntermission instance representing the current stand-by scene.</param>
        /// <returns>true if the stand-by phase is still in progress, false otherwise.</returns>
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
        /// Determines if the system is waiting to launch the next scene.
        /// This method updates the next scene's launch status based on the auto-activation setting of the intermission scene.
        /// </summary>
        /// <param name="sIntermissionSceneStandBy">The current intermission scene details.</param>
        /// <returns>true if not ready to launch the next scene, otherwise false.</returns>
        private bool WaitingToLauchNextScene(STSIntermission sIntermissionSceneStandBy)
        {
            if (sIntermissionSceneStandBy.AutoActiveNextScene == true)
            {
                LauchNextScene = true;
            }

            return !LauchNextScene;
        }

        /// <summary>
        /// Completes the standby phase by setting the flags to launch the next scene
        /// and indicating that standby is no longer in progress.
        /// </summary>
        public void FinishStandBy()
        {
            LauchNextScene = true;
            StandByInProgress = false;
        }

        /// <summary>
        /// Draws the current effect on the GUI.
        /// This method is called automatically by Unity during the OnGUI phase.
        /// </summary>
        void OnGUI()
        {
            if (EffectType != null)
            {
                EffectType.DrawMaster(new Rect(0, 0, Screen.width, Screen.height));
            }
        }

        /// <summary>
        /// Checks if all scenes in the provided list are included in the build settings.
        /// Logs warnings for any scenes that are not found in the build settings.
        /// </summary>
        /// <param name="sScenesList">A list of scene names to check against the build settings.</param>
        /// <returns>true if all scenes are included in the build settings, false otherwise.</returns>
        private bool ScenesAreAllInBuild(List<string> sScenesList)
        {
            bool rReturn = true;
            List<string> tScenesInBuildList = new List<string>();
            for (int tIndex = 0; tIndex < SceneManager.sceneCountInBuildSettings; tIndex++)
            {
                string tScenePath = SceneUtility.GetScenePathByBuildIndex(tIndex);
                string sSceneInBuild = Path.GetFileNameWithoutExtension(tScenePath);
                //Debug.Log("scene ["+tIndex + "] => " + sSceneInBuild + " IN BUILD!");
                tScenesInBuildList.Add(sSceneInBuild);
            }

            foreach (string tScene in sScenesList)
            {
                if (tScenesInBuildList.Contains(tScene) == false)
                {
                    Debug.LogWarning("Scene '" + tScene + "' NOT IN BUILD! STOP THE TRAIN? (array is '" + string.Join("','", sScenesList) + "')");
                    rReturn = false;
                    break;
                }
            }

            return rReturn;
        }

        /// <summary>
        /// Initiates the start of a scene transition.
        /// </summary>
        /// <param name="sData">The data associated with the transition.</param>
        /// <param name="sEffect">The effect to apply during the transition.</param>
        /// <param name="sInterludeDuration">The duration of the interlude effect.</param>
        /// <param name="sActiveScene">Determines if the active scene should be involved in the transition.</param>
        public void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Executes actions to finalize the transition process when entering a scene.
        /// </summary>
        /// <param name="sData">Data related to the ongoing scene transition.</param>
        /// <param name="sActiveScene">Indicates if the current scene being transitioned to is set as the active scene.</param>
        public void OnTransitionEnterFinish(STSTransitionData sData, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initiates the start of a transition exit.
        /// This functionality is executed during the commencement of a scene transition exit.
        /// </summary>
        /// <param name="sData">The transition data associated with the transition.</param>
        /// <param name="sEffect">The type of effect to be applied during the transition.</param>
        /// <param name="sActiveScene">Specifies whether the current scene is active or not.</param>
        public void OnTransitionExitStart(STSTransitionData sData, STSEffectType sEffect, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Executes the actions required to finalize the exit transition.
        /// This method handles the completion of scene transition away from an active scene.
        /// </summary>
        /// <param name="sData">The transition data associated with the current scene transition.</param>
        /// <param name="sActiveScene">Indicates whether the current scene is active or not.</param>
        public void OnTransitionExitFinish(STSTransitionData sData, bool sActiveScene)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Triggered when a transition scene has successfully loaded.
        /// Intended to handle post-loading logic and operations.
        /// </summary>
        /// <param name="sData">Data related to the scene transition.</param>
        public void OnTransitionSceneLoaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the enabling of a scene transition.
        /// </summary>
        /// <param name="sData">The data related to the scene transition.</param>
        public void OnTransitionSceneEnable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the disabling of a scene during a transition.
        /// </summary>
        /// <param name="sData">Data related to the scene transition.</param>
        public void OnTransitionSceneDisable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles actions that should take place just before the scene is unloaded during a transition.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene that will be unloaded.</param>
        public void OnTransitionSceneWillUnloaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Invoked when the loading of the next scene starts.
        /// </summary>
        /// <param name="sData">The data associated with the scene transition.</param>
        /// <param name="sPercent">The percentage of loading completed when this method is called.</param>
        public void OnLoadNextSceneStart(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates the progress of loading the next scene.
        /// </summary>
        /// <param name="sData">The transition data associated with the current operation.</param>
        /// <param name="sPercent">The percentage of the next scene that has been loaded.</param>
        public void OnLoadingNextScenePercent(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the next scene has completed loading.
        /// This method is invoked after the scene has finished loading and any necessary post-load operations.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene load.</param>
        /// <param name="sPercent">The percentage of the load process that is complete, typically 100% when this method is called.</param>
        public void OnLoadNextSceneFinish(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when a standby phase starts.
        /// </summary>
        /// <param name="sStandBy">Parameters related to the standby state.</param>
        public void OnStandByStart(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finalizes the stand-by process.
        /// This method should be called once the intermission is complete.
        /// </summary>
        /// <param name="sStandBy">The intermission data that holds information about the stand-by process.</param>
        public void OnStandByFinish(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the operations to be executed at the start of loading a scene.
        /// </summary>
        /// <param name="sData">The transition data used for loading the scene.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The number of the scene being loaded.</param>
        /// <param name="sScenePercent">The percentage complete for the current scene loading process.</param>
        /// <param name="sPercent">The total percentage complete for all scene loading processes.</param>
        public void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles the progress of loading a scene.
        /// This method updates the loading progress based on the scene percentage and total percentage.
        /// </summary>
        /// <param name="sData">The transition data associated with the current scene transition.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The number of the scene being loaded.</param>
        /// <param name="sScenePercent">The percentage of the scene that has been loaded.</param>
        /// <param name="sPercent">The total percentage of the loading process that has been completed.</param>
        public void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handle the completion of a scene loading process.
        /// This method processes the transition data and scene information provided.
        /// </summary>
        /// <param name="sData">The transition data associated with this scene transition.</param>
        /// <param name="sSceneName">The name of the scene that has finished loading.</param>
        /// <param name="SceneNumber">The number identifier of the scene.</param>
        /// <param name="sScenePercent">The percentage of the scene loaded.</param>
        /// <param name="sPercent">The overall percentage of the loading process completed.</param>
        public void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Fires events and logs information when all assets in a scene are fully loaded and ready for use.
        /// This method signifies the completion of scene loading and performs any necessary setup or initialization.
        /// </summary>
        /// <param name="sData">Transition data associated with the scene that is fully loaded.</param>
        /// <param name="sSceneName">Name of the scene that is fully loaded.</param>
        /// <param name="SceneNumber">The index or identifier of the scene which is fully loaded.</param>
        /// <param name="sPercent">Percentage of the scene loading process that is completed, typically 100 when fully loaded.</param>
        public void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Executes the necessary steps to unload a scene, including updating the transition data and progress percentage.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene unloading process.</param>
        /// <param name="sSceneName">The name of the scene being unloaded.</param>
        /// <param name="SceneNumber">The unique number identifying the scene being unloaded.</param>
        /// <param name="sPercent">The percentage of the unloading process that has been completed.</param>
        public void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
    }
}