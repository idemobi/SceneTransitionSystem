using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents an intermission in the Scene Transition System (STS).
    /// This class is responsible for handling the intermission period between scene transitions,
    /// managing the standby time, and determining whether the next scene should be activated automatically.
    /// </summary>
    public class STSIntermission : SharedInstanceUnity<STSIntermission>
    {
        /// <summary>
        /// The amount of time, in seconds, to wait during a scene transition.
        /// </summary>
        /// <remarks>
        /// This variable defines the minimum stand-by duration before transitioning to the next scene.
        /// </remarks>
        [Tooltip("Minimum stand by on transition scene in seconds")]
        public float StandBySeconds = 0.0f;

        /// <summary>
        /// Determines whether the next scene should be activated automatically
        /// after the current transition and standby period completes.
        /// </summary>
        [Tooltip("The next scene must be active automatically?")]
        public bool AutoActiveNextScene = true;

        /// <summary>
        /// Gets a value indicating whether the STSIntermission scene is loaded.
        /// </summary>
        public bool IsLoaded { get; internal set; } = false;

        /// <summary>
        /// Gets a value indicating whether the intermission is ready to be activated.
        /// </summary>
        /// <value>
        /// <c>true</c> if the intermission is ready to be activated; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyToActivate { get; internal set; } = false;

        /// <summary>
        /// Copies config values from the current instance to another instance of STSIntermission.
        /// </summary>
        /// <param name="sDestination">The target STSIntermission instance that will receive the config values.</param>
        public void CopyFrom(STSIntermission sDestination)
        {
            sDestination.CopyIn(this);
        }

        /// <summary>
        /// Copies the settings from the specified <see cref="STSIntermission"/> instance to this instance.
        /// </summary>
        /// <param name="sDestination">The destination <see cref="STSIntermission"/> instance where the settings will be copied to.</param>
        public void CopyIn(STSIntermission sDestination)
        {
            sDestination.StandBySeconds = this.StandBySeconds;
            sDestination.AutoActiveNextScene = this.AutoActiveNextScene;
        }

        /// <summary>
        /// Transitions the system state from a stand-by status to active by invoking the FinishStandBy method on
        /// both the STSSceneManager and STSAddressableAssets singletons.
        /// </summary>
        public void FinishStandByAction()
        {
            STSSceneManager.Singleton().FinishStandBy();
            STSAddressableAssets.Singleton().FinishStandBy();
        }
    }
}