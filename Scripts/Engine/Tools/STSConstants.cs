namespace SceneTransitionSystem
{
    /// <summary>
    /// Contains constants used throughout the Scene Transition System.
    /// </summary>
    public static class STSConstants
    {
        /// <summary>
        /// The name of the transition controller game object in the root scene.
        /// </summary>
        public static string K_TRANSITION_CONTROLLER_OBJECT_NAME = "STSControllerObject";

        /// <summary>
        /// The name for the default object transition.
        /// </summary>
        public static string K_TRANSITION_DEFAULT_OBJECT_NAME = "STSTransitionDefault";

        /// <summary>
        /// The name of the intermission transition game object in the root scene.
        /// </summary>
        public static string K_TRANSITION_Intermission_OBJECT_NAME = "STSIntermissionDefault";

        /// <summary>
        /// The scene name key used in the payload as a dictionary key.
        /// </summary>
        public static string K_SCENE_NAME_KEY = "SceneNameKey_872d7fe";

        /// <summary>
        /// The scene name key used in payload as dictionary key for intermission scenes.
        /// </summary>
        public static string K_Intermission_SCENE_NAME_KEY = "IntermissionSceneNameKey_88jk7fe";

        /// <summary>
        /// The load mode key used in payload as a dictionary key.
        /// </summary>
        public static string K_LOAD_MODE_KEY = "LoadModeKey_j7vhv8e";

        /// <summary>
        /// The payload data key used in payload as a dictionary key.
        /// </summary>
        public static string K_PAYLOAD_DATA_KEY = "PayloadDataKey_33h52fe";

        /// <summary>
        /// Editor information indicating no little preview is available.
        /// </summary>
        public static string K_NO_LITTLE_PREVIEW = "No little preview";

        /// <summary>
        /// Editor information constant indicating there is no big preview available.
        /// </summary>
        public static string K_NO_BIG_PREVIEW = "No big preview";

        /// <summary>
        /// Option to display a larger preview in the editor.
        /// </summary>
        public static string K_SHOW_BIG_PREVIEW = "Big preview";

        /// <summary>
        /// A constant string representing the command to run a large preview in the editor.
        /// </summary>
        public static string K_RUN_BIG_PREVIEW = "Run big preview";

        /// <summary>
        /// Button text prompting users to get more effects from the Unity Asset Store.
        /// </summary>
        public static string K_ASSET_STORE = "GET MORE EFFECTS";

        /// <summary>
        /// The URL of the asset store where STSEffect assets can be found.
        /// </summary>
        //public static string K_ASSET_STORE_URL = string.Empty;
        public static string K_ASSET_STORE_URL = "https://assetstore.unity.com/?q=STSEffect&orderBy=0";
    }
}