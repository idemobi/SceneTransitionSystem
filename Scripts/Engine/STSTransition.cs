using UnityEngine;
using System.Linq;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a scene transition within the Scene Transition System. This class manages
    /// the effects and duration associated with entering and exiting scenes.
    /// </summary>
    public class STSTransition : SharedInstanceUnity<STSTransition>
    {
        /// <summary>
        /// Specifies the transition effect to be applied when entering a scene.
        /// </summary>
        public STSEffectType EffectOnEnter;

        /// <summary>
        /// Duration between the effects applied during a scene transition.
        /// This duration determines the time interval between the enter and
        /// exit effects within the transition process.
        /// </summary>
        [Range(0.0F, 5.0F)] public float InterEffectDuration = 0.50F;

        /// <summary>
        /// The type of transition effect to be played when exiting a scene.
        /// </summary>
        public STSEffectType EffectOnExit;

        /// <summary>
        /// Initializes the singleton instance of the STSTransition class.
        /// This method ensures that the necessary singleton instances for scene management
        /// and addressable assets are created and initialized. It is called during the
        /// initialization process of the STSTransition instance.
        /// </summary>
        public override void InitInstance()
        {
            base.InitInstance();
            STSSceneManager.Singleton();
            STSAddressableAssets.Singleton();
        }

        /// <summary>
        /// Invoked when the script instance is being loaded.
        /// </summary>
        /// <remarks>
        /// This method is called on the frame when a script is enabled just before
        /// any of the Update methods are called the first time.
        /// </remarks>
        void Start()
        {
        }

        /// <summary>
        /// Copies the transition parameters from the current instance to the specified destination instance.
        /// </summary>
        /// <param name="sDestination">The destination instance where the transition parameters will be copied.</param>
        public void CopyIn(STSTransition sDestination)
        {
            sDestination.EffectOnEnter = this.EffectOnEnter.Dupplicate();
            sDestination.InterEffectDuration = this.InterEffectDuration;
            sDestination.EffectOnExit = this.EffectOnExit.Dupplicate();
        }
    }
}