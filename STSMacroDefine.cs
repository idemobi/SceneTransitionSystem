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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [InitializeOnLoad]
    public class STSMacroDefine : IActiveBuildTargetChanged
    {
        //-------------------------------------------------------------------------------------------------------------
        const string kMacro = "SCENE_TRANSITION_SYSTEM";
        static STSMacroDefine kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        static STSMacroDefine()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new STSMacroDefine();
                kSharedInstance.OnChangedPlatform();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int callbackOrder
        {
            get
            {
                return 0;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            OnChangedPlatform();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnChangedPlatform()
        {
            InstallMacro(EditorUserBuildSettings.selectedBuildTargetGroup);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstallMacro(BuildTargetGroup sBuildTarget)
        {
            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget).Contains(kMacro) == false)
            {
                Debug.Log("Install macro " + kMacro + " in " + sBuildTarget + " player settings");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(sBuildTarget, PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget) + ";" + kMacro);
                if (PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget).Contains(kMacro) == false)
                {
                    Debug.LogError("Fail to install macro " + kMacro + " in " + sBuildTarget + " player settings!");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstallMacroAll()
        {
            Array BuildTargetGroupsArray = Enum.GetValues(typeof(BuildTargetGroup));
            foreach (BuildTargetGroup tBuildTarget in BuildTargetGroupsArray)
            {
                if (tBuildTarget != BuildTargetGroup.Unknown)
                {
                    InstallMacro(tBuildTarget);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif