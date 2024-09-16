using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSAddressableAssets class manages scene transitions and intermissions
    /// within the Scene Transition System (STS). It serves as a singleton instance
    /// that interfaces with STSTransitionInterface and STSIntermissionInterface to
    /// facilitate various scene-related operations.
    /// </summary>
    public partial class STSAddressableAssets : STSSingletonUnity<STSAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Simulates a scene transition with optional transition data and completion callback delegate.
        /// </summary>
        /// <param name="sTransitionData">Optional transition data containing information about the transition. Defaults to null.</param>
        /// <param name="sDelegate">Optional callback delegate to be invoked upon completion of the transition. Defaults to null.</param>
        public static void TransitionSimulate(STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            Singleton().INTERNAL_PlayEffectWithCallBackTransition(SceneManager.GetActiveScene(), sTransitionData, sDelegate);
        }

        /// <summary>
        /// Simulates a scene transition with optional transition data and delegate callback.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to transition to.</param>
        /// <param name="sTransitionData">Optional data associated with the transition.</param>
        /// <param name="sDelegate">Optional delegate to be called upon the effect's completion.</param>
        public static void TransitionSimulate(string sSceneName, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            Singleton().INTERNAL_PlayEffectWithCallBackScene(sSceneName, sTransitionData, sDelegate);
        }

        /// <summary>
        /// Initiates the transition effect and executes a callback when the specified scene is loaded.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to transition to.</param>
        /// <param name="sTransitionData">Optional data related to the scene transition.</param>
        /// <param name="sDelegate">Optional delegate to be invoked once the transition effect completes.</param>
        private void INTERNAL_PlayEffectWithCallBackScene(string sSceneName, STSTransitionData sTransitionData = null, STSDelegate sDelegate = null)
        {
            if (TransitionInProgress == false)
            {
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
        /// Initiates a scene transition with an effect and an optional callback. If a transition is already in
        /// progress, it logs a warning message and does not start a new transition.
        /// </summary>
        /// <param name="sScene">The target scene for the transition.</param>
        /// <param name="sTransitionData">Optional transition data that contains additional information about the transition.</param>
        /// <param name="sDelegate">Optional callback delegate to be invoked after the transition is complete.</param>
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
        /// Handles scene transitions with effects and a callback asynchronously.
        /// </summary>
        /// <param name="sScene">The scene object to transition.</param>
        /// <param name="sTransitionData">Optional transition data containing parameters for the transition.</param>
        /// <param name="sDelegate">Optional delegate to be called after the transition completes.</param>
        /// <returns>Returns an IEnumerator that can be used to control the execution state of the coroutine.</returns>
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