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
    public interface ISTSTransitionStandBy
    {
        void OnLoadNextSceneStart(STSTransitionData sData, float sPercent);
        void OnLoadingNextScenePercent(STSTransitionData sData, float sPercent);
        void OnLoadNextSceneFinish(STSTransitionData sData, float sPercent);

        void OnStandByStart(STSTransitionStandBy sStandBy);
        void OnStandByFinish(STSTransitionStandBy sStandBy);
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// STS Transition standby script. Use during the wating to load in intermediary scene.
    /// </summary>
    public class STSTransitionStandBy : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Intermediate Scene Parameters")]
        [Tooltip("Minimum stand by on transition scene in seconds")]
        public float StandBySeconds = 0.0f;
        [Tooltip("The next scene must be active automatically?")]
        public bool AutoLoadNextScene = true;
        [Tooltip("The gauge to use in canvas")]
        public STSScreenGauge SceneLoadingGauge;
        //[Header("Interfaced")]
        public ISTSTransitionStandBy Interfaced;

        //[Header("Next scene loading progress callbacks")]
        //public STSTransitionLoading LoadNextSceneStart;
        //public STSTransitionLoading LoadingNextScenePercent;
        //public STSTransitionLoading LoadNextSceneFinish;
        //[Header("Stand by callbacks")]
        //public STSStandByEvent StandByStart;
        //public STSStandByEvent StandByFinish;
        //-------------------------------------------------------------------------------------------------------------
        public void CopyFrom(STSTransitionStandBy sDestination)
        {
            sDestination.CopyIn(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyIn(STSTransitionStandBy sDestination)
        {
            sDestination.StandBySeconds = this.StandBySeconds;
            sDestination.AutoLoadNextScene = this.AutoLoadNextScene;
            sDestination.SceneLoadingGauge = this.SceneLoadingGauge;
            sDestination.Interfaced = this.Interfaced;
            //sDestination.LoadNextSceneStart = this.LoadNextSceneStart;
            //sDestination.LoadingNextScenePercent = this.LoadingNextScenePercent;
            //sDestination.LoadNextSceneFinish = this.LoadNextSceneFinish;
            //sDestination.StandByStart = this.StandByStart;
            //sDestination.StandByFinish = this.StandByFinish;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishStandByAction()
        {
            STSTransitionController.Singleton().FinishStandBy();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<ISTSTransitionStandBy>();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================