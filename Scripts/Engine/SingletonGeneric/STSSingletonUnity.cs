using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Defines the root type of the Singleton in the Scene Transition System.
    /// </summary>
    public enum STSSingletonRoot
    {
        /// <summary>
        /// Represents a root type for the STSSingleton that is associated with a GameObject.
        /// </summary>
        GameObject,

        Component,
    }

    /// <summary>
    /// The STSSingletonBasis class serves as a base class for creating singleton behaviors
    /// within the SceneTransitionSystem namespace. It provides methods to manage lifecycle
    /// events and ensure only one instance exists across scenes.
    /// </summary>
    public class STSSingletonBasis : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether the singleton instance has been initialized.
        /// Set to true after the instance is initialized to ensure initialization
        /// occurs only once.
        /// </summary>
        public bool Initialized = false;

        /// <summary>
        /// Determines the root type to be destroyed for the singleton instance.
        /// </summary>
        /// <returns>
        /// A value of type <see cref="STSSingletonRoot"/> indicating whether the GameObject or the Component should be destroyed.
        /// </returns>
        public virtual STSSingletonRoot DestroyRoot()
        {
            return STSSingletonRoot.Component;
        }

        /// <summary>
        /// Initializes the instance of the singleton. This method is typically overridden to include
        /// any specific initialization logic required for the singleton instance.
        /// </summary>
        public virtual void InitInstance()
        {
        }

        /// <summary>
        /// Called when a new scene is loaded. This method can be overridden to implement custom behavior
        /// that should occur whenever a scene is loaded.
        /// </summary>
        /// <param name="sScene">The scene that was loaded.</param>
        /// <param name="sMode">The mode in which the scene was loaded.</param>
        public virtual void OnSceneLoaded(Scene sScene, LoadSceneMode sMode)
        {
        }

        /// <summary>
        /// Handles the actions needed to be performed when a scene is unloaded.
        /// </summary>
        /// <param name="sScene">The scene that has been unloaded.</param>
        public virtual void OnSceneUnLoaded(Scene sScene)
        {
        }
    }

    /// <summary>
    /// A generic singleton class for Unity that derives from STSSingletonBasis.
    /// </summary>
    /// <typeparam name="K">
    /// Specifies the type of the singleton, which must inherit from STSSingletonBasis and have a parameterless constructor.
    /// </typeparam>
    public class STSSingletonUnity<K> : STSSingletonBasis where K : STSSingletonBasis, new()
    {
        /// <summary>
        /// A private static instance of the singleton class, ensuring only one instance of the class exists
        /// throughout the application runtime.
        /// </summary>
        private static K kSingleton = null;

        /// <summary>
        /// Determines the root object to destroy when enforcing the singleton pattern.
        /// </summary>
        /// <returns>
        /// Returns STSSingletonRoot.Component by default, indicating that only the component should be destroyed.
        /// </returns>
        public override STSSingletonRoot DestroyRoot()
        {
            return STSSingletonRoot.Component;
        }

        /// <summary>
        /// Ensures that the singleton instance of the component is initialized and persists across scenes.
        /// If no instance exists, it sets the current instance as the singleton and initializes it.
        /// If an instance already exists and it is not the current instance, it destroys the current component
        /// or its game object based on the destruction policy defined by <see cref="DestroyRoot"/>.
        /// </summary>
        private void Awake()
        {
            //Debug.Log("STSSingleton<K> Awake() for gameobject named '" + gameObject.name + "'");
            //Check if there is already an instance of K
            if (kSingleton == null)
            {
                //Debug.Log("STSSingleton<K> Awake() case kSingleton == null for gameobject named '" + gameObject.name + "'");
                //if not, set it to this.
                kSingleton = this as K;
                if (Initialized == false)
                {
                    //Debug.Log("STSSingleton<K> Awake() case kSingleton.Initialized == false for gameobject named '" + gameObject.name + "'");
                    // Init Instance
                    InitInstance();
                    // scene is use on laded new scene
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    // first install in first scene
                    OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
                    // memorize the init instance
                    Initialized = true;
                }

                //Set K's gameobject to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
                DontDestroyOnLoad(gameObject);
            }

            //If instance already exists:
            if (kSingleton != this)
            {
                //Debug.Log("STSSingleton<K> Awake() case kSingleton != this for gameobject named '" + gameObject.name + "'");
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                //Debug.Log("singleton prevent destruction gameobject named '" + gameObject.name + "'");
                if (DestroyRoot() == STSSingletonRoot.Component)
                {
                    Destroy(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// Determines if a singleton instance of the class exists.
        /// </summary>
        /// <returns>
        /// A boolean value indicating whether the singleton instance is present.
        /// </returns>
        public static bool SingletonExists()
        {
            bool rReturn = kSingleton != null;
            return rReturn;
        }

        /// <summary>
        /// Ensures that only one instance of the specified class exists throughout the application's lifecycle.
        /// </summary>
        /// <typeparam name="K">
        /// The type of class for which the singleton instance will be created. Must inherit from STSSingletonBasis and have a parameterless constructor.
        /// </typeparam>
        /// <returns>
        /// The singleton instance of the specified class.
        /// </returns>
        public static K Singleton()
        {
            //Debug.Log("STSSingleton<K> Singleton()");
            if (kSingleton == null)
            {
                //Debug.Log("STSSingleton<K> Singleton() case kSingleton == null");
                // I need to create singleton
                GameObject tObjToSpawn;
                //spawn object
                tObjToSpawn = new GameObject(typeof(K).Name + " Singleton");
                //Add Components
                tObjToSpawn.AddComponent<K>();
                // keep k_Singleton
                kSingleton = tObjToSpawn.GetComponent<K>();
            }
            else
            {
                //Debug.Log("STSSingleton<K> Singleton() case kSingleton != null (exist in gameobject named '" + kSingleton.gameObject.name + "')");
            }

            return kSingleton;
        }

        /// <summary>
        /// Initializes the instance of the singleton.
        /// This method should be overridden in derived classes to implement specific initialization logic.
        /// Ensures that the singleton instance is properly set up and ready for use.
        /// </summary>
        public override void InitInstance()
        {
            //Debug.Log("STSSingleton<K> InitInstance() for gameobject named '" + gameObject.name + "'");
            // do something by override
        }

        /// <summary>
        /// Handles actions to perform when a new scene has been loaded in the SceneManager.
        /// </summary>
        /// <param name="sScene">The scene that has been loaded.</param>
        /// <param name="sMode">The mode in which the scene was loaded.</param>
        public override void OnSceneLoaded(Scene sScene, LoadSceneMode sMode)
        {
            //Debug.Log("STSSingleton<K> OnSceneLoaded() for gameobject named '" + gameObject.name + "'");
            // do something by override
        }

        /// <summary>
        /// Called when a scene has been unloaded.
        /// </summary>
        /// <param name="sScene">The scene that was unloaded.</param>
        public override void OnSceneUnLoaded(Scene sScene)
        {
            //Debug.Log("STSSingleton<K> OnSceneUnLoaded() for gameobject named '" + gameObject.name + "'");
            // do something by override
        }

        /// <summary>
        /// Cleans up the singleton instance upon the destruction of the GameObject.
        /// If the current instance is the singleton instance, it resets the singleton reference to null.
        /// </summary>
        public void OnDestroy()
        {
            //Debug.Log("STSSingleton<K> OnDestroy() for gameobject named '" + gameObject.name + "'");
            if (kSingleton == this)
            {
                kSingleton = null;
            }
        }
    }
}