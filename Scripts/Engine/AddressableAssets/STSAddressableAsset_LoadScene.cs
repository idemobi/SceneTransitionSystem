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
                //TransitionData = sTransitionData;

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
                    //sScenesToAdd.Remove(tScene);
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
            TransitionInProgress = true;
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActiveScene);
            //-------------------------------
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(tActualScene);
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(tActualScene);
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
                //yield return null;
            }
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
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                AudioListenerEnable(tSceneToDelete, false);
                CameraPreventEnable(tSceneToDelete, false);
                EventSystemEnable(tSceneToDelete, false);
                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }
                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    SceneManager.UnloadSceneAsync(tSceneToRemove);
                    //Addressables.UnloadSceneAsync(tSceneToRemove);
                    //AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);
                    //tAsyncOperationRemove.allowSceneActivation = true;
                }
                tSceneCounter++;
            }
            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                if (!SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    SceneInstance tLoadedScene = new SceneInstance();
                    Task<SceneInstance> k = LoadSceneAsyncWithTask(tSceneToAdd, LoadSceneMode.Additive);
                    tLoadedScene = await k;
                    tOperationList.Add(tSceneToAdd, tLoadedScene);

                    /*AsyncOperation tAsyncOperationAdd = SceneManager.LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive);
                    tAsyncOperationList.Add(tSceneToAdd, tAsyncOperationAdd);
                    tAsyncOperationAdd.allowSceneActivation = false;*/
                }
                tSceneCounter++;
            }
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            /*foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tAsyncOperationList.ContainsKey(tSceneToAdd))
                {
                    Scene tSceneToLoad = SceneManager.GetSceneByName(tSceneToAdd);
                    AsyncOperationHandle<SceneInstance> tAsyncOperationAdd = tAsyncOperationList[tSceneToAdd];
                    if (tAsyncOperationAdd.Status == AsyncOperationStatus.Succeeded)
                    {
                        tAsyncOperationAdd.Result.ActivateAsync();
                        while (tAsyncOperationAdd.PercentComplete < 0.9f)
                        {
                            yield return null;
                        }
                        while (!tAsyncOperationAdd.IsDone)
                        {
                            AudioListenerEnable(tSceneToLoad, false);
                            yield return null;
                        }
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
            }*/
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tOperationList.ContainsKey(tSceneToAdd))
                {
                    SceneInstance tSceneInstance = tOperationList[tSceneToAdd];
                    //tSceneInstance.ActivateAsync();

                    AudioListenerEnable(tSceneInstance.Scene, false);
                    CameraPreventEnable(tSceneInstance.Scene, false);
                    EventSystemEnable(tSceneInstance.Scene, false);
                    STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tSceneInstance.Scene);
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
            CameraPrevent(true);
            AudioListenerPrevent(true);
            // get params
            STSTransition sNextSceneParams = GetTransitionsParams(tNextActiveScene);
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
                SceneManager.UnloadSceneAsync(sActiveScene);
                //Addressables.UnloadSceneAsync(sActiveScene);
                //AsyncOperation tAsyncOperationIntermissionUnload = SceneManager.UnloadSceneAsync(sActualActiveScene);
                //tAsyncOperationIntermissionUnload.allowSceneActivation = true;
            }
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            AnimationTransitionIn(sNextSceneParams, sTransitionData);
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
                break;
                //yield return null;
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
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private async void ChangeScenesWithIntermission(
            string sIntermissionScene,
            string sActualActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            Dictionary<string, SceneInstance> tOperationList = new Dictionary<string, SceneInstance>();

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
            foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }
            // scene start effect transition out!
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            // post scene start effect transition out!
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
            //-------------------------------
            // Intermission SCENE LOAD AND ENABLE
            //-------------------------------
            // load transition scene async
            SceneInstance tLoadedScene = new SceneInstance();
            Task<SceneInstance> k = LoadSceneAsyncWithTask(sIntermissionScene, LoadSceneMode.Additive);
            tLoadedScene = await k;
            Scene tIntermissionScene = tLoadedScene.Scene;
            AudioListenerEnable(tIntermissionScene, false);

            /*AsyncOperation tAsyncOperationIntermission = SceneManager.LoadSceneAsync(sIntermissionScene, LoadSceneMode.Additive);
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
            }*/

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
                if (SceneManager.GetSceneByName(tSceneToRemove).isLoaded)
                {
                    AsyncOperation tAsyncOperationRemove = SceneManager.UnloadSceneAsync(tSceneToRemove);

                    if (tAsyncOperationRemove != null)
                    {
                        tAsyncOperationRemove.allowSceneActivation = true;
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
            foreach (string tSceneToLoad in sScenesToAdd)
            {
                Scene tSceneToAdd = SceneManager.GetSceneByName(tSceneToLoad);
                if (SceneManager.GetSceneByName(tSceneToLoad).isLoaded)
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

                    k = LoadSceneAsyncWithTask(tSceneToLoad, LoadSceneMode.Additive, bProgressCallBack: (p) =>
                    {
                        foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                        {
                            tInterfaced.OnLoadingScenePercent(sTransitionData, tSceneToLoad, tSceneCounter, p, (tSceneCounter + p) / tSceneCount);
                        }
                    });
                    tLoadedScene = await k;
                    tOperationList.Add(tSceneToLoad, tLoadedScene);
                                        
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneFinish(sTransitionData, tSceneToLoad, tSceneCounter, 1.0F, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                }
                tSceneCounter++;
            }
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
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                // scene is loaded!
                if (tOperationList.ContainsKey(tSceneToAdd))
                {
                    SceneInstance tOperationAdd = tOperationList[tSceneToAdd];
                    //tOperationAdd.ActivateAsync();

                    AudioListenerEnable(tOperationAdd.Scene, false);
                    CameraPreventEnable(tOperationAdd.Scene, false);
                    EventSystemEnable(tOperationAdd.Scene, false);
                    STSTransitionInterface[] tSceneToLoadInterfaced = GetTransitionInterface(tOperationAdd.Scene);
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
                    break;
                }
                while (!tAsyncOperationIntermissionUnload.isDone)
                {
                    break;
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
            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        private async Task<SceneInstance> LoadSceneAsyncWithTask(string sReference, LoadSceneMode sMode = LoadSceneMode.Single, bool sActivatedOnLoad = true, Action<float> bProgressCallBack = null)
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