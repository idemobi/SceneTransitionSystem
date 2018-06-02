﻿using System;
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
    [CustomPropertyDrawer(typeof(STSEffectType))]
    public class STSEffectTypeEditor : PropertyDrawer
    {
        //-------------------------------------------------------------------------------------------------------------
        static GUIStyle tPopupFieldStyle;
        static GUIStyle tColorFieldStyle;
        static GUIStyle tTextfieldStyle;
        static GUIStyle tObjectFieldStyle;
        static GUIStyle tNumberFieldStyle;
        //-------------------------------------------------------------------------------------------------------------
        const float kMarge = 4.0F;
        //-------------------------------------------------------------------------------------------------------------
        static STSEffectTypeEditor()
        {
            tPopupFieldStyle = new GUIStyle(EditorStyles.popup);
            tPopupFieldStyle.fixedHeight = tPopupFieldStyle.CalcHeight(new GUIContent("A"), 100);

            tColorFieldStyle = new GUIStyle(EditorStyles.colorField);
            tColorFieldStyle.fixedHeight = tColorFieldStyle.CalcHeight(new GUIContent("A"), 100);

            tTextfieldStyle = new GUIStyle(EditorStyles.textField);
            tTextfieldStyle.fixedHeight = tTextfieldStyle.CalcHeight(new GUIContent("A"), 100);

            tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent("A"), 100);

            tNumberFieldStyle = new GUIStyle(EditorStyles.numberField);
            tNumberFieldStyle.fixedHeight = tNumberFieldStyle.CalcHeight(new GUIContent("A"), 100);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float tH = 0.0F;

            SerializedProperty tEffectName = property.FindPropertyRelative("EffectName");
            string tValue = tEffectName.stringValue;
            int tIndex = STSEffectType.kEffectNameList.IndexOf(tValue);
            if (tIndex < 0 || tIndex >= STSEffectType.kEffectNameList.Count())
            {
                tIndex = 0;
            }
            Type tEffectType = STSEffectType.kEffectTypeList[tIndex];

            // Effet selectio by popup
            tH += tPopupFieldStyle.fixedHeight + kMarge;

            // Tint Primary
            if (tEffectType.GetCustomAttributes(typeof(STSNoTintPrimaryAttribute), true).Length == 0)
            {
                tH += tColorFieldStyle.fixedHeight + kMarge;
            }

            // Tint Secondary
            if (tEffectType.GetCustomAttributes(typeof(STSNoTintSecondaryAttribute), true).Length == 0)
            {
                tH += tColorFieldStyle.fixedHeight + kMarge;
            }

            // Texture Primary
            if (tEffectType.GetCustomAttributes(typeof(STSNoTexturePrimaryAttribute), true).Length == 0)
            {
                tH += tObjectFieldStyle.fixedHeight + kMarge;
            }

            // Texture Secondary
            if (tEffectType.GetCustomAttributes(typeof(STSNoTextureSecondaryAttribute), true).Length == 0)
            {
                tH += tObjectFieldStyle.fixedHeight + kMarge;
            }

            // Parameter One
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterOneAttribute), true).Length == 0)
            {
                tH += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Parameter Two
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterTwoAttribute), true).Length == 0)
            {
                tH += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Parameter Three
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterThreeAttribute), true).Length == 0)
            {
                tH += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Offset
            if (tEffectType.GetCustomAttributes(typeof(STSNoOffsetAttribute), true).Length == 0)
            {
                tH += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Duration
            tH += tNumberFieldStyle.fixedHeight + kMarge;

            // Purcent
            tH += tNumberFieldStyle.fixedHeight + kMarge;


            // Purcent
            tH += 80.0F + kMarge;

            return tH;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float tY = position.y;

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            Rect tRect = new Rect(position.x, tY, position.width, tPopupFieldStyle.fixedHeight);
            SerializedProperty tEffectName = property.FindPropertyRelative("EffectName");
            string tValue = tEffectName.stringValue;
            int tIndex = STSEffectType.kEffectNameList.IndexOf(tValue);
            if (tIndex < 0 || tIndex >= STSEffectType.kEffectNameList.Count())
            {
                tIndex = 0;
            }
            int tIndexNew = EditorGUI.Popup(tRect, new GUIContent(property.displayName), tIndex, STSEffectType.kEffectContentList.ToArray());
            if (EditorGUI.EndChangeCheck() == true)
            {
                if (tIndexNew != tIndex)
                {
                    tValue = STSEffectType.kEffectNameList[tIndexNew];
                    tEffectName.stringValue = tValue;
                    tEffectName.serializedObject.ApplyModifiedProperties();
                }
            }
            tY += tPopupFieldStyle.fixedHeight + kMarge;

            //Debug.Log("tIndexNew = " + tIndexNew);
            //foreach (Type tType in STSEffectType.kEffectTypeList)
            //{
            //    Debug.Log("tType = " + tType.Name);
            //}
            Type tEffectType = STSEffectType.kEffectTypeList[tIndexNew];


            EditorGUI.indentLevel++;

            // Primary Tint
            //EditorGUI.BeginChangeCheck();
            //Rect tRectTintPrimary = new Rect(position.x, tY, position.width, tColorFieldStyle.fixedHeight);
            //SerializedProperty tTintPrimary = property.FindPropertyRelative("TintPrimary");
            //Color tOldTintPrimary = tTintPrimary.colorValue;
            //Color tNewTintPrimary = EditorGUI.ColorField(tRectTintPrimary, new GUIContent("Tint Primary"),  tOldTintPrimary);
            //if (EditorGUI.EndChangeCheck() == true)
            //{
            //    tTintPrimary.colorValue = tNewTintPrimary;
            //    tTintPrimary.serializedObject.ApplyModifiedProperties();
            //}
            //tY += tColorFieldStyle.fixedHeight + kMarge;


            //// Secondary Tint
            //EditorGUI.BeginChangeCheck();
            //Rect tRectTintSecondary = new Rect(position.x, tY, position.width, tColorFieldStyle.fixedHeight);
            //SerializedProperty tTintSecondary = property.FindPropertyRelative("TintSecondary");
            //Color tOldTintSecondary = tTintSecondary.colorValue;
            //Color tNewTintSecondary = EditorGUI.ColorField(tRectTintSecondary, new GUIContent("Tint Secondary"), tOldTintSecondary);
            //if (EditorGUI.EndChangeCheck() == true)
            //{
            //    tTintSecondary.colorValue = tNewTintSecondary;
            //    tTintSecondary.serializedObject.ApplyModifiedProperties();
            //}
            //tY += tColorFieldStyle.fixedHeight + kMarge;

            // Primary Tint
            if (tEffectType.GetCustomAttributes(typeof(STSNoTintPrimaryAttribute), true).Length == 0)
            {
                Rect tRectTintPrimary = new Rect(position.x, tY, position.width, tColorFieldStyle.fixedHeight);
                SerializedProperty tTintPrimary = property.FindPropertyRelative("TintPrimary");
                EditorGUI.PropertyField(tRectTintPrimary, tTintPrimary, false);
                tY += tColorFieldStyle.fixedHeight + kMarge;
            }

            // Secondary Tint
            if (tEffectType.GetCustomAttributes(typeof(STSNoTintSecondaryAttribute), true).Length == 0)
            {
                Rect tRectTintSecondary = new Rect(position.x, tY, position.width, tColorFieldStyle.fixedHeight);
                SerializedProperty tTintSecondary = property.FindPropertyRelative("TintSecondary");
                EditorGUI.PropertyField(tRectTintSecondary, tTintSecondary, false);
                tY += tColorFieldStyle.fixedHeight + kMarge;
            }

            // Primary Texture
            if (tEffectType.GetCustomAttributes(typeof(STSNoTextureSecondaryAttribute), true).Length == 0)
            {
                Rect tRectTexturePrimary = new Rect(position.x, tY, position.width, tObjectFieldStyle.fixedHeight);
                SerializedProperty tTexturePrimary = property.FindPropertyRelative("TexturePrimary");
                EditorGUI.PropertyField(tRectTexturePrimary, tTexturePrimary, false);
                tY += tObjectFieldStyle.fixedHeight + kMarge;
            }

            // Secondary Texture
            if (tEffectType.GetCustomAttributes(typeof(STSNoTextureSecondaryAttribute), true).Length == 0)
            {
                Rect tRectTextureSecondary = new Rect(position.x, tY, position.width, tObjectFieldStyle.fixedHeight);
                SerializedProperty tTextureSecondary = property.FindPropertyRelative("TextureSecondary");
                EditorGUI.PropertyField(tRectTextureSecondary, tTextureSecondary, false);
                tY += tObjectFieldStyle.fixedHeight + kMarge;
            }


            // Parameter One
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterOneAttribute), true).Length == 0)
            {
                Rect tRectParameterOne = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tParameterOne = property.FindPropertyRelative("ParameterOne");
                EditorGUI.PropertyField(tRectParameterOne, tParameterOne, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Parameter Two
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterTwoAttribute), true).Length == 0)
            {
                Rect tRectParameterTwo = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tParameterTwo = property.FindPropertyRelative("ParameterTwo");
                EditorGUI.PropertyField(tRectParameterTwo, tParameterTwo, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
            }
            
            // Parameter Three
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterThreeAttribute), true).Length == 0)
            {
                Rect tRectParameterThree = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tParameterThree = property.FindPropertyRelative("ParameterThree");
                EditorGUI.PropertyField(tRectParameterThree, tParameterThree, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Offset
            if (tEffectType.GetCustomAttributes(typeof(STSNoOffsetAttribute), true).Length == 0)
            {
                Rect tRectOffset = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tOffset = property.FindPropertyRelative("Offset");
                EditorGUI.PropertyField(tRectOffset, tOffset, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
            }

            // Duration
            Rect tRectDuration = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
            SerializedProperty tDuration = property.FindPropertyRelative("Duration");
            //EditorGUI.PropertyField(tRectDuration, tDuration, false);
            EditorGUI.Slider(tRectDuration, tDuration, 0.1F, 10.0F);
            tY += tNumberFieldStyle.fixedHeight + kMarge;

            // Purcent
            Rect tRectPurcent = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
            SerializedProperty tPurcent = property.FindPropertyRelative("Purcent");
            EditorGUI.Slider(tRectPurcent, tPurcent, 0.0F, 1.0F);
            tY += tNumberFieldStyle.fixedHeight + kMarge;

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif