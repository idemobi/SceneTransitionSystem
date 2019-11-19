//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	SceneTransitionSystem for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public interface STSTransitionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        void OnTransitionSceneLoaded(STSTransitionData sData);
        void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration, bool sActiveScene);
        void OnTransitionEnterFinish(STSTransitionData sData, bool sActiveScene);
        void OnTransitionSceneEnable(STSTransitionData sData);
        void OnTransitionSceneDisable(STSTransitionData sData);
        void OnTransitionExitStart(STSTransitionData sData, STSEffectType sEffect, bool sActiveScene);
        void OnTransitionExitFinish(STSTransitionData sData, bool sActiveScene);
        void OnTransitionSceneWillUnloaded(STSTransitionData sData);
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================