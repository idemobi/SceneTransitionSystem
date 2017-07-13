using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace SceneTransitionSystem
{
	public class SceneTransitionSystemFindPackage : ScriptableObject
	{
		public string ScriptFilePath;
		public string ScriptFolder;
		public string ScriptFolderFromAssets;

		private static SceneTransitionSystemFindPackage kShareInstance;

		public static SceneTransitionSystemFindPackage ShareInstance ()
		{
			if (kShareInstance == null) {
				kShareInstance = ScriptableObject.CreateInstance ("SceneTransitionSystemFindPackage") as SceneTransitionSystemFindPackage;
				kShareInstance.ReadPath ();
			}
			return kShareInstance; 
		}

		public void ReadPath ()
		{
			MonoScript tMonoScript = MonoScript.FromScriptableObject (this);
			ScriptFilePath = AssetDatabase.GetAssetPath (tMonoScript);
			FileInfo tFileInfo = new FileInfo (ScriptFilePath);
			ScriptFolder = tFileInfo.Directory.ToString ();
			ScriptFolder = ScriptFolder.Replace ("\\", "/");
			ScriptFolderFromAssets = "Assets"+ScriptFolder.Replace (Application.dataPath, "");
		}

		public static string PackagePath (string sAddPath)
		{
			return ShareInstance ().ScriptFolderFromAssets + sAddPath;
		}
	}
}
#endif