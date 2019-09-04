//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSSceneIntermissionController : STSSceneController, STSIntermissionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnStandByFinish(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnStandByStart(STSIntermission sStandBy)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================