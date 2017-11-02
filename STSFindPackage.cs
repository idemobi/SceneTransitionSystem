//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace SceneTransitionSystem
{
	public class STSFindPackage : ScriptableObject
	{
		public string ScriptFilePath;
		public string ScriptFolder;
		public string ScriptFolderFromAssets;

		private static STSFindPackage kShareInstance;

		public static STSFindPackage ShareInstance ()
		{
			if (kShareInstance == null) {
				kShareInstance = ScriptableObject.CreateInstance ("STSFindPackage") as STSFindPackage;
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