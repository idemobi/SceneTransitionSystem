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
    /// Custom editor for the STSSceneManager which allows customization of
    /// default effects for scene transitions within the Unity Editor.
    /// </summary>
    [CustomEditor(typeof(STSSceneManager))]
    public class STSSceneManagerEditor : Editor
    {
        /// <summary>
        /// Serialized property representing the default effect to play when entering a new scene
        /// in the Scene Transition System.
        /// </summary>
        SerializedProperty SPDefaultEffectOnEnter;

        /// <summary>
        /// Serialized property representing the default effect to be triggered on scene exit transitions.
        /// This property can be configured in the Unity Editor to specify the transition effect that should be used
        /// when the user exits a scene.
        /// </summary>
        SerializedProperty SPDefaultEffectOnExit;

        /// <summary>
        /// Serialized property for the effect applied when a scene transition exits.
        /// </summary>
        SerializedProperty SPEffectOnExit;

        /// <summary>
        /// Unity method called when the editor becomes enabled and active.
        /// Initializes serialized properties used in the custom editor GUI for
        /// <see cref="STSSceneManager"/>.
        /// </summary>
        private void OnEnable()
        {
            SPDefaultEffectOnEnter = serializedObject.FindProperty("DefaultEffectOnEnter");
            SPDefaultEffectOnExit = serializedObject.FindProperty("DefaultEffectOnExit");
        }

        /// <summary>
        /// Custom inspector GUI logic for the STSSceneManager editor.
        /// </summary>
        public override void OnInspectorGUI()
        {
            STSSceneManager tTarget = (STSSceneManager)target;
            serializedObject.Update();
            EditorGUILayout.PropertyField(SPDefaultEffectOnEnter);
            EditorGUILayout.PropertyField(SPDefaultEffectOnExit);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif