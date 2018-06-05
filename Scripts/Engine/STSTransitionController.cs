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
    public class STSTransitionController : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        private static STSTransitionController kSingleton = null;

        // initialized or not?
        private bool Initialized = false;

		// prevent multi transition
        private bool TransitionInProgress = false;

		// prevent user actions during the transition
        private bool PreventUserInteractions = true;

		// Data to transmit to the other scenes
        private STSTransitionData TransitionData;

        // Old scene params
        private STSTransitionParameters OldSceneParams;
        private STSTransitionStandBy OldStandByParams;

        // Previous scene
        private static List<Dictionary<string, object>> PreviousScene = new List<Dictionary<string, object>>();

        // Scenes controlled
        private Scene PreviewScene;
        private STSTransitionParameters PreviewSceneParams;

        private Scene IntermediateScene;
        private string IntermediateSceneName;
        private STSTransitionParameters IntermediateSceneParams;
        private STSTransitionStandBy IntermediateSceneStandBy;
        
        private Scene NextScene;
        private string NextSceneName;
        private STSTransitionParameters NextSceneParams;

		// Scene load mode
        private LoadSceneMode LoadSceneModeSelected;

        private Scene SceneToUnload;
		//private STSTransitionParameters m_SceneToUnloadParams;

        //-------------------------------------------------------------------------------------------------------------
        private STSEffect EffectType;
        //-------------------------------------------------------------------------------------------------------------
        private float StandByTimer;
        private bool LauchNextScene = false;
        private bool StandByInProgress = false;
        //-------------------------------------------------------------------------------------------------------------
		// Class method
		public static void LoadScene(string sNextSceneName,
            LoadSceneMode sLoadSceneMode = LoadSceneMode.Single,
            string sIntermediateSceneName = "",
            STSTransitionData sPayload = null)
		{
			Singleton();
			kSingleton.LoadSceneByNameMethod (sNextSceneName, sLoadSceneMode, sIntermediateSceneName, sPayload);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadPreviousScene()
        {
            Dictionary<string, object> tParams = PreviousScene[PreviousScene.Count - 1];
            object result;
            tParams.TryGetValue(STSConstants.SceneNameKey, out result);
            string tPreviewSceneName = result.ToString();
            tParams.TryGetValue(STSConstants.LoadModeKey, out result);
            LoadSceneMode tLoadMode = (LoadSceneMode)result;
            tParams.TryGetValue(STSConstants.PayloadDataKey, out result);
            //STSTransitionData tPayLoadData = result as STSTransitionData;
            Singleton();
            kSingleton.LoadSceneByNameMethod(tPreviewSceneName, tLoadMode, null, null);
            PreviousScene.RemoveAt(PreviousScene.Count - 1);
        }
        //-------------------------------------------------------------------------------------------------------------
		public static void UnloadScene (string sSceneName, string sNextSceneName, string sIntermediateSceneName = null)
		{
			Singleton();
			kSingleton.UnloadSceneByNameMethod (sSceneName, sIntermediateSceneName, sNextSceneName);
        }
        //-------------------------------------------------------------------------------------------------------------
		public static void UnloadSceneNotActive (string sSceneNotActiveName)
		{
			Singleton();
			kSingleton.UnloadSceneNotActiveByNameMethod (sSceneNotActiveName);
        }
        //-------------------------------------------------------------------------------------------------------------
		// Singleton
		public static STSTransitionController Singleton ()
		{
            //Debug.Log ("STSTransitionController Singleton()");
			if (kSingleton == null) {
				// I need to create singleton
				GameObject tObjToSpawn;
				//spawn object
                tObjToSpawn = new GameObject (STSConstants.TransitionControllerObjectName);
				//Add Components
				tObjToSpawn.AddComponent<STSTransitionController> ();
				// keep k_Singleton
				kSingleton = tObjToSpawn.GetComponent<STSTransitionController> ();
				// Init Instance
				kSingleton.InitInstance ();
				// memorize the init instance
				kSingleton.Initialized = true;

				tObjToSpawn.AddComponent<STSTransitionParameters> ();
				tObjToSpawn.AddComponent<STSTransitionStandBy> ();

			}
			return kSingleton;
        }
        //-------------------------------------------------------------------------------------------------------------
		// Memory managment
		private void InitInstance ()
        {
            //Debug.Log("STSTransitionController InitInstance()");
        }
        //-------------------------------------------------------------------------------------------------------------
		//Awake is always called before any Start functions
		private void Awake ()
        {
            //Debug.Log("STSTransitionController Awake()");
			//Check if instance already exists
			if (kSingleton == null) {
				//if not, set instance to this
				kSingleton = this;
				if (kSingleton.Initialized == false) {
					kSingleton.InitInstance ();
					kSingleton.Initialized = true;
				}
				;
			}
		//If instance already exists and it's not this:
		else if (kSingleton != this) {
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy (gameObject);    
			}
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad (gameObject);
			//Call the InitGame function to initialize the first level 
        }
        //-------------------------------------------------------------------------------------------------------------
		private void OnDestroy ()
        {
            //Debug.Log("STSTransitionController OnDestroy()");
        }
        //-------------------------------------------------------------------------------------------------------------
		// Instance method
		private void UnloadSceneByNameMethod (string sSceneName, string sIntermediateSceneName, string sNextSceneName)
        {
            //Debug.Log("STSTransitionController UnloadSceneByNameMethod()");
			if (TransitionInProgress == false) 
			{
				TransitionInProgress = true;
				PreviewScene = SceneManager.GetSceneByName (sSceneName);
                //m_PreviewSceneName = sSceneName;
				IntermediateSceneName = sIntermediateSceneName;
				NextSceneName = sNextSceneName;

				if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetSceneByName (sNextSceneName).isLoaded == true) 
				{
					// Ok I can unload the scene normally
					LoadSceneByNameMethod (sNextSceneName, LoadSceneMode.Single, sIntermediateSceneName, null);
				} 
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == false && SceneManager.GetSceneByName (sNextSceneName).isLoaded == true) 
				{
					// The scene doesn't exist ... I cannot unload this Scene :-p
					/*WARNING*/
                    Debug.LogWarning ("The Scene '" + sSceneName + "' isn't loaded! Not possible to unload!");
				} 
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetSceneByName (sNextSceneName).isLoaded == false) 
				{
					// The scene to active doesn't exist ... I cannot active this Scene :-p
					/*WARNING*/
                    Debug.LogWarning ("The Next Scene '" + sNextSceneName + "' isn't loaded! Not possible to active!");
					// Perhaps I need to load the next scene ?
					// but I have not the LoadSceneMode (single or additive)
				} 
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == false && SceneManager.GetSceneByName (sNextSceneName).isLoaded == false) 
				{
					// The scene doesn't exist ... I cannot unload this Scene :-p
					// The scene to active doesn't exist ... I cannot active this Scene :-p
					/*WARNING*/
                    Debug.LogWarning ("The Scene '" + sSceneName + "' and Next Scene '" + sNextSceneName + "' are not loaded! Not possible to unload or/and active!");
					// Perhaps I need to load the next scene ?
					// but I have not the LoadSceneMode (single or additive)
				}
			} 
			else 
			{
				/*WARNING*/
                Debug.LogWarning ("Transition allready in progress …");
			}
        }
        //-------------------------------------------------------------------------------------------------------------
		private void UnloadSceneNotActiveByNameMethod (string sSceneName)
        {
            //Debug.Log("STSTransitionController UnloadSceneNotActiveByNameMethod()");
			if (TransitionInProgress == false)
			{
				TransitionInProgress = true;
				PreviewScene = SceneManager.GetActiveScene ();

				if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetActiveScene ().name != sSceneName)
				{
					// Ok I can unload the scene normally
					SceneToUnload = SceneManager.GetSceneByName (sSceneName);
					StartCoroutine (UnloadSceneAsync ());
				}
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetActiveScene ().name == sSceneName)
				{
					// The scene exist but it's the active... I cannot unload this Scene :-p
					/*WARNING*/
                    Debug.LogWarning ("The Scene '" + sSceneName + "' is active! use method : public static void UnloadSceneByName (string sSceneName, string sTransitionSceneName, string sNextSceneName)");
				}
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == false)
				{
					// The scene to unload doesn't exist ... I cannot unload this Scene :-p
					/*WARNING*/
                    Debug.LogWarning ("The Scene '" + sSceneName + "' is not loaded!");
				}
			}
			else
			{
				/*WARNING*/
                Debug.LogWarning ("Transition allready in progress …");
			}
        }
        //-------------------------------------------------------------------------------------------------------------
		private void LoadSceneByNameMethod (string sNextSceneName, LoadSceneMode sLoadSceneMode, string sIntermediateSceneName, STSTransitionData sPayload)
        {
            //Debug.Log("STSTransitionController LoadSceneByNameMethod()");
			if (SceneManager.GetActiveScene ().name != sNextSceneName)
			{
			    if (TransitionInProgress == false)
			    {
				    TransitionInProgress = true;
				    // memorize actual scene
				    PreviewScene = SceneManager.GetActiveScene ();

                    // Save scene param for further use
                    Dictionary<string, object> tParams = new Dictionary<string, object>();
                    tParams.Add(STSConstants.SceneNameKey, SceneManager.GetActiveScene().name);
                    tParams.Add(STSConstants.LoadModeKey, sLoadSceneMode);
                    tParams.Add(STSConstants.PayloadDataKey, sPayload);
                    PreviousScene.Add(tParams);

				    IntermediateSceneName = sIntermediateSceneName;
				    TransitionData = sPayload;
				    if (TransitionData == null)
				    {
					    TransitionData = new STSTransitionData ();
				    }
				    NextSceneName = sNextSceneName;
				    LoadSceneModeSelected = sLoadSceneMode;
				    StartCoroutine (LoadSceneByNameAsync ());
			    }
				else
				{
				    /*WARNING*/
                    Debug.LogWarning ("Transition allready in progress …");
				}
			}
			else
			{
				/*WARNING*/
                Debug.LogWarning ("Transition not necessary : active Scene is the Request Scene");
			}
        }
        //-------------------------------------------------------------------------------------------------------------	
		// Async method
		private IEnumerator UnloadSceneAsync ()
        {
            //Debug.Log("STSTransitionController UnloadSceneAsync()");
			STSTransitionParameters tTransitionParametersScript = GetTransitionsParams (SceneToUnload, true);
			// disable the user interactions
			EventSystemEnable (SceneToUnload, false);
			if (tTransitionParametersScript.ThisSceneDisable != null)
			{
				tTransitionParametersScript.ThisSceneDisable.Invoke (null);
			}
			EventSystemEnable (SceneManager.GetActiveScene (), false);
			// send message to the scene : you will be unloaded
			if (tTransitionParametersScript.ThisSceneWillUnloaded != null)
			{
				tTransitionParametersScript.ThisSceneWillUnloaded.Invoke (null);
			}
			AsyncOperation tAsynchroneUnload;
			tAsynchroneUnload = SceneManager.UnloadSceneAsync (SceneToUnload.name);
			while (!tAsynchroneUnload.isDone)
			{
				yield return null;
			}
			// enable the user interactions
			EventSystemEnable (SceneManager.GetActiveScene (), true);
        }
        //-------------------------------------------------------------------------------------------------------------
		private IEnumerator LoadSceneByNameAsync ()
        {
            //Debug.Log("STSTransitionController LoadSceneByNameAsync()");
			// prepare future old params
			OldSceneParams = this.GetComponent<STSTransitionParameters> ();
			OldStandByParams = this.GetComponent<STSTransitionStandBy> ();

			//-------------------------------
			// PREVIEW SCENE PROCESS
			//-------------------------------
			// get params
			PreviewSceneParams = GetTransitionsParams (PreviewScene, true);
			PreviewSceneParams.CopyIn (OldSceneParams);
			// disable the user interactions
			EventSystemPrevent (false);
			if (PreviewSceneParams.ThisSceneDisable != null)
			{
				PreviewSceneParams.ThisSceneDisable.Invoke (TransitionData);
			}
			// Transition Out will Start
			//if (m_PreviewSceneParams.AnimationOut.Start != null)
			//{
			//	// calcul estimated second
			//	m_PreviewSceneParams.AnimationOut.Start.Invoke (m_TransitionData, m_PreviewSceneParams.AnimationOut.Seconds);
			//}
            if (PreviewSceneParams.OnExitStart != null)
            {
                // calcul estimated second
                PreviewSceneParams.OnExitStart.Invoke(TransitionData);
            }
			// Transition Out GO!
			AnimationTransitionOut (PreviewSceneParams);
			while (AnimationFinished () == false)
			{
				yield return null;
			}

			// Transition Out Finish
			//if (m_PreviewSceneParams.AnimationOut.Finish != null) 
			//{
				//m_PreviewSceneParams.AnimationOut.Finish.Invoke (m_TransitionData);
            //}
            if (PreviewSceneParams.OnExitFinish != null)
            {
                PreviewSceneParams.OnExitFinish.Invoke(TransitionData);
            }
			// Transition setp 1 is finished this scene can be replace by the next or intermediate Scene
			//-------------------------------
			// PREVIEW SCENE IS OUT
			//-------------------------------
			// INTERMEDIATE SCENE PROCESS
			//-------------------------------

			PreviewSceneParams.CopyIn (OldSceneParams);
				
			if (IntermediateSceneName != null)
			{
				//-------------------------------

				// actual scene must be persistent ?
				LoadSceneMode tTransitionSceneMode = LoadSceneModeSelected;
				// load transition scene async
				AsyncOperation tAsynchroneLoadIntermediateOperation;
				tAsynchroneLoadIntermediateOperation = SceneManager.LoadSceneAsync (IntermediateSceneName, tTransitionSceneMode);
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
				IntermediateScene = SceneManager.GetSceneByName (IntermediateSceneName);
				// Active the next scene as root scene 
				SceneManager.SetActiveScene (IntermediateScene);
				// disable audiolistener of preview scene
				AudioListenerPrevent ();
				// remove preview scene 
				if (LoadSceneModeSelected == LoadSceneMode.Single) 
				{
					// if m_SceneActual is allready loaded i need to unloaded it
					if (SceneManager.GetSceneByName (PreviewScene.name).isLoaded) 
					{
						AsyncOperation tAsynchroneUnloadActualScene;
						tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync (PreviewScene.name);
						while (tAsynchroneUnloadActualScene.progress < 0.9f) 
						{
							yield return null;
						}
						if (PreviewSceneParams.ThisSceneWillUnloaded != null) 
						{
							PreviewSceneParams.ThisSceneWillUnloaded.Invoke (TransitionData);
						}
						while (!tAsynchroneUnloadActualScene.isDone) 
						{
							yield return null;
						}
					}
				}

				// get params
				IntermediateSceneParams = GetTransitionsParams (IntermediateScene, false);
				// disable the user interactions until fadein 
				EventSystemEnable (IntermediateScene, false);
				// intermediate scene is loaded
				if (IntermediateSceneParams.ThisSceneLoaded != null) {
					IntermediateSceneParams.ThisSceneLoaded.Invoke (TransitionData);
				}
				// animation in
				//if (m_IntermediateSceneParams.AnimationIn.Start != null) {
					//m_IntermediateSceneParams.AnimationIn.Start.Invoke (m_TransitionData, m_IntermediateSceneParams.AnimationIn.Seconds);
                //}
                if (IntermediateSceneParams.OnEnterStart != null)
                {
                    IntermediateSceneParams.OnEnterStart.Invoke(TransitionData);
                }
				// animation in Go!
				AnimationTransitionIn (IntermediateSceneParams);
				while (AnimationFinished () == false) {
					yield return null;
				}
				// animation in Finish
				//if (m_IntermediateSceneParams.AnimationIn.Finish != null) {
					//m_IntermediateSceneParams.AnimationIn.Finish.Invoke (m_TransitionData);
                //}
                if (IntermediateSceneParams.OnEnterFinish != null)
                {
                    IntermediateSceneParams.OnEnterFinish.Invoke(TransitionData);
                }
				// enable the user interactions 
				EventSystemEnable (IntermediateScene, true);
				// enable the user interactions 
				if (IntermediateSceneParams.ThisSceneEnable != null) {
					IntermediateSceneParams.ThisSceneEnable.Invoke (TransitionData);
				}
				// start stand by
				IntermediateSceneStandBy = GetStandByParams (IntermediateScene);
				if (IntermediateSceneStandBy.StandByStart != null) {
                    IntermediateSceneStandBy.StandByStart.Invoke (IntermediateSceneStandBy);
				}
				StandBy ();
				// and load next scene async 
				//-------------------------------
				// INTERMEDIATE SCENE IS IN PLACE
				//-------------------------------
			} 
			else 
			{
				// So! no transition! then the intermediate params are the preview scene params
				IntermediateSceneParams = GetTransitionsParams (PreviewScene, true);
				// And the StandBy params are the preview scene StandBy params
				IntermediateSceneStandBy = GetStandByParams (PreviewScene);
				if (IntermediateSceneStandBy.StandByStart != null) {
                    IntermediateSceneStandBy.StandByStart.Invoke (IntermediateSceneStandBy);
				}
				StandBy ();
				// and load next scene async 
				//-------------------------------
				// NO INTERMEDIATE SCENE IS IN PLACE, I USE THE PREVIEW SCENE
				//-------------------------------
			}


			IntermediateSceneParams.CopyIn (OldSceneParams);
			IntermediateSceneStandBy.CopyIn (OldStandByParams);

			//-------------------------------
			// NEXT SCENE PROCESS
			//-------------------------------
			// load Next Scene
			// actual scene must be persistent ?
			LoadSceneMode tNextSceneMode = LoadSceneModeSelected;
			bool tNextSceneAllRedayExist = false;
			// If scene with this name allready exist I use the old instance
			if (SceneManager.GetSceneByName (NextSceneName).isLoaded) 
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
				if (IntermediateSceneStandBy.LoadNextSceneStart != null) 
				{
					IntermediateSceneStandBy.LoadNextSceneStart.Invoke (TransitionData, 0.0f);
				}
				AsyncOperation tAsynchroneloadNext;
				tAsynchroneloadNext = SceneManager.LoadSceneAsync (NextSceneName, tNextSceneMode);
				tAsynchroneloadNext.allowSceneActivation = false;
				// load next scene async can send percent :-)
				while (tAsynchroneloadNext.progress < 0.9f) 
				{
					if (IntermediateSceneStandBy.LoadingNextScenePercent != null) 
					{
						IntermediateSceneStandBy.LoadingNextScenePercent.Invoke (TransitionData, tAsynchroneloadNext.progress);
					}
					yield return null;
				}

				// need to send call back now (anticipate :-/ ) 
				if (IntermediateSceneStandBy.LoadNextSceneFinish != null) 
				{
					IntermediateSceneStandBy.LoadNextSceneFinish.Invoke (TransitionData, 1.0f);
				}

				// when finish test if transition scene is allways in standby
				if (IntermediateSceneName != null) 
				{

					//-------------------------------
					// INTERMEDIATE SCENE UNLOAD
					//-------------------------------
					EventSystemEnable (IntermediateScene, true);
					// continue standby if it's necessary
					while (StandByIsProgressing()) 
					{
						yield return null;
					}
					// As sson as possible 
					if (IntermediateSceneStandBy.StandByFinish != null) 
					{
                        IntermediateSceneStandBy.StandByFinish.Invoke (IntermediateSceneStandBy);
					}
					// Waiting to load the next Scene
					while (WaitingToLauchNextScene()) 
					{
						//Debug.Log ("StandByIsNotFinished loop");
						yield return null;
					}
					// stanby is finished And the next scene can be lauch
					// disable user interactions on the intermediate scene
					EventSystemEnable (IntermediateScene, false);
					if (IntermediateSceneParams.ThisSceneDisable != null) 
					{
						IntermediateSceneParams.ThisSceneDisable.Invoke (TransitionData);
					}
					// intermediate scene Transition Out start 
					//if (m_IntermediateSceneParams.AnimationOut.Start != null) 
					//{
						//m_IntermediateSceneParams.AnimationOut.Start.Invoke (m_TransitionData, m_IntermediateSceneParams.AnimationOut.Seconds);
                    //}
                    if (IntermediateSceneParams.OnExitStart != null)
                    {
                        IntermediateSceneParams.OnExitStart.Invoke(TransitionData);
                    }
					// intermediate scene Transition Out GO! 
					AnimationTransitionOut (IntermediateSceneParams);
					while (AnimationFinished () == false) 
					{
						yield return null;
					}
					// intermediate scene Transition Out finished! 
					//if (m_IntermediateSceneParams.AnimationOut.Finish != null) 
					//{
						//m_IntermediateSceneParams.AnimationOut.Finish.Invoke (m_TransitionData);
                    //}
                    if (IntermediateSceneParams.OnExitFinish != null)
                    {
                        IntermediateSceneParams.OnExitFinish.Invoke(TransitionData);
                    }
					// fadeout is finish
					// will unloaded the intermediate scene
					if (IntermediateSceneParams.ThisSceneWillUnloaded != null) 
					{
						IntermediateSceneParams.ThisSceneWillUnloaded.Invoke (TransitionData);
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
				NextScene = SceneManager.GetSceneByName (NextSceneName);
				NextSceneParams = GetTransitionsParams (NextScene, false);
				// when finish test if transition scene is allways in standby
				if (IntermediateSceneName != null) 
				{
					//-------------------------------
					// INTERMEDIATE SCENE UNLOAD
					//-------------------------------
					// continue standby if it's necessary
					EventSystemEnable (IntermediateScene, true);

					while (StandByIsProgressing()) 
					{
						yield return null;
					}
					// send call back for standby finished
					if (IntermediateSceneStandBy.StandByFinish != null) 
					{
                        IntermediateSceneStandBy.StandByFinish.Invoke (IntermediateSceneStandBy);
					}
					// Waiting to load the next Scene
					while (WaitingToLauchNextScene()) {
						//Debug.Log ("StandByIsNotFinished loop");
						yield return null;
					}
					// stanby is finish
					// disable user interactions on the transition scene
					EventSystemEnable (IntermediateScene, false);

					// disable the transition scene
					if (IntermediateSceneParams.ThisSceneDisable != null) 
					{
						IntermediateSceneParams.ThisSceneDisable.Invoke (TransitionData);
					}
					// Transition scene disappear by fadeout 
					AnimationTransitionOut (IntermediateSceneParams);
					//if (m_IntermediateSceneParams.AnimationOut.Start != null) 
					//{
						//m_IntermediateSceneParams.AnimationOut.Start.Invoke (m_TransitionData, m_IntermediateSceneParams.AnimationOut.Seconds);
                    //}
                    if (IntermediateSceneParams.OnExitStart != null)
                    {
                        IntermediateSceneParams.OnExitStart.Invoke(TransitionData);
                    }
					while (AnimationFinished () == false) 
					{
						yield return null;
					}
					//if (m_IntermediateSceneParams.AnimationOut.Finish != null) 
					//{
						//m_IntermediateSceneParams.AnimationOut.Finish.Invoke (m_TransitionData);
                    //}
                    if (IntermediateSceneParams.OnExitFinish != null)
                    {
                        IntermediateSceneParams.OnExitFinish.Invoke(TransitionData);
                    }
					// fadeout is finish
					// unloaded the transition scene
					if (IntermediateSceneParams.ThisSceneWillUnloaded != null) 
					{
						IntermediateSceneParams.ThisSceneWillUnloaded.Invoke (TransitionData);
					}
				}
			}
			NextScene = SceneManager.GetSceneByName (NextSceneName);
			// unload Intermediate Scene anyway
			if (LoadSceneModeSelected == LoadSceneMode.Additive && IntermediateSceneName != null) 
			{
				AsyncOperation tAsynchroneUnloadTransition;
				tAsynchroneUnloadTransition = SceneManager.UnloadSceneAsync (IntermediateSceneName);

				while (tAsynchroneUnloadTransition.progress < 0.9f) {
					yield return null;
				}

				while (!tAsynchroneUnloadTransition.isDone) {
					yield return null;
				}
			}
			//remove preview scene (if not remove before)
			if (LoadSceneModeSelected == LoadSceneMode.Single) 
			{
				// if m_SceneActual is allready loaded i need to unloaded it
				if (SceneManager.GetSceneByName (PreviewScene.name).isLoaded) 
				{
					AsyncOperation tAsynchroneUnloadActualScene;
					tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync (PreviewScene.name);
					if (PreviewSceneParams.ThisSceneWillUnloaded != null) 
					{
						PreviewSceneParams.ThisSceneWillUnloaded.Invoke (TransitionData);
					}
					while (tAsynchroneUnloadActualScene.progress < 0.9f) 
					{
						yield return null;
					}
			
					while (!tAsynchroneUnloadActualScene.isDone) {
						yield return null;
					}
				}
				else 
				{
					// Ok this scene allready unloading (perhaps in the intermediate scene)
				}
			}
			// Active the next scene as root scene 
			SceneManager.SetActiveScene (NextScene);
			AudioListenerPrevent ();
			// get params
			NextSceneParams = GetTransitionsParams (NextScene, false);
			// disable user interactions on the next scene
			EventSystemEnable (NextScene, false);
			// scene is loaded
			if (tNextSceneAllRedayExist == false) {
				if (NextSceneParams.ThisSceneLoaded != null) {
					NextSceneParams.ThisSceneLoaded.Invoke (TransitionData);
				}
			}
			// Next scene appear by fade in 
			AnimationTransitionIn (NextSceneParams);
			//if (m_NextSceneParams.AnimationIn.Start != null) {
				//m_NextSceneParams.AnimationIn.Start.Invoke (m_TransitionData, m_NextSceneParams.AnimationIn.Seconds);
            //}
            if (NextSceneParams.OnEnterStart != null)
            {
                NextSceneParams.OnEnterStart.Invoke(TransitionData);
            }
			while (AnimationFinished () == false) {
				yield return null;
			}
			//if (m_NextSceneParams.AnimationIn.Finish != null) {
				//m_NextSceneParams.AnimationIn.Finish.Invoke (m_TransitionData);
            //}
            if (NextSceneParams.OnEnterFinish != null)
            {
                NextSceneParams.OnEnterFinish.Invoke(TransitionData);
            }
			// fadein is finish
			// next scene user interaction enable
			EventSystemPrevent (true);
			// next scene is enable
			if (NextSceneParams.ThisSceneEnable != null) {
				NextSceneParams.ThisSceneEnable.Invoke (TransitionData);
			}

			NextSceneParams.CopyIn (OldSceneParams);

			// My transition is finish. I can do an another transition
			TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
		// toolbox method

		private void AudioListenerPrevent ()
        {
            //Debug.Log("STSTransitionController AudioListenerPrevent()");
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene tScene = SceneManager.GetSceneAt (i);
				if (tScene.isLoaded) {
						AudioListenerEnable (tScene, false);
				}
			}
			AudioListenerEnable (SceneManager.GetActiveScene (), true);
        }
        //-------------------------------------------------------------------------------------------------------------
		private void AudioListenerEnable (Scene sScene, bool sEnable)
        {
            //Debug.Log("STSTransitionController AudioListenerEnable()");
			if (sScene.isLoaded) {
				AudioListener tAudioListener = null;
				GameObject[] tAllRootObjects = sScene.GetRootGameObjects ();
				foreach (GameObject tObject in tAllRootObjects) {
					if (tObject.GetComponent<AudioListener> () != null) {
						tAudioListener = tObject.GetComponent<AudioListener> ();
					}
				}
				if (tAudioListener != null) {
					tAudioListener.enabled = sEnable;
				} else {
					//Debug.Log ("No <AudioListener> type component found in the root Objects. Becarefull!");
				}
			}
        }
        //-------------------------------------------------------------------------------------------------------------
		private void EventSystemPrevent (bool sEnable)
        {
            //Debug.Log("STSTransitionController EventSystemPrevent()");
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene tScene = SceneManager.GetSceneAt (i);
				if (tScene.isLoaded) {
					EventSystemEnable (tScene, false);
				}
			}
			EventSystemEnable (SceneManager.GetActiveScene(), sEnable);
        }
        //-------------------------------------------------------------------------------------------------------------
		private void EventSystemEnable (Scene sScene, bool sEnable)
        {
            //Debug.Log("STSTransitionController EventSystemEnable()");
			if (PreventUserInteractions == true) 
			{
				EventSystem tEventSystem = null;
				GameObject[] tAllRootObjects = sScene.GetRootGameObjects ();
				foreach (GameObject tObject in tAllRootObjects) 
				{
					if (tObject.GetComponent<EventSystem> () != null) 
					{
						tEventSystem = tObject.GetComponent<EventSystem> ();
					}
				}
				if (tEventSystem != null) 
				{
					tEventSystem.enabled = sEnable;
				} 
				else 
				{
					//Debug.Log ("No <EventSystem> type component found in the root Objects. Becarefull!");
				}
			}
        }
        //-------------------------------------------------------------------------------------------------------------
		private STSTransitionParameters GetTransitionsParams (Scene sScene, bool sStartTransition)
        {
            //Debug.Log("STSTransitionController GetTransitionsParams()");
			STSTransitionParameters tTransitionParametersScript = null;
			GameObject[] tAllRootObjects = sScene.GetRootGameObjects ();
			foreach (GameObject tObject in tAllRootObjects) 
			{
				if (tObject.GetComponent<STSTransitionParameters> () != null) 
				{
					tTransitionParametersScript = tObject.GetComponent<STSTransitionParameters> ();
				}
			}
			if (tTransitionParametersScript == null) 
			{
				// create Game Object?
				//Debug.Log ("NO PARAMS");
                GameObject tObjToSpawn = new GameObject (STSConstants.TransitionControllerObjectName);
				tObjToSpawn.AddComponent<STSTransitionParameters> ();
				tTransitionParametersScript = (STSTransitionParameters)tObjToSpawn.GetComponent<STSTransitionParameters> ();
                tTransitionParametersScript.EffectOnEnter = STSEffectType.Default.Dupplicate();
                tTransitionParametersScript.EffectOnExit = STSEffectType.Default.Dupplicate();
			}
			return tTransitionParametersScript;
        }
        //-------------------------------------------------------------------------------------------------------------
		private STSTransitionStandBy GetStandByParams (Scene sScene)
        {
            //Debug.Log("STSTransitionController GetStandByParams()");
            //			TransitionStandByScript tTransitionStandByScript = GameObject.FindObjectOfType<TransitionStandByScript> ();
            STSTransitionStandBy tTransitionStandByScript = null;
			GameObject[] tAllRootObjects = sScene.GetRootGameObjects ();
			foreach (GameObject tObject in tAllRootObjects) 
			{
				if (tObject.GetComponent<STSTransitionStandBy> () != null) 
				{
					tTransitionStandByScript = tObject.GetComponent<STSTransitionStandBy> ();
				}
			}
			if (tTransitionStandByScript == null) 
			{
                GameObject tObjToSpawn = new GameObject (STSConstants.TransitionControllerObjectName);
				tObjToSpawn.AddComponent<STSTransitionStandBy> ();
				tTransitionStandByScript = (STSTransitionStandBy)tObjToSpawn.GetComponent<STSTransitionStandBy> ();
				tTransitionStandByScript.StandBySeconds = 5.0f;
				tTransitionStandByScript.AutoLoadNextScene = true;
			}

			return tTransitionStandByScript;
        }
		//private int m_DrawDepth = -1000;
        //-------------------------------------------------------------------------------------------------------------
		private bool AnimationProgress ()
        {
            //Debug.Log("STSTransitionController AnimationProgress()");
            //return m_AnimationInProgress;
            return EffectType.AnimIsPlaying;
        }
        //-------------------------------------------------------------------------------------------------------------
		private bool AnimationFinished ()
        {
            //Debug.Log("STSTransitionController AnimationFinished()");
            //return m_AnimationFinish;
            return EffectType.AnimIsFinished;
        }
        //-------------------------------------------------------------------------------------------------------------
		private void AnimationTransitionIn (STSTransitionParameters sThisSceneParameters)
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
		private void AnimationTransitionOut (STSTransitionParameters sThisSceneParameters)
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
		private void StandBy ()
        {
            //Debug.Log("STSTransitionController StandBy()");
			StandByTimer = 0.0f;
			StandByInProgress = true;
			LauchNextScene = false;
        }
        //-------------------------------------------------------------------------------------------------------------
		private bool StandByIsProgressing ()
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
		private bool WaitingToLauchNextScene ()
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
		public void FinishStandBy ()
        {
            //Debug.Log("STSTransitionController FinishStandBy()");
			EventSystemEnable (kSingleton.IntermediateScene, false);
			LauchNextScene = true;
            StandByInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
		void OnGUI ()
        {
            //Debug.Log("STSTransitionController OnGUI()");
            if (EffectType != null)
            {
                EffectType.DrawMaster(new Rect(0, 0, Screen.width, Screen.height));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================