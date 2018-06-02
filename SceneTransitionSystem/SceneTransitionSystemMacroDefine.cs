using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;

[InitializeOnLoad]
public class SceneTransitionSystemMacroDefine :  IActiveBuildTargetChanged
{
    //-------------------------------------------------------------------------------------------------------------
	const string kMacro = "SCENE_TRANSITION_SYSTEM";
    static SceneTransitionSystemMacroDefine kSharedInstance;
    //-------------------------------------------------------------------------------------------------------------
	static SceneTransitionSystemMacroDefine ()
	{
		if (kSharedInstance == null) {
			kSharedInstance = new SceneTransitionSystemMacroDefine ();
			kSharedInstance.OnChangedPlatform ();
		}
    }
    //-------------------------------------------------------------------------------------------------------------
    public int callbackOrder { get { return 0; } }
    //-------------------------------------------------------------------------------------------------------------
	public void OnActiveBuildTargetChanged (BuildTarget previousTarget, BuildTarget newTarget)
	{
		OnChangedPlatform ();
    }
    //-------------------------------------------------------------------------------------------------------------
	public  void OnChangedPlatform ()
	{
		InstallMacro (EditorUserBuildSettings.selectedBuildTargetGroup);
    }
    //-------------------------------------------------------------------------------------------------------------
	public void InstallMacro (BuildTargetGroup sBuildTarget)
	{
		if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) {
			Debug.Log ("Install macro " + kMacro + " in " + sBuildTarget + " player settings");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (sBuildTarget, PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget) + ";" + kMacro);
			if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) {
				Debug.LogError ("Fail to install macro " + kMacro + " in " + sBuildTarget + " player settings!");
			}
		}
    }
    //-------------------------------------------------------------------------------------------------------------
	public void InstallMacroAll ()
	{
		Array BuildTargetGroupsArray = Enum.GetValues (typeof(BuildTargetGroup));
		foreach (BuildTargetGroup tBuildTarget in BuildTargetGroupsArray) {
			if (tBuildTarget != BuildTargetGroup.Unknown) {
				InstallMacro (tBuildTarget);
			}
		}
    }
    //-------------------------------------------------------------------------------------------------------------
}
#endif
