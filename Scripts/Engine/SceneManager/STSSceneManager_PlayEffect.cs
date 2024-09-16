using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;


namespace SceneTransitionSystem
{
    /// <summary>
    /// Manages scene transitions and intermissions within the Scene Transition System (STS).
    /// This class provides methods to simulate transitions with optional data and callbacks.
    /// </summary>
    public partial class STSSceneManager : STSSingletonUnity<STSSceneManager>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Simulates a transition effect for the active scene.
        /// </summary>
        /// <param name="sTransitionData">Optional transition data to customize the transition effect.</param>
        /// <param name="sDelegate">Optional delegate to execute after the transition completes.</param>
        public static void TransitionSimulate(STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            Singleton().INTERNAL_PlayEffectWithCallBackTransition(SceneManager.GetActiveScene(), sTransitionData, sDelegate);
        }

        /// <summary>
        /// Simulates a scene transition effect and invokes a callback delegate if provided.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to transition to.</param>
        /// <param name="sTransitionData">Optional data for the transition effect.</param>
        /// <param name="sDelegate">Optional delegate to be called after the transition.</param>
        public static void TransitionSimulate(string sSceneName, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            Singleton().INTERNAL_PlayEffectWithCallBackScene(sSceneName, sTransitionData, sDelegate);
        }

        /// <summary>
        /// Initiates the playback of a scene transition effect and invokes a callback when the transition is complete.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to transition to.</param>
        /// <param name="sTransitionData">Optional transition data parameters.</param>
        /// <param name="sDelegate">Optional callback delegate to be invoked after the transition effect completes.</param>
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
                    Scene tScene = SceneManager.GetSceneByName(sSceneName);
                    StartCoroutine(INTERNAL_PlayEffectWithCallBackSceneAsync(tScene, sTransitionData, sDelegate));
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
        /// Handles the transition to a new scene with an optional effect and callback.
        /// </summary>
        /// <param name="sScene">The scene to transition to.</param>
        /// <param name="sTransitionData">Optional data for the transition effect.</param>
        /// <param name="sDelegate">Optional callback delegate to be invoked after the transition.</param>
        private void INTERNAL_PlayEffectWithCallBackTransition(Scene sScene, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            if (TransitionInProgress == false)
            {
                StartCoroutine(INTERNAL_PlayEffectWithCallBackSceneAsync(sScene, sTransitionData, sDelegate));
            }
            else
            {
                Debug.LogWarning(K_TRANSITION_IN_PROGRESS);
            }
        }

        /// <summary>
        /// Plays the transition effect for a given scene asynchronously and executes a callback once the transition is complete.
        /// </summary>
        /// <param name="sScene">The scene to which the transition effect should be applied.</param>
        /// <param name="sTransitionData">Optional. The transition data containing parameters for the transition.</param>
        /// <param name="sDelegate">Optional. The callback delegate to be executed once the transition is complete.</param>
        /// <returns>An IEnumerator used for coroutine execution of the transition effect.</returns>
        private IEnumerator INTERNAL_PlayEffectWithCallBackSceneAsync(Scene sScene, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            TransitionInProgress = true;
            EventSystemPrevent(false);
            STSTransition tTransitionParams = GetTransitionsParams(sScene);
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(sScene);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }
            AnimationTransitionOut(tTransitionParams, sTransitionData);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tTransitionParams.EffectOnExit, true);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }
            if (sDelegate != null)
            {
                sDelegate(sTransitionData);
            }
            AnimationTransitionIn(tTransitionParams, sTransitionData);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tTransitionParams.EffectOnEnter, tTransitionParams.InterEffectDuration, true);
            }
            while (AnimationFinished() == false)
            {
                yield return null;
            }
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }
            EventSystemPrevent(true);
            CameraPrevent(true);
            AudioListenerPrevent(true);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }
            TransitionInProgress = false;
        }
        
    }
    
}
