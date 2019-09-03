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
        public static void AddSubScene(string sAdditionalSceneName, string sIntermediateSceneName = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, new List<string> { sAdditionalSceneName }, null, sIntermediateSceneName, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSubScenes(List<string> sAdditionalScenes, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, sAdditionalScenes, null, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScene(string sNextActiveScene, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, null, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, null, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScene(string sSceneToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, new List<string> { sSceneToRemove }, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScenes(List<string> sScenesToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name, null, sScenesToRemove, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScene(string sNextActiveScene, string sSceneToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, new List<string> { sSceneToRemove }, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScenes(string sNextActiveScene, List<string> sScenesToRemove, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, sScenesToRemove, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScene(string sNextActiveScene, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Remove(tScene.name);
            }
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, null, tScenesToRemove, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermediateScene = null, STSTransitionData sTransitionData = null, string sKey = null)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Add(tScene.name);
            }
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, tScenesToRemove, sIntermediateScene, sTransitionData, true, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_ChangeScenes(
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            string sIntermediateScene,
            STSTransitionData sTransitionData,
            bool sHistorical, string sKey)
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

                if (sHistorical == true)
                {
                    INTERNAL_AddNavigation(sNextActiveScene, sScenesToAdd, sIntermediateScene, sTransitionData, sKey);
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
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }

            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------

            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;

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
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                //Debug.Log("tSceneToRemove :" + tSceneToRemove);
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete, false);
                if (tSceneToDeleteParams.Interfaced != null)
                {
                    tSceneToDeleteParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
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
                Debug.Log("tSceneToRemove :" + tSceneToRemove + " finish!");
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
                    tAsyncOperationAdd.allowSceneActivation = true;
                    while (!tAsyncOperationAdd.isDone)
                    {
                        yield return null;
                    }
                    Scene tNextScene = SceneManager.GetSceneByName(tSceneToAdd);
                    STSTransition tNextSceneParams = GetTransitionsParams(tNextScene, false);
                    if (tNextSceneParams.Interfaced != null)
                    {
                        tNextSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                    EventSystemPrevent(false);
                    AudioListenerPrevent();
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " finish!");
                }
            }
            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            AudioListenerPrevent();
            // get params
            STSTransition sNextSceneParams = GetTransitionsParams(tNextActiveScene, false);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // INTERMEDIATE UNLOAD
            //-------------------------------
            if (tRemoveActual == true)
            {
                if (tActualSceneParams.Interfaced != null)
                {
                    tActualSceneParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                AsyncOperation tAsyncOperationIntermediateUnload = SceneManager.UnloadSceneAsync(sActualActiveScene);
                tAsyncOperationIntermediateUnload.allowSceneActivation = true; //? needed?
                                                                               //while (tAsyncOperationIntermediateUnload.progress < 0.9f)
                                                                               //{
                                                                               //    yield return null;
                                                                               //}
                                                                               //while (!tAsyncOperationIntermediateUnload.isDone)
                                                                               //{
                                                                               //    yield return null;
                                                                               //}
            }
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(sNextSceneParams);
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // fadein is finish
            EventSystemPrevent(true);
            // next scene is enable
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
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

            //AsyncOperation tAsyncOperation;
            Dictionary<string, AsyncOperation> tAsyncOperationList = new Dictionary<string, AsyncOperation>();
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            TransitionInProgress = true;
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
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            //-------------------------------
            // INTERMEDIATE SCENE LOAD AND ENABLE
            //-------------------------------
            // load transition scene async
            AsyncOperation tAsyncOperationIntermediate = SceneManager.LoadSceneAsync(sIntermediateScene, LoadSceneMode.Additive);
            tAsyncOperationIntermediate.allowSceneActivation = true;
            while (tAsyncOperationIntermediate.progress < 0.9f)
            {
                yield return null;
            }
            while (!tAsyncOperationIntermediate.isDone)
            {
                yield return null;
            }
            // get Transition Scene
            Scene tIntermediateScene = SceneManager.GetSceneByName(sIntermediateScene);
            // Active the next scene as root scene 
            SceneManager.SetActiveScene(tIntermediateScene);
            // disable audiolistener of preview scene
            AudioListenerPrevent();
            // get params
            STSTransition tIntermediateSceneParams = GetTransitionsParams(tIntermediateScene, false);
            // disable the user interactions until fadein 
            EventSystemEnable(tIntermediateScene, false);
            // intermediate scene is loaded
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
            }
            // animation in
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            // animation in Go!
            AnimationTransitionIn(tIntermediateSceneParams);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // animation in Finish
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // enable the user interactions 
            EventSystemEnable(tIntermediateScene, true);
            // enable the user interactions 
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }

            //-------------------------------
            // INTERMEDIATE SCENE START STAND BY
            //-------------------------------
            // start stand by

            STSIntermediate tIntermediateSceneStandBy = GetStandByParams(tIntermediateScene);
            if (tIntermediateSceneStandBy.Interfaced != null)
            {
                tIntermediateSceneStandBy.Interfaced.OnStandByStart(tIntermediateSceneStandBy);
            }

            StandBy();

            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------

            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;

            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------

            foreach (string tSceneToAdd in sScenesToAdd)
            {
                //Debug.Log("tSceneToAdd :" + tSceneToAdd);
                if (SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    if (tIntermediateSceneStandBy.Interfaced != null)
                    {
                        tIntermediateSceneStandBy.Interfaced.OnSceneAllReadyLoaded(sTransitionData, tSceneToAdd, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " allready finish!");
                }
                else
                {
                    if (tIntermediateSceneStandBy.Interfaced != null)
                    {
                        tIntermediateSceneStandBy.Interfaced.OnLoadNextSceneStart(sTransitionData, tSceneToAdd, tSceneCounter, 0.0F, 0.0F);
                    }

                    AsyncOperation tAsyncOperationAdd = SceneManager.LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive);
                    tAsyncOperationList.Add(tSceneToAdd, tAsyncOperationAdd);
                    tAsyncOperationAdd.allowSceneActivation = false;
                    while (tAsyncOperationAdd.progress < 0.9f)
                    {
                        if (tIntermediateSceneStandBy.Interfaced != null)
                        {
                            tIntermediateSceneStandBy.Interfaced.OnLoadingNextScenePercent(sTransitionData, tSceneToAdd, tSceneCounter, tAsyncOperationAdd.progress, (tSceneCounter + tAsyncOperationAdd.progress) / tSceneCount);
                        }
                        yield return null;
                    }
                    if (tIntermediateSceneStandBy.Interfaced != null)
                    {
                        tIntermediateSceneStandBy.Interfaced.OnLoadNextSceneFinish(sTransitionData, tSceneToAdd, tSceneCounter, 1.0F, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " 90%!");
                }
                tSceneCounter++;
            }
            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                //Debug.Log("tSceneToRemove :" + tSceneToRemove);
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete, false);
                if (tSceneToDeleteParams.Interfaced != null)
                {
                    tSceneToDeleteParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);
                    tAsyncOperationRemove.allowSceneActivation = true; //? needed?
                    //while (tAsyncOperationRemove.progress < 0.9f)
                    //{
                    //    if (tIntermediateSceneStandBy.Interfaced != null)
                    //    {
                    //        tIntermediateSceneStandBy.Interfaced.OnLoadingNextScenePercent(sTransitionData, tSceneToRemove, tSceneCounter, tAsyncOperationRemove.progress, (tSceneCounter + tAsyncOperationRemove.progress) / tSceneCount);
                    //    }
                    //    yield return null;
                    //}
                    //while (!tAsyncOperationRemove.isDone)
                    //{
                    //    yield return null;
                    //}
                }
                if (tIntermediateSceneStandBy.Interfaced != null)
                {
                    tIntermediateSceneStandBy.Interfaced.OnUnloadScene(sTransitionData, tSceneToRemove, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                }
                tSceneCounter++;
                Debug.Log("tSceneToRemove :" + tSceneToRemove + " finish!");
            }
            //-------------------------------
            // INTERMEDIATE STAND BY
            //-------------------------------
            while (StandByIsProgressing(tIntermediateSceneStandBy))
            {
                yield return null;
            }
            // As soon as possible 
            if (tIntermediateSceneStandBy.Interfaced != null)
            {
                tIntermediateSceneStandBy.Interfaced.OnStandByFinish(tIntermediateSceneStandBy);
            }
            // Waiting to load the next Scene
            while (WaitingToLauchNextScene(tIntermediateSceneStandBy))
            {
                //Debug.Log ("StandByIsNotFinished loop");
                yield return null;
            }

            //-------------------------------
            // INTERMEDIATE GO TO NEXT SCENE PROCESS
            //-------------------------------
            // stanby is finished And the next scene can be lauch
            // disable user interactions on the intermediate scene
            EventSystemEnable(tIntermediateScene, false);
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // intermediate scene Transition Out start 
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            // intermediate scene Transition Out GO! 
            AnimationTransitionOut(tIntermediateSceneParams);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // intermediate scene Transition Out finished! 
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            // fadeout is finish
            // will unloaded the intermediate scene
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
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
                    tAsyncOperationAdd.allowSceneActivation = true;
                    while (!tAsyncOperationAdd.isDone)
                    {
                        yield return null;
                    }
                    Scene tNextScene = SceneManager.GetSceneByName(tSceneToAdd);
                    STSTransition tNextSceneParams = GetTransitionsParams(tNextScene, false);
                    if (tNextSceneParams.Interfaced != null)
                    {
                        tNextSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                    EventSystemPrevent(false);
                    AudioListenerPrevent();
                    //Debug.Log("tSceneToAdd :" + tSceneToAdd + " finish!");
                }
            }
            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            AudioListenerPrevent();
            // get params
            STSTransition sNextSceneParams = GetTransitionsParams(tNextActiveScene, false);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // INTERMEDIATE UNLOAD
            //-------------------------------
            AsyncOperation tAsyncOperationIntermediateUnload = SceneManager.UnloadSceneAsync(tIntermediateScene);
            tAsyncOperationIntermediateUnload.allowSceneActivation = true; //? needed?
            while (tAsyncOperationIntermediateUnload.progress < 0.9f)
            {
                yield return null;
            }
            while (!tAsyncOperationIntermediateUnload.isDone)
            {
                yield return null;
            }

            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(sNextSceneParams);
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // fadein is finish
            EventSystemPrevent(true);
            // next scene is enable
            if (sNextSceneParams.Interfaced != null)
            {
                sNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================