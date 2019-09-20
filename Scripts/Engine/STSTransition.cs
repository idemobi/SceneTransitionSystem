﻿//=====================================================================================================================
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
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            STSSceneManager.Singleton();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Interfaced = GetComponent<STSTransitionInterface>();
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