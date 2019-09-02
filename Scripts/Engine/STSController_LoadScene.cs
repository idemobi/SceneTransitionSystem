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
using System;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class STSController : MonoBehaviour, STSTransitionInterface, STSIntermediateInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSubScene(string sAdditionalSceneName, string sIntermediateSceneName = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, new List<string> { sAdditionalSceneName }, null, sIntermediateSceneName, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSubScenes(List<string> sAdditionalScenes, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, sAdditionalScenes, null, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScene(string sNextActiveScene, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, null, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, null, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScene(string sSceneToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, new List<string> { sSceneToRemove }, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScenes(List<string> sScenesToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, sScenesToRemove, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScene(string sNextActiveScene, string sSceneToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, new List<string> { sSceneToRemove }, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScenes(string sNextActiveScene, List<string> sScenesToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, sScenesToRemove, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScene(string sNextActiveScene, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Remove(tScene.name);
            }
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, tScenesToRemove, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermediateScene = null, STSTransitionData sTransitionData = null)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Remove(tScene.name);
            }
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, tScenesToRemove, sIntermediateScene, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_ChangeScenes(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            string sIntermediateScene,
            STSTransitionData sTransitionData)
        {
            if (TransitionInProgress == true)
            {
                if (sScenesToAdd == null)
                {
                    sScenesToAdd = new List<string>();
                }
                if (sScenesToRemove == null)
                {
                    sScenesToRemove = new List<string>();
                }
                if (sScenesToAdd.Contains(sActualActiveScene) == true)
                {
                    sScenesToAdd.Remove(sActualActiveScene);
                }
                if (sScenesToRemove.Contains(sNextActiveScene) == true)
                {
                    sScenesToRemove.Remove(sNextActiveScene);
                }
                if (sScenesToAdd.Contains(sNextActiveScene) == false)
                {
                    sScenesToAdd.Add(sNextActiveScene);
                }
                bool tPossible = false;
                List<Scene> tScenes = new List<Scene>();
                for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
                {
                    Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                    tScenes.Add(tScene);
                    sScenesToAdd.Remove(tScene.name);
                    if (tScene.name == sActualActiveScene)
                    {
                        tPossible = true;
                    }
                }
                if (tPossible == true)
                {
                    if (string.IsNullOrEmpty(sIntermediateScene))
                    {
                        StartCoroutine(INTERNAL_ChangeScenesWithoutIntermediate(sActualActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData));
                    }
                    else
                    {
                        StartCoroutine(INTERNAL_ChangeScenesWithIntermediate(sIntermediateScene, sActualActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData));
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
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator INTERNAL_ChangeScenesWithoutIntermediate(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            while (AnimationFinished() == false)
            {
                yield return null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator INTERNAL_ChangeScenesWithIntermediate(
            string sIntermediateScene,
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            //-------------------------------
            TransitionInProgress = false;
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActualActiveScene);
            //-------------------------------
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene, true);
            // disable the user interactions
            EventSystemPrevent(false);
            // post scene is disable!
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // post scene start effect transition out!
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData);
            }
            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams);
            // waiting effect will finish
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // post scene finish effcet transition out
            if (ActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }

            //TODO : Load sIntermediateScene or Activate sIntermediateScene

            //-------------------------------
            // load transition scene async
            AsyncOperation tAsynchroneLoadIntermediateOperation;
            tAsynchroneLoadIntermediateOperation = SceneManager.LoadSceneAsync(IntermediateSceneName, LoadSceneMode.Additive);
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
            // get params
            IntermediateSceneParams = GetTransitionsParams(IntermediateScene, false);
            // disable the user interactions until fadein 
            EventSystemEnable(IntermediateScene, false);
            // intermediate scene is loaded
            if (IntermediateSceneParams.Interfaced != null)
            {
                IntermediateSceneParams.Interfaced.OnTransitionSceneLoaded(TransitionData);
            }
            // animation in
            if (IntermediateSceneParams.Interfaced != null)
            {
                IntermediateSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
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
            // enable the user interactions 
            EventSystemEnable(IntermediateScene, true);
            // enable the user interactions 
            if (IntermediateSceneParams.Interfaced != null)
            {
                IntermediateSceneParams.Interfaced.OnTransitionSceneEnable(TransitionData);
            }
            // start stand by
            IntermediateSceneStandBy = GetStandByParams(IntermediateScene);
            if (IntermediateSceneStandBy.Interfaced != null)
            {
                IntermediateSceneStandBy.Interfaced.OnStandByStart(IntermediateSceneStandBy);
            }
            StandBy();

            //TODO : Load sNextActiveScene or Activate sNextActiveScene

            //TODO : Unload sScenesToRemove list 

            //TODO : Load sScenesToAdd list if necessary


            //TODO : play effect Out sIntermediateScene

            //TODO : active sNextActiveScene

            //TODO : Unload sIntermediateScene

            //TODO : play effect in sNextActiveScene



            // Transition setp 1 is finished this scene can be replace by the next or intermediate Scene
            //-------------------------------
            // ACTUAL SCENE IS CONCLUDED
            //-------------------------------
            // INTERMEDIATE SCENE PROCESS
            //-------------------------------
            if (string.IsNullOrEmpty(IntermediateSceneName) == false)
            {
                //-------------------------------
                // load transition scene async
                AsyncOperation tAsynchroneLoadIntermediateOperation;
                tAsynchroneLoadIntermediateOperation = SceneManager.LoadSceneAsync(IntermediateSceneName, LoadSceneMode.Additive);
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
                // get params
                IntermediateSceneParams = GetTransitionsParams(IntermediateScene, false);
                // disable the user interactions until fadein 
                EventSystemEnable(IntermediateScene, false);
                // intermediate scene is loaded
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionSceneLoaded(TransitionData);
                }
                // animation in
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
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
                // enable the user interactions 
                EventSystemEnable(IntermediateScene, true);
                // enable the user interactions 
                if (IntermediateSceneParams.Interfaced != null)
                {
                    IntermediateSceneParams.Interfaced.OnTransitionSceneEnable(TransitionData);
                }
                // start stand by
                IntermediateSceneStandBy = GetStandByParams(IntermediateScene);
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnStandByStart(IntermediateSceneStandBy);
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
                IntermediateSceneParams = GetTransitionsParams(ActualScene, true);
                // And the StandBy params are the preview scene StandBy params
                IntermediateSceneStandBy = GetStandByParams(ActualScene);

                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnStandByStart(IntermediateSceneStandBy);
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
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnLoadNextSceneStart(TransitionData, 0.0F);
                }
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
                    yield return null;
                }
                // need to send call back now (anticipate :-/ ) 
                if (IntermediateSceneStandBy.Interfaced != null)
                {
                    IntermediateSceneStandBy.Interfaced.OnLoadNextSceneFinish(TransitionData, 1.0f);
                }
                // scene is loaded!
                NextScene = SceneManager.GetSceneByName(NextSceneName);
                NextSceneParams = GetTransitionsParams(NextScene, false);
                if (NextSceneParams.Interfaced != null)
                {
                    NextSceneParams.Interfaced.OnTransitionSceneLoaded(TransitionData);
                }
                // when finish test if transition scene is allways in standby
                if (string.IsNullOrEmpty(IntermediateSceneName) == false)
                {
                    //-------------------------------
                    // INTERMEDIATE SCENE UNLOAD
                    //-------------------------------
                    EventSystemEnable(IntermediateScene, true);
                    // continue standby if it's necessary
                    while (StandByIsProgressing(IntermediateSceneStandBy))
                    {
                        yield return null;
                    }
                    // As soon as possible 
                    if (IntermediateSceneStandBy.Interfaced != null)
                    {
                        IntermediateSceneStandBy.Interfaced.OnStandByFinish(IntermediateSceneStandBy);
                    }
                    // Waiting to load the next Scene
                    while (WaitingToLauchNextScene(IntermediateSceneStandBy))
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
                    // intermediate scene Transition Out start 
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
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
                    // fadeout is finish
                    // will unloaded the intermediate scene
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                }
                tAsynchroneloadNext.allowSceneActivation = true;
                while (!tAsynchroneloadNext.isDone)
                {
                    yield return null;
                }
                // remove actual scene 
                if (LoadSceneModeSelected == LoadSceneMode.Single)
                {
                    // if m_SceneActual is allready loaded i need to unloaded it
                    if (SceneManager.GetSceneByName(ActualScene.name).isLoaded)
                    {
                        if (ActualSceneParams.Interfaced != null)
                        {
                            ActualSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                        }
                        AsyncOperation tAsynchroneUnloadActualScene;
                        tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync(ActualScene.name);
                        while (tAsynchroneUnloadActualScene.progress < 0.9f)
                        {
                            yield return null;
                        }
                        while (!tAsynchroneUnloadActualScene.isDone)
                        {
                            yield return null;
                        }
                    }
                }
            }
            else
            {
                //-------------------------------
                // NEXT SCENE ALLREADY LOADED
                //-------------------------------
                // remove actual scene 
                if (LoadSceneModeSelected == LoadSceneMode.Single)
                {
                    // if m_SceneActual is allready loaded i need to unloaded it
                    if (SceneManager.GetSceneByName(ActualScene.name).isLoaded)
                    {
                        if (ActualSceneParams.Interfaced != null)
                        {
                            ActualSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                        }
                        AsyncOperation tAsynchroneUnloadActualScene;
                        tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync(ActualScene.name);
                        while (tAsynchroneUnloadActualScene.progress < 0.9f)
                        {
                            yield return null;
                        }
                        while (!tAsynchroneUnloadActualScene.isDone)
                        {
                            yield return null;
                        }
                    }
                }

                NextScene = SceneManager.GetSceneByName(NextSceneName);
                NextSceneParams = GetTransitionsParams(NextScene, false);
                // when finish test if transition scene is allways in standby
                if (string.IsNullOrEmpty(IntermediateSceneName) == false)
                {
                    //-------------------------------
                    // INTERMEDIATE SCENE UNLOAD
                    //-------------------------------
                    // continue standby if it's necessary
                    EventSystemEnable(IntermediateScene, true);
                    while (StandByIsProgressing(IntermediateSceneStandBy))
                    {
                        yield return null;
                    }
                    // send call back for standby finished
                    if (IntermediateSceneStandBy.Interfaced != null)
                    {
                        IntermediateSceneStandBy.Interfaced.OnStandByFinish(IntermediateSceneStandBy);
                    }
                    // Waiting to load the next Scene
                    while (WaitingToLauchNextScene(IntermediateSceneStandBy))
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
                    // Transition scene disappear by fadeout 
                    AnimationTransitionOut(IntermediateSceneParams);
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionExitStart(TransitionData);
                    }
                    while (AnimationFinished() == false)
                    {
                        yield return null;
                    }
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionExitFinish(TransitionData);
                    }
                    // fadeout is finish
                    if (IntermediateSceneParams.Interfaced != null)
                    {
                        IntermediateSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
                    }
                }
            }
            NextScene = SceneManager.GetSceneByName(NextSceneName);
            // unload Intermediate Scene anyway
            if (LoadSceneModeSelected == LoadSceneMode.Additive && string.IsNullOrEmpty(IntermediateSceneName) == false)
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
            ////remove preview scene (if not remove before)
            //if (LoadSceneModeSelected == LoadSceneMode.Single)
            //{
            //    // if m_SceneActual is allready loaded i need to unloaded it
            //    if (SceneManager.GetSceneByName(ActualScene.name).isLoaded)
            //    {
            //        AsyncOperation tAsynchroneUnloadActualScene;
            //        tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync(ActualScene.name);
            //        if (ActualSceneParams.Interfaced != null)
            //        {
            //            ActualSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
            //        }
            //        //foreach (STSTransitionInterface tParameters in ListPreviewScene)
            //        //{
            //        //    tParameters.OnTransitionSceneWillUnloaded(TransitionData);
            //        //}
            //        while (tAsynchroneUnloadActualScene.progress < 0.9f)
            //        {
            //            yield return null;
            //        }

            //        while (!tAsynchroneUnloadActualScene.isDone)
            //        {
            //            yield return null;
            //        }
            //    }
            //    else
            //    {
            //        // Ok this scene allready unloading (perhaps in the intermediate scene)
            //    }
            //}
            // Active the next scene as root scene 
            SceneManager.SetActiveScene(NextScene);
            AudioListenerPrevent();
            //// remove preview scene 
            //if (LoadSceneModeSelected == LoadSceneMode.Single)
            //{
            //    // if m_SceneActual is allready loaded i need to unloaded it
            //    if (ActualScene.isLoaded)
            //    {
            //        AsyncOperation tAsynchroneUnloadActualScene;
            //        tAsynchroneUnloadActualScene = SceneManager.UnloadSceneAsync(ActualScene);
            //        while (tAsynchroneUnloadActualScene.progress < 0.9f)
            //        {
            //            yield return null;
            //        }
            //        if (ActualSceneParams.Interfaced != null)
            //        {
            //            ActualSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
            //        }
            //        //foreach (STSTransitionInterface tParameters in ListPreviewScene)
            //        //{
            //        //    tParameters.OnTransitionSceneWillUnloaded(TransitionData);
            //        //}
            //        while (!tAsynchroneUnloadActualScene.isDone)
            //        {
            //            yield return null;
            //        }
            //    }
            //}

            // get params
            NextSceneParams = GetTransitionsParams(NextScene, false);
            //ListNextScene = GetTransitionInterface(NextScene);
            //Debug.Log("ListNextScene count: =" + ListNextScene.Count);
            //List<STSTransitionInterface> tListNextScene = GetTransitionInterface(NextScene);
            // disable user interactions on the next scene
            EventSystemEnable(NextScene, false);
            // scene is loaded
            //if (tNextSceneAllRedayExist == false)
            //{
            //    if (NextSceneParams.Interfaced != null)
            //    {
            //        NextSceneParams.Interfaced.OnTransitionSceneLoaded(TransitionData);
            //    }
            //    //foreach (STSTransitionInterface tParameters in ListNextScene)
            //    //{
            //    //    Debug.Log("try OnTransitionSceneLoaded");
            //    //    tParameters.OnTransitionSceneLoaded(TransitionData);
            //    //}
            //}
            // Next scene appear by fade in 
            AnimationTransitionIn(NextSceneParams);
            if (NextSceneParams.Interfaced != null)
            {
                NextSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (NextSceneParams.Interfaced != null)
            {
                NextSceneParams.Interfaced.OnTransitionEnterFinish(TransitionData);
            }
            // fadein is finish
            // next scene user interaction enable
            EventSystemPrevent(true);
            // next scene is enable
            if (NextSceneParams.Interfaced != null)
            {
                NextSceneParams.Interfaced.OnTransitionSceneEnable(TransitionData);
            }
            NextSceneParams.CopyIn(OldSceneParams);
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================