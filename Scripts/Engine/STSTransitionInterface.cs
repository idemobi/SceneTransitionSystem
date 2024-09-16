namespace SceneTransitionSystem
{
    /// <summary>
    /// Interface for handling various scene transition events within
    /// the Scene Transition System (STS).
    /// </summary>
    public interface STSTransitionInterface
    {
        /// Invoked when a scene transition is completed and the new scene is loaded.
        /// <param name="sData">Data relevant to the scene transition process.</param>
        void OnTransitionSceneLoaded(STSTransitionData sData);

        /// Called when the transition into a new scene starts.
        /// <param name="sData">Data relevant to the scene transition process.</param>
        /// <param name="sEffect">The effect type to be used during the transition.</param>
        /// <param name="sInterludeDuration">The duration of the interlude effect.</param>
        /// <param name="sActiveScene">Indicates whether the scene being transitioned into will be the active scene.</param>
        void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration, bool sActiveScene);

        /// <summary>
        /// Called when the transition entering phase has finished.
        /// </summary>
        /// <param name="sData">The transition data associated with the current transition.</param>
        /// <param name="sActiveScene">Indicates whether the scene being transitioned to is the active scene.</param>
        void OnTransitionEnterFinish(STSTransitionData sData, bool sActiveScene);

        /// <summary>
        /// Method to handle enable transitions for a scene. Implement this method to define custom behavior
        /// that should occur when a scene transition is enabled.
        /// </summary>
        /// <param name="sData">The data related to the scene transition.</param>
        void OnTransitionSceneEnable(STSTransitionData sData);

        /// <summary>
        /// This method is called to handle the disabling of a transition scene within the Scene Transition System.
        /// </summary>
        /// <param name="sData">Data related to the transition scene.</param>
        void OnTransitionSceneDisable(STSTransitionData sData);

        /// <summary>
        /// Called when the transition exit process is started.
        /// </summary>
        /// <param name="sData">The transition data containing information about the transition.</param>
        /// <param name="sEffect">The effect type to be applied during the transition process.</param>
        /// <param name="sActiveScene">Indicates whether the active scene is included in the transition.</param>
        void OnTransitionExitStart(STSTransitionData sData, STSEffectType sEffect, bool sActiveScene);

        /// Called when a transition exit finishes.
        /// <param name="sData">The transition data associated with the event.</param>
        /// <param name="sActiveScene">Indicates whether the transition is related to an active scene.</param>
        void OnTransitionExitFinish(STSTransitionData sData, bool sActiveScene);

        /// <summary>
        /// Invoked when the transition scene is about to be unloaded.
        /// </summary>
        /// <param name="sData">The data associated with the transition.</param>
        void OnTransitionSceneWillUnloaded(STSTransitionData sData);
    }
}