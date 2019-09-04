//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class STSController : MonoBehaviour, STSTransitionInterface, STSIntermissionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_SCENE_MUST_BY_LOADED = "Scene must be loaded!";
        const string K_TRANSITION_IN_PROGRESS = "Transition in progress";
        //-------------------------------------------------------------------------------------------------------------
        private static STSController kSingleton = null;
        //-------------------------------------------------------------------------------------------------------------
        // initialized or not?
        private bool Initialized = false;
        // prevent multi transition
        private bool TransitionInProgress = false;
        // prevent user actions during the transition
        private bool PreventUserInteractions = true;
        //-------------------------------------------------------------------------------------------------------------
        public STSEffectType DefaultEffectOnEnter;
        public STSEffectType DefaultEffectOnExit;
        private STSEffect EffectType;
        //-------------------------------------------------------------------------------------------------------------
        private float StandByTimer;
        private bool LauchNextScene = false;
        private bool StandByInProgress = false;
        //-------------------------------------------------------------------------------------------------------------
        // Singleton
        public static STSController Singleton()
        {
            //Debug.Log ("STSTransitionController Singleton()");
            if (kSingleton == null)
            {
                // I need to create singleton
                GameObject tObjToSpawn;
                //spawn object
                tObjToSpawn = new GameObject(STSConstants.K_TRANSITION_CONTROLLER_OBJECT_NAME);
                //Add Components
                tObjToSpawn.AddComponent<STSController>();
                // keep k_Singleton
                kSingleton = tObjToSpawn.GetComponent<STSController>();
                // Init Instance
                kSingleton.InitInstance();
                // memorize the init instance
                kSingleton.Initialized = true;
                tObjToSpawn.AddComponent<STSTransition>();
                tObjToSpawn.AddComponent<STSIntermission>();

            }
            return kSingleton;
        }
        //-------------------------------------------------------------------------------------------------------------
        // Memory managment
        private void InitInstance()
        {
            //Debug.Log("STSTransitionController InitInstance()");
        }
        //-------------------------------------------------------------------------------------------------------------
        //Awake is always called before any Start functions
        private void Awake()
        {
            //Check if instance already exists
            if (kSingleton == null)
            {
                //if not, set instance to this
                kSingleton = this;
                if (kSingleton.Initialized == false)
                {
                    kSingleton.InitInstance();
                    kSingleton.Initialized = true;
                }
            }
            //If instance already exists and it's not this:
            else if (kSingleton != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
            //Call the InitGame function to initialize the first level 
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            //Debug.Log("STSTransitionController OnDestroy()");
        }
        //-------------------------------------------------------------------------------------------------------------
        // toolbox method
        private void AudioListenerPrevent()
        {
            Debug.Log("STSTransitionController AudioListenerPrevent()");
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene tScene = SceneManager.GetSceneAt(i);
                if (tScene.isLoaded)
                {
                    AudioListenerEnable(tScene, false);
                }
            }
            AudioListenerEnable(SceneManager.GetActiveScene(), true);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AudioListenerEnable(Scene sScene, bool sEnable)
        {
            //Debug.Log("STSTransitionController AudioListenerEnable()");
            if (sScene.isLoaded)
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
        //-------------------------------------------------------------------------------------------------------------
        private void EventSystemPrevent(bool sEnable)
        {
            //Debug.Log("STSTransitionController EventSystemPrevent()");
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
        //-------------------------------------------------------------------------------------------------------------
        private void EventSystemEnable(Scene sScene, bool sEnable)
        {
            //Debug.Log("STSTransitionController EventSystemEnable()");
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
        //-------------------------------------------------------------------------------------------------------------
        private STSTransition GetTransitionsParams(Scene sScene, bool sStartTransition) //TODO remove sStartTransition
        {
            //Debug.Log("STSTransitionController GetTransitionsParams()");
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
            return tTransitionParametersScript;
        }
        //-------------------------------------------------------------------------------------------------------------
        private STSIntermission GetStandByParams(Scene sScene)
        {
            //Debug.Log("STSTransitionController GetStandByParams()");
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

            return tTransitionStandByScript;
        }
        //private int m_DrawDepth = -1000;
        //-------------------------------------------------------------------------------------------------------------
        private bool AnimationProgress()
        {
            return EffectType.AnimIsPlaying;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool AnimationFinished()
        {
            return EffectType.AnimIsFinished;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AnimationTransitionIn(STSTransition sThisSceneParameters)
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
            EffectType.StartEffectEnter(new Rect(0, 0, Screen.width, Screen.height), tOldColor, tInterlude);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AnimationTransitionOut(STSTransition sThisSceneParameters)
        {
            EffectType = sThisSceneParameters.EffectOnExit.GetEffect();
            if (EffectType == null)
            {
                EffectType = new STSEffectFade();
            }
            EffectType.StartEffectExit(new Rect(0, 0, Screen.width, Screen.height));
        }
        //-------------------------------------------------------------------------------------------------------------
        private void StandBy()
        {
            StandByTimer = 0.0f;
            StandByInProgress = true;
            LauchNextScene = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool StandByIsProgressing(STSIntermission sIntermissionSceneStandBy)
        {
            StandByTimer += Time.deltaTime;
            if (StandByTimer >= sIntermissionSceneStandBy.StandBySeconds)
            {
                StandByInProgress = false;
            }
            return StandByInProgress;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool WaitingToLauchNextScene(STSIntermission sIntermissionSceneStandBy)
        {
            if (sIntermissionSceneStandBy.AutoLoadNextScene == true)
            {
                LauchNextScene = true;
            }
            return !LauchNextScene;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishStandBy()
        {
            LauchNextScene = true;
            StandByInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnGUI()
        {
            if (EffectType != null)
            {
                EffectType.DrawMaster(new Rect(0, 0, Screen.width, Screen.height));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionEnterStart(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionEnterFinish(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionExitStart(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionExitFinish(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionSceneLoaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionSceneEnable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionSceneDisable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnTransitionSceneWillUnloaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnLoadNextSceneStart(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnLoadingNextScenePercent(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnLoadNextSceneFinish(STSTransitionData sData, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnStandByStart(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnStandByFinish(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================