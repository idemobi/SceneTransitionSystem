using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;


namespace SceneTransitionSystem
{
    /// <summary>
    /// Manages scene transitions in the Scene Transition System (STS).
    /// </summary>
    public partial class STSSceneManager : STSSingletonUnity<STSSceneManager>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Adds a sub-scene to the current active scene with optional transition and intermission scenes.
        /// </summary>
        /// <param name="sAdditionalSceneName">The name of the additional scene to add.</param>
        /// <param name="sIntermissionSceneName">The name of the intermission scene to display during the transition. Default is null.</param>
        /// <param name="sTransitionData">Optional transition data to apply during the scene change. Default is null.</param>
        /// <param name="sAllowCyclic">A boolean indicating whether cyclic transitions are allowed. Default is false.</param>
        public static void AddSubScene(string sAdditionalSceneName, string sIntermissionSceneName = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, new List<string> { sAdditionalSceneName }, null, sIntermissionSceneName, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Adds multiple sub-scenes to the current active scene.
        /// </summary>
        /// <param name="sAdditionalScenes">A list of names of the additional scenes to be added.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene, if any.</param>
        /// <param name="sTransitionData">An object containing transition data for the scene change.</param>
        /// <param name="sAllowCyclic">A flag indicating whether cyclic transitions are allowed.</param>
        public static void AddSubScenes(List<string> sAdditionalScenes, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, sAdditionalScenes, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Changes the currently active scene to the specified scene, optionally passing through an intermission scene
        /// and using specified transition data.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the next scene to be made active.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene to be used, if any. Defaults to null.</param>
        /// <param name="sTransitionData">Transition data to be used during the scene change. Defaults to null.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic transitions are allowed. Defaults to false.</param>
        public static void AddScene(string sNextActiveScene, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Adds multiple scenes to the scene manager with optional intermission and transition data.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the scene to be the next active scene.</param>
        /// <param name="sScenesToAdd">A list of scenes to be added.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene, if any.</param>
        /// <param name="sTransitionData">Optional transition data for scene transitions.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic transitions are allowed.</param>
        public static void AddScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Removes a specified sub-scene with optional intermission and transition data.
        /// </summary>
        /// <param name="sSceneToRemove">The name of the sub-scene to remove.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene to transition through, if any.</param>
        /// <param name="sTransitionData">Optional transition data for the scene removal process.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic transitions are allowed.</param>
        public static void RemoveSubScene(string sSceneToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, new List<string> { sSceneToRemove }, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Removes a list of sub scenes from the current active scene.
        /// </summary>
        /// <param name="sScenesToRemove">A list of scene names to be removed.</param>
        /// <param name="sIntermissionScene">An optional intermission scene to be shown during the transition.</param>
        /// <param name="sTransitionData">Optional transition data to manage the transition effects.</param>
        /// <param name="sAllowCyclic">If set to true, allows cyclic transitions.</param>
        public static void RemoveSubScenes(List<string> sScenesToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, sScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Removes a specified scene and transitions to the next active scene.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the scene to transition to after removing the specified scene.</param>
        /// <param name="sSceneToRemove">The name of the scene to be removed.</param>
        /// <param name="sIntermissionScene">Optional. The name of a scene to be used as an intermission during the transition.</param>
        /// <param name="sTransitionData">Optional. Data associated with the transition process.</param>
        /// <param name="sAllowCyclic">Optional. A boolean value that specifies whether cyclic transitions are allowed.</param>
        public static void RemoveScene(string sNextActiveScene, string sSceneToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, new List<string> { sSceneToRemove }, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Removes a list of scenes from the current active scene and transitions to the next scene.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the scene to transition to after removal.</param>
        /// <param name="sScenesToRemove">The list of scene names to be removed.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene, if any. Defaults to null.</param>
        /// <param name="sTransitionData">Additional data for the transition. Defaults to null.</param>
        /// <param name="sAllowCyclic">Flag to allow cyclic transitions. Defaults to false.</param>
        public static void RemoveScenes(string sNextActiveScene, List<string> sScenesToRemove, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, sScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Replaces the current scene with the specified next active scene, with optional intermission and transition parameters.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the next active scene to load.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene, if any, to use during the transition. Default is null.</param>
        /// <param name="sTransitionData">Additional transition data for the scene change. Default is null.</param>
        /// <param name="sAllowCyclic">Flag to indicate whether cyclic transitions are allowed. Default is false.</param>
        public static void ReplaceAllByScene(string sNextActiveScene, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            ReplaceAllByScenes(sNextActiveScene, null, sIntermissionScene, sTransitionData, sAllowCyclic);
        }

        /// <summary>
        /// Replaces the current active scene and all additional scenes with new scenes, optionally using an intermission scene and transition data.
        /// </summary>
        /// <param name="sNextActiveScene">The next active scene to be set after the replacement.</param>
        /// <param name="sScenesToAdd">Array of new scenes to be added.</param>
        /// <param name="sIntermissionScene">An optional intermission scene to be used during the transition.</param>
        /// <param name="sTransitionData">Optional data containing transition-related information.</param>
        /// <param name="sAllowCyclic">A boolean indicating whether cyclic transitions are allowed.</param>
        public static void ReplaceAllByScenes(STSScene sNextActiveScene, STSScene[] sScenesToAdd, STSScene sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            List<string> tScenesToAdd = new List<string>();
            if (sScenesToAdd != null)
            {
                foreach (STSScene tScene in sScenesToAdd)
                {
                    tScenesToAdd.Add(tScene.GetSceneShortName());
                }
            }

            string tNextActiveScene = string.Empty;
            if (sNextActiveScene != null)
            {
                tNextActiveScene = sNextActiveScene.GetSceneShortName();
            }

            string tIntermissionScene = string.Empty;
            if (sIntermissionScene != null)
            {
                tIntermissionScene = sIntermissionScene.GetSceneShortName();
            }

            ReplaceAllByScenes(tNextActiveScene, tScenesToAdd, tIntermissionScene, sTransitionData, sAllowCyclic);
        }

        /// <summary>
        /// Replaces all currently active scenes with a new set of scenes, including an optional intermission scene and transition data.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the scene to be the next active scene.</param>
        /// <param name="sScenesToAdd">A list of scene names to be added during the transition.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene to be used during the transition (optional).</param>
        /// <param name="sTransitionData">Transition data to be used during the scene transition (optional).</param>
        /// <param name="sAllowCyclic">A flag indicating whether cyclic transitions are allowed.</param>
        public static void ReplaceAllByScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Add(tScene.name);
            }

            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, tScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Changes the active scene, adding and removing scenes as specified, with optional intermission and transition data.
        /// </summary>
        /// <param name="sActualActiveScene">The name of the currently active scene.</param>
        /// <param name="sNextActiveScene">The name of the next scene to be made active.</param>
        /// <param name="sScenesToAdd">A list of scene names to add.</param>
        /// <param name="sScenesToRemove">A list of scene names to remove.</param>
        /// <param name="sIntermissionScene">The name of an optional intermission scene.</param>
        /// <param name="sTransitionData">Transition data for the scene change.</param>
        /// <param name="sHistorical">Indicates if the change should be logged for historical tracking.</param>
        /// <param name="sAllowCyclic">Indicates if cyclic scene changes are allowed.</param>
        private void INTERNAL_ChangeScenes(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            string sIntermissionScene,
            STSTransitionData sTransitionData,
            bool sHistorical,
            bool sAllowCyclic = false)
        {
            if (TransitionInProgress == false)
            {
                // null protection
                if (sScenesToAdd == null)
                {
                    sScenesToAdd = new List<string>();
                }

                if (sScenesToRemove == null)
                {
                    sScenesToRemove = new List<string>();
                }

                // trim
                while (sScenesToAdd.Contains(string.Empty) == true)
                {
                    sScenesToAdd.Remove(string.Empty);
                }

                while (sScenesToRemove.Contains(string.Empty) == true)
                {
                    sScenesToRemove.Remove(string.Empty);
                }

                // active scene protection
                if (SceneManager.GetSceneByName(sNextActiveScene).isLoaded == false)
                {
                    if (sScenesToAdd.Contains(sNextActiveScene) == false)
                    {
                        sScenesToAdd.Add(sNextActiveScene);
                    }
                }

                if (sScenesToRemove.Contains(sNextActiveScene) == true)
                {
                    sScenesToRemove.Remove(sNextActiveScene);
                }

                // futur scenes protection
                foreach (string tSceneName in sScenesToAdd)
                {
                    if (sScenesToRemove.Contains(tSceneName) == true)
                    {
                        sScenesToRemove.Remove(tSceneName);
                    }
                }

                // test possibilities
                bool tPossible = false;
                //List<Scene> tScenes = new List<Scene>(); // Unused ?
                for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
                {
                    Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                    //tScenes.Add(tScene);
                    sScenesToAdd.Remove(tScene.name);
                    if (tScene.name == sActualActiveScene)
                    {
                        tPossible = true;
                    }
                }

                List<string> tAllScenesListS = new List<string>();
                tAllScenesListS.Add(sNextActiveScene);
                tAllScenesListS.Add(sIntermissionScene);
                tAllScenesListS.AddRange(sScenesToAdd);
                tAllScenesListS.AddRange(sScenesToRemove);
                List<string> tAllScenesList = new List<string>();
                foreach (string tScen in tAllScenesListS)
                {
                    if (string.IsNullOrEmpty(tScen) == false)
                    {
                        if (tAllScenesList.Contains(tScen) == false)
                        {
                            tAllScenesList.Add(tScen);
                        }
                    }
                }

                if (ScenesAreAllInBuild(tAllScenesList) == false)
                {
                    Debug.LogWarning(K_SCENE_UNKNOW);
                    return;
                }

                if (sAllowCyclic == true && sNextActiveScene == sActualActiveScene)
                {
                    if (sScenesToAdd.Contains(sActualActiveScene) == false)
                    {
                        sScenesToAdd.Add(sActualActiveScene);
                    }

                    if (sScenesToRemove.Contains(sActualActiveScene) == false)
                    {
                        sScenesToRemove.Add(sActualActiveScene);
                    }
                }
                else if (sHistorical == true) // Never add a navigation for a cycled scene changed
                {
                    INTERNAL_AddNavigation(sNextActiveScene, sScenesToAdd, sIntermissionScene, sTransitionData);
                }

                if (tPossible == true)
                {
                    if (string.IsNullOrEmpty(sIntermissionScene))
                    {
                        StartCoroutine(INTERNAL_ChangeScenesWithoutIntermission(sActualActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData));
                    }
                    else
                    {
                        StartCoroutine(INTERNAL_ChangeScenesWithIntermission(sIntermissionScene, sActualActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData));
                    }
                }
                else
                {
                    Debug.LogWarning(K_SCENE_MUST_BY_LOADED);
                }
            }
            else
            {
                Debug.LogWarning(K_TRANSITION_IN_PROGRESS);
            }
        }

        /// <summary>
        /// Handles the transition between scenes without any intermission period.
        /// </summary>
        /// <param name="sActualActiveScene">The name of the currently active scene.</param>
        /// <param name="sNextActiveScene">The name of the scene to activate next.</param>
        /// <param name="sScenesToAdd">A list of scenes to add during the transition.</param>
        /// <param name="sScenesToRemove">A list of scenes to remove during the transition.</param>
        /// <param name="sTransitionData">Additional data required for the transition.</param>
        /// <returns>IEnumerator to control the scene transition process.</returns>
        private IEnumerator INTERNAL_ChangeScenesWithoutIntermission(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            bool tRemoveActual = false;
            if (sScenesToRemove.Contains(sActualActiveScene))
            {
                sScenesToRemove.Remove(sActualActiveScene);
                tRemoveActual = true;
            }

            //AsyncOperation tAsyncOperation;
            Dictionary<string, AsyncOperation> tAsyncOperationList = new Dictionary<string, AsyncOperation>();
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            TransitionInProgress = true;
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActualActiveScene);
            //-------------------------------
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(tActualScene);
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(tActualScene);
            // disable the user interactions
            EventSystemPrevent(false);
            // post scene is disable!
            //if (tActualSceneParams.Interfaced != null)
            //{
            //    tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }

            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            // post scene start effect transition out!
            //if (tActualSceneParams.Interfaced != null)
            //{
            //    tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit);
            //}
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, false);
            }

            // waiting effect will finish
            while (AnimationFinished() == false)
            {
                yield return null;
            }

            // post scene finish effcet transition out
            //if (tActualSceneParams.Interfaced != null)
            //{
            //    tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }

            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;
            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                //Debug.Log("tSceneToRemove :" + tSceneToRemove);
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                AudioListenerEnable(tSceneToDelete, false);
                CameraPreventEnable(tSceneToDelete, false);
                EventSystemEnable(tSceneToDelete, false);
                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                //STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete, false);
                //if (tSceneToDeleteParams.Interfaced != null)
                //{
                //    tSceneToDeleteParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                //}
                foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }

                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);
                    tAsyncOperationRemove.allowSceneActivation = true; //? needed?
                    //while (tAsyncOperationRemove.progress < 0.9f)
                    //{
                    //    yield return null;
                    //}
                    //while (!tAsyncOperationRemove.isDone)
                    //{
                    //    yield return null;
                    //}
                }

                tSceneCounter++;
                //Debug.Log("tSceneToRemove :" + tSceneToRemove + " finish!");
            }

            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                //Debug.Log("tSceneToAdd :" + tSceneToAdd);
                if (SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " allready finish!");
                }
                else
                {
                    AsyncOperation tAsyncOperationAdd = SceneManager.LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive);
                    tAsyncOperationList.Add(tSceneToAdd, tAsyncOperationAdd);
                    tAsyncOperationAdd.allowSceneActivation = false;
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " 90%!");
                }

                tSceneCounter++;
            }

            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tAsyncOperationList.ContainsKey(tSceneToAdd))
                {
                    Scene tSceneToLoad = SceneManager.GetSceneByName(tSceneToAdd);
                    AsyncOperation tAsyncOperationAdd = tAsyncOperationList[tSceneToAdd];
                    tAsyncOperationAdd.allowSceneActivation = true;
                    while (tAsyncOperationAdd.progress < 0.9f)
                    {
                        yield return null;
                    }

                    while (!tAsyncOperationAdd.isDone)
                    {
                        AudioListenerEnable(tSceneToLoad, false);
                        yield return null;
                    }

                    AudioListenerEnable(tSceneToLoad, false);
                    CameraPreventEnable(tSceneToLoad, false);
                    EventSystemEnable(tSceneToLoad, false);
                    STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tSceneToLoad);
                    foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                    {
                        tInterfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " finish!");
                }
            }

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            CameraPrevent(true);
            AudioListenerPrevent(true);
            // get params
            STSTransition tNextSceneParams = GetTransitionsParams(tNextActiveScene);
            STSTransitionInterface[] tNextSceneInterfaced = GetTransitionInterface(tNextActiveScene);
            STSTransitionInterface[] tOtherNextSceneInterfaced = GetOtherTransitionInterface(tNextActiveScene);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            if (tRemoveActual == true)
            {
                foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }

                AsyncOperation tAsyncOperationIntermissionUnload = SceneManager.UnloadSceneAsync(sActualActiveScene);
                tAsyncOperationIntermissionUnload.allowSceneActivation = true;
            }

            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(tNextSceneParams, sTransitionData);
            //if (sNextSceneParams.Interfaced != null)
            //{
            //    sNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData, sNextSceneParams.EffectOnEnter, sNextSceneParams.InterEffectDuration);
            //}
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            //if (sNextSceneParams.Interfaced != null)
            //{
            //    sNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, false);
            }

            // fadein is finish
            EventSystemPrevent(true);
            // next scene is enable
            //if (sNextSceneParams.Interfaced != null)
            //{
            //    sNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }

        /// <summary>
        /// Handles the transition between scenes with an intermission.
        /// </summary>
        /// <param name="sIntermissionScene">The intermission scene to load.</param>
        /// <param name="sActualActiveScene">The currently active scene before transition.</param>
        /// <param name="sNextActiveScene">The scene to load after the intermission.</param>
        /// <param name="sScenesToAdd">A list of scenes to add during the transition.</param>
        /// <param name="sScenesToRemove">A list of scenes to remove during the transition.</param>
        /// <param name="sTransitionData">Additional data related to the transition.</param>
        /// <returns>An IEnumerator used to manage the coroutine for the scene transition.</returns>
        private IEnumerator INTERNAL_ChangeScenesWithIntermission(
            string sIntermissionScene,
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            //AsyncOperation tAsyncOperation;
            Dictionary<string, AsyncOperation> tAsyncOperationList = new Dictionary<string, AsyncOperation>();
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            TransitionInProgress = true;
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActualActiveScene);
            //-------------------------------
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(tActualScene);
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(tActualScene);
            // disable the user interactions
            EventSystemPrevent(false);
            // post scene is disable!
            //if (tActualSceneParams.Interfaced != null)
            //{
            //    tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }

            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            // post scene start effect transition out!
            //if (tActualSceneParams.Interfaced != null)
            //{
            //    tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit);
            //}
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, false);
            }

            // waiting effect will finish
            while (AnimationFinished() == false)
            {
                yield return null;
            }

            // post scene finish effcet transition out
            //if (tActualSceneParams.Interfaced != null)
            //{
            //    tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }

            //-------------------------------
            // Intermission SCENE LOAD AND ENABLE
            //-------------------------------
            // load transition scene async
            AsyncOperation tAsyncOperationIntermission = SceneManager.LoadSceneAsync(sIntermissionScene, LoadSceneMode.Additive);
            tAsyncOperationIntermission.allowSceneActivation = true;
            while (tAsyncOperationIntermission.progress < 0.9f)
            {
                yield return null;
            }

            Scene tIntermissionScene = SceneManager.GetSceneByName(sIntermissionScene);
            while (!tAsyncOperationIntermission.isDone)
            {
                AudioListenerEnable(tIntermissionScene, false);
                yield return null;
            }

            // get Transition Scene
            // Active the next scene as root scene 
            SceneManager.SetActiveScene(tIntermissionScene);
            // disable audiolistener of preview scene
            CameraPrevent(true);
            AudioListenerPrevent(true);
            // disable the user interactions until fadein 
            EventSystemEnable(tIntermissionScene, false);
            // get params
            STSTransition tIntermissionSceneParams = GetTransitionsParams(tIntermissionScene);
            STSTransitionInterface[] tIntermissionSceneInterfaced = GetTransitionInterface(tIntermissionScene);
            STSIntermissionInterface[] tIntermissionInterfaced = GetIntermissionInterface(tIntermissionScene);
            STSIntermission tIntermissionSceneStandBy = GetStandByParams(tIntermissionScene);
            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;
            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                //Debug.Log("tSceneToRemove :" + tSceneToRemove);
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete);
                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                //if (tSceneToDeleteParams.Interfaced != null)
                //{
                //    tSceneToDeleteParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                //}
                foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }

                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);

                    if (tAsyncOperationRemove != null)
                    {
                        tAsyncOperationRemove.allowSceneActivation = true; //? needed?
                        //while (tAsyncOperationRemove.progress < 0.9f)
                        //{
                        //    if (tIntermissionSceneStandBy.Interfaced != null)
                        //    {
                        //        tIntermissionSceneStandBy.Interfaced.OnLoadingNextScenePercent(sTransitionData, tSceneToRemove, tSceneCounter, tAsyncOperationRemove.progress, (tSceneCounter + tAsyncOperationRemove.progress) / tSceneCount);
                        //    }
                        //    yield return null;
                        //}
                        //while (!tAsyncOperationRemove.isDone)
                        //{
                        //    yield return null;
                        //}
                    }
                    else
                    {
                        Debug.LogWarning("UnloadSceneAsync is not possible for " + tSceneToRemove);
                    }
                }

                foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                {
                    tInterfaced.OnUnloadScene(sTransitionData, tSceneToRemove, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                }

                tSceneCounter++;
                //Debug.Log("tSceneToRemove :" + tSceneToRemove + " finish!");
            }

            //-------------------------------
            // Intermission start enter animation
            //-------------------------------
            // get params
            // Intermission scene is loaded
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneLoaded(sTransitionData);
            }

            // animation in Go!
            AnimationTransitionIn(tIntermissionSceneParams, sTransitionData);
            // animation in
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnEnter, tIntermissionSceneParams.InterEffectDuration);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnEnter, tIntermissionSceneParams.InterEffectDuration, true);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            // animation in Finish
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }

            // enable the user interactions 
            EventSystemEnable(tIntermissionScene, true);
            // enable the user interactions 
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }

            //-------------------------------
            // Intermission SCENE START STAND BY
            //-------------------------------
            // start stand by
            foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
            {
                tInterfaced.OnStandByStart(tIntermissionSceneStandBy);
            }

            StandBy();
            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            foreach (string tSceneToLoad in sScenesToAdd)
            {
                //Debug.Log("tSceneToAdd :" + tSceneToAdd);
                Scene tSceneToAdd = SceneManager.GetSceneByName(tSceneToLoad);
                if (SceneManager.GetSceneByName(tSceneToLoad).isLoaded)
                {
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnSceneAllReadyLoaded(sTransitionData, tSceneToLoad, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                    }

                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " allready finish!");
                    AudioListenerEnable(tSceneToAdd, false);
                    CameraPreventEnable(tSceneToAdd, false);
                    EventSystemEnable(tSceneToAdd, false);
                }
                else
                {
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneStart(sTransitionData, tSceneToLoad, tSceneCounter, 0.0F, 0.0F);
                    }

                    AsyncOperation tAsyncOperationAdd = SceneManager.LoadSceneAsync(tSceneToLoad, LoadSceneMode.Additive);
                    tAsyncOperationList.Add(tSceneToLoad, tAsyncOperationAdd);
                    tAsyncOperationAdd.allowSceneActivation = false;
                    while (tAsyncOperationAdd.progress < 0.9f)
                    {
                        Debug.LogWarning(tAsyncOperationAdd.progress + ", (" + tSceneCounter + " + " + tAsyncOperationAdd.progress + ") /" + tSceneCount);
                        foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                        {
                            tInterfaced.OnLoadingScenePercent(sTransitionData, tSceneToLoad, tSceneCounter, tAsyncOperationAdd.progress, (tSceneCounter + tAsyncOperationAdd.progress) / tSceneCount);
                        }

                        yield return null;
                    }

                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneFinish(sTransitionData, tSceneToLoad, tSceneCounter, 1.0F, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " 90%!");
                }

                tSceneCounter++;
            }

            ;
            tIntermissionSceneStandBy.IsLoaded = true;
            //-------------------------------
            // Intermission STAND BY
            //-------------------------------
            while (StandByIsProgressing(tIntermissionSceneStandBy))
            {
                yield return null;
            }

            // As soon as possible 
            foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
            {
                tInterfaced.OnStandByFinish(tIntermissionSceneStandBy);
            }

            tIntermissionSceneStandBy.IsReadyToActivate = true;
            // Waiting to load the next Scene
            while (WaitingToLauchNextScene(tIntermissionSceneStandBy))
            {
                //Debug.Log ("StandByIsNotFinished loop");
                yield return null;
            }

            //-------------------------------
            // Intermission GO TO NEXT SCENE PROCESS
            //-------------------------------
            // stanby is finished And the next scene can be lauch
            // disable user interactions on the Intermission scene
            EventSystemEnable(tIntermissionScene, false);
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }

            // Intermission scene Transition Out GO! 
            AnimationTransitionOut(tIntermissionSceneParams, sTransitionData);
            // Intermission scene Transition Out start 
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnExit, tIntermissionSceneParams.InterEffectDuration);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnExit, tIntermissionSceneParams.InterEffectDuration, true);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            // Intermission scene Transition Out finished! 
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }

            // fadeout is finish
            // will unloaded the Intermission scene
            //if (tIntermissionSceneParams.Interfaced != null)
            //{
            //    tIntermissionSceneParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
            }

            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tAsyncOperationList.ContainsKey(tSceneToAdd))
                {
                    AsyncOperation tAsyncOperationAdd = tAsyncOperationList[tSceneToAdd];
                    Scene tSceneToLoad = SceneManager.GetSceneByName(tSceneToAdd);
                    tAsyncOperationAdd.allowSceneActivation = true;
                    while (!tAsyncOperationAdd.isDone)
                    {
                        //AudioListenerEnable(tSceneToLoad, false);
                        yield return null;
                    }

                    AudioListenerEnable(tSceneToLoad, false);
                    CameraPreventEnable(tSceneToLoad, false);
                    EventSystemEnable(tSceneToLoad, false);
                    STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tSceneToLoad);
                    foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                    {
                        tInterfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                }
            }

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            AudioListenerPrevent(true);
            CameraPrevent(true);
            // get params
            STSTransition sNextSceneParams = GetTransitionsParams(tNextActiveScene);
            STSTransitionInterface[] tNextSceneInterfaced = GetTransitionInterface(tNextActiveScene);
            STSTransitionInterface[] tOtherNextSceneInterfaced = GetOtherTransitionInterface(tNextActiveScene);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            AsyncOperation tAsyncOperationIntermissionUnload = SceneManager.UnloadSceneAsync(tIntermissionScene);
            if (tAsyncOperationIntermissionUnload != null)
            {
                tAsyncOperationIntermissionUnload.allowSceneActivation = true; //? needed?
                while (tAsyncOperationIntermissionUnload.progress < 0.9f)
                {
                    yield return null;
                }

                while (!tAsyncOperationIntermissionUnload.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                Debug.LogWarning("UnloadSceneAsync is not possible for " + tIntermissionScene);
            }

            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(sNextSceneParams, sTransitionData);
            //if (sNextSceneParams.Interfaced != null)
            //{
            //    sNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData, sNextSceneParams.EffectOnEnter, sNextSceneParams.InterEffectDuration);
            //}
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, sNextSceneParams.EffectOnEnter, sNextSceneParams.InterEffectDuration, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, sNextSceneParams.EffectOnEnter, sNextSceneParams.InterEffectDuration, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            //if (sNextSceneParams.Interfaced != null)
            //{
            //    sNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, false);
            }

            // fadein is finish
            EventSystemPrevent(true);
            // next scene is enable
            //if (sNextSceneParams.Interfaced != null)
            //{
            //    sNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            //}
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
    }
}