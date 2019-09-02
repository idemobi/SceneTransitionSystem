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
        public static void ActiveScene(string sSceneNameToActive, string sIntermediateSceneName = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ActiveScene(SceneManager.GetActiveScene().name, sSceneNameToActive, sIntermediateSceneName, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_ActiveScene(string sActualActiveSceneName, string sSceneNameToActive, string sIntermediateSceneName = null, STSTransitionData sTransitionData = null)
        {
            if (TransitionInProgress == false)
            {
                List<string> tScenes = new List<string>();
                for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
                {
                    Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                    tScenes.Add(tScene.name);
                }
                if (tScenes.Contains(sActualActiveSceneName) && tScenes.Contains(sSceneNameToActive))
                {
                    if (string.IsNullOrEmpty(sIntermediateSceneName))
                    {
                        StartCoroutine(INTERNAL_ActiveSceneWithoutIntermediateAsync(sTransitionData, sActualActiveSceneName, sSceneNameToActive));
                    }
                    else
                    {
                        StartCoroutine(INTERNAL_ActiveSceneWithIntermediateAsync(sTransitionData, sActualActiveSceneName, sSceneNameToActive, sIntermediateSceneName));
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
        private IEnumerator INTERNAL_ActiveSceneWithoutIntermediateAsync(STSTransitionData sTransitionData, string sActualActiveSceneName, string sSceneNameToActive)
        {
            TransitionInProgress = true;
            STSTransition tActualSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sActualActiveSceneName), true);
            STSTransition tNextSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sSceneNameToActive), true);
            EventSystemPrevent(false);
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            AnimationTransitionOut(tActualSceneParams);
            if (ActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sSceneNameToActive));
            AudioListenerPrevent();
            AnimationTransitionIn(tNextSceneParams);
            if (tNextSceneParams.Interfaced != null)
            {
                tNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (tNextSceneParams.Interfaced != null)
            {
                tNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            EventSystemPrevent(true);
            if (tNextSceneParams.Interfaced != null)
            {
                tNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator INTERNAL_ActiveSceneWithIntermediateAsync(STSTransitionData sTransitionData, string sActualActiveSceneName, string sSceneNameToActive, string sIntermediateSceneName)
        {
            TransitionInProgress = true;
            STSTransition tActualSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sActualActiveSceneName), true);
            STSTransition tNextSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sSceneNameToActive), true);
            EventSystemPrevent(false);
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            AnimationTransitionOut(tActualSceneParams);
            if (ActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (tActualSceneParams.Interfaced != null)
            {
                tActualSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }

            //TODO: load intermedite scene 
            // load transition scene async
            AsyncOperation tAsynchroneLoadIntermediateOperation;
            tAsynchroneLoadIntermediateOperation = SceneManager.LoadSceneAsync(sIntermediateSceneName, LoadSceneMode.Additive);
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
            Scene tIntermediateScene = SceneManager.GetSceneByName(sIntermediateSceneName);
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
            // start stand by
            STSIntermediate tIntermediateSceneStandBy = GetStandByParams(tIntermediateScene);
            if (tIntermediateSceneStandBy.Interfaced != null)
            {
                tIntermediateSceneStandBy.Interfaced.OnStandByStart(tIntermediateSceneStandBy);
            }
            StandBy();
            while (StandByIsProgressing(tIntermediateSceneStandBy))
            {
                yield return null;
            }
            // send call back for standby finished
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
            // stanby is finish
            // disable user interactions on the transition scene
            EventSystemEnable(tIntermediateScene, false);


            // disable user interactions on the intermediate scene
            EventSystemEnable(tIntermediateScene, false);
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionSceneDisable(TransitionData);
            }
            // intermediate scene Transition Out start 
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionEnterStart(TransitionData);
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
                tIntermediateSceneParams.Interfaced.OnTransitionExitFinish(TransitionData);
            }
            // fadeout is finish
            // will unloaded the intermediate scene
            if (tIntermediateSceneParams.Interfaced != null)
            {
                tIntermediateSceneParams.Interfaced.OnTransitionSceneWillUnloaded(TransitionData);
            }
            AsyncOperation tAsynchroneUnloadTransition;
            tAsynchroneUnloadTransition = SceneManager.UnloadSceneAsync(sIntermediateSceneName);
            while (tAsynchroneUnloadTransition.progress < 0.9f)
            {
                yield return null;
            }
            while (!tAsynchroneUnloadTransition.isDone)
            {
                yield return null;
            }



            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sSceneNameToActive));
            AudioListenerPrevent();
            AnimationTransitionIn(tNextSceneParams);
            if (tNextSceneParams.Interfaced != null)
            {
                tNextSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (tNextSceneParams.Interfaced != null)
            {
                tNextSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            EventSystemPrevent(true);
            if (tNextSceneParams.Interfaced != null)
            {
                tNextSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================