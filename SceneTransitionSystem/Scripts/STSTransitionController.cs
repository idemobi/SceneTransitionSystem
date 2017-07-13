using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

namespace SceneTransitionSystem
{
    public class STSTransitionController : MonoBehaviour
	{
		private static STSTransitionController k_Singleton = null;

		// initialized or not?
		private bool m_Initialized = false;

		// prevent multi transition
		private bool m_TransitionInProgress = false;

		// prevent user actions during the transition
		private bool m_PreventUserInteractions = true;

		// Data to transmit to the other scenes
		private STSTransitionData m_TransitionData;
		private STSTransitionParameters m_OldSceneParams;
		private STSTransitionStandBy m_OldStandByParams;

        // Previous scene
        private static List<Dictionary<string, object>> _previousScene = new List<Dictionary<string, object>>();

        // Scenes controlled
        private Scene m_PreviewScene;
		private STSTransitionParameters m_PreviewSceneParams;

		private Scene m_IntermediateScene;
		private string m_IntermediateSceneName;
		private STSTransitionParameters m_IntermediateSceneParams;
		private STSTransitionStandBy m_IntermediateStandByParams;
        
        private Scene m_NextScene;
		private string m_NextSceneName;
		private STSTransitionParameters m_NextSceneParams;

		// Scene load mode
		private LoadSceneMode m_LoadSceneMode;

		private Scene m_SceneToUnload;
		private STSTransitionParameters m_SceneToUnloadParams;

		// temporary property from the TransitionParametersScript of active Scene
//		private TransitionEventScript m_ThisSceneLoaded;
//		private TransitionEventEstimatedSecondsScript m_FadeInStart;
//		private TransitionEventScript m_FadeInFinish;
//		private TransitionEventScript m_ThisSceneEnable;
//
//		private TransitionEventScript m_ThisSceneDisable;
//		private TransitionEventEstimatedSecondsScript m_FadeOutStart;
//		private TransitionEventScript m_FadeOutFinish;
//		private TransitionEventScript m_ThisSceneWillUnloaded;
//
//		private TransitionLoadingScript m_NextSceneLoadingStart;
//		private TransitionLoadingScript m_NextSceneLoadingPercent;
//		private TransitionLoadingScript m_NextSceneLoadingFinish;
//		private TransitionEventScript m_StandByFinish;

		// Class method
		public static void LoadScene(string sNextSceneName,
            LoadSceneMode sLoadSceneMode = LoadSceneMode.Single,
            string sIntermediateSceneName = "",
            STSTransitionData sPayload = null)
		{
			Singleton();
			k_Singleton.LoadSceneByNameMethod (sNextSceneName, sLoadSceneMode, sIntermediateSceneName, sPayload);
		}

        public static void LoadPreviousScene()
        {
            Dictionary<string, object> tParams = _previousScene[_previousScene.Count - 1];

            object result;
            tParams.TryGetValue("sceneName", out result);
            string tPreviewSceneName = result.ToString();

            tParams.TryGetValue("loadMode", out result);
            LoadSceneMode tLoadMode = (LoadSceneMode)result;

            tParams.TryGetValue("payloadData", out result);
            //STSTransitionData tPayLoadData = result as STSTransitionData;

            Singleton();
            k_Singleton.LoadSceneByNameMethod(tPreviewSceneName, tLoadMode, null, null);

            _previousScene.RemoveAt(_previousScene.Count - 1);
        }

		public static void UnloadScene (string sSceneName, string sNextSceneName, string sIntermediateSceneName = null)
		{
			Singleton();
			k_Singleton.UnloadSceneByNameMethod (sSceneName, sIntermediateSceneName, sNextSceneName);
		}

		public static void UnloadSceneNotActive (string sSceneNotActiveName)
		{
			Singleton();
			k_Singleton.UnloadSceneNotActiveByNameMethod (sSceneNotActiveName);
		}
		
//		public TransitionControllerScript () 
//		{
//			Debug.Log ("TransitionControllerScript Constructor");
//			if (k_Singleton != null) 
//			{
//				if (k_Singleton != this) 
//				{
//					Destroy (this);
//					this = k_Singleton;
//				}
//			}
//		}

		// Singleton
		public static STSTransitionController Singleton ()
		{
			//Debug.Log ("TransitionControllerScript Singleton");
			if (k_Singleton == null) {
				// I need to create singleton
				GameObject tObjToSpawn;
				//spawn object
				tObjToSpawn = new GameObject ("TransitionControllerObject");
				//Add Components
				tObjToSpawn.AddComponent<STSTransitionController> ();
				// keep k_Singleton
				k_Singleton = tObjToSpawn.GetComponent<STSTransitionController> ();
				// Init Instance
				k_Singleton.InitInstance ();
				// memorize the init instance
				k_Singleton.m_Initialized = true;

				tObjToSpawn.AddComponent<STSTransitionParameters> ();
				tObjToSpawn.AddComponent<STSTransitionStandBy> ();

			}
			return k_Singleton;
		}

		// Memory managment
		private void InitInstance ()
		{
			//Debug.Log ("TransitionControllerScript InitInstance");
		}

