using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;


namespace SceneTransitionSystem
{
    /// <summary>
    /// Manages scene transitions within the application. Uses Singleton pattern to ensure a single instance.
    /// Implements both STSTransitionInterface and STSIntermissionInterface to handle transition and intermission behaviors.
    /// </summary>
    public partial class STSSceneManager : STSSingletonUnity<STSSceneManager>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Activates a specified scene, optionally using an intermission scene and transition data.
        /// </summary>
        /// <param name="sSceneNameToActive">The name of the scene to activate.</param>
        /// <param name="sIntermissionSceneName">An optional parameter for the name of an intermission scene.</param>
        /// <param name="sTransitionData">An optional parameter containing transition data.</param>
        public static void ActiveScene(string sSceneNameToActive, string sIntermissionSceneName = null, STSTransitionData sTransitionData = null)
        {
            Singleton().INTERNAL_ActiveScene(SceneManager.GetActiveScene().name, sSceneNameToActive, sIntermissionSceneName, sTransitionData);
        }

        /// <summary>
        /// Switches the active scene to the specified scene, with optional intermission and transition data.
        /// </summary>
        /// <param name="sActualActiveSceneName">The name of the currently active scene.</param>
        /// <param name="sSceneNameToActive">The name of the scene to switch to.</param>
        /// <param name="sIntermissionSceneName">The name of the intermission scene, if any.</param>
        /// <param name="sTransitionData">The transition data for the scene switch, if any.</param>
        private void INTERNAL_ActiveScene(string sActualActiveSceneName, string sSceneNameToActive, string sIntermissionSceneName = null, STSTransitionData sTransitionData = null)
        {
            if (TransitionInProgress == false)
            {
                List<string> tAllScenesListS = new List<string>();
                tAllScenesListS.Add(sActualActiveSceneName);
                tAllScenesListS.Add(sSceneNameToActive);
                tAllScenesListS.Add(sIntermissionSceneName);
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

        /// <summary>
        /// Handles the transition of scenes without intermission asynchronously.
        /// </summary>
        /// <param name="sTransitionData">Data related to the scene transition.</param>
        /// <param name="sActualActiveSceneName">Name of the currently active scene.</param>
        /// <param name="sSceneNameToActive">Name of the scene to be activated.</param>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        private IEnumerator INTERNAL_ActiveSceneWithoutIntermissionAsync(STSTransitionData sTransitionData, string sActualActiveSceneName, string sSceneNameToActive)
        {
            TransitionInProgress = true;
            STSTransition tActualSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sActualActiveSceneName));
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(SceneManager.GetSceneByName(sActualActiveSceneName));
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(SceneManager.GetSceneByName(sActualActiveSceneName));
            STSTransition tNextSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sSceneNameToActive));
            STSTransitionInterface[] tNextSceneInterfaced = GetTransitionInterface(SceneManager.GetSceneByName(sSceneNameToActive));
            EventSystemPrevent(false);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }

            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sSceneNameToActive));
            CameraPrevent(true);
            AudioListenerPrevent(true);
            AnimationTransitionIn(tNextSceneParams, sTransitionData);
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, false);
            }

            EventSystemPrevent(true);
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }

            TransitionInProgress = false;
        }

        /// <summary>
        /// Manages the scene transition with intermission asynchronously within the Scene Transition System (STS).
        /// </summary>
        /// <param name="sTransitionData">The data related to the transition, including settings and parameters.</param>
        /// <param name="sActualActiveSceneName">The name of the currently active scene before the transition starts.</param>
        /// <param name="sSceneNameToActive">The name of the scene that will become active after the transition completes.</param>
        /// <param name="sIntermissionSceneName">The name of the intermission scene to be displayed during the transition.</param>
        /// <returns>An IEnumerator to handle the asynchronous behavior for the scene transition with intermission.</returns>
        private IEnumerator INTERNAL_ActiveSceneWithIntermissionAsync(STSTransitionData sTransitionData, string sActualActiveSceneName, string sSceneNameToActive, string sIntermissionSceneName)
        {
            TransitionInProgress = true;
            STSTransition tActualSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sActualActiveSceneName));
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(SceneManager.GetSceneByName(sActualActiveSceneName));
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(SceneManager.GetSceneByName(sActualActiveSceneName));
            STSTransition tNextSceneParams = GetTransitionsParams(SceneManager.GetSceneByName(sSceneNameToActive));
            STSTransitionInterface[] tNextSceneInterfaced = GetTransitionInterface(SceneManager.GetSceneByName(sSceneNameToActive));
            EventSystemPrevent(false);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }

            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }

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
            CameraPrevent(true);
            AudioListenerPrevent(true);
            // get params
            STSTransition tIntermissionSceneParams = GetTransitionsParams(tIntermissionScene);
            STSTransitionInterface[] tIntermissionSceneInterfaced = GetTransitionInterface(tIntermissionScene);
            STSIntermissionInterface[] tIntermissionInterfaced = GetIntermissionInterface(tIntermissionScene);
            // disable the user interactions until fadein 
            EventSystemEnable(tIntermissionScene, false);
            // Intermission scene is loaded
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneLoaded(sTransitionData);
            }

            // animation in Go!
            AnimationTransitionIn(tIntermissionSceneParams, sTransitionData);
            // animation in
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnEnter, tIntermissionSceneParams.InterEffectDuration, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnEnter, tIntermissionSceneParams.InterEffectDuration, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            // animation in Finish
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, false);
            }

            // enable the user interactions 
            EventSystemEnable(tIntermissionScene, true);
            // enable the user interactions 
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }

            // start stand by
            STSIntermission tIntermissionSceneStandBy = GetStandByParams(tIntermissionScene);
            foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
            {
                tInterfaced.OnStandByStart(tIntermissionSceneStandBy);
            }

            StandBy();
            while (StandByIsProgressing(tIntermissionSceneStandBy))
            {
                yield return null;
            }

            // send call back for standby finished
            foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
            {
                tInterfaced.OnStandByFinish(tIntermissionSceneStandBy);
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
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }

            // Intermission scene Transition Out GO! 
            AnimationTransitionOut(tIntermissionSceneParams, sTransitionData);
            // Intermission scene Transition Out start 
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnExit, tIntermissionSceneParams.InterEffectDuration, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnExit, tIntermissionSceneParams.InterEffectDuration, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            // Intermission scene Transition Out finished! 
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }

            // fadeout is finish
            // will unloaded the Intermission scene
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
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

            CameraPrevent(true);
            AudioListenerPrevent(true);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sSceneNameToActive));
            AnimationTransitionIn(tNextSceneParams, sTransitionData);
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, false);
            }

            while (AnimationFinished() == false)
            {
                yield return null;
            }

            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }

            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, false);
            }

            EventSystemPrevent(true);
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }

            TransitionInProgress = false;
        }
    }
}