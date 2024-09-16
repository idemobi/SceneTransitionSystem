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

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSAddressableAssets class is responsible for managing the transition between scenes,
    /// allowing for addition, removal, and replacement of scenes.
    /// </summary>
    public partial class STSAddressableAssets : STSSingletonUnity<STSAddressableAssets>, STSTransitionInterface, STSIntermissionInterface
    {
        /// <summary>
        /// Adds a new scene to the current active scene while also specifying an intermission scene and transition data.
        /// </summary>
        /// <param name="sActiveScene">Name of the currently active scene.</param>
        /// <param name="sNextActiveScene">Name of the scene to be activated next.</param>
        /// <param name="sIntermissionScene">Name of the scene to be used as an intermission.</param>
        /// <param name="sTransitionData">Data related to the transition between scenes. Optional.</param>
        /// <param name="sAllowCyclic">Flag to allow cyclic transitions. Optional.</param>
        public static void AddScene(string sActiveScene, string sNextActiveScene, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, null, null, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Removes a specified scene from the current set of active scenes and transitions to the next active scene with optional intermission and transition data.
        /// </summary>
        /// <param name="sActiveScene">The name of the currently active scene.</param>
        /// <param name="sNextActiveScene">The name of the next scene to become active after removal.</param>
        /// <param name="sSceneToRemove">The name of the scene to be removed.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene to be displayed during the transition, if any.</param>
        /// <param name="sTransitionData">Optional transition data to be used during the scene transition.</param>
        /// <param name="sAllowCyclic">Flag indicating if cyclic transitions are allowed.</param>
        public static void RemoveScene(string sActiveScene, string sNextActiveScene, string sSceneToRemove, string sIntermissionScene, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            Singleton().INTERNAL_ChangeScenes(sActiveScene, sNextActiveScene, null, new List<string> { sSceneToRemove }, sIntermissionScene, sTransitionData, true, sAllowCyclic);
        }

        /// <summary>
        /// Replaces all currently active scenes with a new scene, optionally using an intermission scene during the transition.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the scene to activate.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene. Optional parameter, defaults to null.</param>
        /// <param name="sTransitionData">Data related to the scene transition. Optional parameter, defaults to null.</param>
        /// <param name="sAllowCyclic">Indicates whether cyclic transitions are allowed. Optional parameter, defaults to false.</param>
        public static void ReplaceAllByScene(string sNextActiveScene, string sIntermissionScene = null, STSTransitionData sTransitionData = null, bool sAllowCyclic = false)
        {
            ReplaceAllByScenes(sNextActiveScene, null, sIntermissionScene, sTransitionData, sAllowCyclic);
        }

        /// Replaces all scenes in the transition system with the provided next active scene and additional scenes.
        /// @param sNextActiveScene The next active scene to set.
        /// @param sScenesToAdd An array of scenes to add.
        /// @param sIntermissionScene The intermission scene to set, if any.
        /// @param sTransitionData Additional transition data.
        /// @param sAllowCyclic Specifies whether to allow cyclic transitions.
        /// /
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

        /// <summary>
        /// Replaces all current scenes with new scenes.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the next active scene.</param>
        /// <param name="sScenesToAdd">A list of names of scenes to add. Can be null if no additional scenes are to be added.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene. Can be null if no intermission is needed.</param>
        /// <param name="sTransitionData">Transition data to use during the scene transition. Can be null if no specific transition data is required.</param>
        /// <param name="sAllowCyclic">Boolean flag indicating if cyclic transitions are allowed.</param>
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

        /// <summary>
        /// Handles the transition between different scenes, including addition, removal,
        /// and replacement of scenes within the application.
        /// </summary>
        /// <param name="sActiveScene">The name of the currently active scene.</param>
        /// <param name="sNextActiveScene">The name of the next scene to be set as active.</param>
        /// <param name="sScenesToAdd">A list of scene names to be added during the transition.</param>
        /// <param name="sScenesToRemove">A list of scene names to be removed during the transition.</param>
        /// <param name="sIntermissionScene">The name of the intermission scene, if any.</param>
        /// <param name="sTransitionData">Optional data relevant to the transition.</param>
        /// <param name="sHistorical">Flag indicating if the transition should be remembered in history.</param>
        /// <param name="sAllowCyclic">Flag indicating if cyclic transitions are allowed.</param>
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

        /// Changes scenes without an intermission.
        /// <param name="sActiveScene">The name of the currently active scene.</param>
        /// <param name="sNextActiveScene">The name of the next scene to be activated.</param>
        /// <param name="sScenesToAdd">A list of scene names to be added.</param>
        /// <param name="sScenesToRemove">A list of scene names to be removed.</param>
        /// <param name="sTransitionData">Transition data containing additional information required for the scene transition.</param>
        private async void ChangeScenesWithoutIntermission(
            string sActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            TransitionInProgress = true;
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
            await ActualSceneDisable(sActiveScene, sTransitionData);

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

                if (tSceneToDelete.isLoaded)
                {
                    await UnloadSceneAsync(tSceneToDelete.name);
                }

                tSceneCounter++;
            }

            //-------------------------------
            // LOADED SCENES ADDED
            //-------------------------------
            int tTaskCounter = 0;
            //STSPerformance.StartTimer();
            Task[] tTasks = new Task[sScenesToAdd.Count];
            foreach (string tSceneToAdd in sScenesToAdd)
            {
                if (!SceneManager.GetSceneByName(tSceneToAdd).isLoaded)
                {
                    Task<SceneInstance> k = LoadSceneAsync(tSceneToAdd, LoadSceneMode.Additive, false);
                    tTasks[tTaskCounter] = k;
                }

                tSceneCounter++;
                tTaskCounter++;
            }

            await Task.WhenAll(tTasks);
            await Task.Yield();
            //STSPerformance.EndTimer();

            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            await ActiveAddedScenes(tTasks, sTransitionData);

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = await ActivateNextScene(sNextActiveScene);

            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            if (tRemoveActual == true)
            {
                OnTransitionSceneWillUnloaded(GetInterface(sActiveScene), sTransitionData);

                await UnloadSceneAsync(sActiveScene);
            }

            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            await NextSceneEnable(tNextActiveScene, sTransitionData);

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
            await Task.Yield();
        }

        /// Changes the current scene to the next scene with an intermission scene in between.
        /// <param name="sIntermissionScene">The name of the intermission scene to be shown between current and next scenes.</param>
        /// <param name="sActiveScene">The name of the currently active scene.</param>
        /// <param name="sNextActiveScene">The name of the scene to be activated next.</param>
        /// <param name="sScenesToAdd">A list of scene names to be added during the transition.</param>
        /// <param name="sScenesToRemove">A list of scene names to be removed during the transition.</param>
        /// <param name="sTransitionData">Additional transition data to handle custom behaviors during the scene change.</param>
        private async void ChangeScenesWithIntermission(
            string sIntermissionScene,
            string sActiveScene,
            string sNextActiveScene,
            List<string> sScenesToAdd,
            List<string> sScenesToRemove,
            STSTransitionData sTransitionData)
        {
            TransitionInProgress = true;
            float tSceneCount = sScenesToAdd.Count + sScenesToRemove.Count;
            int tSceneCounter = 0;

            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            await ActualSceneDisable(sActiveScene, sTransitionData);

            //-------------------------------
            // Intermission SCENE LOAD AND ENABLE
            //-------------------------------
            SceneInstance tLoadedScene = new SceneInstance();
            Task<SceneInstance> tSceneInstance = LoadSceneAsync(sIntermissionScene, LoadSceneMode.Additive);
            tLoadedScene = await tSceneInstance;
            AddLoadedScene(tLoadedScene);
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

                if (tSceneToDelete.isLoaded)
                {
                    await UnloadSceneAsync(tSceneToDelete.name);
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
            AnimationTransitionIn(tIntermissionSceneParams, sTransitionData);
            OnTransitionEnterStart(tIntermissionSceneInterfaced, sTransitionData, tIntermissionSceneParams);

            while (AnimationFinished() == false)
            {
                await Task.Yield();
            }

            OnTransitionEnterFinish(tIntermissionSceneInterfaced, sTransitionData);
            EventSystemEnable(tIntermissionScene, true);
            OnTransitionSceneEnable(tIntermissionSceneInterfaced, sTransitionData);

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
            //STSPerformance.StartTimer();
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
                    //Debug.LogWarning("OnLoadingSceneStart: (" + tSceneCounter + ", 0.0F, 0.0F)");
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneStart(sTransitionData, tSceneToLoad, tSceneCounter, 0.0F, 0.0F);
                    }

                    Task<SceneInstance> k = LoadSceneAsync(tSceneToLoad, LoadSceneMode.Additive, false, bProgressCallBack: (p) =>
                    {
                        //Debug.LogWarning("OnLoadingScenePercent: (" + tSceneCounter + ", " + p + ", " + (tSceneCounter + p) / tSceneCount + ")");
                        foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                        {
                            tInterfaced.OnLoadingScenePercent(sTransitionData, tSceneToLoad, tSceneCounter, p, (tSceneCounter + p) / tSceneCount);
                        }
                    });
                    tTasks[tTaskCounter] = k;

                    //Debug.LogWarning("OnLoadingSceneFinish: (" + tSceneCounter + ", 1.0F, " + (tSceneCounter + 1.0F) / tSceneCount + ")");
                    foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
                    {
                        tInterfaced.OnLoadingSceneFinish(sTransitionData, tSceneToLoad, tSceneCounter, 1.0F, (tSceneCounter + 1.0F) / tSceneCount);
                    }
                }

                tSceneCounter++;
                tTaskCounter++;
            }

            await Task.WhenAll(tTasks);
            await Task.Yield();
            tIntermissionSceneStandBy.IsLoaded = true;
            //STSPerformance.EndTimer();

            //-------------------------------
            // Intermission STAND BY
            //-------------------------------
            while (StandByIsProgressing(tIntermissionSceneStandBy))
            {
                await Task.Yield();
            }

            // As soon as possible 
            foreach (STSIntermissionInterface tInterfaced in tIntermissionInterfaced)
            {
                tInterfaced.OnStandByFinish(tIntermissionSceneStandBy);
            }

            tIntermissionSceneStandBy.IsReadyToActivate = true;
            // Waiting to load the next Scene
            while (WaitingToLauchNextScene(tIntermissionSceneStandBy))
            {
                await Task.Yield();
            }

            //-------------------------------
            // Intermission GO TO NEXT SCENE PROCESS
            //-------------------------------
            // stanby is finished And the next scene can be lauch
            // disable user interactions on the Intermission scene
            EventSystemEnable(tIntermissionScene, false);
            OnTransitionSceneDisable(tIntermissionSceneInterfaced, sTransitionData);
            AnimationTransitionOut(tIntermissionSceneParams, sTransitionData);
            OnTransitionEnterStart(tIntermissionSceneInterfaced, sTransitionData, tIntermissionSceneParams);

            while (AnimationFinished() == false)
            {
                await Task.Yield();
            }

            OnTransitionExitFinish(tIntermissionSceneInterfaced, sTransitionData);
            OnTransitionSceneWillUnloaded(tIntermissionSceneInterfaced, sTransitionData);

            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            await ActiveAddedScenes(tTasks, sTransitionData);
            await Task.Yield();

            //-------------------------------
            // NEXT SCENE PROCESS
            //-------------------------------
            Scene tNextActiveScene = await ActivateNextScene(sNextActiveScene);

            //-------------------------------
            // Intermission UNLOAD
            //-------------------------------
            await UnloadSceneAsync(tIntermissionScene.name);

            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            await NextSceneEnable(tNextActiveScene, sTransitionData);

            // My transition is finish. I can do an another transition
            TransitionInProgress = false;
            await Task.Yield();
        }

        /// <summary>
        /// Disables the currently active scene based on the provided transition data. This involves handling various
        /// transition interfaces and animations before marking the scene as disabled.
        /// </summary>
        /// <param name="sActiveScene">The name of the currently active scene that needs to be disabled.</param>
        /// <param name="sTransitionData">Data containing details about the transition used to handle the scene disable process.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task ActualSceneDisable(string sActiveScene, STSTransitionData sTransitionData)
        {
            //-------------------------------
            // ACTUAL SCENE DISABLE
            //-------------------------------
            Scene tActualScene = SceneManager.GetSceneByName(sActiveScene);
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            STSTransitionInterface[] tActualSceneInterfaced = GetTransitionInterface(tActualScene);
            STSTransitionInterface[] tOtherSceneInterfaced = GetOtherTransitionInterface(tActualScene);

            EventSystemPrevent(false);
            OnTransitionSceneDisable(tActualSceneInterfaced, sTransitionData);
            AnimationTransitionOut(tActualSceneParams, sTransitionData);
            OnTransitionExitStart(tActualSceneInterfaced, sTransitionData, tActualSceneParams);
            OnTransitionExitStart(tOtherSceneInterfaced, sTransitionData, tActualSceneParams, false);

            while (AnimationFinished() == false)
            {
                await Task.Yield();
            }

            OnTransitionExitFinish(tActualSceneInterfaced, sTransitionData);
            OnTransitionExitFinish(tOtherSceneInterfaced, sTransitionData, false);
        }

        /// <summary>
        /// Activates the specified next scene by setting it as the active scene in the SceneManager.
        /// </summary>
        /// <param name="sNextActiveScene">The name of the scene to be activated next.</param>
        /// <returns>A Task representing the asynchronous operation, with a Scene as the result once the scene is activated.</returns>
        public async Task<Scene> ActivateNextScene(string sNextActiveScene)
        {
            Scene tNextActiveScene = SceneManager.GetSceneByName(sNextActiveScene);
            while (true) // Mandatory piece of code preventing from trying to activate an unloaded scene.
            {
                try
                {
                    SceneManager.SetActiveScene(tNextActiveScene);
                    break;
                }
                catch (Exception)
                {
                    await Task.Yield();
                }
            }

            CameraPrevent(true);
            AudioListenerPrevent(true);
            EventSystemEnable(tNextActiveScene, false);
            return tNextActiveScene;
        }

        /// <summary>
        /// Enables the next scene during the transition process.
        /// </summary>
        /// <param name="sNextActiveScene">The scene that will be the next active scene.</param>
        /// <param name="sTransitionData">Data related to the transition process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task NextSceneEnable(Scene sNextActiveScene, STSTransitionData sTransitionData)
        {
            //-------------------------------
            // NEXT SCENE ENABLE
            //-------------------------------
            STSTransition tNextSceneParams = GetTransitionsParams(sNextActiveScene);
            STSTransitionInterface[] tNextSceneInterfaced = GetTransitionInterface(sNextActiveScene);
            STSTransitionInterface[] tOtherNextSceneInterfaced = GetOtherTransitionInterface(sNextActiveScene);

            AnimationTransitionIn(tNextSceneParams, sTransitionData);
            OnTransitionEnterStart(tNextSceneInterfaced, sTransitionData, tNextSceneParams);
            OnTransitionEnterStart(tOtherNextSceneInterfaced, sTransitionData, tNextSceneParams, false);

            while (AnimationFinished() == false)
            {
                await Task.Yield();
            }

            OnTransitionEnterFinish(tNextSceneInterfaced, sTransitionData);
            OnTransitionEnterFinish(tOtherNextSceneInterfaced, sTransitionData, false);
            EventSystemPrevent(true);
            OnTransitionSceneEnable(tNextSceneInterfaced, sTransitionData);
        }

        /// <summary>
        /// Activates the scenes that have been added during the scene transition process.
        /// </summary>
        /// <param name="sTasks">An array of tasks representing the scenes to be activated.</param>
        /// <param name="sTransitionData">Data containing information relevant to the transition process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ActiveAddedScenes(Task[] sTasks, STSTransitionData sTransitionData)
        {
            //-------------------------------
            // ACTIVE ADDED SCENES
            //-------------------------------
            foreach (Task<SceneInstance> j in sTasks)
            {
                SceneInstance tInstance = j.Result;
                AsyncOperation tAsyncOperation = tInstance.ActivateAsync();

                while (!tAsyncOperation.isDone)
                {
                    await Task.Yield();
                }

                AddLoadedScene(tInstance);

                AudioListenerEnable(tInstance.Scene, false);
                CameraPreventEnable(tInstance.Scene, false);
                EventSystemEnable(tInstance.Scene, false);

                OnTransitionSceneLoaded(GetTransitionInterface(tInstance.Scene), sTransitionData);
            }
        }

        /// <summary>
        /// Loads a scene asynchronously with the specified parameters.
        /// </summary>
        /// <param name="sReference">The reference of the scene to load.</param>
        /// <param name="sMode">The load mode for the scene (Single or Additive), default is Single.</param>
        /// <param name="sActivatedOnLoad">If true, the scene will be activated once loaded, default is true.</param>
        /// <param name="bProgressCallBack">An optional callback that receives progress updates.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded SceneInstance.</returns>
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
            if (tHandle.Status == AsyncOperationStatus.Succeeded)
            {
                rScene = tHandle.Result;
            }
            else
            {
                Debug.LogWarning("Addressable " + tHandle.DebugName + " load error");
            }

            return rScene;
        }

        /// <summary>
        /// Unloads a scene asynchronously.
        /// </summary>
        /// <param name="sScene">The scene instance to be unloaded.</param>
        /// <param name="bProgressCallBack">Optional callback for progress updates.</param>
        /// <returns>A task that represents the asynchronous unload operation.
        /// The task result contains a boolean indicating if the unload operation completed successfully.</returns>
        private async Task<bool> UnloadSceneAsync(SceneInstance sScene, Action<float> bProgressCallBack = null)
        {
            AsyncOperationHandle<SceneInstance> tHandle = Addressables.UnloadSceneAsync(sScene);
            while (!tHandle.IsDone)
            {
                bProgressCallBack?.Invoke(tHandle.PercentComplete);
                await Task.Yield();
            }

            await tHandle.Task;

            SceneInstanceLoaded.Remove(sScene.Scene.name);

            return tHandle.IsDone;
        }

        /// <summary>
        /// Asynchronously unloads a scene by its name.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to unload.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task UnloadSceneAsync(string sSceneName)
        {
            AsyncOperation tAsyncOperation = SceneManager.UnloadSceneAsync(sSceneName);
            /*tAsyncOperation.completed += (asyncOperation) =>
            {
                Debug.LogWarning("-----------");
                Debug.LogWarning("completed unload! " + sSceneName);
                Debug.LogWarning("isdone: " + tAsyncOperation.isDone);
                Debug.LogWarning("-----------");
            };*/
            while (!tAsyncOperation.isDone)
            {
                await Task.Yield();
            }
        }

        /// <summary>
        /// Unloads the specified scene instance asynchronously.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to unload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UnloadSceneInstance(string sSceneName)
        {
            SceneInstance tInstance = GetInstance(sSceneName);
            if (string.IsNullOrEmpty(tInstance.Scene.name))
            {
                await UnloadSceneAsync(sSceneName);
            }
            else
            {
                await UnloadSceneAsync(tInstance);
                /*Task<bool> tDone = UnloadSceneAsync(tInstance, bProgressCallBack: (p) =>
                {
                    Debug.LogWarning("pourcent: " + p);
                });*/
            }
        }

        /// <summary>
        /// Retrieves the transition interfaces for a given scene name.
        /// </summary>
        /// <param name="sSceneName">The name of the scene for which to retrieve the transition interfaces.</param>
        /// <returns>An array of transition interfaces associated with the specified scene.</returns>
        private STSTransitionInterface[] GetInterface(string sSceneName)
        {
            Scene tActualScene = SceneManager.GetSceneByName(sSceneName);
            STSTransition tActualSceneParams = GetTransitionsParams(tActualScene);
            return GetTransitionInterface(tActualScene);
        }

        /// <summary>
        /// Retrieves the instance of a scene by its name from the loaded scene instances.
        /// </summary>
        /// <param name="sSceneName">The name of the scene to retrieve the instance for.</param>
        /// <returns>The <c>SceneInstance</c> corresponding to the given scene name. If the scene is not found, returns a new <c>SceneInstance</c>.</returns>
        private SceneInstance GetInstance(string sSceneName)
        {
            SceneInstance rInstance = new SceneInstance();
            foreach (KeyValuePair<string, SceneInstance> k in SceneInstanceLoaded)
            {
                if (k.Key.Equals(sSceneName))
                {
                    rInstance = k.Value;
                    break;
                }
            }

            return rInstance;
        }

        /// <summary>
        /// Adds a loaded scene to the SceneInstanceLoaded dictionary if it does not already exist.
        /// </summary>
        /// <param name="sInstance">The SceneInstance object representing the loaded scene.</param>
        private void AddLoadedScene(SceneInstance sInstance)
        {
            if (!SceneInstanceLoaded.ContainsKey(sInstance.Scene.name))
            {
                SceneInstanceLoaded.Add(sInstance.Scene.name, sInstance);
            }
            else
            {
                Debug.LogWarning(sInstance.Scene.name + " already be added in SceneInstanceLoaded!");
            }
        }

        /// <summary>
        /// Handles the start of a transition into a new scene, utilizing the provided transition interfaces and data.
        /// </summary>
        /// <param name="sInterface">An array of interfaces that implement the STSTransitionInterface, which define actions to perform upon scene transition start.</param>
        /// <param name="sData">An object containing the data relevant to the transition.</param>
        /// <param name="sParams">An object defining the parameters for the transition effect.</param>
        /// <param name="sActiveScene">Optional boolean indicating whether the current scene is the active scene. Defaults to true.</param>
        private void OnTransitionEnterStart(STSTransitionInterface[] sInterface, STSTransitionData sData, STSTransition sParams, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionEnterStart(sData, sParams.EffectOnEnter, sParams.InterEffectDuration, sActiveScene);
            }
        }

        /// <summary>
        /// Invoked at the completion of the transition enter phase.
        /// Notifies all provided transition interfaces of the completion.
        /// </summary>
        /// <param name="sInterface">An array of transition interfaces to be notified.</param>
        /// <param name="sData">Transition data relevant to the transition process.</param>
        /// <param name="sActiveScene">Indicates whether the scene is currently active. Default is true.</param>
        private void OnTransitionEnterFinish(STSTransitionInterface[] sInterface, STSTransitionData sData, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionEnterFinish(sData, sActiveScene);
            }
        }

        /// Handles the initial phase of the scene transition exit process.
        /// <param name="sInterface">An array of interfaces that implement the STSTransitionInterface.</param>
        /// <param name="sData">Data related to the transition.</param>
        /// <param name="sSceneParams">Parameters specific to the scene transition.</param>
        /// <param name="sActiveScene">Optional boolean parameter indicating if the current scene is active. Default is true.</param>
        private void OnTransitionExitStart(STSTransitionInterface[] sInterface, STSTransitionData sData, STSTransition sSceneParams, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionExitStart(sData, sSceneParams.EffectOnExit, sActiveScene);
            }
        }

        /// <summary>
        /// Method to signal the completion of a transition exit.
        /// Calls the OnTransitionExitFinish method on each provided transition interface.
        /// </summary>
        /// <param name="sInterface">An array of transition interfaces to notify about the transition exit completion.</param>
        /// <param name="sData">Data associated with the transition.</param>
        /// <param name="sActiveScene">Indicates whether the active scene should be affected by the transition. Default is true.</param>
        private void OnTransitionExitFinish(STSTransitionInterface[] sInterface, STSTransitionData sData, bool sActiveScene = true)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionExitFinish(sData, sActiveScene);
            }
        }

        /// <summary>
        /// Enables the transition scene with specified transition interfaces and associated transition data.
        /// </summary>
        /// <param name="sInterface">An array of transition interfaces to enable the transition scene.</param>
        /// <param name="sData">The data associated with the transition to be used by the interfaces.</param>
        private void OnTransitionSceneEnable(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneEnable(sData);
            }
        }

        /// <summary>
        /// Disables the transition scenes for the specified transition interfaces.
        /// </summary>
        /// <param name="sInterface">An array of transition interfaces that will have their transition scenes disabled.</param>
        /// <param name="sData">Data associated with the transition process.</param>
        private void OnTransitionSceneDisable(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneDisable(sData);
            }
        }

        /// <summary>
        /// Invokes the OnTransitionSceneWillUnloaded method on all provided transition interfaces
        /// with the given transition data. This method is called when a scene is about to be unloaded.
        /// </summary>
        /// <param name="sInterface">An array of STSTransitionInterface instances that will have their OnTransitionSceneWillUnloaded method called.</param>
        /// <param name="sData">The STSTransitionData object containing data relevant to the scene transition.</param>
        private void OnTransitionSceneWillUnloaded(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneWillUnloaded(sData);
            }
        }

        /// <summary>
        /// Called when the transition scene has been loaded.
        /// </summary>
        /// <param name="sInterface">An array of interfaces that implement the STSTransitionInterface.</param>
        /// <param name="sData">An instance of STSTransitionData containing data related to the transition.</param>
        private void OnTransitionSceneLoaded(STSTransitionInterface[] sInterface, STSTransitionData sData)
        {
            foreach (STSTransitionInterface k in sInterface)
            {
                k.OnTransitionSceneLoaded(sData);
            }
        }
    }
}