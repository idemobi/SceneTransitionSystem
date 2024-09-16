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
    /// Custom editor for the STSTransition component.
    /// </summary>
    [CustomEditor(typeof(STSTransition))]
    public class STSTransitionEditor : Editor
    {
        /// <summary>
        /// Serialized property representing the effect to be applied when transitioning into a scene.
        /// </summary>
        SerializedProperty SPEffectOnEnter;

        /// <summary>
        /// Serialized property representing the duration between entry and exit effects in a scene transition.
        /// </summary>
        SerializedProperty SPInterEffectDuration;

        /// <summary>
        /// SerializedProperty representing the visual effect to be applied
        /// when exiting a scene transition within the Scene Transition System.
        /// </summary>
        SerializedProperty SPEffectOnExit;

        /// <summary>
        /// Initializes the serialized properties when the editor is enabled.
        /// This method is called by Unity when the editor script is first loaded or reloaded.
        /// </summary>
        private void OnEnable()
        {
            SPEffectOnEnter = serializedObject.FindProperty("EffectOnEnter");
            SPInterEffectDuration = serializedObject.FindProperty("InterEffectDuration");
            SPEffectOnExit = serializedObject.FindProperty("EffectOnExit");
        }

        /// <summary>
        /// Custom inspector GUI for the STSTransition component.
        /// </summary>
        /// <remarks>
        /// This method overrides the default Inspector GUI for the STSTransition component
        /// and provides custom GUI elements for modifying its serialized properties.
        /// </remarks>
        /// <seealso cref="STSTransition"/>
        public override void OnInspectorGUI()
        {
            STSTransition tTarget = (STSTransition)target;
            serializedObject.Update();
            EditorGUILayout.PropertyField(SPEffectOnEnter);
            EditorGUILayout.PropertyField(SPEffectOnExit);
            EditorGUILayout.PropertyField(SPInterEffectDuration);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif