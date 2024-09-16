using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Base class for shared instances within the Scene Transition System.
    /// </summary>
    public class SharedInstanceBasis : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether the shared instance has been initialized.
        /// </summary>
        public bool Initialized = false;

        /// <summary>
        /// Initializes the instance of the shared basis class.
        /// Must be overridden in derived classes to provide specific initialization logic.
        /// </summary>
        public virtual void InitInstance()
        {
        }
    }

    /// <summary>
    /// Represents a generic singleton pattern for Unity MonoBehaviour across scenes.
    /// </summary>
    /// <typeparam name="K">Type parameter that extends SharedInstanceBasis, used to create singleton instance per scene.</typeparam>
    public class SharedInstanceUnity<K> : SharedInstanceBasis where K : SharedInstanceBasis, new()
    {
        /// <summary>
        /// A dictionary that maps each Scene to its corresponding shared instance of type K.
        /// This allows different scenes to have their own unique instances of a specific shared
        /// instance type, ensuring that scene-specific data and behavior can be managed independently.
        /// </summary>
        private static Dictionary<Scene, K> kSharedInstanceBySceneList = new Dictionary<Scene, K>();

        /// <summary>
        /// A static reference to the shared instance of the generic type <typeparamref name="K"/>
        /// within the <see cref="SceneTransitionSystem"/> namespace. This instance follows the
        /// singleton pattern and is associated with a specific scene.
        /// </summary>
        private static K kSharedInstance = null;

        /// <summary>
        /// Initializes the shared instance for the current GameObject if it has not been
        /// initialized. Checks if an instance already exists in the current scene; if not,
        /// adds the instance to the dictionary of shared instances and initializes it.
        /// </summary>
        private void Awake()
        {
            //Debug.Log("SharedInstanceUnity<K> Awake() for gameobject named '" + gameObject.name + "'");
            kSharedInstance = this as K;
            //Check if there is already an instance of K
            Scene tScene = gameObject.scene;
            if (kSharedInstanceBySceneList.ContainsKey(tScene) == false)
            {
                //Debug.Log("SharedInstanceUnity<K> Awake() case kSharedInstance == null for gameobject named '" + gameObject.name + "'");
                //if not, set it to this.
                kSharedInstanceBySceneList.Add(tScene, this as K);
                if (Initialized == false)
                {
                    //Debug.Log("SharedInstanceUnity<K> Awake() case kSharedInstance.Initialized == false for gameobject named '" + gameObject.name + "'");
                    // Init Instance
                    InitInstance();
                    // memorize the init instance
                    Initialized = true;
                }
            }
        }

        /// <summary>
        /// Retrieves the last shared instance of the specified type.
        /// </summary>
        /// <returns>
        /// The last shared instance of type K if it exists; otherwise, null.
        /// </returns>
        public static K LastSharedInstance()
        {
            return kSharedInstance;
        }

        /// <summary>
        /// Retrieves the shared instance of the specified type for the active scene.
        /// If no instance exists for the scene, a new one will be created.
        /// </summary>
        /// <typeparam name="K">The type of the shared instance, which must inherit from SharedInstanceBasis.</typeparam>
        /// <returns>The shared instance of the specified type for the active scene.</returns>
        public static K SharedInstance()
        {
            return SharedInstance(SceneManager.GetActiveScene());
        }

        /// <summary>
        /// Determines if a shared instance exists for the given scene.
        /// </summary>
        /// <param name="sScene">The scene to check for the existence of a shared instance.</param>
        /// <returns>True if a shared instance exists for the scene; otherwise, false.</returns>
        public static bool SharedInstanceExists(Scene sScene)
        {
            bool rReturn = kSharedInstanceBySceneList.ContainsKey(sScene);
            return rReturn;
        }

        /// <summary>
        /// Returns the shared instance of the specified type for the given scene.
        /// If the instance does not exist, it creates one.
        /// </summary>
        /// <param name="sScene">The scene for which the shared instance is required.</param>
        /// <returns>The shared instance of the specified type for the given scene.</returns>
        public static K SharedInstance(Scene sScene)
        {
            K rReturn = null;
            //Debug.Log("SharedInstanceUnity<K> SharedInstance()");
            if (kSharedInstanceBySceneList.ContainsKey(sScene) == false)
            {
                Scene tActual = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(sScene);
                //Debug.Log("SharedInstanceUnity<K> Singleton() case kSharedInstance == null");
                // I need to create singleton
                GameObject tObjToSpawn;
                //spawn object
                tObjToSpawn = new GameObject(typeof(K).Name + " SharedInstance");
                //Add Components
                tObjToSpawn.AddComponent<K>();
                // keep k_Singleton
                rReturn = tObjToSpawn.GetComponent<K>();
                SceneManager.SetActiveScene(tActual);
            }
            else
            {
                rReturn = kSharedInstanceBySceneList[sScene];
                //Debug.Log("SharedInstanceUnity<K> Singleton() case kSharedInstance != null (exist in gameobject named '" + rReturn.gameObject.name + "')");
            }

            return rReturn;
        }

        /// <summary>
        /// Initializes the shared instance.
        /// This method is intended to be overridden in derived classes to provide
        /// custom initialization logic. It is called during the `Awake` phase of the Unity lifecycle.
        /// </summary>
        public override void InitInstance()
        {
            //Debug.Log("SharedInstanceUnity<K> InitInstance() for gameobject named '" + gameObject.name + "'");
            // do something by override
        }

        /// <summary>
        /// This method is called when the MonoBehaviour will be destroyed.
        /// It removes the instance from the dictionary that tracks shared instances by scene
        /// and resets the static instance if the current instance is the same.
        /// </summary>
        public void OnDestroy()
        {
            //Debug.Log("SharedInstanceUnity<K> OnDestroy() for gameobject named '" + gameObject.name + "'");
            Scene tScene = gameObject.scene;
            K tThis = this as K;
            if (kSharedInstanceBySceneList.ContainsKey(tScene) == true)
            {
                kSharedInstanceBySceneList.Remove(tScene);
            }

            if (kSharedInstance == tThis)
            {
                kSharedInstance = null;
            }
        }
    }
}