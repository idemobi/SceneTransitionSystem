using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Provides functionality to handle scene transitions through a button press.
    /// </summary>
    public class STSAddressableAssetButton : MonoBehaviour
    {
        /// <summary>
        /// Represents the current active scene in the scene transition system.
        /// </summary>
        public STSScene ActiveScene;

        /// <summary>
        /// Intermission scene to be used during transitions between active and additional scenes.
        /// </summary>
        public STSScene IntermissionScene;

        /// <summary>
        /// An array of additional scenes to be included in the scene transition process.
        /// These scenes will be added alongside the active scene during a transition.
        /// </summary>
        public STSScene[] AdditionnalScenes;

        /// <summary>
        /// Initiates a scene transition by replacing the current active scene
        /// and additional scenes with new ones, using an intermission scene as well.
        /// </summary>
        public void RunTransition()
        {
            Debug.Log("STSSceneButton RunTransition()");
            STSAddressableAssets.ReplaceAllByScenes(ActiveScene, AdditionnalScenes, IntermissionScene);
        }
    }
}