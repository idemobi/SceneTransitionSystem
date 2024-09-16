using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Controller class for managing scene intermissions in the Scene Transition System (STS).
    /// </summary>
    public class STSSceneIntermissionController : STSSceneController, STSIntermissionInterface
    {
        /// <summary>
        /// Represents a user interface gauge component that displays a horizontal fill value.
        /// </summary>
        /// <remarks>
        /// The gauge can be configured to expand horizontally or vertically, with min and max values
        /// for each direction. It supports smooth transitions and animations for value changes.
        /// </remarks>
        [Header("Gauge")] public STSScreenGauge Gauge;

        /// <summary>
        /// Executes initialization processes during the start of a scene transition.
        /// This includes setting the Gauge to hidden and resetting its horizontal value.
        /// </summary>
        /// <param name="sData">Data associated with the transition.</param>
        /// <param name="sEffect">Effect type used during the transition.</param>
        /// <param name="sInterludeDuration">Duration of the interlude period in the transition.</param>
        /// <param name="sActiveScene">Boolean indicating if the active scene is part of the transition.</param>
        public override void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration, bool sActiveScene)
        {
            base.OnTransitionEnterStart(sData, sEffect, sInterludeDuration, sActiveScene);
            if (Gauge != null)
            {
                Gauge.SetHidden(true);
                Gauge.HorizontalValue = 0.0F;
            }
        }

        /// <summary>
        /// Initializes the STSSceneIntermissionController. This method will be called by Unity
        /// when the script instance is being loaded.
        /// </summary>
        /// <remarks>
        /// If a Gauge is assigned, it will be set to hidden, and its horizontal value will be
        /// initialized to 0.0F. This prepares the Gauge to be used for scene transitioning.
        /// </remarks>
        private void Start()
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(true);
                Gauge.HorizontalValue = 0.0F;
            }
        }

        /// <summary>
        /// Handles the finalization steps when all scenes are fully loaded.
        /// This method can be overridden in derived classes to provide custom behavior
        /// when all scenes have completed loading.
        /// </summary>
        /// <param name="sData">Transition data containing relevant information about the scene transition.</param>
        /// <param name="sSceneName">The name of the scene that has been loaded.</param>
        /// <param name="SceneNumber">The number associated with the loaded scene.</param>
        /// <param name="sPercent">The percentage of the total loading process completed.</param>
        public virtual void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }

            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnSceneAllReadyLoaded() " + sSceneName + " (n°" + SceneNumber + ") Total loading " + sPercent.ToString("P"));
            }
        }

        /// <summary>
        /// Method triggered when the scene loading process starts.
        /// </summary>
        /// <param name="sData">Transition data associated with the scene loading.</param>
        /// <param name="sSceneName">The name of the scene being loaded.</param>
        /// <param name="SceneNumber">The number identifier of the scene being loaded.</param>
        /// <param name="sScenePercent">The percentage of the current scene's loading progress.</param>
        /// <param name="sPercent">The overall percentage of the total loading progress.</param>
        public virtual void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }

            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnLoadingSceneStart() " + sSceneName + " (n°" + SceneNumber + " loading " + sScenePercent.ToString("P") + ") Total loading " + sPercent.ToString("P"));
            }
        }

        /// <summary>
        /// Updates the loading percentage of a scene and manages the visibility and value of the loading gauge.
        /// </summary>
        /// <param name="sData">Transition data related to the scene loading process.</param>
        /// <param name="sSceneName">Name of the scene being loaded.</param>
        /// <param name="SceneNumber">Number identifier of the scene.</param>
        /// <param name="sScenePercent">Current loading percentage of the specific scene.</param>
        /// <param name="sPercent">Overall loading percentage across all scenes.</param>
        public virtual void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }

            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnLoadingScenePercent() " + sSceneName + " (n°" + SceneNumber + " loading " + sScenePercent.ToString("P") + ") Total loading " + sPercent.ToString("P"));
            }
        }

        /// <summary>
        /// Called when the loading of a scene is finished.
        /// </summary>
        /// <param name="sData">The transition data associated with the scene loading process.</param>
        /// <param name="sSceneName">The name of the scene that has finished loading.</param>
        /// <param name="SceneNumber">The number identifier for the scene.</param>
        /// <param name="sScenePercent">Percentage of the scene that has been loaded.</param>
        /// <param name="sPercent">Total percentage of all scenes loaded.</param>
        public virtual void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }

            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnLoadingSceneFinish() " + sSceneName + " (n°" + SceneNumber + " loading " + sScenePercent.ToString("P") + ") Total loading " + sPercent.ToString("P"));
            }
        }

        /// <summary>
        /// Handles the operations required when unloading a scene.
        /// </summary>
        /// <param name="sData">Data related to the transition.</param>
        /// <param name="sSceneName">The name of the scene being unloaded.</param>
        /// <param name="SceneNumber">The identifier number of the scene.</param>
        /// <param name="sPercent">The percentage of completion for the unloading process.</param>
        public virtual void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }

            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnUnloadScene() " + sSceneName + " (n°" + SceneNumber + ") Total loading " + sPercent.ToString("P"));
            }
        }

        /// <summary>
        /// Handles the completion of the standby phase during a scene transition.
        /// </summary>
        /// <param name="sStandBy">The STSIntermission instance representing the current standby state.</param>
        public virtual void OnStandByFinish(STSIntermission sStandBy)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnStandByFinish() ");
            }
        }

        /// <summary>
        /// Method called when the standby phase of the intermission starts.
        /// </summary>
        /// <param name="sStandBy">An instance of <see cref="STSIntermission"/> representing the standby intermission.</param>
        public virtual void OnStandByStart(STSIntermission sStandBy)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnStandByStart() ");
            }
        }
    }
}