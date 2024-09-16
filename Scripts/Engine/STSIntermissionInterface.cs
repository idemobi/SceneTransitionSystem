namespace SceneTransitionSystem
{
    /// <summary>
    /// Interface defining methods for handling intermissions during scene transitions.
    /// </summary>
    public interface STSIntermissionInterface
    {
        /// Called when all scenes have been fully loaded.
        /// <param name="sData">Transition data associated with the scene loading process.</param>
        /// <param name="sSceneName">Name of the scene that was loaded.</param>
        /// <param name="SceneNumber">Index of the scene in the loading sequence.</param>
        /// <param name="sPercent">Percentage of the total transition process completed.</param>
        void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent);

        /// <summary>
        /// This method is invoked at the start of the scene loading process.
        /// </summary>
        /// <param name="sData">The transition data for the scene.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The sequence number of the scene.</param>
        /// <param name="sScenePercent">The percentage of the current scene's loading progress.</param>
        /// <param name="sPercent">The overall percentage of the loading process.</param>
        void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent);

        /// <summary>
        /// Method invoked to update the loading progress of a scene.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene loading process.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The numerical identifier for the scene being loaded.</param>
        /// <param name="sScenePercent">The percentage progress of the current scene being loaded.</param>
        /// <param name="sPercent">The cumulative percentage progress of all scenes being loaded.</param>
        void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent);

        /// Called when the loading of a scene has finished.
        /// <param name="sData">Transition data associated with the scene transition.</param>
        /// <param name="sSceneName">Name of the scene that has finished loading.</param>
        /// <param name="SceneNumber">The number identifier of the scene that has finished loading.</param>
        /// <param name="sScenePercent">The specific percentage of the scene that has been loaded, typically this will be 1.0f when the scene is fully loaded.</param>
        /// <param name="sPercent">The overall percentage of the entire loading sequence.</param>
        void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent);

        /// Handles actions to be performed when a scene is being unloaded.
        /// <param name="sData">The transition data associated with the scene transition.</param>
        /// <param name="sSceneName">The name of the scene being unloaded.</param>
        /// <param name="SceneNumber">The numerical identifier of the scene being unloaded.</param>
        /// <param name="sPercent">The percentage of the current transition process completed.</param>
        void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent);

        /// <summary>
        /// Triggered when the stand-by state begins within the scene transition system.
        /// </summary>
        /// <param name="sStandBy">Instance of STSIntermission that represents the stand-by state.</param>
        void OnStandByStart(STSIntermission sStandBy);

        /// <summary>
        /// Method to handle the completion of the standby phase during a scene transition.
        /// </summary>
        /// <param name="sStandBy">The STSIntermission instance representing the completed standby phase.</param>
        void OnStandByFinish(STSIntermission sStandBy);
    }
}