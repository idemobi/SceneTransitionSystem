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
    public class STSSceneController : MonoBehaviour, STSTransitionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionSceneLoaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionEnterFinish(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionEnterStart(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionSceneEnable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionSceneDisable(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionExitStart(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionExitFinish(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnTransitionSceneWillUnloaded(STSTransitionData sData)
        {
            //throw new System.NotImplementedException();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================