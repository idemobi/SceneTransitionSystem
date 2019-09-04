
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Transition scene system
    /// </summary>
    //-----------------------------------------------------------------------------------------------------------------
    public class STSScenesPackage
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ActiveSceneName;
        public List<string> ScenesNameList = new List<string>();
        public string IntermissionScene;
        public STSTransitionData Datas;
        public string Key;
        //-------------------------------------------------------------------------------------------------------------
        public STSScenesPackage(string sActiveSceneName, List<string> sScenesNameList, string sIntermissionScene, STSTransitionData sDatas,string sKey)
        {
            ActiveSceneName = sActiveSceneName;
            ScenesNameList = sScenesNameList;
            IntermissionScene = sIntermissionScene;
            Datas = sDatas;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
