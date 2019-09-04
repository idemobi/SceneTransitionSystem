//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomEditor(typeof(STSIntermission))]
    public class STSIntermissionEditor : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            STSIntermission tTarget = (STSIntermission)target;
            if (tTarget.gameObject.GetComponent<STSIntermissionInterface>() != null)
            {
                serializedObject.Update();
                DrawDefaultInspector();
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                EditorGUILayout.HelpBox("Need component with interface ISTSTransitionStandBy!", MessageType.Error);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif