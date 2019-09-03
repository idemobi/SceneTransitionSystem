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
    public interface STSIntermediateInterface
    {
        void OnLoadNextSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent);
        void OnLoadingNextScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent);
        void OnLoadNextSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent);
        void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent);

        void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent);

        void OnStandByStart(STSIntermediate sStandBy);
        void OnStandByFinish(STSIntermediate sStandBy);
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSIntermediate : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Intermediate Scene Parameters")]
        [Tooltip("Minimum stand by on transition scene in seconds")]
        public float StandBySeconds = 0.0f;
        [Tooltip("The next scene must be active automatically?")]
        public bool AutoLoadNextScene = true;
        //-------------------------------------------------------------------------------------------------------------
        public STSIntermediateInterface Interfaced;
        //-------------------------------------------------------------------------------------------------------------
        public void CopyFrom(STSIntermediate sDestination)
        {
            sDestination.CopyIn(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyIn(STSIntermediate sDestination)
        {
            sDestination.StandBySeconds = this.StandBySeconds;
            sDestination.AutoLoadNextScene = this.AutoLoadNextScene;
            sDestination.Interfaced = this.Interfaced;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishStandByAction()
        {
            STSController.Singleton().FinishStandBy();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<STSIntermediateInterface>();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================