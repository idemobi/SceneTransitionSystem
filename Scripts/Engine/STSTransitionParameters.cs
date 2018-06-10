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
    public class STSTransitionParameters : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("On enter scene effect")]
        public STSEffectType EffectOnEnter;
        [Header("Between effects fade transition")]
        [Range(0.0F, 5.0F)]
        public float InterEffectDuration = 0.50F;
        [Header("On exit scene effect")]
        public STSEffectType EffectOnExit;
        //-------------------------------------------------------------------------------------------------------------
        [Header("On enter effect callback")]
        public STSTransitionEvent OnEnterStart;
        public STSTransitionEvent OnEnterFinish;
        [Header("On exit effect callback")]
        public STSTransitionEvent OnExitStart;
        public STSTransitionEvent OnExitFinish;
        [Header("This Scene state callback")]
        public STSTransitionEvent ThisSceneLoaded;
        public STSTransitionEvent ThisSceneEnable;
        public STSTransitionEvent ThisSceneDisable;
        public STSTransitionEvent ThisSceneWillUnloaded;
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Awake()
        {
            // test if Transition controller exist
            STSTransitionController.Singleton();
        }
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        void Update()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        void PlayExitAndEnterNow()
        {
            Debug.LogWarning("TODO => PlayExitAndEnterNow()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyIn(STSTransitionParameters sDestination)
        {
            sDestination.EffectOnEnter = this.EffectOnEnter.Dupplicate();
            sDestination.InterEffectDuration = this.InterEffectDuration;

            sDestination.EffectOnExit = this.EffectOnExit.Dupplicate();
            sDestination.OnEnterStart = this.OnEnterStart;
            sDestination.OnEnterFinish = this.OnEnterFinish;
            sDestination.OnExitStart = this.OnExitStart;
            sDestination.OnExitFinish = this.OnExitFinish;

            sDestination.ThisSceneLoaded = this.ThisSceneLoaded;
            sDestination.ThisSceneEnable = this.ThisSceneEnable;
            sDestination.ThisSceneDisable = this.ThisSceneDisable;
            sDestination.ThisSceneWillUnloaded = this.ThisSceneWillUnloaded;

        }
        //--------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================