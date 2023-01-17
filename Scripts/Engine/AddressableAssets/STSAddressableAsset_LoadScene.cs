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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using System.Threading.Tasks;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class STSSceneManagerAddressableAssets : STSSingletonUnity<STSSceneManagerAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        STSTransitionData TransitionData;
        //-------------------------------------------------------------------------------------------------------------
        // ADD Additive Scene
        //-------------------------------------------------------------------------------------------------------------
        /*public static void AddSubScene(SceneInstance sAdditionalSceneName, SceneInstance sIntermissionSceneName, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene(), new List<SceneInstance> { sAdditionalSceneName }, null, sIntermissionSceneName, sTransitionData, true, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSubScenes(List<SceneInstance> sAdditionalScenes, SceneInstance sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene(), sAdditionalScenes, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }*/
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScene(string sActiveScene, string sNextActiveScene, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, null, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddScenes(string sActiveScene, string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, sScenesToAdd, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        // REMOVE Additive Scene
        //-------------------------------------------------------------------------------------------------------------
        /*public static void RemoveSubScene(SceneInstance sSceneToRemove, SceneInstance sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene(), null, new List<SceneInstance> { sSceneToRemove }, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveSubScenes(List<SceneInstance> sScenesToRemove, SceneInstance sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene(), null, sScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }*/
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScene(string sActiveScene, string sNextActiveScene, string sSceneToRemove, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, null, new List<string> { sSceneToRemove }, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveScenes(string sActiveScene, string sNextActiveScene, List<string> sScenesToRemove, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, null, sScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        // ADD a new Scene
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScene(string sActiveScene, string sNextActiveScene, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            List<string> tScenesToAdd = new List<string>();
            ReplaceAllByScenes(sActiveScene, sNextActiveScene, tScenesToAdd, sIntermissionScene, sTransitionData, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScenes(string sActiveScene, string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Add(tScene.name);
            }

            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, sScenesToAdd, tScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }
        //=============================================================================================================
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        private void INTERNAL_ChangeScenes(
            string sActiveScene,
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
                if (sScenesToAdd == null) sScenesToAdd = new List<string>();
                if (sScenesToRemove == null) sScenesToRemove = new List<string>();

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
                for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
                {
                    Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                    sScenesToAdd.Remove(tScene.name);
                    if (tScene.name == sActiveScene)
                    {
                        tPossible = true;
                    }
                }

                List<string> tAllScenes = new List<string>();
                tAllScenes.Add(sNextActiveScene);
                tAllScenes.Add(sIntermissionScene);
                tAllScenes.AddRange(sScenesToAdd);
                tAllScenes.AddRange(sScenesToRemove);
                List<string> tAllScenesList = new List<string>();
                foreach (string tScen in tAllScenes)
                {
                    if (tAllScenesList.Contains(tScen) == false)
                    {
                        tAllScenesList.Add(tScen);
                    }
                }

                if (sAllowCyclic == true && sNextActiveScene == sActiveScene)
                {
                    if (sScenesToAdd.Contains(sActiveScene) == false)
                    {
                        sScenesToAdd.Add(sActiveScene);
                    }

                    if (sScenesToRemove.Contains(sActiveScene) == false)
                    {
                        sScenesToRemove.Add(sActiveScene);
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
                        ChangeScenesWithoutIntermission(sActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData);
                    }
                    else
                    {
                        ChangeScenesWithIntermission(sIntermissionScene, sActiveScene, sNextActiveScene, sScenesToAdd, sScenesToRemove, sTransitionData);
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
        private async void ChangeScenesWithoutIntermission(
            string sActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            TransitionInProgress = true;

            Dictionary<string, SceneInstance> tOperationList = new Dictionary<string, SceneInstance>();

            bool tRemoveActual = false;
            if (sScenesToRemove.Contains(sActiveScene))
            {
                sScenesToRemove.Remove(sActiveScene);
                tRemoveActual = true;
            }
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            ActualSceneDisable(sActiveScene, sTransitionData);
            //-------------------------------
            // COUNT SCENES TO REMOVE OR ADD
            //-------------------------------
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;
            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string k in sScenesToRemove)
            {
                Scene tSceneToDelete = SceneManager.GetSceneByName(k);
                AudioListenerEnable(tSceneToDelete, false);
                CameraPreventEnable(tSceneToDelete, false);
                EventSystemEnable(tSceneToDelete, false);
                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                if (tSceneToDelete.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(tSceneToDelete);
                    //await UnloadSceneAsync(tSceneToDelete); // need a SceneInstance
                }
                tSceneCounter++;
            }
            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            int tTaskCounter = 0;
            STSPerformance.StartTimer();
            Task[] tTasks = new Task[sScenesToAdd.Count];
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                if (!SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    Task<SceneInstance> k = LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive, false, bProgressCallBack: (p) =>
                    {
                        Debug.LogWarning("pourcent: " + p);
                    });
                    tTasks[tTaskCounter] = k;
                }
                tSceneCounter++;
                tTaskCounter++;
            }
            await Task.WhenAll(tTasks);
            STSPerformance.EndTimer();
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            await Task.Delay(1000);
            foreach(Task<SceneInstance> j in tTasks)
            {
                SceneInstance tInstance = j.Result;
                tInstance.ActivateAsync();

                AudioListenerEnable(tInstance.Scene, false);
                CameraPreventEnable(tInstance.Scene, false);
                EventSystemEnable(tInstance.Scene, false);
                STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tInstance.Scene);
                foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                {
                    tInterfaced.OnTransitionSceneLoaded(sTransitionData);
                }
            }
            await Task.Delay(1000);
            /*foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tOperationList.ContainsKey(tSceneToAdd))
                {
                    SceneInstance tSceneInstance = tOperationList[tSceneToAdd];
                    tSceneInstance.ActivateAsync();

                    AudioListenerEnable(tSceneInstance.Scene, false);
                    CameraPreventEnable(tSceneInstance.Scene, false);
                    EventSystemEnable(tSceneInstance.Scene, false);
                    STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tSceneInstance.Scene);
                    foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                    {
                        tInterfaced.OnTransitionSceneLoaded(sTransitionData);
                    }
                }
            }*/
            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            CameraPrevent(true);
            AudioListenerPrevent(true);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            if (tRemoveActual == true)
            {
                foreach (STSTransitionInterface tInterfaced in GetInterface(sActiveScene))
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                SceneManager.UnloadSceneAsync(sActiveScene);
                //await UnloadSceneAsync(sActiveScene); // need a SceneInstance
            }
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            NextSceneEnable(tNextActiveScene, sTransitionData);

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private async void ChangeScenesWithIntermission(
            string sIntermissionScene,
            string sActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            TransitionInProgress = true;
            Dictionary<string, SceneInstance> tOperationList = new Dictionary<string, SceneInstance>();
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            ActualSceneDisable(sActiveScene, sTransitionData);
            //-------------------------------
            // Intermission SCENE LOAD AND ENABLE
            //-------------------------------
            // load transition scene async
            SceneInstance tLoadedScene = new SceneInstance();
            Task<SceneInstance> tSceneInstance = LoadSceneAsync(sIntermissionScene, LoadSceneMode.Additive, bProgressCallBack: (p) =>
            {
                Debug.LogWarning("pourcent: " + p);
            });
            tLoadedScene = await tSceneInstance;
            Scene tIntermissionScene = tLoadedScene.Scene;
            AudioListenerEnable(tIntermissionScene, false);
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
                // fadeout is finish
                // will unloaded the  scene
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete);
                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                if (tSceneToDelete.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(tSceneToRemove);
                    //await UnloadSceneAsync(tSceneToRemove); // need a SceneInstance
                }

                foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                {
                    tInterfaced.OnUnloadScene(sTransitionData, tSceneToRemove, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                }
                tSceneCounter++;
            }
            //-------------------------------
            // Intermission start enter animation
            //-------------------------------
            // get params
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
            while (AnimationFinished() == false)
            {
                break;
            }
            // animation in Finish
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }
            // enable the user interactions 
            EventSystemEnable(tIntermissionScene, true);
            // enable the user interactions 
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
            int tTaskCounter = 0;
            STSPerformance.StartTimer();
            Task[] tTasks = new Task[sScenesToAdd.Count];
            foreach (string tSceneToLoad in sScenesToAdd)
            {
                Scene tSceneToAdd = SceneManager.GetSceneByName(tSceneToLoad);
                if (tSceneToAdd.isLoaded)
                {
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnSceneAllReadyLoaded(sTransitionData, tSceneToLoad, tSceneCounter, (tSceneCounter + 1.0F) / tSceneCount);
                    }
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

                    Task<SceneInstance> k = LoadSceneAsync(tSceneToLoad, LoadSceneMode.Additive, false,  bProgressCallBack: (p) =>
                    {
                        Debug.LogWarning(p + ", (" + tSceneCounter + " + " + p + ") /" + tSceneCount);
                        foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                        {
                            tInterfaced.OnLoadingScenePercent(sTransitionData, tSceneToLoad, tSceneCounter, p, (tSceneCounter + p) / tSceneCount);
                        }
                    });
                    tTasks[tTaskCounter] = k;
                                        
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneFinish(sTransitionData, tSceneToLoad, tSceneCounter, 1.0F, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                }
                tSceneCounter++;
                tTaskCounter++;
            }
            await Task.WhenAll(tTasks);
            STSPerformance.EndTimer();
            //-------------------------------
            // Intermission STAND BY
            //-------------------------------
            while (StandByIsProgressing(tIntermissionSceneStandBy))
            {
                break;
            }
            // As soon as possible 
            foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
            {
                tInterfaced.OnStandByFinish(tIntermissionSceneStandBy);
            }
            // Waiting to load the next Scene
            while (WaitingToLauchNextScene(tIntermissionSceneStandBy))
            {
                break;
            }
            //-------------------------------
            // Intermission GO TO NEXT SCENE PROCESS
            //-------------------------------
            // stanby is finished And the next scene can be lauch
            // disable user interactions on the Intermission scene
            EventSystemEnable(tIntermissionScene, false);
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
            while (AnimationFinished() == false)
            {
                break;
            }
            // Intermission scene Transition Out finished! 
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }
            // fadeout is finish
            // will unloaded the Intermission scene
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
            }
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            await Task.Delay(1000);
            foreach(Task<SceneInstance> j in tTasks)
            {
                SceneInstance tInstance = j.Result;
                tInstance.ActivateAsync();

                AudioListenerEnable(tInstance.Scene, false);
                CameraPreventEnable(tInstance.Scene, false);
                EventSystemEnable(tInstance.Scene, false);
                STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tInstance.Scene);
                foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                {
                    tInterfaced.OnTransitionSceneLoaded(sTransitionData);
                }
            }
            await Task.Delay(1000);
            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            AudioListenerPrevent(true);
            CameraPrevent(true);
            EventSystemEnable(tNextActiveScene, false);
            // Next scene appear by fade in
            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            SceneManager.UnloadSceneAsync(tIntermissionScene);
            //await UnloadSceneAsync(tIntermissionScene); // need a SceneInstance
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            NextSceneEnable(tNextActiveScene, sTransitionData);

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void ActualSceneDisable(string sActiveScene, STSTransitionData sTransitionData)
        {
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActiveScene);
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(tActualScene);
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(tActualScene);
            //-------------------------------
            // disable the user interactions
            EventSystemPrevent(false);
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
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
                break;
            }
            // post scene finish effcet transition out
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }
            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void NextSceneEnable(Scene sNextActiveScene, STSTransitionData sTransitionData)
        {
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            STSTransition tNextSceneParams = GetTransitionsParams(sNextActiveScene);
            STSTransitionInterface[] tNextSceneInterfaced = GetTransitionInterface(sNextActiveScene);
            STSTransitionInterface[] tOtherNextSceneInterfaced = GetOtherTransitionInterface(sNextActiveScene);
            //-------------------------------
            AnimationTransitionIn(tNextSceneParams, sTransitionData);
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
                break;
            }
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
            foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private STSTransitionInterface[] GetInterface(string sSceneName)
        {
            Scene tActualScene = SceneManager.GetSceneByName(sSceneName);
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            return GetTransitionInterface(tActualScene);
        }
        //-------------------------------------------------------------------------------------------------------------
        private async Task<SceneInstance> LoadSceneAsync(string sReference, LoadSceneMode sMode = LoadSceneMode.Single, bool sActivatedOnLoad = true, Action<float> bProgressCallBack = null)
        {
            SceneInstance rScene = new SceneInstance();
            AssetReference tReference = new AssetReference(sReference);
            AsyncOperationHandle<SceneInstance> tHandle = Addressables.LoadSceneAsync(tReference, sMode, sActivatedOnLoad);
            while (!tHandle.IsDone)
            {
                bProgressCallBack?.Invoke(tHandle.PercentComplete);
                await Task.Yield();
            }
            await tHandle.Task;
            if(tHandle.Status == AsyncOperationStatus.Succeeded)
            {
                rScene = tHandle.Result;
            }
            else
            {
                Debug.LogWarning("Addressable " + tHandle.DebugName + " load error");
            }
            return rScene;
        }
        //-------------------------------------------------------------------------------------------------------------
        private async Task<bool> UnloadSceneAsync(SceneInstance sInstance, Action<float> bProgressCallBack = null)
        {
            AsyncOperationHandle<SceneInstance> tHandle = Addressables.UnloadSceneAsync(sInstance);
            while (!tHandle.IsDone)
            {
                bProgressCallBack?.Invoke(tHandle.PercentComplete);
                await Task.Yield();
            }
            await tHandle.Task;
            return tHandle.IsDone;
        }
        //-------------------------------------------------------------------------------------------------------------
        /*void OnCompleted(AsyncOperationHandle<SceneInstance> handle)
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                var sceneInstance = handle.Result;
                sceneInstance.ActivateAsync();
                if (handle.IsDone)
                {
                    AudioListenerEnable(sceneInstance.Scene, false);
                    CameraPreventEnable(sceneInstance.Scene, false);
                    EventSystemEnable(sceneInstance.Scene, false);
                    STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(sceneInstance.Scene);
                    foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                    {
                        tInterfaced.OnTransitionSceneLoaded(TransitionData);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Addressable " + handle.DebugName + " load error");
            }
        }*/
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================