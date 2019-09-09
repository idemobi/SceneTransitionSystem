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
using UnityEngine;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public interface STSTransitionInterface
    {
        void OnTransitionSceneLoaded(STSTransitionData sData);
        void OnTransitionEnterStart(STSTransitionData sData, STSEffectType sEffect, float sInterludeDuration);
        void OnTransitionEnterFinish(STSTransitionData sData);
        void OnTransitionSceneEnable(STSTransitionData sData);
        void OnTransitionSceneDisable(STSTransitionData sData);
        void OnTransitionExitStart(STSTransitionData sData, STSEffectType sEffect);
        void OnTransitionExitFinish(STSTransitionData sData);
        void OnTransitionSceneWillUnloaded(STSTransitionData sData);
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================