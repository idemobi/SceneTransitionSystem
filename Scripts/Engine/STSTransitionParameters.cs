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
    public interface ISTSTransitionParameters
    {
        void OnTransitionEnterStart(STSTransitionData sData);
        void OnTransitionEnterFinish(STSTransitionData sData);
        void OnTransitionExitStart(STSTransitionData sData);
        void OnTransitionExitFinish(STSTransitionData sData);
        void OnTransitionSceneLoaded(STSTransitionData sData);
        void OnTransitionSceneEnable(STSTransitionData sData);
        void OnTransitionSceneDisable(STSTransitionData sData);
        void OnTransitionSceneWillUnloaded(STSTransitionData sData);
    }
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
        public STSScreenGauge SceneLoadingGauge;
        //-------------------------------------------------------------------------------------------------------------
        //[Header("Interfaced")]
        public ISTSTransitionParameters Interfaced;
        //[Header("On enter effect callback")]
        //public STSTransitionEvent OnEnterStart;
        //public STSTransitionEvent OnEnterFinish;
        //[Header("On exit effect callback")]
        //public STSTransitionEvent OnExitStart;
        //public STSTransitionEvent OnExitFinish;
        //[Header("This Scene state callback")]
        //public STSTransitionEvent ThisSceneLoaded;
        //public STSTransitionEvent ThisSceneEnable;
        //public STSTransitionEvent ThisSceneDisable;
        //public STSTransitionEvent ThisSceneWillUnloaded;
        //-------------------------------------------------------------------------------------------------------------
        private STSEffect EffectOnEnterDup;
        private STSEffect EffectOnExitDup;
        private bool ExitInProgress = false;
        private bool EnterInProgress = false;
        private bool ExitAndEnterInProgress = false;
        private bool PlayInProgress = false;
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            // test if Transition controller exist
            STSTransitionController.Singleton();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<ISTSTransitionParameters>();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Update()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnGUI()
        {
            if (PlayInProgress == true)
            {
                if (ExitInProgress == true)
                {
                    if (EffectOnExitDup.AnimIsFinished == false)
                    {
                        EffectOnExitDup.DrawMaster(new Rect(0, Screen.height, Screen.width, -Screen.height));
                    }
                    else
                    {
                        ExitInProgress = false;
                        //if (OnExitFinish != null)
                        //{
                        //    OnExitFinish.Invoke(null);
                        //}
                        if (Interfaced != null)
                        {
                            Interfaced.OnTransitionExitFinish(null);
                        }
                        if (ExitAndEnterInProgress == true)
                        {
                            PlayEnterNow();
                        }
                        else
                        {
                            PlayInProgress = false;
                        }
                    }
                }
                if (EnterInProgress == true)
                {
                    if (EffectOnEnterDup.AnimIsFinished == false)
                    {
                        EffectOnEnterDup.DrawMaster(new Rect(0, Screen.height, Screen.width, -Screen.height));
                    }
                    else
                    {
                        EnterInProgress = false;
                        ExitAndEnterInProgress = false; // anyway
                        //if (OnEnterFinish != null)
                        //{
                        //    OnEnterFinish.Invoke(null);
                        //}
                        if (Interfaced != null)
                        {
                            Interfaced.OnTransitionEnterFinish(null);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PlayExitNow()
        {
            //if (OnExitStart != null)
            //{
            //    OnExitStart.Invoke(null);
            //}
            if (Interfaced != null)
            {
                Interfaced.OnTransitionExitStart(null);
            }
            ExitInProgress = true;
            PlayInProgress = true;
            EffectOnExitDup = EffectOnExit.GetEffect();
            EffectOnExitDup.StartEffectExit(new Rect(0, Screen.height, Screen.width, -Screen.height));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PlayEnterNow()
        {
            //if (OnxEnterStart != null)
            //{
            //    OnEnterStart.Invoke(null);
            //}
            if (Interfaced != null)
            {
                Interfaced.OnTransitionEnterStart(null);
            }
            EnterInProgress = true;
            PlayInProgress = true;
            EffectOnExitDup = EffectOnExit.GetEffect();
            EffectOnEnterDup = EffectOnEnter.GetEffect();
            EffectOnEnterDup.StartEffectEnter(new Rect(0, Screen.height, Screen.width, -Screen.height), EffectOnExitDup.TintPrimary, InterEffectDuration);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PlayExitAndEnterNow()
        {
            ExitAndEnterInProgress = true;
            PlayExitNow();
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool PreventUserInteractions = true;
        //-------------------------------------------------------------------------------------------------------------
        private void EventSystemPrevent(bool sEnable)
        {
            //Debug.Log("STSTransitionController EventSystemPrevent()");
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene tScene = SceneManager.GetSceneAt(i);
                if (tScene.isLoaded)
                {
                    EventSystemEnable(tScene, false);
                }
            }
            EventSystemEnable(SceneManager.GetActiveScene(), sEnable);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void EventSystemEnable(Scene sScene, bool sEnable)
        {
            //Debug.Log("STSTransitionController EventSystemEnable()");
            if (PreventUserInteractions == true)
            {
                EventSystem tEventSystem = null;
                GameObject[] tAllRootObjects = sScene.GetRootGameObjects();
                foreach (GameObject tObject in tAllRootObjects)
                {
                    if (tObject.GetComponent<EventSystem>() != null)
                    {
                        tEventSystem = tObject.GetComponent<EventSystem>();
                    }
                }
                if (tEventSystem != null)
                {
                    tEventSystem.enabled = sEnable;
                }
                else
                {
                    //Debug.Log ("No <EventSystem> type component found in the root Objects. Becarefull!");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CopyIn(STSTransitionParameters sDestination)
        {
            sDestination.EffectOnEnter = this.EffectOnEnter.Dupplicate();
            sDestination.InterEffectDuration = this.InterEffectDuration;
            sDestination.EffectOnExit = this.EffectOnExit.Dupplicate();

            sDestination.Interfaced = this.Interfaced;
            sDestination.SceneLoadingGauge = this.SceneLoadingGauge;

            //sDestination.OnEnterStart = this.OnEnterStart;
            //sDestination.OnEnterFinish = this.OnEnterFinish;
            //sDestination.OnExitStart = this.OnExitStart;
            //sDestination.OnExitFinish = this.OnExitFinish;

            //sDestination.ThisSceneLoaded = this.ThisSceneLoaded;
            //sDestination.ThisSceneEnable = this.ThisSceneEnable;
            //sDestination.ThisSceneDisable = this.ThisSceneDisable;
            //sDestination.ThisSceneWillUnloaded = this.ThisSceneWillUnloaded;

        }
        //--------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================