		//Awake is always called before any Start functions
		private void Awake ()
		{
			//Debug.Log ("TransitionControllerScript Awake");
			//Check if instance already exists
			if (k_Singleton == null) {
				//if not, set instance to this
				k_Singleton = this;
				if (k_Singleton.m_Initialized == false) {
					k_Singleton.InitInstance ();
					k_Singleton.m_Initialized = true;
				}
				;
			}
		//If instance already exists and it's not this:
		else if (k_Singleton != this) {
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy (gameObject);    
			}
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad (gameObject);
			//Call the InitGame function to initialize the first level 
		}

		private void OnDestroy ()
		{
			//Debug.Log ("TransitionControllerScript OnDestroy");
		}

		// Instance method
		private void UnloadSceneByNameMethod (string sSceneName, string sIntermediateSceneName, string sNextSceneName)
		{
			if (m_TransitionInProgress == false) 
			{
				m_TransitionInProgress = true;
				m_PreviewScene = SceneManager.GetSceneByName (sSceneName);
                //m_PreviewSceneName = sSceneName;
				m_IntermediateSceneName = sIntermediateSceneName;
				m_NextSceneName = sNextSceneName;

				if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetSceneByName (sNextSceneName).isLoaded == true) 
				{
					// Ok I can unload the scene normally
					LoadSceneByNameMethod (sNextSceneName, LoadSceneMode.Single, sIntermediateSceneName, null);
				} 
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == false && SceneManager.GetSceneByName (sNextSceneName).isLoaded == true) 
				{
					// The scene doesn't exist ... I cannot unload this Scene :-p
					/*WARNING*/
					Debug.Log ("The Scene '" + sSceneName + "' isn't loaded! Not possible to unload!");
				} 
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetSceneByName (sNextSceneName).isLoaded == false) 
				{
					// The scene to active doesn't exist ... I cannot active this Scene :-p
					/*WARNING*/
					Debug.Log ("The Next Scene '" + sNextSceneName + "' isn't loaded! Not possible to active!");
					// Perhaps I need to load the next scene ?
					// but I have not the LoadSceneMode (single or additive)
				} 
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == false && SceneManager.GetSceneByName (sNextSceneName).isLoaded == false) 
				{
					// The scene doesn't exist ... I cannot unload this Scene :-p
					// The scene to active doesn't exist ... I cannot active this Scene :-p
					/*WARNING*/
					Debug.Log ("The Scene '" + sSceneName + "' and Next Scene '" + sNextSceneName + "' are not loaded! Not possible to unload or/and active!");
					// Perhaps I need to load the next scene ?
					// but I have not the LoadSceneMode (single or additive)
				}
			} 
			else 
			{
				/*WARNING*/
				Debug.Log ("Transition allready in progress …");
			}
		}

		private void UnloadSceneNotActiveByNameMethod (string sSceneName)
		{
			if (m_TransitionInProgress == false)
			{
				m_TransitionInProgress = true;
				m_PreviewScene = SceneManager.GetActiveScene ();

				if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetActiveScene ().name != sSceneName)
				{
					// Ok I can unload the scene normally
					m_SceneToUnload = SceneManager.GetSceneByName (sSceneName);
					StartCoroutine (UnloadSceneAsync ());
				}
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == true && SceneManager.GetActiveScene ().name == sSceneName)
				{
					// The scene exist but it's the active... I cannot unload this Scene :-p
					/*WARNING*/
					Debug.Log ("The Scene '" + sSceneName + "' is active! use method : public static void UnloadSceneByName (string sSceneName, string sTransitionSceneName, string sNextSceneName)");
				}
				else if (SceneManager.GetSceneByName (sSceneName).isLoaded == false)
				{
					// The scene to unload doesn't exist ... I cannot unload this Scene :-p
					/*WARNING*/
					Debug.Log ("The Scene '" + sSceneName + "' is not loaded!");
				}
			}
			else
			{
				/*WARNING*/
				Debug.Log ("Transition allready in progress …");
			}
		}

		private void LoadSceneByNameMethod (string sNextSceneName, LoadSceneMode sLoadSceneMode, string sIntermediateSceneName, STSTransitionData sPayload)
		{
			if (SceneManager.GetActiveScene ().name != sNextSceneName)
			{
			    if (m_TransitionInProgress == false)
			    {
				    m_TransitionInProgress = true;
				    // memorize actual scene
				    m_PreviewScene = SceneManager.GetActiveScene ();

                    // Save scene param for further use
                    Dictionary<string, object> tParams = new Dictionary<string, object>();
                    tParams.Add("sceneName", SceneManager.GetActiveScene().name);
                    tParams.Add("loadMode", sLoadSceneMode);
                    tParams.Add("payloadData", sPayload);
                    _previousScene.Add(tParams);

				    m_IntermediateSceneName = sIntermediateSceneName;
				    m_TransitionData = sPayload;
				    if (m_TransitionData == null)
				    {
					    m_TransitionData = new STSTransitionData ();
				    }
				    m_NextSceneName = sNextSceneName;
				    m_LoadSceneMode = sLoadSceneMode;
				    StartCoroutine (LoadSceneByNameAsync ());
			    }
				else
				{
				    /*WARNING*/
				    Debug.Log ("Transition allready in progress …");
				}
			}
			else
			{
				/*WARNING*/
				Debug.Log ("Transition not necessary : active Scene is the Request Scene");
			}
		}
			
		// Async method
		private IEnumerator UnloadSceneAsync ()
		{
			STSTransitionParameters tTransitionParametersScript = GetTransitionsParams (m_SceneToUnload, true);
			// disable the user interactions
			EventSystemEnable (m_SceneToUnload, false);
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
			tAsynchroneUnload = SceneManager.UnloadSceneAsync (m_SceneToUnload.name);
			while (!tAsynchroneUnload.isDone)
			{
				yield return null;
			}
			// enable the user interactions
			EventSystemEnable (SceneManager.GetActiveScene (), true);
		}

		private IEnumerator LoadSceneByNameAsync ()
		{
			// prepare future old params
			m_OldSceneParams = this.GetComponent<STSTransitionParameters> ();
			m_OldStandByParams = this.GetComponent<STSTransitionStandBy> ();

			//-------------------------------
			// PREVIEW SCENE PROCESS
			//-------------------------------
			// get params
			m_PreviewSceneParams = GetTransitionsParams (m_PreviewScene, true);
			m_PreviewSceneParams.CopyIn (m_OldSceneParams);
			// disable the user interactions
			EventSystemPrevent (false);
			if (m_PreviewSceneParams.ThisSceneDisable != null)
			{
				m_PreviewSceneParams.ThisSceneDisable.Invoke (m_TransitionData);
			}
			// Transition Out will Start
			if (m_PreviewSceneParams.AnimationOut.Start != null)
			{
				// calcul estimated second
				m_PreviewSceneParams.AnimationOut.Start.Invoke (m_TransitionData, m_PreviewSceneParams.AnimationOut.Seconds);
			}
			// Transition Out GO!
			AnimationTransitionOut (m_PreviewSceneParams);
			while (AnimationFinished () == false)
			{
				yield return null;
			}

			// Transition Out Finish
			if (m_PreviewSceneParams.AnimationOut.Finish != null) 
			{
				m_PreviewSceneParams.AnimationOut.Finish.Invoke (m_TransitionData);
			}
			// Transition setp 1 is finished this scene can be replace by the next or intermediate Scene
			//-------------------------------
			// PREVIEW SCENE IS OUT
			//-------------------------------
			// INTERMEDIATE SCENE PROCESS
			//-------------------------------

			m_PreviewSceneParams.CopyIn (m_OldSceneParams);
				
			if (m_IntermediateSceneName != null)
			{
				//-------------------------------

				// actual scene must be persistent ?
				LoadSceneMode tTransitionSceneMode = m_LoadSceneMode;
				// load transition scene async
				AsyncOperation tAsynchroneLoadIntermediateOperation;
				tAsynchroneLoadIntermediateOperation = SceneManager.LoadSceneAsync (m_IntermediateSceneName, tTransitionSceneMode);
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
				m_IntermediateScene = SceneManager.GetSceneByName (m_IntermediateSceneName);
				// Active the next scene as root scene 
				SceneManager.SetActiveScene (m_IntermediateScene);
				// disable audiolistener of preview scene
				AudioListenerPrevent ();
				// remove preview scene 
				if (m_LoadSceneMode == LoadSceneMode.Single) 
				{
					// if m_SceneActual is allready loaded i need to unloaded it
					if (SceneManager.GetSceneByName (m_PreviewScene.name).isLoaded) 
					{
						AsyncOperation tAsynchroneUnloadActualScene;
						tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync (m_PreviewScene.name);
						while (tAsynchroneUnloadActualScene.progress < 0.9f) 
						{
							yield return null;
						}
						if (m_PreviewSceneParams.ThisSceneWillUnloaded != null) 
						{
							m_PreviewSceneParams.ThisSceneWillUnloaded.Invoke (m_TransitionData);
						}
						while (!tAsynchroneUnloadActualScene.isDone) 
						{
							yield return null;
						}
					}
				}

				// get params
				m_IntermediateSceneParams = GetTransitionsParams (m_IntermediateScene, false);
				// disable the user interactions until fadein 
				EventSystemEnable (m_IntermediateScene, false);
				// intermediate scene is loaded
				if (m_IntermediateSceneParams.ThisSceneLoaded != null) {
					m_IntermediateSceneParams.ThisSceneLoaded.Invoke (m_TransitionData);
				}
				// animation in
				if (m_IntermediateSceneParams.AnimationIn.Start != null) {
					m_IntermediateSceneParams.AnimationIn.Start.Invoke (m_TransitionData, m_IntermediateSceneParams.AnimationIn.Seconds);
				}
				// animation in Go!
				AnimationTransitionIn (m_IntermediateSceneParams);
				while (AnimationFinished () == false) {
					yield return null;
				}
				// animation in Finish
				if (m_IntermediateSceneParams.AnimationIn.Finish != null) {
					m_IntermediateSceneParams.AnimationIn.Finish.Invoke (m_TransitionData);
				}
				// enable the user interactions 
				EventSystemEnable (m_IntermediateScene, true);
				// enable the user interactions 
				if (m_IntermediateSceneParams.ThisSceneEnable != null) {
					m_IntermediateSceneParams.ThisSceneEnable.Invoke (m_TransitionData);
				}
				// start stand by
				m_IntermediateStandByParams = GetStandByParams (m_IntermediateScene);
				if (m_IntermediateStandByParams.StandByStart != null) {
					m_IntermediateStandByParams.StandByStart.Invoke (m_TransitionData);
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
				m_IntermediateSceneParams = GetTransitionsParams (m_PreviewScene, true);
				// And the StandBy params are the preview scene StandBy params
				m_IntermediateStandByParams = GetStandByParams (m_PreviewScene);
				if (m_IntermediateStandByParams.StandByStart != null) {
					m_IntermediateStandByParams.StandByStart.Invoke (m_TransitionData);
				}
				StandBy ();
				// and load next scene async 
				//-------------------------------
				// NO INTERMEDIATE SCENE IS IN PLACE, I USE THE PREVIEW SCENE
				//-------------------------------
			}


			m_IntermediateSceneParams.CopyIn (m_OldSceneParams);
			m_IntermediateStandByParams.CopyIn (m_OldStandByParams);

			//-------------------------------
			// NEXT SCENE PROCESS
			//-------------------------------
			// load Next Scene
			// actual scene must be persistent ?
			LoadSceneMode tNextSceneMode = m_LoadSceneMode;
			bool tNextSceneAllRedayExist = false;
			// If scene with this name allready exist I use the old instance
			if (SceneManager.GetSceneByName (m_NextSceneName).isLoaded) 
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
				if (m_IntermediateStandByParams.LoadNextSceneStart != null) 
				{
					m_IntermediateStandByParams.LoadNextSceneStart.Invoke (m_TransitionData, 0.0f);
				}
				AsyncOperation tAsynchroneloadNext;
				tAsynchroneloadNext = SceneManager.LoadSceneAsync (m_NextSceneName, tNextSceneMode);
				tAsynchroneloadNext.allowSceneActivation = false;
				// load next scene async can send percent :-)
				while (tAsynchroneloadNext.progress < 0.9f) 
				{
					if (m_IntermediateStandByParams.LoadingNextScenePercent != null) 
					{
						m_IntermediateStandByParams.LoadingNextScenePercent.Invoke (m_TransitionData, tAsynchroneloadNext.progress);
					}
					yield return null;
				}

				// need to send call back now (anticipate :-/ ) 
				if (m_IntermediateStandByParams.LoadNextSceneFinish != null) 
				{
					m_IntermediateStandByParams.LoadNextSceneFinish.Invoke (m_TransitionData, 1.0f);
				}

				// when finish test if transition scene is allways in standby
				if (m_IntermediateSceneName != null) 
				{

					//-------------------------------
					// INTERMEDIATE SCENE UNLOAD
					//-------------------------------
					EventSystemEnable (m_IntermediateScene, true);
					// continue standby if it's necessary
					while (StandByIsProgressing()) 
					{
						yield return null;
					}
					// As sson as possible 
					if (m_IntermediateStandByParams.StandByFinish != null) 
					{
						m_IntermediateStandByParams.StandByFinish.Invoke (m_TransitionData);
					}
					// Waiting to load the next Scene
					while (WaitingToLauchNextScene()) 
					{
						//Debug.Log ("StandByIsNotFinished loop");
						yield return null;
					}
					// stanby is finished And the next scene can be lauch
					// disable user interactions on the intermediate scene
					EventSystemEnable (m_IntermediateScene, false);
					if (m_IntermediateSceneParams.ThisSceneDisable != null) 
					{
						m_IntermediateSceneParams.ThisSceneDisable.Invoke (m_TransitionData);
					}
					// intermediate scene Transition Out start 
					if (m_IntermediateSceneParams.AnimationOut.Start != null) 
					{
						m_IntermediateSceneParams.AnimationOut.Start.Invoke (m_TransitionData, m_IntermediateSceneParams.AnimationOut.Seconds);
					}
					// intermediate scene Transition Out GO! 
					AnimationTransitionOut (m_IntermediateSceneParams);
					while (AnimationFinished () == false) 
					{
						yield return null;
					}
					// intermediate scene Transition Out finished! 
					if (m_IntermediateSceneParams.AnimationOut.Finish != null) 
					{
						m_IntermediateSceneParams.AnimationOut.Finish.Invoke (m_TransitionData);
					}
					// fadeout is finish
					// will unloaded the intermediate scene
					if (m_IntermediateSceneParams.ThisSceneWillUnloaded != null) 
					{
						m_IntermediateSceneParams.ThisSceneWillUnloaded.Invoke (m_TransitionData);
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
				m_NextScene = SceneManager.GetSceneByName (m_NextSceneName);
				m_NextSceneParams = GetTransitionsParams (m_NextScene, false);
				// when finish test if transition scene is allways in standby
				if (m_IntermediateSceneName != null) 
				{
					//-------------------------------
					// INTERMEDIATE SCENE UNLOAD
					//-------------------------------
					// continue standby if it's necessary
					EventSystemEnable (m_IntermediateScene, true);

					while (StandByIsProgressing()) 
					{
						yield return null;
					}
					// send call back for standby finished
					if (m_IntermediateStandByParams.StandByFinish != null) 
					{
						m_IntermediateStandByParams.StandByFinish.Invoke (m_TransitionData);
					}
					// Waiting to load the next Scene
					while (WaitingToLauchNextScene()) {
						//Debug.Log ("StandByIsNotFinished loop");
						yield return null;
					}
					// stanby is finish
					// disable user interactions on the transition scene
					EventSystemEnable (m_IntermediateScene, false);

					// disable the transition scene
					if (m_IntermediateSceneParams.ThisSceneDisable != null) 
					{
						m_IntermediateSceneParams.ThisSceneDisable.Invoke (m_TransitionData);
					}
					// Transition scene disappear by fadeout 
					AnimationTransitionOut (m_IntermediateSceneParams);
					if (m_IntermediateSceneParams.AnimationOut.Start != null) 
					{
						m_IntermediateSceneParams.AnimationOut.Start.Invoke (m_TransitionData, m_IntermediateSceneParams.AnimationOut.Seconds);
					}
					while (AnimationFinished () == false) 
					{
						yield return null;
					}
					if (m_IntermediateSceneParams.AnimationOut.Finish != null) 
					{
						m_IntermediateSceneParams.AnimationOut.Finish.Invoke (m_TransitionData);
					}
					// fadeout is finish
					// unloaded the transition scene
					if (m_IntermediateSceneParams.ThisSceneWillUnloaded != null) 
					{
						m_IntermediateSceneParams.ThisSceneWillUnloaded.Invoke (m_TransitionData);
					}
				}
			}
			m_NextScene = SceneManager.GetSceneByName (m_NextSceneName);
			// unload Intermediate Scene anyway
			if (m_LoadSceneMode == LoadSceneMode.Additive && m_IntermediateSceneName != null) 
			{
				AsyncOperation tAsynchroneUnloadTransition;
				tAsynchroneUnloadTransition = SceneManager.UnloadSceneAsync (m_IntermediateSceneName);

				while (tAsynchroneUnloadTransition.progress < 0.9f) {
					yield return null;
				}

				while (!tAsynchroneUnloadTransition.isDone) {
					yield return null;
				}
			}
			//remove preview scene (if not remove before)
			if (m_LoadSceneMode == LoadSceneMode.Single) 
			{
				// if m_SceneActual is allready loaded i need to unloaded it
				if (SceneManager.GetSceneByName (m_PreviewScene.name).isLoaded) 
				{
					AsyncOperation tAsynchroneUnloadActualScene;
					tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync (m_PreviewScene.name);
					if (m_PreviewSceneParams.ThisSceneWillUnloaded != null) 
					{
						m_PreviewSceneParams.ThisSceneWillUnloaded.Invoke (m_TransitionData);
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
			SceneManager.SetActiveScene (m_NextScene);
			AudioListenerPrevent ();
			// get params
			m_NextSceneParams = GetTransitionsParams (m_NextScene, false);
			// disable user interactions on the next scene
			EventSystemEnable (m_NextScene, false);
			// scene is loaded
			if (tNextSceneAllRedayExist == false) {
				if (m_NextSceneParams.ThisSceneLoaded != null) {
					m_NextSceneParams.ThisSceneLoaded.Invoke (m_TransitionData);
				}
			}
			// Next scene appear by fade in 
			AnimationTransitionIn (m_NextSceneParams);
			if (m_NextSceneParams.AnimationIn.Start != null) {
				m_NextSceneParams.AnimationIn.Start.Invoke (m_TransitionData, m_NextSceneParams.AnimationIn.Seconds);
			}
			while (AnimationFinished () == false) {
				yield return null;
			}
			if (m_NextSceneParams.AnimationIn.Finish != null) {
				m_NextSceneParams.AnimationIn.Finish.Invoke (m_TransitionData);
			}
			// fadein is finish
			// next scene user interaction enable
			EventSystemPrevent (true);
			// next scene is enable
			if (m_NextSceneParams.ThisSceneEnable != null) {
				m_NextSceneParams.ThisSceneEnable.Invoke (m_TransitionData);
			}

			m_NextSceneParams.CopyIn (m_OldSceneParams);

			// My transition is finish. I can do an another transition
			m_TransitionInProgress = false;
		}

		// toolbox method

		private void AudioListenerPrevent ()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene tScene = SceneManager.GetSceneAt (i);
				if (tScene.isLoaded) {
						AudioListenerEnable (tScene, false);
				}
			}
			AudioListenerEnable (SceneManager.GetActiveScene (), true);
		}

		private void AudioListenerEnable (Scene sScene, bool sEnable)
		{
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

		private void EventSystemPrevent (bool sEnable)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene tScene = SceneManager.GetSceneAt (i);
				if (tScene.isLoaded) {
					EventSystemEnable (tScene, false);
				}
			}
			EventSystemEnable (SceneManager.GetActiveScene(), sEnable);
		}

		private void EventSystemEnable (Scene sScene, bool sEnable)
		{

			if (m_PreventUserInteractions == true) 
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

		private STSTransitionParameters GetTransitionsParams (Scene sScene, bool sStartTransition)
		{
//			if (sStartTransition == false) 
//			{
////			m_FadeInPreviewColor = m_FadeInColor;
//				m_FadeOutPreviewColor = m_FadeOutColor;
//			}
			//Debug.Log ("GetFadeParams");
//			TransitionParametersScript tTransitionParametersScript = GameObject.FindObjectOfType<TransitionParametersScript> ();
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
				GameObject tObjToSpawn = new GameObject ("TransitionControllerObject");
				tObjToSpawn.AddComponent<STSTransitionParameters> ();
				tTransitionParametersScript = (STSTransitionParameters)tObjToSpawn.GetComponent<STSTransitionParameters> ();
				tTransitionParametersScript.AnimationIn.Style = STSAnimationStyle.FadeIn;
				tTransitionParametersScript.AnimationOut.Style = STSAnimationStyle.FadeOut;
				tTransitionParametersScript.AnimationIn.Color = Color.black;
				tTransitionParametersScript.AnimationOut.Color = Color.black;
				tTransitionParametersScript.AnimationIn.Texture = null;
				tTransitionParametersScript.AnimationOut.Texture = null;
				tTransitionParametersScript.AnimationIn.Seconds = 0.8f;
				tTransitionParametersScript.AnimationOut.Seconds = 0.8f;
			} 
//			else 
//			{
//				m_FadeInColor = tTransitionParametersScript.TransitionInColor;
//				m_FadeOutColor = tTransitionParametersScript.TransitionOutColor;
//
//				m_FadeInTexture = tTransitionParametersScript.TransitionInTexture;
//				m_FadeOutTexture = tTransitionParametersScript.TransitionOutTexture;
//
//				m_FadeInSeconds = tTransitionParametersScript.TransitionInSpeed;
//				m_FadeOutSeconds = tTransitionParametersScript.TransitionOutSpeed;
//
//				m_ThisSceneLoaded = tTransitionParametersScript.NextSceneLoaded;
//				m_FadeInStart = tTransitionParametersScript.TransitionInStart;
//				m_FadeInFinish = tTransitionParametersScript.TransitionInFinish;
//				m_ThisSceneEnable = tTransitionParametersScript.NextSceneEnable;
//
//				m_ThisSceneDisable = tTransitionParametersScript.ThisSceneDisable;
//				m_FadeOutStart = tTransitionParametersScript.TransitionOutStart;
//				m_FadeOutFinish = tTransitionParametersScript.TransitionOutFinish;
//				m_ThisSceneWillUnloaded = tTransitionParametersScript.ThisSceneWillUnloaded;
//			}
//
//			if (tTransitionStandByScript == null)
//			{
//				m_StandByInSeconds = 5.0f;
//				m_AutoLoadNextScene = true;
//				m_NextSceneLoadingStart = null;
//				m_NextSceneLoadingPercent = null;
//				m_NextSceneLoadingFinish = null;
//				m_StandByFinish = null;
//			} 
//			else 
//			{
//				m_StandByInSeconds = tTransitionStandByScript.StandByInSeconds;
//				m_AutoLoadNextScene = tTransitionStandByScript.AutoLoadNextScene;
//				m_NextSceneLoadingStart = tTransitionStandByScript.LoadNextSceneStart;
//				m_NextSceneLoadingPercent = tTransitionStandByScript.LoadingNextScenePercent;
//				m_NextSceneLoadingFinish = tTransitionStandByScript.LoadNextSceneFinish;
//				m_StandByFinish = tTransitionStandByScript.StandByFinish;
//			}
//
//			if (sStartTransition == true) {
////			m_FadeInPreviewColor = m_FadeOutColor;
//				m_FadeOutPreviewColor = m_FadeOutColor;
//			}

			return tTransitionParametersScript;
		}



		private STSTransitionStandBy GetStandByParams (Scene sScene)
		{
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
				GameObject tObjToSpawn = new GameObject ("TransitionControllerObject");
				tObjToSpawn.AddComponent<STSTransitionStandBy> ();
				tTransitionStandByScript = (STSTransitionStandBy)tObjToSpawn.GetComponent<STSTransitionStandBy> ();
				tTransitionStandByScript.StandBySeconds = 5.0f;
				tTransitionStandByScript.AutoLoadNextScene = true;
			}

			return tTransitionStandByScript;
		}












		// Fade effect method
		private STSAnimationStyle m_AnimationStyle = STSAnimationStyle.None;
		private Texture2D m_AnimationTexture;
//		private Texture2D m_FadeInTexture;
//		private Texture2D m_FadeOutTexture;

		public Color m_AnimationColor = Color.yellow;
//		private Color m_FadeInColor = Color.red;
//		private Color m_FadeOutColor = Color.red;

		public Color m_AnimationPreviewColor = Color.yellow;
//		private Color m_FadeOutPreviewColor = Color.red;

//		private float m_FadeInSeconds = 1.0f;
		// the fadding speed
//		private float m_FadeOutSeconds = 1.0f;
		// the fadding speed
		private float m_AnimationSpeed = 0.8f;
		// the fadding speed

		private int m_DrawDepth = -1000;
		// the texture's order in the draw hiearchy: a low number means it rendre on top
		//private float m_AlphaValue = 0.0f;
		//the textur's alpha value between 0 and 1

		// Animation counter
		private float m_AnimationCounter = 0.0f;
		private int m_AnimationDirection = -1;
		// the direction to fade : in = -1 or out = 1

		private bool m_AnimationFinish = false;
		private bool m_AnimationInProgress = false;


		private bool AnimationProgress ()
		{
			return m_AnimationInProgress;
		}

		private bool AnimationFinished ()
		{
			return m_AnimationFinish;
		}

		private void AnimationTransitionIn (STSTransitionParameters sThisSceneParameters)
		{
			m_AnimationCounter = 1.0f;
			m_AnimationStyle = sThisSceneParameters.AnimationIn.Style;
			m_AnimationTexture = sThisSceneParameters.AnimationIn.Texture;
			m_AnimationColor = sThisSceneParameters.AnimationIn.Color;
			m_AnimationSpeed = 0.50f / sThisSceneParameters.AnimationIn.Seconds; // m_AlphaValue += m_FadeDirection * m_FadeSpeed * Time.deltaTime;
			//Debug.Log ("m_OldSceneParams = " + m_OldSceneParams);
			if (m_OldSceneParams != null) 
				{
				m_AnimationPreviewColor = m_OldSceneParams.AnimationOut.Color;
				} 
				else 
				{
				//Debug.Log ("m_OldSceneParams is null ?!");
				m_AnimationPreviewColor = sThisSceneParameters.AnimationOut.Color;
				}
				m_AnimationDirection = -1;
				m_AnimationInProgress = true;
				m_AnimationFinish = false;
		}

		private void AnimationTransitionOut (STSTransitionParameters sThisSceneParameters)
		{
			m_AnimationCounter = 0.0f;
			m_AnimationStyle = sThisSceneParameters.AnimationOut.Style;
			m_AnimationTexture = sThisSceneParameters.AnimationOut.Texture;
			m_AnimationColor = sThisSceneParameters.AnimationOut.Color;
			m_AnimationSpeed = 0.50f / sThisSceneParameters.AnimationOut.Seconds; // m_AlphaValue += m_FadeDirection * m_FadeSpeed * Time.deltaTime;
			if (m_OldSceneParams != null) 
			{
				m_AnimationPreviewColor = m_OldSceneParams.AnimationOut.Color;
			} 
			else 
			{
				m_AnimationPreviewColor = sThisSceneParameters.AnimationOut.Color;
			}
			m_AnimationDirection = 1;
			m_AnimationInProgress = true;
			m_AnimationFinish = false;
		}

//		private float BeginFade (int sDirection)
//		{
//			m_AnimationDirection = sDirection;
//			m_AnimationInProgress = true;
//			m_AnimationFinish = false;
//			return (m_AnimationSpeed); // return the fadespeed variable so it's easy to time the appplicationLoadLevel;
//		}
//
//		private void FadeInImmediatly ()
//		{
//			//Debug.Log ("FadeInImmediatly");
//			m_AnimationInProgress = true;
//			m_AnimationFinish = false;
//			m_AlphaValue = 0;
//		}
//
//		private void FadeOutImmediatly ()
//		{
//			//Debug.Log ("FadeOutImmediatly");
//			m_AnimationInProgress = true;
//			m_AnimationFinish = false;
//			m_AlphaValue = 1;
//		}

		// Stand by method

//		private float m_StandByInSeconds = 5.0f;
//		private bool m_AutoLoadNextScene = true;
		private float m_StandByTimer;
		private bool m_LauchNextScene = false;
		private bool m_StandByInProgress = false;

		private void StandBy ()
		{
			m_StandByTimer = 0.0f;
			m_StandByInProgress = true;
			m_LauchNextScene = false;
		}

		private bool StandByIsProgressing ()
		{
			return m_StandByInProgress;
		}

		private bool StandByIsFinished ()
		{
			return m_LauchNextScene;
		}

		private bool WaitingToLauchNextScene ()
		{
			return !m_LauchNextScene;
		}

		public static void FinishStandBy ()
		{
            STSTransitionController.Singleton();
			k_Singleton.EventSystemEnable (k_Singleton.m_IntermediateScene, false);
			k_Singleton.m_LauchNextScene = true;
			k_Singleton.m_StandByInProgress = true;
		}



	// animations



		// override method

		void OnGUI ()
		{

			// waiting StandBy
			if (m_IntermediateStandByParams != null) 
			{
				if (m_StandByInProgress == true) 
				{
					m_StandByTimer += Time.deltaTime;
					if (m_StandByTimer >= m_IntermediateStandByParams.StandBySeconds) 
					{
						m_StandByInProgress = false;
						if (m_IntermediateStandByParams.AutoLoadNextScene == true) 
						{
							m_LauchNextScene = true;
						}
					}
				}
			}
			switch (m_AnimationStyle) {
			case STSAnimationStyle.FadeIn:
				{
					Animation_FadeIn ();
				}
				break;
			case STSAnimationStyle.FadeOut:
				{
					Animation_FadeOut ();
				}
				break;
			case STSAnimationStyle.ShutterBottomIn:
				{
					Animation_ShutterCutting (1, 1, 10, 1, 0, 1);
				}
				break;
			case STSAnimationStyle.ShutterTopIn:
				{
					Animation_ShutterCutting (1, 1, 10, 1, 0, -1);
				}
				break;
			case STSAnimationStyle.ShutterRightIn:
				{
					Animation_ShutterCutting (1, 1, 1, 10, 1, 0);
				}
				break;
			case STSAnimationStyle.ShutterLeftIn:
				{
					Animation_ShutterCutting (1, 1, 1, 10, -1, 0);
				}
				break;
			case STSAnimationStyle.ShutterBottomOut:
				{
					Animation_ShutterCutting (1, 1, 10, 1, 0, 1);
				}
				break;
			case STSAnimationStyle.ShutterTopOut:
				{
					Animation_ShutterCutting (1, 1, 10, 1, 0, -1);
				}
				break;
			case STSAnimationStyle.ShutterRightOut:
				{
					Animation_ShutterCutting (1, 1, 1, 10, 1, 0);
				}
				break;
			case STSAnimationStyle.ShutterLeftOut:
				{
					Animation_ShutterCutting (1, 1, 1, 10, -1, 0);
				}
				break;
			}

			//			if (m_AnimationStyle == TransitionParametersScript.AnimationStyle.FadeIn) 
			//			{
			//				Animation_FadeIn ();
			//
			//			}
			//			else if (m_AnimationStyle == TransitionParametersScript.AnimationStyle.FadeOut) 
			//			{
			//				Animation_FadeOut ();
			//
			//			}
		}



	void Animation_Next() {
			//Debug.Log ("Animation_Next m_AnimationInProgress =" + m_AnimationInProgress);
		if (m_AnimationInProgress == true) 
			{
				m_AnimationCounter +=  m_AnimationDirection * m_AnimationSpeed * Time.deltaTime;
				m_AnimationCounter = Mathf.Clamp01 (m_AnimationCounter);
				if (m_AnimationCounter == 0 && m_AnimationDirection == -1) 
				{
					m_AnimationFinish = true;
					m_AnimationInProgress = false;
				}
				else if (m_AnimationCounter == 1 && m_AnimationDirection == 1) 
				{
					m_AnimationFinish = true;
					m_AnimationInProgress = false;
				}
			}
		}

	private void Animation_FadeIn() 
	{
		Animation_Fade(-1);
	}
	private void Animation_FadeOut() 
	{
		Animation_Fade(1);
	}
	private void Animation_Fade(float sDirection) 
	{
			float tAlpha = m_AnimationCounter;
//			if (sDirection < 0) {
//				tAlpha = 1 - tAlpha;
//			}
			//Debug.Log ("Animation_Fade direction = " + sDirection.ToString() + " alpha value = " + tAlpha.ToString() + " m_AnimationCounter = " + m_AnimationCounter.ToString());
		// Draw the animation
			Color tfadeColor = Color.Lerp (m_AnimationColor, m_AnimationPreviewColor, m_AnimationCounter*2.0f);
		//Color tfadeColor = m_AnimationColor;
			Color tfadeColorAlpha = new Color (tfadeColor.r, tfadeColor.g, tfadeColor.b, tAlpha);
		if (m_AnimationTexture == null) 
		{
			DrawQuad (new Rect (0, 0, Screen.width, Screen.height), tfadeColorAlpha);
		} 
		else 
		{
			GUI.color = tfadeColorAlpha;	// set the alpha value
			GUI.depth = m_DrawDepth; 	// make the black texture render on top (draw last)
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), m_AnimationTexture); // draw the texture to fit the entire screen area
		}

			// calculate the next animation
			Animation_Next();
		}

		void Animation_Shutter (float sWidth, float sHeight, float sWidthCutting, float sHeightCutting, float sXDirection, float sYDirection)
		{
			Color tfadeColor = Color.Lerp (m_AnimationColor, m_AnimationPreviewColor, m_AnimationCounter*2.0f);
			float tX = (1-m_AnimationCounter)*sXDirection;
			float tY = (1-m_AnimationCounter)*sYDirection;

			if (sYDirection == 0) {
				tY = 0;
			}
			if (sXDirection == 0) {
				tX = 0;
			}
			Debug.Log ("tX = " + tX.ToString() + "  tY = " + tY.ToString());
			Rect tRect = new Rect (Screen.width*tX, Screen.height*tY, Screen.width, Screen.height);
			if (m_AnimationTexture == null) 
			{
				DrawQuad (tRect, tfadeColor);
			} 
			else 
			{
				GUI.color = tfadeColor;	// set the alpha value
				GUI.depth = m_DrawDepth; 	// make the black texture render on top (draw last)
				GUI.DrawTexture (tRect, m_AnimationTexture); // draw the texture to fit the entire screen area
			}
			// animation 
			Animation_Next();
		}


		void Animation_ShutterCutting (float sWidth, float sHeight, float sWidthCutting, float sHeightCutting, float sXDirection, float sYDirection)
		{
			Color tfadeColor = Color.Lerp (m_AnimationColor, m_AnimationPreviewColor, m_AnimationCounter*2.0f);
			float tX = (1-m_AnimationCounter)*sXDirection;
			float tY = (1-m_AnimationCounter)*sYDirection;

			if (sYDirection == 0) {
				tY = 0;
			}
			if (sXDirection == 0) {
				tX = 0;
			}
			float tIncrX = Screen.width / sWidthCutting;
			for (int i = 0; i < sWidthCutting; i++) 
			{
				float Tyy = Screen.height * tY - i * tIncrX;

				//Debug.Log ("tX = " + tX.ToString () + "  tY = " + tY.ToString ());
				Rect tRect = new Rect (Screen.width * tX +tIncrX*i, Tyy, tIncrX, Screen.height);
				if (m_AnimationTexture == null) {
					DrawQuad (tRect, tfadeColor);
				} else {
					GUI.color = tfadeColor;	// set the alpha value
					GUI.depth = m_DrawDepth; 	// make the black texture render on top (draw last)
					GUI.DrawTexture (tRect, m_AnimationTexture); // draw the texture to fit the entire screen area
				}
			}
			// animation 
			Animation_Next();
		}







		// graphic tool box

		void DrawQuad (Rect position, Color color)
		{
			Texture2D tTexture = new Texture2D (1, 1);
			tTexture.SetPixel (0, 0, color);
			tTexture.Apply ();
			GUI.depth = m_DrawDepth; 
			GUI.skin.box.normal.background = tTexture;
			GUI.Box (position, GUIContent.none);
		}

		// end class
	}

	// end namespace
}