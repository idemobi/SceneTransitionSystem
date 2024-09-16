using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSScene class represents a scene within the Scene Transition System.
    /// </summary>
    [Serializable]
    public class STSScene
    {
        /// <summary>
        /// The file path of the scene intended for loading or referencing within the Scene Transition System.
        /// </summary>
        public string ScenePath;

        /// <summary>
        /// The path of the scene.
        /// </summary>
        public STSScene()
        {
            ScenePath = string.Empty;
        }

        /// <summary>
        /// Retrieves the Unity Scene object associated with the ScenePath.
        /// </summary>
        /// <returns>
        /// Returns a UnityEngine.SceneManagement.Scene object
        /// that corresponds to the given ScenePath.
        /// </returns>
        public Scene GetScene()
        {
            return SceneManager.GetSceneByPath(ScenePath);
        }

        /// <summary>
        /// Extracts and returns the filename without its extension from the ScenePath property.
        /// </summary>
        /// <returns>
        /// The filename without its extension from the ScenePath property.
        /// </returns>
        public string GetSceneShortName()
        {
            return Path.GetFileNameWithoutExtension(ScenePath);
        }
    }
}