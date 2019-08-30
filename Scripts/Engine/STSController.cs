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
    public partial class STSController : MonoBehaviour /*, STSTransitionInterface, STSIntermediateInterface*/
    {
        //-------------------------------------------------------------------------------------------------------------
        private static STSController kSingleton = null;

        // initialized or not?
        private bool Initialized = false;

        // prevent multi transition
        private bool TransitionInProgress = false;

        // prevent user actions during the transition
        private bool PreventUserInteractions = true;

        // Data to transmit to the other scenes
        private STSTransitionData TransitionData;

        // Old scene params
        private STSTransition OldSceneParams;
        private STSIntermediate OldStandByParams;

        // Previous scene
        private static List<Dictionary<string, object>> PreviousScene = new List<Dictionary<string, object>>();

        // Scenes controlled
        private Scene PreviewScene;
        private STSTransition PreviewSceneParams;
        List<STSTransitionInterface> ListPreviewScene;

        private Scene IntermediateScene;
        private string IntermediateSceneName;
        private STSTransition IntermediateSceneParams;
        private STSIntermediate IntermediateSceneStandBy;
        List<STSTransitionInterface> ListIntermediate;
        List<STSIntermediateInterface> ListIntermediateStandBy;

        private Scene NextScene;
        private string NextSceneName;
        private STSTransition NextSceneParams;
        List<STSTransitionInterface> ListNextScene;

        // Scene load mode
        private LoadSceneMode LoadSceneModeSelected;

        private Scene SceneToUnload;
        List<STSTransitionInterface> ListSceneToUnload;
        //private STSTransitionParameters m_SceneToUnloadParams;

        //-------------------------------------------------------------------------------------------------------------
        public STSEffectType DefaultEffectOnEnter;
        public STSEffectType DefaultEffectOnExit;
        private STSEffect EffectType;
        //-------------------------------------------------------------------------------------------------------------
        private float StandByTimer;
        private bool LauchNextScene = false;
        private bool StandByInProgress = false;
        ////-------------------------------------------------------------------------------------------------------------
        //private List<STSTransitionInterface> TransitionList = new List<STSTransitionInterface>();
        //private List<STSIntermediateInterface> StandByList = new List<STSIntermediateInterface>();

        //private List<STSTransitionInterface> OldTransitionList = new List<STSTransitionInterface>();
        //private List<STSIntermediateInterface> OldStandByList = new List<STSIntermediateInterface>();
        //-------------------------------------------------------------------------------------------------------------
        //public void AddTransitionCallBack(STSTransitionInterface sObject)
        //{
        //    if (TransitionList.Contains(sObject) == false)
        //    {
        //        TransitionList.Add(sObject);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void RemoveTransitionCallBack(STSTransitionInterface sObject)
        //{
        //    if (TransitionList.Contains(sObject) == true)
        //    {
        //        TransitionList.Remove(sObject);
        //    }
        //    if (OldTransitionList.Contains(sObject) == true)
        //    {
        //        OldTransitionList.Remove(sObject);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void AddStandByCallBack(STSIntermediateInterface sObject)
        //{
        //    if (StandByList.Contains(sObject) == false)
        //    {
        //        StandByList.Add(sObject);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void RemoveStandByCallBack(STSIntermediateInterface sObject)
        //{
        //    if (StandByList.Contains(sObject) == true)
        //    {
        //        StandByList.Remove(sObject);
        //    }
        //    if (OldStandByList.Contains(sObject) == true)
        //    {
        //        OldStandByList.Remove(sObject);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        // Class method
        public static void LoadScene(string sNextSceneName,
            LoadSceneMode sLoadSceneMode = LoadSceneMode.Single,
            string sIntermediateSceneName = null,
            STSTransitionData sPayload = null)
        {
            Singleton();
            kSingleton.LoadSceneByNameMethod(sNextSceneName, sLoadSceneMode, sIntermediateSceneName, sPayload);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadPreviousScene(int sStackPayload = 2)
        {
            if (PreviousScene.Count == 0)
            {
                return;
            }

            Dictionary<string, object> tParams = PreviousScene[PreviousScene.Count - 1];
            object result;

            // Get previous scene
            tParams.TryGetValue(STSConstants.K_SCENE_NAME_KEY, out result);
            string tPreviewSceneName = result.ToString();

            // Get lscene oad mode
            tParams.TryGetValue(STSConstants.K_LOAD_MODE_KEY, out result);
            LoadSceneMode tLoadMode = (LoadSceneMode)result;

            // Get intermediate scene
            string tIntermediateSceneName = null;
            if (tParams.TryGetValue(STSConstants.K_INTERMEDIATE_SCENE_NAME_KEY, out result) && result != null)
            {
                tIntermediateSceneName = result.ToString();
            }

            // Get Payload Data from PreviousScene - 2
            STSTransitionData tPayLoadData = null;
            int tIndex = PreviousScene.Count - sStackPayload;
            if (tIndex >= 0)
            {
                tParams = PreviousScene[tIndex];
                if (tParams.TryGetValue(STSConstants.K_PAYLOAD_DATA_KEY, out result) && result != null)
                {
                    tPayLoadData = result as STSTransitionData;
                }
            }

            Singleton();
            kSingleton.LoadSceneByNameMethod(tPreviewSceneName, tLoadMode, tIntermediateSceneName, tPayLoadData, true);

            PreviousScene.RemoveAt(PreviousScene.Count - 1);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UnloadScene(string sSceneName, string sNextSceneName, string sIntermediateSceneName = null)
        {
            Singleton();
            kSingleton.UnloadSceneByNameMethod(sSceneName, sIntermediateSceneName, sNextSceneName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UnloadSceneNotActive(string sSceneNotActiveName)
        {
            Singleton();
            kSingleton.UnloadSceneNotActiveByNameMethod(sSceneNotActiveName);
        }
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
                tObjToSpawn.AddComponent<STSIntermediate>();

            }
            return kSingleton;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetHistory()
        {
            PreviousScene.Clear();
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
                ;
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
        // Instance method
        private void UnloadSceneByNameMethod(string sSceneName, string sIntermediateSceneName, string sNextSceneName)
        {
            //Debug.Log("STSTransitionController UnloadSceneByNameMethod()");
            if (TransitionInProgress == false)
            {
                TransitionInProgress = true;
                PreviewScene = SceneManager.GetSceneByName(sSceneName);
                //m_PreviewSceneName = sSceneName;
                IntermediateSceneName = sIntermediateSceneName;
                NextSceneName = sNextSceneName;

                if (SceneManager.GetSceneByName(sSceneName).isLoaded == true && SceneManager.GetSceneByName(sNextSceneName).isLoaded == true)
                {
                    // Ok I can unload the scene normally
                    LoadSceneByNameMethod(sNextSceneName, LoadSceneMode.Single, sIntermediateSceneName, null);
                }
                else if (SceneManager.GetSceneByName(sSceneName).isLoaded == false && SceneManager.GetSceneByName(sNextSceneName).isLoaded == true)
                {
                    // The scene doesn't exist ... I cannot unload this Scene :-p
                    /*WARNING*/
                    Debug.LogWarning("The Scene '" + sSceneName + "' isn't loaded! Not possible to unload!");
                }
                else if (SceneManager.GetSceneByName(sSceneName).isLoaded == true && SceneManager.GetSceneByName(sNextSceneName).isLoaded == false)
                {
                    // The scene to active doesn't exist ... I cannot active this Scene :-p
                    /*WARNING*/
                    Debug.LogWarning("The Next Scene '" + sNextSceneName + "' isn't loaded! Not possible to active!");
                    // Perhaps I need to load the next scene ?
                    // but I have not the LoadSceneMode (single or additive)
                }
                else if (SceneManager.GetSceneByName(sSceneName).isLoaded == false && SceneManager.GetSceneByName(sNextSceneName).isLoaded == false)
                {
                    // The scene doesn't exist ... I cannot unload this Scene :-p
                    // The scene to active doesn't exist ... I cannot active this Scene :-p
                    /*WARNING*/
                    Debug.LogWarning("The Scene '" + sSceneName + "' and Next Scene '" + sNextSceneName + "' are not loaded! Not possible to unload or/and active!");
                    // Perhaps I need to load the next scene ?
                    // but I have not the LoadSceneMode (single or additive)
                }
            }
            else
            {
                /*WARNING*/
                Debug.LogWarning("Transition allready in progress …");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UnloadSceneNotActiveByNameMethod(string sSceneName)
        {
            //Debug.Log("STSTransitionController UnloadSceneNotActiveByNameMethod()");
            if (TransitionInProgress == false)
            {
                TransitionInProgress = true;
                PreviewScene = SceneManager.GetActiveScene();

                if (SceneManager.GetSceneByName(sSceneName).isLoaded == true && SceneManager.GetActiveScene().name != sSceneName)
                {
                    // Ok I can unload the scene normally
                    SceneToUnload = SceneManager.GetSceneByName(sSceneName);
                    StartCoroutine(UnloadSceneAsync());
                }
                else if (SceneManager.GetSceneByName(sSceneName).isLoaded == true && SceneManager.GetActiveScene().name == sSceneName)
                {
                    // The scene exist but it's the active... I cannot unload this Scene :-p
                    /*WARNING*/
                    Debug.LogWarning("The Scene '" + sSceneName + "' is active! use method : public static void UnloadSceneByName (string sSceneName, string sTransitionSceneName, string sNextSceneName)");
                }
                else if (SceneManager.GetSceneByName(sSceneName).isLoaded == false)
                {
                    // The scene to unload doesn't exist ... I cannot unload this Scene :-p
                    /*WARNING*/
                    Debug.LogWarning("The Scene '" + sSceneName + "' is not loaded!");
                }
            }
            else
            {
                /*WARNING*/
                Debug.LogWarning("Transition allready in progress …");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void LoadSceneByNameMethod(string sNextSceneName, LoadSceneMode sLoadSceneMode, string sIntermediateSceneName, STSTransitionData sPayload, bool sLoadPreviousScene = false)
        {
            if (SceneManager.GetActiveScene().name != sNextSceneName)
            {
                if (TransitionInProgress == false)
                {
                    TransitionInProgress = true;

                    //List<STSTransitionInterface> tTransitionList = TransitionList;
                    //List<STSIntermediateInterface> tStandByList = StandByList;

                    //List<STSTransitionInterface> tOldTransitionList = OldTransitionList;
                    //List<STSIntermediateInterface> tOldStandByList = OldStandByList;

                    //TransitionList = tOldTransitionList;
                    //StandByList = tOldStandByList;

                    //OldTransitionList = tTransitionList;
                    //OldStandByList = tStandByList;

                    //TransitionList.Clear();
                    //StandByList.Clear();

                    // Save scene param for further use if not a previous scene
                    if (sLoadPreviousScene == false)
                    {
                        Dictionary<string, object> tParams = new Dictionary<string, object>
                        {
                            { STSConstants.K_SCENE_NAME_KEY, SceneManager.GetActiveScene().name },
                            { STSConstants.K_LOAD_MODE_KEY, sLoadSceneMode },
                            { STSConstants.K_INTERMEDIATE_SCENE_NAME_KEY, sIntermediateSceneName },
                            { STSConstants.K_PAYLOAD_DATA_KEY, sPayload }
                        };
                        PreviousScene.Add(tParams);
                    }

                    // memorize actual scene
                    PreviewScene = SceneManager.GetActiveScene();

                    IntermediateSceneName = sIntermediateSceneName;

                    TransitionData = sPayload;
                    if (TransitionData == null)
                    {
                        TransitionData = new STSTransitionData();
                    }

                    NextSceneName = sNextSceneName;
                    LoadSceneModeSelected = sLoadSceneMode;
                    StartCoroutine(LoadSceneByNameAsync());
                }
                else
                {
                    /*WARNING*/
                    Debug.LogWarning("Transition allready in progress …");
                }
            }
            else
            {
                /*WARNING*/
                Debug.LogWarning("Transition not necessary : active Scene is the Request Scene");
            }
        }
        //-------------------------------------------------------------------------------------------------------------	
        // Async method
        private IEnumerator UnloadSceneAsync()
        {
            //Debug.Log("STSTransitionController UnloadSceneAsync()");
            STSTransition tTransitionParametersScript = GetTransitionsParams(SceneToUnload, true);

            ListSceneToUnload = GetTransitionInterface(SceneToUnload);
            // disable the user interactions
            EventSystemEnable(SceneToUnload, false);
            foreach (STSTransitionInterface tParameters in ListSceneToUnload)
            {
                tParameters.OnTransitionSceneDisable(null);
            }
            if (tTransitionParametersScript.Interfaced != null)
            {
                tTransitionParametersScript.Interfaced.OnTransitionSceneDisable(null);
            }
            EventSystemEnable(SceneManager.GetActiveScene(), false);
            foreach (STSTransitionInterface tParameters in ListSceneToUnload)
            {
                tParameters.OnTransitionSceneWillUnloaded(null);
            }
            if (tTransitionParametersScript.Interfaced != null)
            {
                tTransitionParametersScript.Interfaced.OnTransitionSceneWillUnloaded(null);
            }
            AsyncOperation tAsynchroneUnload;
            tAsynchroneUnload = SceneManager.UnloadSceneAsync(SceneToUnload.name);
            while (!tAsynchroneUnload.isDone)
            {
                yield return null;
            }
            // enable the user interactions
            EventSystemEnable(SceneManager.GetActiveScene(), true);
        }
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator LoadSceneByNameAsync()
        {
            //Debug.Log("STSTransitionController LoadSceneByNameAsync()");
            // prepare future old params
            OldSceneParams = this.GetComponent<STSTransition>();
            OldStandByParams = this.GetComponent<STSIntermediate>();

            //-------------------------------
            // PREVIEW SCENE PROCESS
            //-------------------------------
            // get params
            PreviewSceneParams = GetTransitionsParams(PreviewScene, true);
            PreviewSceneParams.CopyIn(OldSceneParams);
            ListPreviewScene = GetTransitionInterface(PreviewScene);
            Debug.Log("ListPreviewScene count =" + ListPreviewScene.Count);
            // disable the user interactions
            EventSystemPrevent(false);
            if (PreviewSceneParams.Interfaced != null)
            {
                PreviewSceneParams.Interfaced.OnTransitionSceneDisable(TransitionData);
            }
            foreach (STSTransitionInterface tParameters in ListPreviewScene)
            {
                tParameters.OnTransitionSceneDisable(TransitionData);
            }
            // Transition Out will Start
            if (PreviewSceneParams.Interfaced != null)
            {
                // calcul estimated second
                PreviewSceneParams.Interfaced.OnTransitionExitStart(TransitionData);
            }
            foreach (STSTransitionInterface tParameters in ListPreviewScene)
            {
                tParameters.OnTransitionExitStart(TransitionData);
            }
            // Transition Out GO!
            AnimationTransitionOut(PreviewSceneParams);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // Transition Out Finish
            if (PreviewSceneParams.Interfaced != null)
            {
                PreviewSceneParams.Interfaced.OnTransitionExitFinish(TransitionData);
            }
            foreach (STSTransitionInterface tParameters in ListPreviewScene)
            {
                tParameters.OnTransitionExitFinish(TransitionData);
            }
            // Transition setp 1 is finished this scene can be replace by the next or intermediate Scene
            //-------------------------------
            // PREVIEW SCENE IS OUT
            //-------------------------------
            // INTERMEDIATE SCENE PROCESS
            //-------------------------------

            PreviewSceneParams.CopyIn(OldSceneParams);

            if (IntermediateSceneName != null)
            {
                //-------------------------------

                // actual scene must be persistent ?
                LoadSceneMode tTransitionSceneMode = LoadSceneModeSelected;
                // load transition scene async
                AsyncOperation tAsynchroneLoadIntermediateOperation;
                tAsynchroneLoadIntermediateOperation = SceneManager.LoadSceneAsync(IntermediateSceneName, tTransitionSceneMode);
                tAsynchroneLoadIntermediateOperation.allowSceneActivation = false;
                while (tAsynchroneLoadIntermediateOperation.progress < 0.9f)
                {
                    yield return null;
                }
                // Intermediate scene will be active
                tAsynchroneLoadIntermediateOperation.allowSceneActivation = true;
                while (!tAsynchroneLoadIntermediateOperation.isDone)
                {
                    yield return null;
                }
                // get Transition Scene
                IntermediateScene = SceneManager.GetSceneByName(IntermediateSceneName);
                // Active the next scene as root scene 
                SceneManager.SetActiveScene(IntermediateScene);
                // disable audiolistener of preview scene
                AudioListenerPrevent();
                // remove preview scene 
                if (LoadSceneModeSelected == LoadSceneMode.Single)
                {
                    // if m_SceneActual is allready loaded i need to unloaded it
                    if (SceneManager.GetSceneByName(PreviewScene.name).isLoaded)
                    {
                        AsyncOperation tAsynchroneUnloadActualScene;
                        tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync(PreviewScene.name);
                        while (tAsynchroneUnloadActualScene.progress < 0.9f)
                        {
                            yield return null;
                        }
                        if (PreviewSceneParams.Interfaced != null)
                        {
                            PreviewSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                        }
                        foreach (STSTransitionInterface tParameters in ListPreviewScene)
                        {
                            tParameters.OnTransitionSceneWillUnloaded(TransitionData);
                        }
                        while (!tAsynchroneUnloadActualScene.isDone)
                        {
                            yield return null;
                        }
                    }
                }

                // get params
                IntermediateSceneParams = GetTransitionsParams(IntermediateScene, false);
                // disable the user interactions until fadein 
                EventSystemEnable(IntermediateScene, false);
                ListIntermediate = GetTransitionInterface(IntermediateScene);
                Debug.Log("ListIntermediate count =" + ListIntermediate.Count);
                // intermediate scene is loaded
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionSceneLoaded(TransitionData);
                }
                foreach (STSTransitionInterface tParameters in ListIntermediate)
                {
                    tParameters.OnTransitionSceneLoaded(TransitionData);
                }
                // animation in
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
                }
                foreach (STSTransitionInterface tParameters in ListIntermediate)
                {
                    tParameters.OnTransitionEnterStart(TransitionData);
                }
                // animation in Go!
                AnimationTransitionIn(IntermediateSceneParams);
                while (AnimationFinished() == false)
                {
                    yield return null;
                }
                // animation in Finish
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionEnterFinish(TransitionData);
                }
                foreach (STSTransitionInterface tParameters in ListIntermediate)
                {
                    tParameters.OnTransitionEnterFinish(TransitionData);
                }
                // enable the user interactions 
                EventSystemEnable(IntermediateScene, true);
                // enable the user interactions 
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionSceneEnable(TransitionData);
                }
                foreach (STSTransitionInterface tParameters in ListIntermediate)
                {
                    tParameters.OnTransitionSceneEnable(TransitionData);
                }
                // start stand by
                IntermediateSceneStandBy = GetStandByParams(IntermediateScene);
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnStandByStart(IntermediateSceneStandBy);
                }
                foreach (STSIntermediateInterface tParameters in ListIntermediate)
                {
                    tParameters.OnStandByStart(IntermediateSceneStandBy);
                }
                StandBy();
                // and load next scene async 
                //-------------------------------
                // INTERMEDIATE SCENE IS IN PLACE
                //-------------------------------
            }
            else
            {
                // So! no transition! then the intermediate params are the preview scene params
                IntermediateSceneParams = GetTransitionsParams(PreviewScene, true);
                // And the StandBy params are the preview scene StandBy params
                IntermediateSceneStandBy = GetStandByParams(PreviewScene);
                ListIntermediateStandBy = GetIntermediateInterface(PreviewScene);
                //if (IntermediateSceneStandBy.StandByStart != null) {
                //                IntermediateSceneStandBy.StandByStart.Invoke (IntermediateSceneStandBy);
                //}
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnStandByStart(IntermediateSceneStandBy);
                }
                foreach (STSIntermediateInterface tParameters in ListIntermediateStandBy)
                {
                    tParameters.OnStandByStart(IntermediateSceneStandBy);
                }
                StandBy();
                // and load next scene async 
                //-------------------------------
                // NO INTERMEDIATE SCENE IS IN PLACE, I USE THE PREVIEW SCENE
                //-------------------------------
            }


            IntermediateSceneParams.CopyIn(OldSceneParams);
            IntermediateSceneStandBy.CopyIn(OldStandByParams);

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            // load Next Scene
            // actual scene must be persistent ?
            LoadSceneMode tNextSceneMode = LoadSceneModeSelected;
            bool tNextSceneAllRedayExist = false;
            // If scene with this name allready exist I use the old instance
            if (SceneManager.GetSceneByName(NextSceneName).isLoaded)
            {
                // active next scene basically 
                tNextSceneAllRedayExist = true;

            }

            if (tNextSceneAllRedayExist == false)
            {
                //-------------------------------
                // NEXT SCENE NEED TO BE LOADING
                //-------------------------------
                // load next scene async
                ListIntermediateStandBy = GetIntermediateInterface(IntermediateScene);
                Debug.Log("ListIntermediateStandBy count =" + ListIntermediateStandBy.Count);
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnLoadNextSceneStart(TransitionData, 0.0F);
                }
                foreach (STSIntermediateInterface tParameters in ListIntermediateStandBy)
                {
                    tParameters.OnLoadNextSceneStart(TransitionData, 0.0F);
                }
                //if (IntermediateSceneStandBy.SceneLoadingGauge != null)
                //{
                //    IntermediateSceneStandBy.SceneLoadingGauge.SetVerticalValue(0.0F);
                //    IntermediateSceneStandBy.SceneLoadingGauge.SetHorizontalValue(0.0F);
                //}
                AsyncOperation tAsynchroneloadNext;
                tAsynchroneloadNext = SceneManager.LoadSceneAsync(NextSceneName, tNextSceneMode);
                tAsynchroneloadNext.allowSceneActivation = false;
                // load next scene async can send percent :-)
                while (tAsynchroneloadNext.progress < 0.9f)
                {
                    if (IntermediateSceneStandBy.Interfaced != null)
                    {
                        IntermediateSceneStandBy.Interfaced.OnLoadingNextScenePercent(TransitionData, tAsynchroneloadNext.progress);

                    }
                    foreach (STSIntermediateInterface tParameters in ListIntermediateStandBy)
                    {
                        tParameters.OnLoadingNextScenePercent(TransitionData, tAsynchroneloadNext.progress);
                    }
                    //if (IntermediateSceneStandBy.SceneLoadingGauge != null)
                    //{
                    //    IntermediateSceneStandBy.SceneLoadingGauge.SetVerticalValue(tAsynchroneloadNext.progress);
                    //    IntermediateSceneStandBy.SceneLoadingGauge.SetHorizontalValue(tAsynchroneloadNext.progress);
                    //}
                    yield return null;
                }

                // need to send call back now (anticipate :-/ ) 
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnLoadNextSceneFinish(TransitionData, 1.0f);
                }
                foreach (STSIntermediateInterface tParameters in ListIntermediateStandBy)
                {
                    tParameters.OnLoadingNextScenePercent(TransitionData, 1.0F);
                }
                //if (IntermediateSceneStandBy.SceneLoadingGauge != null)
                //{
                //    IntermediateSceneStandBy.SceneLoadingGauge.SetVerticalValue(1.0F);
                //    IntermediateSceneStandBy.SceneLoadingGauge.SetHorizontalValue(1.0F);
                //}

                // when finish test if transition scene is allways in standby
                if (IntermediateSceneName != null)
                {

                    //-------------------------------
                    // INTERMEDIATE SCENE UNLOAD
                    //-------------------------------
                    EventSystemEnable(IntermediateScene, true);
                    // continue standby if it's necessary
                    while (StandByIsProgressing())
                    {
                        yield return null;
                    }
                    // As sson as possible 
                    if (IntermediateSceneStandBy.Interfaced != null)
                    {
                        IntermediateSceneStandBy.Interfaced.OnStandByFinish(IntermediateSceneStandBy);
                    }
                    foreach (STSIntermediateInterface tParameters in ListIntermediateStandBy)
                    {
                        tParameters.OnStandByFinish(IntermediateSceneStandBy);
                    }
                    // Waiting to load the next Scene
                    while (WaitingToLauchNextScene())
                    {
                        //Debug.Log ("StandByIsNotFinished loop");
                        yield return null;
                    }
                    // stanby is finished And the next scene can be lauch
                    // disable user interactions on the intermediate scene
                    EventSystemEnable(IntermediateScene, false);
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionSceneDisable(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionSceneDisable(TransitionData);
                    }
                    // intermediate scene Transition Out start 
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionEnterStart(TransitionData);
                    }
                    // intermediate scene Transition Out GO! 
                    AnimationTransitionOut(IntermediateSceneParams);
                    while (AnimationFinished() == false)
                    {
                        yield return null;
                    }
                    // intermediate scene Transition Out finished! 
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionExitFinish(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionExitFinish(TransitionData);
                    }
                    // fadeout is finish
                    // will unloaded the intermediate scene
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                }
                tAsynchroneloadNext.allowSceneActivation = true;
                while (!tAsynchroneloadNext.isDone)
                {
                    yield return null;
                }
            }
            else
            {

                //-------------------------------
                // NEXT SCENE ALLREADY LOADED
                //-------------------------------
                NextScene = SceneManager.GetSceneByName(NextSceneName);
                NextSceneParams = GetTransitionsParams(NextScene, false);
                ListNextScene = GetTransitionInterface(NextScene);
                Debug.Log("ListNextScene count =" + ListNextScene.Count);
                // when finish test if transition scene is allways in standby
                if (IntermediateSceneName != null)
                {
                    //-------------------------------
                    // INTERMEDIATE SCENE UNLOAD
                    //-------------------------------
                    // continue standby if it's necessary
                    EventSystemEnable(IntermediateScene, true);

                    while (StandByIsProgressing())
                    {
                        yield return null;
                    }
                    // send call back for standby finished
                    if (IntermediateSceneStandBy.Interfaced != null)
                    {
                        IntermediateSceneStandBy.Interfaced.OnStandByFinish(IntermediateSceneStandBy);
                    }
                    foreach (STSIntermediateInterface tParameters in ListIntermediateStandBy)
                    {
                        tParameters.OnStandByFinish(IntermediateSceneStandBy);
                    }
                    // Waiting to load the next Scene
                    while (WaitingToLauchNextScene())
                    {
                        //Debug.Log ("StandByIsNotFinished loop");
                        yield return null;
                    }
                    // stanby is finish
                    // disable user interactions on the transition scene
                    EventSystemEnable(IntermediateScene, false);

                    // disable the transition scene
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionSceneDisable(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionSceneDisable(TransitionData);
                    }
                    // Transition scene disappear by fadeout 
                    AnimationTransitionOut(IntermediateSceneParams);
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionExitStart(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionExitStart(TransitionData);
                    }
                    while (AnimationFinished() == false)
                    {
                        yield return null;
                    }
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionExitFinish(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionExitFinish(TransitionData);
                    }
                    // fadeout is finish
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListIntermediate)
                    {
                        tParameters.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                }
            }
            NextScene = SceneManager.GetSceneByName(NextSceneName);
            // unload Intermediate Scene anyway
            if (LoadSceneModeSelected == LoadSceneMode.Additive && IntermediateSceneName != null)
            {
                AsyncOperation tAsynchroneUnloadTransition;
                tAsynchroneUnloadTransition = SceneManager.UnloadSceneAsync(IntermediateSceneName);

                while (tAsynchroneUnloadTransition.progress < 0.9f)
                {
                    yield return null;
                }

                while (!tAsynchroneUnloadTransition.isDone)
                {
                    yield return null;
                }
            }
            //remove preview scene (if not remove before)
            if (LoadSceneModeSelected == LoadSceneMode.Single)
            {
                // if m_SceneActual is allready loaded i need to unloaded it
                if (SceneManager.GetSceneByName(PreviewScene.name).isLoaded)
                {
                    AsyncOperation tAsynchroneUnloadActualScene;
                    tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync(PreviewScene.name);
                    if (PreviewSceneParams.Interfaced != null)
                    {
                        PreviewSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                    foreach (STSTransitionInterface tParameters in ListPreviewScene)
                    {
                        tParameters.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                    while (tAsynchroneUnloadActualScene.progress < 0.9f)
                    {
                        yield return null;
                    }

                    while (!tAsynchroneUnloadActualScene.isDone)
                    {
                        yield return null;
                    }
                }
                else
                {
                    // Ok this scene allready unloading (perhaps in the intermediate scene)
                }
            }
            // Active the next scene as root scene 
            SceneManager.SetActiveScene(NextScene);
            AudioListenerPrevent();
            // get params
            NextSceneParams = GetTransitionsParams(NextScene, false);
            ListNextScene = GetTransitionInterface(NextScene);
            Debug.Log("ListNextScene count =" + ListNextScene.Count);
            //List<STSTransitionInterface> tListNextScene = GetTransitionInterface(NextScene);
            // disable user interactions on the next scene
            EventSystemEnable(NextScene, false);
            // scene is loaded
            if (tNextSceneAllRedayExist == false)
            {
                if (NextSceneParams.Interfaced != null)
                {
                    NextSceneParams.Interfaced.OnTransitionSceneLoaded(TransitionData);
                }
                foreach (STSTransitionInterface tParameters in ListNextScene)
                {
                    Debug.Log("try OnTransitionSceneLoaded");
                    tParameters.OnTransitionSceneLoaded(TransitionData);
                }
            }
            // Next scene appear by fade in 
            AnimationTransitionIn(NextSceneParams);
            if (NextSceneParams.Interfaced != null)
            {
                NextSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
            }
            foreach (STSTransitionInterface tParameters in ListNextScene)
            {
                tParameters.OnTransitionEnterStart(TransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (NextSceneParams.Interfaced != null)
            {
                NextSceneParams.Interfaced.OnTransitionEnterFinish(TransitionData);
            }
            foreach (STSTransitionInterface tParameters in ListNextScene)
            {
                tParameters.OnTransitionEnterFinish(TransitionData);
            }
            // fadein is finish
            // next scene user interaction enable
            EventSystemPrevent(true);
            // next scene is enable
            if (NextSceneParams.Interfaced != null)
            {
                NextSceneParams.Interfaced.OnTransitionSceneEnable(TransitionData);
            }
            foreach (STSTransitionInterface tParameters in ListNextScene)
            {
                tParameters.OnTransitionSceneEnable(TransitionData);
            }

            NextSceneParams.CopyIn(OldSceneParams);

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        // toolbox method

        private void AudioListenerPrevent()
        {
            //Debug.Log("STSTransitionController AudioListenerPrevent()");
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
        private List<STSTransitionInterface> GetTransitionInterface(Scene sScene)
        {
            List<STSTransitionInterface> rReturn = new List<STSTransitionInterface>();
            // if (string.IsNullOrEmpty(sScene.name) == false)
            {
                GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
                foreach (GameObject tObject in tAllRootObjects)
                {
                    STSTransitionInterface tT = tObject.GetComponent<STSTransitionInterface>();
                    if (tT!=null)
                    {
                        Debug.Log("GetTransitionInterface add this ");
                       // rReturn.Add(tT);
                    }
                    //foreach (STSTransitionInterface tR in GetComponentsInChildren<STSTransitionInterface>())
                    //{
                    //    Debug.Log("GetTransitionInterface add one ");
                    //    rReturn.Add(tR);
                    //}
                }
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        private List<STSIntermediateInterface> GetIntermediateInterface(Scene sScene)
        {
            List<STSIntermediateInterface> rReturn = new List<STSIntermediateInterface>();
            // if (string.IsNullOrEmpty(sScene.name) == false)
            {
                GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
                foreach (GameObject tObject in tAllRootObjects)
                {
                    STSIntermediateInterface tT = tObject.GetComponent<STSIntermediateInterface>();
                    if (tT!=null)
                    {
                        Debug.Log("GetIntermediateInterface add this ");
                       // rReturn.Add(tT);
                    }
                    //foreach (STSIntermediateInterface tR in GetComponentsInChildren<STSIntermediateInterface>())
                    //{
                    //    Debug.Log("GetIntermediateInterface add one ");
                    //    rReturn.Add(tR);
                    //}
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private STSTransition GetTransitionsParams(Scene sScene, bool sStartTransition)
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
                // create Game Object?
                //Debug.Log ("NO PARAMS");
                GameObject tObjToSpawn = new GameObject(STSConstants.K_TRANSITION_CONTROLLER_OBJECT_NAME);
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
            }
            return tTransitionParametersScript;
        }
        //-------------------------------------------------------------------------------------------------------------
        private STSIntermediate GetStandByParams(Scene sScene)
        {
            //Debug.Log("STSTransitionController GetStandByParams()");
            STSIntermediate tTransitionStandByScript = null;
            GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
            // quick solution?!
            foreach (GameObject tObject in tAllRootObjects)
            {
                if (tObject.GetComponent<STSIntermediate>() != null)
                {
                    tTransitionStandByScript = tObject.GetComponent<STSIntermediate>();
                    break;
                }
            }
            // slower solution?!
            if (tTransitionStandByScript == null)
            {
                foreach (GameObject tObject in tAllRootObjects)
                {
                    if (tObject.GetComponentInChildren<STSIntermediate>() != null)
                    {
                        tTransitionStandByScript = tObject.GetComponent<STSIntermediate>();
                        break;
                    }
                }
            }
            // no solution?!
            if (tTransitionStandByScript == null)
            {
                GameObject tObjToSpawn = new GameObject(STSConstants.K_TRANSITION_CONTROLLER_OBJECT_NAME);
                tObjToSpawn.AddComponent<STSIntermediate>();
                tTransitionStandByScript = (STSIntermediate)tObjToSpawn.GetComponent<STSIntermediate>();
                tTransitionStandByScript.StandBySeconds = 5.0f;
                tTransitionStandByScript.AutoLoadNextScene = true;
            }

            return tTransitionStandByScript;
        }
        //private int m_DrawDepth = -1000;
        //-------------------------------------------------------------------------------------------------------------
        private bool AnimationProgress()
        {
            //Debug.Log("STSTransitionController AnimationProgress()");
            //return m_AnimationInProgress;
            return EffectType.AnimIsPlaying;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool AnimationFinished()
        {
            //Debug.Log("STSTransitionController AnimationFinished()");
            //return m_AnimationFinish;
            return EffectType.AnimIsFinished;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AnimationTransitionIn(STSTransition sThisSceneParameters)
        {
            //Debug.Log("STSTransitionController AnimationTransitionIn()");
            Color tOldColor = Color.black;
            float tInterlude = 0;
            if (EffectType != null)
            {
                // I get the old value of 
                tOldColor = EffectType.TintPrimary;
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
            //Debug.Log("STSTransitionController AnimationTransitionOut()");
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
            //Debug.Log("STSTransitionController StandBy()");
            StandByTimer = 0.0f;
            StandByInProgress = true;
            LauchNextScene = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool StandByIsProgressing()
        {
            //Debug.Log("STSTransitionController StandByIsProgressing()");
            StandByTimer += Time.deltaTime;
            if (StandByTimer >= IntermediateSceneStandBy.StandBySeconds)
            {
                StandByInProgress = false;
            }
            return StandByInProgress;
        }
        //-------------------------------------------------------------------------------------------------------------
        //private bool StandByIsFinished ()
        //     {
        //         Debug.Log("STSTransitionController StandByIsFinished()");
        //return m_LauchNextScene;
        //}
        //-------------------------------------------------------------------------------------------------------------
        private bool WaitingToLauchNextScene()
        {
            //Debug.Log("STSTransitionController WaitingToLauchNextScene()");
            if (IntermediateSceneStandBy.AutoLoadNextScene == true)
            {
                // force auto active scene
                LauchNextScene = true;
            }
            return !LauchNextScene;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishStandBy()
        {
            //Debug.Log("STSTransitionController FinishStandBy()");
            EventSystemEnable(kSingleton.IntermediateScene, false);
            LauchNextScene = true;
            StandByInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnGUI()
        {
            //Debug.Log("STSTransitionController OnGUI()");
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
            Debug.Log("jhjhkjkhjhk");
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
        public void OnStandByStart(STSIntermediate sStandBy)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnStandByFinish(STSIntermediate sStandBy)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================