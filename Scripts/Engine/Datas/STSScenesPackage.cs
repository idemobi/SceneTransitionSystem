using System.Collections.Generic;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a package of scenes for the scene transition system.
    /// </summary>
    public class STSScenesPackage
    {
        /// <summary>
        /// Represents the name of the currently active scene within the scene transition system.
        /// </summary>
        public string ActiveSceneName;

        /// <summary>
        /// A list of scene names that are part of the scene transition package.
        /// </summary>
        public List<string> ScenesNameList = new List<string>();

        /// <summary>
        /// The IntermissionScene variable represents the name of the intermission scene
        /// that is used during scene transitions. This scene typically acts as a
        /// temporary scene or loading screen that is displayed while the active scene
        /// is being loaded or transitioned out.
        /// </summary>
        public string IntermissionScene;

        /// <summary>
        /// Represents transition data associated with the scene package.
        /// </summary>
        public STSTransitionData Datas;

        /// <summary>
        /// Represents a package containing scenes for the Scene Transition System (STS).
        /// </summary>
        public STSScenesPackage(string sActiveSceneName, List<string> sScenesNameList, string sIntermissionScene, STSTransitionData sDatas)
        {
            ActiveSceneName = sActiveSceneName;
            ScenesNameList = sScenesNameList;
            IntermissionScene = sIntermissionScene;
            Datas = sDatas;
        }
    }
}