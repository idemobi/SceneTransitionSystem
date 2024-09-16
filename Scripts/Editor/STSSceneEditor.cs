using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Custom property drawer for the <see cref="STSScene"/> class to handle
    /// the display and modification of scene paths within the Unity Editor.
    /// </summary>
    [CustomPropertyDrawer(typeof(STSScene))]
    public class STSSceneEditor : PropertyDrawer
    {
        /// <summary>
        /// Custom property drawer to be used in the Unity Editor for displaying and selecting scenes in the build settings.
        /// </summary>
        /// <param name="position">Rect defining the position and dimensions of the GUI element.</param>
        /// <param name="property">SerializedProperty representing the property being drawn.</param>
        /// <param name="label">GUIContent label containing the text and tooltip for the property field.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty tPath = property.FindPropertyRelative("ScenePath");
            List<string> tPathList = new List<string>();
            tPathList.Add("None");
            foreach (EditorBuildSettingsScene tScene in EditorBuildSettings.scenes)
            {
                tPathList.Add(tScene.path);
            }

            int tPathIndex = tPathList.IndexOf(tPath.stringValue);
            int tPathIndexNext = EditorGUI.Popup(position, property.displayName, tPathIndex, tPathList.ToArray());
            if (tPathIndex != tPathIndexNext)
            {
                if (tPathIndexNext > 0)
                {
                    tPath.stringValue = tPathList[tPathIndexNext];
                }
                else
                {
                    tPath.stringValue = string.Empty;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif