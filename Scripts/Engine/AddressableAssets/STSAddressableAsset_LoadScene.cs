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
    public partial class STSAddressableAssets : STSSingletonUnity<STSAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
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
        /*public static void AddScenes(string sActiveScene, string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, sScenesToAdd, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }*/
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
        /*public static void RemoveScenes(string sActiveScene, string sNextActiveScene, List<string> sScenesToRemove, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, null, sScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }*/
        //-------------------------------------------------------------------------------------------------------------
        // ADD a new Scene
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScene(string sNextActiveScene, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            ReplaceAllByScenes(sNextActiveScene, null, sIntermissionScene, sTransitionData, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScenes(STSScene sNextActiveScene, STSScene[] sScenesToAdd, STSScene sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            List<string> tScenesToAdd = new List<string>();
            if (sScenesToAdd != null)
            {
                foreach (STSScene tScene in sScenesToAdd)
                {
                    tScenesToAdd.Add(tScene.GetSceneShortName());
                }
            }
            string tNextActiveScene = string.Empty;
            if (sNextActiveScene != null)
            {
                tNextActiveScene = sNextActiveScene.GetSceneShortName();
            }
            string tIntermissionScene = string.Empty;
            if (sIntermissionScene != null)
            {
                tIntermissionScene = sIntermissionScene.GetSceneShortName();
            }
            ReplaceAllByScenes(tNextActiveScene, tScenesToAdd, tIntermissionScene, sTransitionData, sAllowCyclic);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReplaceAllByScenes(string sNextActiveScene, List<string> sScenesToAdd, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            List<string> tScenesToRemove = new List<string>();
            for (int tSceneIndex = 0; tSceneIndex < SceneManager.sceneCount; tSceneIndex++)
            {
                Scene tScene = SceneManager.GetSceneAt(tSceneIndex);
                tScenesToRemove.Add(tScene.name);
            }
            Singleton().INTERNAL_ChangeScenes(SceneManager.GetActiveScene().name, sNextActiveScene, sScenesToAdd, tScenesToRemove, sIntermissionScene, sTransitionData, true, sAllowCyclic);
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
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;

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
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string k in sScenesToRemove)
            {
                Scene tSceneToDelete = SceneManager.GetSceneByName(k);
                AudioListenerEnable(tSceneToDelete, false);
                CameraPreventEnable(tSceneToDelete, false);
                EventSystemEnable(tSceneToDelete, false);

                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                OnTransitionSceneWillUnloaded(tSceneToDeleteInterfaced, sTransitionData);
                /*foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }*/

                if (tSceneToDelete.isLoaded)
                {
                    UnloadSceneAsync(tSceneToDelete.name);
                    //SceneManager.UnloadSceneAsync(tSceneToDelete);
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
            ActiveAddedScenes(tTasks, sTransitionData);
            await Task.Delay(1000);

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            CameraPrevent(true);
            AudioListenerPrevent(true);
            EventSystemEnable(tNextActiveScene, false);

            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            if (tRemoveActual == true)
            {
                OnTransitionSceneWillUnloaded(GetInterface(sActiveScene), sTransitionData);
                /*foreach (STSTransitionInterface tInterfaced in GetInterface(sActiveScene))
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }*/
                UnloadSceneAsync(sActiveScene);
                //SceneManager.UnloadSceneAsync(sActiveScene);
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
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;

            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            ActualSceneDisable(sActiveScene, sTransitionData);

            //-------------------------------
            // Intermission SCENE LOAD AND ENABLE
            //-------------------------------
            SceneInstance tLoadedScene = new SceneInstance();
            Task<SceneInstance> tSceneInstance = LoadSceneAsync(sIntermissionScene, LoadSceneMode.Additive, bProgressCallBack: (p) =>
            {
                Debug.LogWarning("pourcent: " + p);
            });
            tLoadedScene = await tSceneInstance;
            Scene tIntermissionScene = tLoadedScene.Scene;

            AudioListenerEnable(tIntermissionScene, false);
            CameraPrevent(true);
            AudioListenerPrevent(true);
            EventSystemEnable(tIntermissionScene, false);

            // get params
            STSTransition tIntermissionSceneParams = GetTransitionsParams(tIntermissionScene);
            STSTransitionInterface[] tIntermissionSceneInterfaced = GetTransitionInterface(tIntermissionScene);
            STSIntermissionInterface[] tIntermissionInterfaced = GetIntermissionInterface(tIntermissionScene);
            STSIntermission tIntermissionSceneStandBy = GetStandByParams(tIntermissionScene);

            //-------------------------------
            // UNLOADED SCENES REMOVED
            //-------------------------------
            foreach (string tSceneToRemove in sScenesToRemove)
            {
                Scene tSceneToDelete = SceneManager.GetSceneByName(tSceneToRemove);
                STSTransition tSceneToDeleteParams = GetTransitionsParams(tSceneToDelete);
                STSTransitionInterface[] tSceneToDeleteInterfaced = GetTransitionInterface(tSceneToDelete);
                
                OnTransitionSceneWillUnloaded(tSceneToDeleteInterfaced, sTransitionData);
                /*foreach (STSTransitionInterface tInterfaced in tSceneToDeleteInterfaced)
                {
                    tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
                }*/

                if (tSceneToDelete.isLoaded)
                {
                    UnloadSceneAsync(tSceneToDelete.name);
                    //SceneManager.UnloadSceneAsync(tSceneToDelete);
                    //await UnloadSceneAsync(tSceneToDelete); // need a SceneInstance
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
            OnTransitionSceneLoaded(tIntermissionSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneLoaded(sTransitionData);
            }*/

            AnimationTransitionIn(tIntermissionSceneParams, sTransitionData);

            OnTransitionEnterStart(tIntermissionSceneInterfaced, sTransitionData, tIntermissionSceneParams);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnEnter, tIntermissionSceneParams.InterEffectDuration, true);
            }*/

            while (AnimationFinished() == false)
            {
                break;
            }

            OnTransitionEnterFinish(tIntermissionSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }*/

            EventSystemEnable(tIntermissionScene, true);

            OnTransitionSceneEnable(tIntermissionSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }*/

            //-------------------------------
            // Intermission SCENE START STAND BY
            //-------------------------------
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
                    Debug.LogWarning("OnLoadingSceneStart: (" + tSceneCounter + ", 0.0F, 0.0F)");
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneStart(sTransitionData, tSceneToLoad, tSceneCounter, 0.0F, 0.0F);
                    }

                    Task<SceneInstance> k = LoadSceneAsync(tSceneToLoad, LoadSceneMode.Additive, false,  bProgressCallBack: (p) =>
                    {
                        Debug.LogWarning("OnLoadingScenePercent: (" + tSceneCounter + ", " + p + ", " + (tSceneCounter + p) / tSceneCount + ")");
                        foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                        {
                            tInterfaced.OnLoadingScenePercent(sTransitionData, tSceneToLoad, tSceneCounter, p, (tSceneCounter + p) / tSceneCount);
                        }
                    });
                    tTasks[tTaskCounter] = k;
                    
                    Debug.LogWarning("OnLoadingSceneFinish: (" + tSceneCounter + ", 1.0F, " + (tSceneCounter + 1.0F) / tSceneCount + ")");
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
            
            OnTransitionSceneDisable(tIntermissionSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }*/
            
            AnimationTransitionOut(tIntermissionSceneParams, sTransitionData);
            
            OnTransitionEnterStart(tIntermissionSceneInterfaced, sTransitionData, tIntermissionSceneParams);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tIntermissionSceneParams.EffectOnExit, tIntermissionSceneParams.InterEffectDuration, true);
            }*/

            while (AnimationFinished() == false)
            {
                break;
            }

            OnTransitionExitFinish(tIntermissionSceneInterfaced, sTransitionData);
            OnTransitionSceneWillUnloaded(tIntermissionSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }
            foreach (STSTransitionInterface tInterfaced in tIntermissionSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneWillUnloaded(sTransitionData);
            }*/

            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            await Task.Delay(1000);
            ActiveAddedScenes(tTasks, sTransitionData);
            await Task.Delay(1000);

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            SceneManager.SetActiveScene(tNextActiveScene);
            CameraPrevent(true);
            AudioListenerPrevent(true);
            EventSystemEnable(tNextActiveScene, false);

            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            UnloadSceneAsync(tIntermissionScene.name);
            //SceneManager.UnloadSceneAsync(tIntermissionScene);
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
            EventSystemPrevent(false);

            OnTransitionSceneDisable(tActualSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneDisable(sTransitionData);
            }*/

            AnimationTransitionOut(tActualSceneParams, sTransitionData);

            OnTransitionExitStart(tActualSceneInterfaced, sTransitionData, tActualSceneParams);
            OnTransitionExitStart(tOtherSceneInterfaced, sTransitionData, tActualSceneParams, false);
            /*foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, true);
            }
            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitStart(sTransitionData, tActualSceneParams.EffectOnExit, false);
            }*/

            while (AnimationFinished() == false)
            {
                break;
            }
            
            OnTransitionExitFinish(tActualSceneInterfaced, sTransitionData);
            OnTransitionExitFinish(tOtherSceneInterfaced, sTransitionData, false);
            /*foreach (STSTransitionInterface tInterfaced in tActualSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, true);
            }
            foreach (STSTransitionInterface tInterfaced in tOtherSceneInterfaced)
            {
                tInterfaced.OnTransitionExitFinish(sTransitionData, false);
            }*/
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

            OnTransitionEnterStart(tNextSceneInterfaced, sTransitionData, tNextSceneParams);
            OnTransitionEnterStart(tOtherNextSceneInterfaced, sTransitionData, tNextSceneParams, false);
            /*foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, true);
            }
            foreach (STSTransitionInterface tInterfaced in tOtherNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterStart(sTransitionData, tNextSceneParams.EffectOnEnter, tNextSceneParams.InterEffectDuration, false);
            }*/

            while (AnimationFinished() == false)
            {
                break;
            }
            
            OnTransitionEnterFinish(tNextSceneInterfaced, sTransitionData);
            OnTransitionEnterFinish(tOtherNextSceneInterfaced, sTransitionData, false);
            /*foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, true);
            }
            foreach (STSTransitionInterface tInterfaced in tOtherNextSceneInterfaced)
            {
                tInterfaced.OnTransitionEnterFinish(sTransitionData, false);
            }*/

            EventSystemPrevent(true);
            
            OnTransitionSceneEnable(tNextSceneInterfaced, sTransitionData);
            /*foreach (STSTransitionInterface tInterfaced in tNextSceneInterfaced)
            {
                tInterfaced.OnTransitionSceneEnable(sTransitionData);
            }*/
        }
        //-------------------------------------------------------------------------------------------------------------
        private void ActiveAddedScenes(Task[] sTasks, STSTransitionData sTransitionData)
        {
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            foreach(Task<SceneInstance> j in sTasks)
            {
                SceneInstance tInstance = j.Result;
                tInstance.ActivateAsync();

                AudioListenerEnable(tInstance.Scene, false);
                CameraPreventEnable(tInstance.Scene, false);
                EventSystemEnable(tInstance.Scene, false);

                OnTransitionSceneLoaded(GetTransitionInterface(tInstance.Scene), sTransitionData);
                /*foreach (STSTransitionInterface tInterfaced in tSceneToLoadInterfaced)
                {
                    tInterfaced.OnTransitionSceneLoaded(sTransitionData);
                }*/
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
        private void OnTransitionEnterStart(STSTransitionInterface[] sInterface, STSTransitionData sData, STSTransition sParams, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionEnterStart(sData, sParams.EffectOnEnter, sParams.InterEffectDuration, sActiveScene);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionEnterFinish(STSTransitionInterface[] sInterface, STSTransitionData sData, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionEnterFinish(sData, sActiveScene);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionExitStart(STSTransitionInterface[] sInterface, STSTransitionData sData, STSTransition sSceneParams, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionExitStart(sData, sSceneParams.EffectOnExit, sActiveScene);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionExitFinish(STSTransitionInterface[] sInterface, STSTransitionData sData, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionExitFinish(sData, sActiveScene);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionSceneEnable(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneEnable(sData);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionSceneDisable(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneDisable(sData);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionSceneWillUnloaded(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneWillUnloaded(sData);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnTransitionSceneLoaded(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneLoaded(sData);
            }
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
        private void UnloadSceneAsync(string sSceneName)
        {
            Debug.LogWarning("> UnloadSceneAsync: " + sSceneName);
            AsyncOperation tOperation = SceneManager.UnloadSceneAsync(sSceneName);
            tOperation.completed += (asyncOperation) =>
            {
                Debug.LogWarning("-----------");
                Debug.LogWarning("completed unload! " + sSceneName);
                Debug.LogWarning("isdone: " + tOperation.isDone);
                Debug.LogWarning("-----------");
            };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================