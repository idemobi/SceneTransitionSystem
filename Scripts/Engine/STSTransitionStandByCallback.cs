﻿//=====================================================================================================================
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
    /// <summary>
    /// STS Transition standby script. Use during the waiting to load in intermediary scene.
    /// </summary>
    public class STSTransitionStandByCallback : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public ISTSTransitionStandBy Interfaced;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<ISTSTransitionStandBy>();
            if (Interfaced != null)
            {
                STSTransitionController.Singleton().AddStandByCallBack(Interfaced);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            if (Interfaced != null)
            {
                STSTransitionController.Singleton().RemoveStandByCallBack(Interfaced);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================