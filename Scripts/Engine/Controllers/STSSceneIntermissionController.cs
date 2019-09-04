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
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSSceneIntermissionController : STSSceneController, STSIntermissionInterface
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Gauge")]
        public STSScreenGauge Gauge;
        //-------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(true);
                Gauge.HorizontalValue = 0.0F;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnSceneAllReadyLoaded(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnSceneAllReadyLoaded() " + sSceneName + "(" + SceneNumber + ") Total loading " + sPercent.ToString("P"));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnLoadingSceneStart(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnLoadingSceneStart() " + sSceneName + "(" + SceneNumber + " loading " + sScenePercent.ToString("P") + ") Total loading " + sPercent.ToString("P"));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnLoadingScenePercent(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnLoadingScenePercent() " + sSceneName + "(" + SceneNumber + " loading " + sScenePercent.ToString("P") + ") Total loading " + sPercent.ToString("P"));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnLoadingSceneFinish(STSTransitionData sData, string sSceneName, int SceneNumber, float sScenePercent, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnLoadingSceneFinish() " + sSceneName + "(" + SceneNumber + " loading " + sScenePercent.ToString("P") + ") Total loading " + sPercent.ToString("P"));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnUnloadScene(STSTransitionData sData, string sSceneName, int SceneNumber, float sPercent)
        {
            if (Gauge != null)
            {
                Gauge.SetHidden(false);
                Gauge.SetHorizontalValue(sPercent);
            }
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnUnloadScene() " + sSceneName + "(" + SceneNumber + ") Total loading " + sPercent.ToString("P"));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnStandByFinish(STSIntermission sStandBy)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnStandByFinish() ");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnStandByStart(STSIntermission sStandBy)
        {
            if (ActiveLog == true)
            {
                Debug.Log("<color=" + LogTagColor.ToString() + ">" + this.gameObject.scene.name + "</color> OnStandByStart() ");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================