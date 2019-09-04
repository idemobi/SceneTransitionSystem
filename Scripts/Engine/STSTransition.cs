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
    public class STSTransition : MonoBehaviour
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
        public STSTransitionInterface Interfaced;
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
            STSSceneManager.Singleton();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<STSTransitionInterface>();
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
                        ExitAndEnterInProgress = false;
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
        public void CopyIn(STSTransition sDestination)
        {
            sDestination.EffectOnEnter = this.EffectOnEnter.Dupplicate();
            sDestination.InterEffectDuration = this.InterEffectDuration;
            sDestination.EffectOnExit = this.EffectOnExit.Dupplicate();
            sDestination.Interfaced = this.Interfaced;
        }
        //--------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================