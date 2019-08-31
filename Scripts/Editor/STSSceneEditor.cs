﻿//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomPropertyDrawer(typeof(STSScene))]
    public class STSSceneEditor : PropertyDrawer
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty tPath = property.FindPropertyRelative("ScenePath");
            List<string> tPathList = new List<string>();
            foreach(EditorBuildSettingsScene tScene in EditorBuildSettings.scenes)
            {
                tPathList.Add(tScene.path);
            }
            int tPathIndex = tPathList.IndexOf(tPath.stringValue);
            int tPathIndexNext = EditorGUI.Popup(position, property.displayName, tPathIndex, tPathList.ToArray());
            if (tPathIndexNext >= 0 && tPathIndex != tPathIndexNext)
            {
                tPath.stringValue = EditorBuildSettings.scenes[tPathIndexNext].path;
            }
            EditorGUI.EndProperty();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif