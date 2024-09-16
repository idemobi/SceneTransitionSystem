using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Custom editor for the `STSAddressableAssets` class used within the Unity Editor.
    /// </summary>
    /// <remarks>
    /// This class customizes the Inspector GUI for `STSAddressableAssets` objects,
    /// allowing developers to directly interact with and modify relevant properties through the Unity Editor.
    /// </remarks>
    [CustomEditor(typeof(STSAddressableAssets))]
    public class STSAddressableAssetsEditor : Editor
    {
        /// <summary>
        /// Serialized property for the default effect to apply when entering a new scene.
        /// </summary>
        SerializedProperty SPDefaultEffectOnEnter;

        /// <summary>
        /// Serialized property representing the default transition effect applied when exiting a scene.
        /// </summary>
        SerializedProperty SPDefaultEffectOnExit;

        /// <summary>
        /// Serialized property representing the scene transition effect to be played when exiting a scene.
        /// </summary>
        SerializedProperty SPEffectOnExit;

        /// <summary>
        /// This method is called when the STSAddressableAssetsEditor is enabled.
        /// Initializes serialized properties used for representing the default
        /// effects on scene enter and exit in the editor.
        /// </summary>
        private void OnEnable()
        {
            SPDefaultEffectOnEnter = serializedObject.FindProperty("DefaultEffectOnEnter");
            SPDefaultEffectOnExit = serializedObject.FindProperty("DefaultEffectOnExit");
        }

        /// <summary>
        /// Custom Inspector GUI for the STSAddressableAssetsEditor.
        /// </summary>
        /// <remarks>
        /// Overrides the default inspector GUI to display serialized properties for the default
        /// transition effects on enter and exit within the Unity editor.
        /// </remarks>
        public override void OnInspectorGUI()
        {
            STSAddressableAssets tTarget = (STSAddressableAssets)target;
            serializedObject.Update();
            EditorGUILayout.PropertyField(SPDefaultEffectOnEnter);
            EditorGUILayout.PropertyField(SPDefaultEffectOnExit);
            serializedObject.ApplyModifiedProperties();
        }
    }
}


#endif