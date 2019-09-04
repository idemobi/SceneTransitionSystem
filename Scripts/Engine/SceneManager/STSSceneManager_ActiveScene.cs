//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	SceneTransitionSystem for Unity3D
//
//  All rights reserved by ideMobi
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
    public partial class STSSceneManager : MonoBehaviour, STSTransitionInterface, STSIntermissionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void ActiveScene(string sSceneNameToActive, string sIntermissionSceneName = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ActiveScene(SceneManager.GetActiveScene().name, sSceneNameToActive, sIntermissionSceneName, sTransitionData);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_ActiveScene(string sActualActiveSceneName, string sSceneNameToActive, string sIntermissionSceneName = null, STSTransitionData sTransitionData = null)
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
                    if (string.IsNullOrEmpty(sIntermissionSceneName))
                    {
                        StartCoroutine(INTERNAL_ActiveSceneWithoutIntermissionAsync(sTransitionData, sActualActiveSceneName, sSceneNameToActive));
                    }
                    else
                    {
                        StartCoroutine(INTERNAL_ActiveSceneWithIntermissionAsync(sTransitionData, sActualActiveSceneName, sSceneNameToActive, sIntermissionSceneName));
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
        private IEnumerator INTERNAL_ActiveSceneWithoutIntermissionAsync(STSTransitionData sTransitionData, string sActualActiveSceneName, string sSceneNameToActive)
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
            if (tActualSceneParams.Interfaced != null)
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
        private IEnumerator INTERNAL_ActiveSceneWithIntermissionAsync(STSTransitionData sTransitionData, string sActualActiveSceneName, string sSceneNameToActive, string sIntermissionSceneName)
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
            if (tActualSceneParams.Interfaced != null)
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
            AsyncOperation tAsynchroneLoadIntermissionOperation;
            tAsynchroneLoadIntermissionOperation = SceneManager.LoadSceneAsync(sIntermissionSceneName, LoadSceneMode.Additive);
            tAsynchroneLoadIntermissionOperation.allowSceneActivation = false;
            while (tAsynchroneLoadIntermissionOperation.progress < 0.9f)
            {
                yield return null;
            }
            // Intermission scene will be active
            tAsynchroneLoadIntermissionOperation.allowSceneActivation = true;
            while (!tAsynchroneLoadIntermissionOperation.isDone)
            {
                yield return null;
            }
            // get Transition Scene
            Scene tIntermissionScene = SceneManager.GetSceneByName(sIntermissionSceneName);
            // Active the next scene as root scene 
            SceneManager.SetActiveScene(tIntermissionScene);
            // disable audiolistener of preview scene
            AudioListenerPrevent();
            // get params
            STSTransition tIntermissionSceneParams = GetTransitionsParams(tIntermissionScene, false);
            // disable the user interactions until fadein 
            EventSystemEnable(tIntermissionScene, false);
            // Intermission scene is loaded
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneLoaded(sTransitionData);
            }
            // animation in
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            // animation in Go!
            AnimationTransitionIn(tIntermissionSceneParams);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // animation in Finish
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            // enable the user interactions 
            EventSystemEnable(tIntermissionScene, true);
            // enable the user interactions 
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            // start stand by
            STSIntermission tIntermissionSceneStandBy = GetStandByParams(tIntermissionScene);
            if (tIntermissionSceneStandBy.Interfaced != null)
            {
                tIntermissionSceneStandBy.Interfaced.OnStandByStart(tIntermissionSceneStandBy);
            }
            StandBy();
            while (StandByIsProgressing(tIntermissionSceneStandBy))
            {
                yield return null;
            }
            // send call back for standby finished
            if (tIntermissionSceneStandBy.Interfaced != null)
            {
                tIntermissionSceneStandBy.Interfaced.OnStandByFinish(tIntermissionSceneStandBy);
            }
            // Waiting to load the next Scene
            while (WaitingToLauchNextScene(tIntermissionSceneStandBy))
            {
                //Debug.Log ("StandByIsNotFinished loop");
                yield return null;
            }
            // stanby is finish
            // disable user interactions on the transition scene
            EventSystemEnable(tIntermissionScene, false);


            // disable user interactions on the Intermission scene
            EventSystemEnable(tIntermissionScene, false);
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // Intermission scene Transition Out start 
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            // Intermission scene Transition Out GO! 
            AnimationTransitionOut(tIntermissionSceneParams);
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            // Intermission scene Transition Out finished! 
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            // fadeout is finish
            // will unloaded the Intermission scene
            if (tIntermissionSceneParams.Interfaced != null)
            {
                tIntermissionSceneParams.Interfaced.OnTransitionSceneWillUnloaded(sTransitionData);
            }
            AsyncOperation tAsynchroneUnloadTransition;
            tAsynchroneUnloadTransition = SceneManager.UnloadSceneAsync(sIntermissionSceneName);
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