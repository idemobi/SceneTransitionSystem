using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SceneTransitionSystem;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class SceneIntermediateController : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public string m_ThisSceneTitle = "Scene …";
        public string m_NextSceneName = "";
        public Text m_TitleLabel;
        public Text m_SubTitleLabel;
        public Text m_PercentLabel;
        public Button NextButton;
        public STSScreenGauge LoadingGauge;
        //-------------------------------------------------------------------------------------------------------------
        //// Use this for initialization
        void Awake ()
        {
            if (NextButton != null)
            {
                NextButton.gameObject.SetActive(false);
            }
            if (LoadingGauge != null)
            {
                LoadingGauge.gameObject.SetActive(false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start ()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        void Update ()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GoToNextScene()
        {
            Debug.Log("GoToNextScene");
            STSTransitionController.Singleton().FinishStandBy();
        }
        // Public methods for the Actions callback for Transition Scene
        //-------------------------------------------------------------------------------------------------------------
        public void LoadIntermediateScene(STSTransitionData sTransitionDataScript)
        {
            Debug.Log(m_ThisSceneTitle + "GOOD LoadNextSceneStart with data named ' " + sTransitionDataScript.InternalName + "'");
            m_TitleLabel.text = sTransitionDataScript.Title;
            m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
            m_PercentLabel.text = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadNextSceneStart(STSTransitionData sTransitionDataScript, float sPercent)
        {
            Debug.Log(m_ThisSceneTitle + "GOOD LoadNextSceneStart with data named ' " + sTransitionDataScript.InternalName + "'");
            m_TitleLabel.text = sTransitionDataScript.Title;
            m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
            m_PercentLabel.text = "" + sPercent.ToString("P") + "%";

            if (LoadingGauge != null)
            {
                LoadingGauge.gameObject.SetActive(true);
                LoadingGauge.HorizontalValue=0.0F;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadingNextScenePercent(STSTransitionData sTransitionDataScript, float sPercent)
        {
            Debug.Log(m_ThisSceneTitle + "GOOD LoadingNextScenePercent with data named ' " + sTransitionDataScript.InternalName + "' " + sPercent.ToString("P") + " %");
            m_TitleLabel.text = sTransitionDataScript.Title;
            m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
            m_PercentLabel.text = "" + sPercent.ToString("P") + "%";
            if (LoadingGauge != null)
            {
                LoadingGauge.HorizontalValue=sPercent;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadNextSceneFinish(STSTransitionData sTransitionDataScript, float sPercent)
        {
            Debug.Log(m_ThisSceneTitle + "GOOD LoadNextSceneFinish with data named ' " + sTransitionDataScript.InternalName + "'");
            m_TitleLabel.text = sTransitionDataScript.Title;
            m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
            m_PercentLabel.text = "" + sPercent.ToString("P") + "%";
            if (LoadingGauge != null)
            {
                LoadingGauge.HorizontalValue=1.0F;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StandByIsStarted(STSTransitionStandBy sTransitionStandBy)
        {
            Debug.Log("StandByIsStarted");
            if (sTransitionStandBy.AutoLoadNextScene == false)
            {
                if (NextButton != null)
                {
                    NextButton.gameObject.SetActive(true);
                }
            }
            //if (LoadingGauge != null)
            //{
            //    LoadingGauge.gameObject.SetActive(false);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StandByIsFinished(STSTransitionStandBy sTransitionStandBy)
        {
            Debug.Log("StandByIsFinished");
            if (sTransitionStandBy.AutoLoadNextScene == false)
            {
                if (NextButton != null)
                {
                    NextButton.gameObject.SetActive(true);
                }
            }
            if (LoadingGauge != null)
            {
                //LoadingGauge.IsVisible = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
