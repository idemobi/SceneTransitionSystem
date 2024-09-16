using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents different debug colors used for logging in the Scene Transition System.
    /// </summary>
    public enum STSSceneDebugColor
    {
        /// <summary>
        /// Specifies the debug color as black for logging purposes in the Scene Transition System.
        /// </summary>
        black,

        /// <summary>
        /// Specifies the debug color as red for logging purposes in the Scene Transition System.
        /// </summary>
        red,

        /// <summary>
        /// Represents the green color option for debugging scene transitions.
        /// When the <see cref="STSSceneController.ActiveLog"/> is set to true and this color is selected,
        /// debug messages will be displayed in green within the Unity console.
        /// </summary>
        green,

        /// <summary>
        /// Represents debug colors for the scene transition system.
        /// </summary>
        yellow,

        /// <summary>
        /// Represents the color blue for debugging purposes within the scene transition system.
        /// </summary>
        blue,

        /// <summary>
        /// Represents the gray color option for scene debug logging.
        /// </summary>
        gray,
    }

    /// <summary>
    /// The STSSceneController class is responsible for managing various scene transitions in a Unity application.
    /// It provides several virtual methods which can be overridden to handle specific stages of the transition process.
    /// </summary>
    /// <remarks>
    /// This class implements the STSTransitionInterface and logs transition events based on the ActiveLog property setting.
    /// The log messages can be color-coded by setting the LogTagColor property with a value from STSSceneDebugColor.
    /// </remarks>
    public class STSSceneController : MonoBehaviour, STSTransitionInterface
    {
        /// <summary>
        /// When set to true, enables logging of various scene transition events.
        /// </summary>
        [Header("Debug Mode")] public bool ActiveLog = false;

        /// <summary>
        /// Specifies the color used for logging scene transition messages.
        /// </summary>
        /// <remarks>
        /// This variable controls the color of the log messages that are printed when the <see cref="ActiveLog"/> flag is true.
        /// Available colors are defined in the <see cref="STSSceneDebugColor"/> enum.
        /// </remarks>
        public STSSceneDebugColor LogTagColor = STSSceneDebugColor.black;

        /// <summary>
        /// Called when the transition scene has been successfully loaded.
        /// </summary>
        /// <param name="sData">Data related to the current scene transition.</param>
        public virtual void OnTransitionSceneLoaded(STSTransitionData sData)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionSceneLoaded()");
            }
        }

        /// Called when the scene transition animation or effect finishes entering.
        /// <param name="sData">The transition data associated with the current transition.</param> <param name="sActiveScene">Indicates whether the current scene is the active scene.</param>
        /// /
        public virtual void OnTransitionEnterFinish(STSTransitionData sData, bool sActiveScene)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionEnterFinish()");
            }
        }

        /// <summary>
        /// Invoked at the start of a scene transition entering process.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene.</param>
        /// <param name="sEffect">The effect type to be applied during the transition.</param>
        /// <param name="sInterludeDuration">The duration of the interlude during the transition.</param>
        /// <param name="sActiveScene">Indicates whether the current scene is active.</param>
        public virtual void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration, bool sActiveScene)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionEnterStart()");
            }
        }

        /// Called when the transition scene is enabled.
        /// <param name="sData">Data related to the current scene transition.</param>
        public virtual void OnTransitionSceneEnable(STSTransitionData sData)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionSceneEnable()");
            }
        }

        /// <summary>
        /// Called when a scene transition is disabled.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene.</param>
        public virtual void OnTransitionSceneDisable(STSTransitionData sData)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionSceneDisable()");
            }
        }

        /// <summary>
        /// Called at the start of a transition exit process.
        /// </summary>
        /// <param name="sData">Data associated with the scene transition.</param>
        /// <param name="sEffect">Effect to be used during the transition.</param>
        /// <param name="sActiveScene">Indicates if the scene to be unloaded is the active scene.</param>
        public virtual void OnTransitionExitStart(STSTransitionData sData, STSEffectType sEffect, bool sActiveScene)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionExitStart()");
            }
        }

        /// <summary>
        /// Called when the scene transition exit finishes.
        /// </summary>
        /// <param name="sData">Data related to the current transition.</param>
        /// <param name="sActiveScene">Indicates whether the current scene is active.</param>
        public virtual void OnTransitionExitFinish(STSTransitionData sData, bool sActiveScene)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionExitFinish()");
            }
        }

        /// <summary>
        /// This method is called just before a scene is unloaded during a transition.
        /// It logs a message if ActiveLog is enabled.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene transition.</param>
        public virtual void OnTransitionSceneWillUnloaded(STSTransitionData sData)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnTransitionSceneWillUnloaded()");
            }
        }
    }
}