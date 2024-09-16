using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Manages a single instance of AudioListener across different scenes,
    /// ensuring that no multiple AudioListeners exist simultaneously.
    /// </summary>
    public partial class STSAudioListener : STSSingletonUnity<STSAudioListener>
    {
        /// <summary>
        /// SharedAudioListener is the singleton instance of the AudioListener component
        /// for the SceneTransitionSystem. It ensures that only one AudioListener is active
        /// at any given time across scenes, preventing audio conflicts and issues.
        /// </summary>
        AudioListener SharedAudioListener;

        /// <summary>
        /// The object that defines the position of the shared audio listener in the scene.
        /// </summary>
        public GameObject AudioFollowObject;

        /// <summary>
        /// Represents the default camera used by the STSAudioListener.
        /// This camera is typically the main camera in the scene. It is
        /// set during the initialization and scene management processes
        /// to ensure consistent audio listener behavior.
        /// </summary>
        Camera DefaultCamera;

        /// <summary>
        /// A method that is executed after a scene load to initialize the audio listener singleton instance.
        /// This method is decorated with the [RuntimeInitializeOnLoadMethod] attribute, ensuring it runs
        /// automatically at the appropriate point in the Unity scene loading lifecycle.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            Singleton();
        }

        // Memory managment
        /// <summary>
        /// Initializes the singleton instance of the STSAudioListener class by adding an AudioListener component.
        /// </summary>
        public override void InitInstance()
        {
            SharedAudioListener = gameObject.AddComponent<AudioListener>();
        }

        /// <summary>
        /// Called when the script instance is being enabled.
        /// This method subscribes to the SceneManager's sceneLoaded and sceneUnloaded events
        /// to handle audio listeners when scenes are loaded or unloaded. It also ensures
        /// that there is only one active AudioListener in the scene by calling the Prevent method.
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
            Prevent();
        }

        /// <summary>
        /// Handles actions to perform when a new scene has been loaded in the SceneManager.
        /// </summary>
        /// <param name="scene">The scene that has been loaded.</param>
        /// <param name="mode">The mode in which the scene was loaded.</param>
        public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Prevent();
        }

        /// <summary>
        /// Called when a scene has been unloaded.
        /// </summary>
        /// <param name="scene">The scene that was unloaded.</param>
        public override void OnSceneUnLoaded(Scene scene)
        {
            Prevent();
        }

        /// <summary>
        /// Prevents the creation of multiple AudioListener components in the scene.
        /// Ensures that only the shared AudioListener exists by destroying any others.
        /// Additionally, sets the DefaultCamera to the main camera in the scene.
        /// Called on various scene events and component enablement to maintain a single AudioListener.
        /// </summary>
        private void Prevent()
        {
            foreach (AudioListener tAudio in FindObjectsOfType<AudioListener>())
            {
                if (tAudio != SharedAudioListener)
                {
                    Destroy(tAudio);
                }
            }

            DefaultCamera = Camera.main;
        }

        /// <summary>
        /// Updates the position of the AudioListener in each frame.
        /// </summary>
        /// <remarks>
        /// If <c>AudioFollowObject</c> is set, the position of the AudioListener will
        /// be updated to match the position of the AudioFollowObject. If
        /// <c>AudioFollowObject</c> is not set but <c>DefaultCamera</c> is available,
        /// the position of the AudioListener will be updated to match the position of
        /// the DefaultCamera. This ensures that the AudioListener is always positioned
        /// appropriately based on the available objects in the scene.
        /// </remarks>
        private void Update()
        {
            if (AudioFollowObject != null)
            {
                transform.position = AudioFollowObject.transform.position;
            }
            else
            {
                if (DefaultCamera != null)
                {
                    transform.position = DefaultCamera.transform.position;
                }
            }
        }
    }
}