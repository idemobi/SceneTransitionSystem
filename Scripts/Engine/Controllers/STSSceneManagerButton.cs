using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSSceneManagerButton class is responsible for initiating scene transitions.
    /// It utilizes the STSSceneManager to replace the current scene with designated scenes,
    /// potentially using an intermission scene in between.
    /// </summary>
    public class STSSceneManagerButton : MonoBehaviour
    {
        /// <summary>
        /// Represents the currently active scene in the Scene Transition System.
        /// </summary>
        /// <remarks>
        /// This variable is used to store the scene that is currently active and
        /// can be transitioned to or replaced by other scenes in the system. It
        /// plays a central role in managing scene transitions.
        /// </remarks>
        public STSScene ActiveScene;

        /// <summary>
        /// Represents the scene used as an intermediate step during the scene transition.
        /// This scene is displayed temporarily while the transition between the active scene
        /// and the next scene(s) occurs, allowing for a smoother user experience.
        /// </summary>
        public STSScene IntermissionScene;

        /// <summary>
        /// An array of additional scenes that are involved in the transition process
        /// managed by the Scene Transition System. These scenes will be included alongside
        /// the active and intermission scenes during the execution of the transition.
        /// </summary>
        public STSScene[] AdditionnalScenes;

        /// <summary>
        /// Initiates the transition process to the specified active scene, optionally including additional scenes and an intermission scene.
        /// </summary>
        public void RunTransition()
        {
            Debug.Log("STSSceneButton RunTransition()");
            STSSceneManager.ReplaceAllByScenes(ActiveScene, AdditionnalScenes, IntermissionScene);
        }
    }
}