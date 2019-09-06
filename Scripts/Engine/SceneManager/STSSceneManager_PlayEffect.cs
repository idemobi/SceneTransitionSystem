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
        public static void TransitionSimulate(STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            STSTransition tTransitionParams = Singleton().GetTransitionsParams(SceneManager.GetActiveScene(), true);
            Singleton().INTERNAL_PlayEffectWithCallBackTransition(tTransitionParams, sTransitionData, sDelegate);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void TransitionSimulate(string sSceneName, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            Singleton().INTERNAL_PlayEffectWithCallBackScene(sSceneName, sTransitionData, sDelegate);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void INTERNAL_TransitionSimulate(STSTransition sTransitionParams, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            Singleton().INTERNAL_PlayEffectWithCallBackTransition(sTransitionParams, sTransitionData, sDelegate);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_PlayEffectWithCallBackScene(string sSceneName, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            if (TransitionInProgress == false)
            {
                List<string> tAllScenesList = new List<string>();
                tAllScenesList.Add(sSceneName);
                if (ScenesAreAllInBuild(tAllScenesList) == false)
                {
                    Debug.LogWarning(K_SCENE_UNKNOW);
                    return;
                }
                List<string> tScenes = new List<string>();
                for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
                {
                    Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                    tScenes.Add(tScene.name);
                }
                if (tScenes.Contains(sSceneName))
                {
                    STSTransition tTransitionParams = GetTransitionsParams(SceneManager.GetSceneByName(sSceneName), true);
                    StartCoroutine(INTERNAL_PlayEffectWithCallBackSceneAsync(tTransitionParams, sTransitionData, sDelegate));
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
        private void INTERNAL_PlayEffectWithCallBackTransition(STSTransition sTransitionParams, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            if (TransitionInProgress == false)
            {
                StartCoroutine(INTERNAL_PlayEffectWithCallBackSceneAsync(sTransitionParams, sTransitionData, sDelegate));
            }
            else
            {
                Debug.LogWarning(K_TRANSITION_IN_PROGRESS);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator INTERNAL_PlayEffectWithCallBackSceneAsync(STSTransition sTransitionParams, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            TransitionInProgress = true;
            EventSystemPrevent(false);
            if (sTransitionParams.Interfaced != null)
            {
                sTransitionParams.Interfaced.OnTransitionSceneDisable(sTransitionData);
            }
            AnimationTransitionOut(sTransitionParams, sTransitionData);
            if (sTransitionParams.Interfaced != null)
            {
                sTransitionParams.Interfaced.OnTransitionExitStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (sTransitionParams.Interfaced != null)
            {
                sTransitionParams.Interfaced.OnTransitionExitFinish(sTransitionData);
            }
            if (sDelegate != null)
            {
                sDelegate(sTransitionData);
            }
            AnimationTransitionIn(sTransitionParams, sTransitionData);
            if (sTransitionParams.Interfaced != null)
            {
                sTransitionParams.Interfaced.OnTransitionEnterStart(sTransitionData);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            if (sTransitionParams.Interfaced != null)
            {
                sTransitionParams.Interfaced.OnTransitionEnterFinish(sTransitionData);
            }
            EventSystemPrevent(true);
            if (sTransitionParams.Interfaced != null)
            {
                sTransitionParams.Interfaced.OnTransitionSceneEnable(sTransitionData);
            }
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================