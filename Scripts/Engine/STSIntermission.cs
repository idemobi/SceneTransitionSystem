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
    public class STSIntermission : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Intermission Scene Parameters")]
        [Tooltip("Minimum stand by on transition scene in seconds")]
        public float StandBySeconds = 0.0f;
        [Tooltip("The next scene must be active automatically?")]
        public bool AutoLoadNextScene = true;
        //-------------------------------------------------------------------------------------------------------------
        public STSIntermissionInterface Interfaced;
        //-------------------------------------------------------------------------------------------------------------
        public void CopyFrom(STSIntermission sDestination)
        {
            sDestination.CopyIn(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyIn(STSIntermission sDestination)
        {
            sDestination.StandBySeconds = this.StandBySeconds;
            sDestination.AutoLoadNextScene = this.AutoLoadNextScene;
            sDestination.Interfaced = this.Interfaced;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishStandByAction()
        {
            STSSceneManager.Singleton().FinishStandBy();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<STSIntermissionInterface>();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================