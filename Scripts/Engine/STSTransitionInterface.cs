//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using UnityEngine;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public interface STSTransitionInterface
    {
        void OnTransitionSceneLoaded(STSTransitionData sData);
        void OnTransitionEnterStart(STSTransitionData sData);
        void OnTransitionEnterFinish(STSTransitionData sData);
        void OnTransitionSceneEnable(STSTransitionData sData);
        void OnTransitionSceneDisable(STSTransitionData sData);
        void OnTransitionExitStart(STSTransitionData sData);
        void OnTransitionExitFinish(STSTransitionData sData);
        void OnTransitionSceneWillUnloaded(STSTransitionData sData);
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================