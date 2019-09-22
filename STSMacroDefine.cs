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

using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[InitializeOnLoad]
	public class STSMacroDefine :  IActiveBuildTargetChanged
	{
		//-------------------------------------------------------------------------------------------------------------
		static STSMacroDefine kSharedInstance = new STSMacroDefine("SCENE_TRANSITION_SYSTEM", false);
        //-------------------------------------------------------------------------------------------------------------
        string Macro;
        bool Install;
		//-------------------------------------------------------------------------------------------------------------
		public STSMacroDefine (string sMacro, bool sInstall)
		{
            Macro = sMacro;
            Install = sInstall;
            OnChangedPlatform();
		}
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		public void OnActiveBuildTargetChanged (BuildTarget previousTarget, BuildTarget newTarget)
		{
			OnChangedPlatform ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnChangedPlatform ()
		{
			InstallMacro (EditorUserBuildSettings.selectedBuildTargetGroup);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void InstallMacro (BuildTargetGroup sBuildTarget)
		{
            List<string> tMacroList = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget).Split(new char[] { ';' }));
            if (Install == true)
            {
               if (tMacroList.Contains(Macro)==false)
                {
                    tMacroList.Add(Macro);
                }
            }
            else
            {
                if (tMacroList.Contains(Macro)==true)
                {
                    tMacroList.Remove(Macro);
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(sBuildTarget,string.Join(";",tMacroList));
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif