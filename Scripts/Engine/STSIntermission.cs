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
    public class STSIntermission : SharedInstanceUnity<STSIntermission>
    {
        //-------------------------------------------------------------------------------------------------------------
        [Tooltip("Minimum stand by on transition scene in seconds")]
        public float StandBySeconds = 0.0f;
        [Tooltip("The next scene must be active automatically?")]
        public bool AutoActiveNextScene = true;
        //-------------------------------------------------------------------------------------------------------------
        public bool IsLoaded { get; internal set; } = false;
        public bool IsReadyToActivate { get; internal set; } = false;
        //-------------------------------------------------------------------------------------------------------------
        public void CopyFrom(STSIntermission sDestination)
        {
            sDestination.CopyIn(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyIn(STSIntermission sDestination)
        {
            sDestination.StandBySeconds = this.StandBySeconds;
            sDestination.AutoActiveNextScene = this.AutoActiveNextScene;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishStandByAction()
        {
            STSSceneManager.Singleton().FinishStandBy();
            STSAddressableAssets.Singleton().FinishStandBy();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================