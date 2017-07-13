using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
public class SceneTransitionSystemMacroDefine
{
	const string kMacro = "SCENE_TRANSITION_SYSTEM";
	static SceneTransitionSystemMacroDefine ()
	{
		OnChangedPlatform ();
		EditorUserBuildSettings.activeBuildTargetChanged += OnChangedPlatform;
	}
	static void OnChangedPlatform() {
		InstallMacro (EditorUserBuildSettings.selectedBuildTargetGroup);
	}
	static void InstallMacro (BuildTargetGroup sBuildTarget)
	{
		if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) 
		{
			Debug.Log ("Install macro " + kMacro + " in " + sBuildTarget + " player settings");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (sBuildTarget, PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget) + ";" + kMacro);
			if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) 
			{
				Debug.LogError ("Fail to install macro " + kMacro + " in " + sBuildTarget + " player settings!");
			}
		}
	}
	static void InstallMacroAll ()
	{
		Array BuildTargetGroupsArray = Enum.GetValues (typeof(BuildTargetGroup));
		foreach (BuildTargetGroup tBuildTarget in BuildTargetGroupsArray) {
			if (tBuildTarget != BuildTargetGroup.Unknown) {
				InstallMacro (tBuildTarget);
			}
		}
	}
}
#endif
