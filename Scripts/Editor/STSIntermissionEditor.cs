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

namespace SceneTransitionSystem
{
    /// <summary>
    /// Custom editor for the STSIntermission class, used to manage the display
    /// and interaction of STSIntermission properties within the Unity Editor.
    /// </summary>
    [CustomEditor(typeof(STSIntermission))]
    public class STSIntermissionEditor : Editor
    {
        /// <summary>
        /// Serialized property representing the minimum stand by duration in seconds for a transition scene.
        /// </summary>
        /// <remarks>
        /// This property is used in the Unity Editor to set and display the minimum duration the system should wait
        /// on the transition scene before proceeding. It directly corresponds to the <see cref="STSIntermission.StandBySeconds"/> field.
        /// </remarks>
        SerializedProperty SPStandBySeconds;

        /// <summary>
        /// Serialized property representing the next scene loading behavior during intermission.
        /// This property determines whether the next scene should be activated automatically once it is loaded.
        /// </summary>
        SerializedProperty SPActiveLoadNextScene;

        /// <summary>
        /// Initializes serialized properties for the custom editor when the editor is enabled.
        /// </summary>
        /// <remarks>
        /// This method is called automatically by the Unity Editor when the custom editor is created.
        /// It locates and assigns the appropriate serialized properties from the target object to be used in the custom inspector.
        /// The properties initialized are:
        /// - SPStandBySeconds: Represents the standby duration in seconds.
        /// - SPActiveLoadNextScene: Determines if the next scene should be loaded automatically.
        /// </remarks>
        private void OnEnable()
        {
            SPStandBySeconds = serializedObject.FindProperty("StandBySeconds");
            SPActiveLoadNextScene = serializedObject.FindProperty("AutoActiveNextScene");
        }

        /// <summary>
        /// Overrides the default inspector GUI for the STSIntermission component.
        /// This method updates the serialized object representation and displays
        /// custom fields in the inspector for user-defined properties.
        /// </summary>
        public override void OnInspectorGUI()
        {
            STSIntermission tTarget = (STSIntermission)target;
            serializedObject.Update();
            EditorGUILayout.PropertyField(SPStandBySeconds);
            EditorGUILayout.PropertyField(SPActiveLoadNextScene);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif