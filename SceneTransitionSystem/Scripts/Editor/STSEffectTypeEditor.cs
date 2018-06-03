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

            // FiveCross
            if (tEffectType.GetCustomAttributes(typeof(STSNoFiveCrossAttribute), true).Length == 0)
            {
                tH += tPopupFieldStyle.fixedHeight + kMarge;
            }

            // FiveCross
            if (tEffectType.GetCustomAttributes(typeof(STSNoNineCrossAttribute), true).Length == 0)
            {
                tH += tPopupFieldStyle.fixedHeight + kMarge;
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
        //float LocalPurcent = 0.0F;
        STSEffect rReturn = null;
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
                    rReturn = null;
                }
            }
            tY += tPopupFieldStyle.fixedHeight + kMarge;
            bool tNewReturn = false;
            if (rReturn == null)
            {
                string tName = tEffectName.stringValue;
                int tIndexDD = STSEffectType.kEffectNameList.IndexOf(tName);
                if (tIndexDD < 0 || tIndexDD >= STSEffectType.kEffectNameList.Count())
                {
                    rReturn = new STSEffectFade();
                }
                else
                {
                    Type tEffectTypeDD = STSEffectType.kEffectTypeList[tIndexDD];
                    rReturn = (STSEffect)Activator.CreateInstance(tEffectTypeDD);
                }
                tNewReturn = true;
            }
            Type tEffectType = STSEffectType.kEffectTypeList[tIndexNew];

            EditorGUI.indentLevel++;



            EditorGUI.BeginChangeCheck();

            // Primary Tint
            if (tEffectType.GetCustomAttributes(typeof(STSNoTintPrimaryAttribute), true).Length == 0)
            {
                Rect tRectTintPrimary = new Rect(position.x, tY, position.width, tColorFieldStyle.fixedHeight);
                SerializedProperty tTintPrimary = property.FindPropertyRelative("TintPrimary");
                EditorGUI.PropertyField(tRectTintPrimary, tTintPrimary, false);
                tY += tColorFieldStyle.fixedHeight + kMarge;
                rReturn.TintPrimary = tTintPrimary.colorValue;
            }

            // Secondary Tint
            if (tEffectType.GetCustomAttributes(typeof(STSNoTintSecondaryAttribute), true).Length == 0)
            {
                Rect tRectTintSecondary = new Rect(position.x, tY, position.width, tColorFieldStyle.fixedHeight);
                SerializedProperty tTintSecondary = property.FindPropertyRelative("TintSecondary");
                EditorGUI.PropertyField(tRectTintSecondary, tTintSecondary, false);
                tY += tColorFieldStyle.fixedHeight + kMarge;
                rReturn.TintSecondary = tTintSecondary.colorValue;
            }

            // Primary Texture
            if (tEffectType.GetCustomAttributes(typeof(STSNoTextureSecondaryAttribute), true).Length == 0)
            {
                Rect tRectTexturePrimary = new Rect(position.x, tY, position.width, tObjectFieldStyle.fixedHeight);
                SerializedProperty tTexturePrimary = property.FindPropertyRelative("TexturePrimary");
                EditorGUI.PropertyField(tRectTexturePrimary, tTexturePrimary, false);
                tY += tObjectFieldStyle.fixedHeight + kMarge;
                rReturn.TexturePrimary = (Texture2D)tTexturePrimary.objectReferenceValue;
            }

            // Secondary Texture
            if (tEffectType.GetCustomAttributes(typeof(STSNoTextureSecondaryAttribute), true).Length == 0)
            {
                Rect tRectTextureSecondary = new Rect(position.x, tY, position.width, tObjectFieldStyle.fixedHeight);
                SerializedProperty tTextureSecondary = property.FindPropertyRelative("TextureSecondary");
                EditorGUI.PropertyField(tRectTextureSecondary, tTextureSecondary, false);
                tY += tObjectFieldStyle.fixedHeight + kMarge;
                rReturn.TextureSecondary = (Texture2D)tTextureSecondary.objectReferenceValue;
            }

            // Parameter One
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterOneAttribute), true).Length == 0)
            {
                Rect tRectParameterOne = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tParameterOne = property.FindPropertyRelative("ParameterOne");
                EditorGUI.PropertyField(tRectParameterOne, tParameterOne, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
                rReturn.ParameterOne = tParameterOne.intValue;
            }

            // Parameter Two
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterTwoAttribute), true).Length == 0)
            {
                Rect tRectParameterTwo = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tParameterTwo = property.FindPropertyRelative("ParameterTwo");
                EditorGUI.PropertyField(tRectParameterTwo, tParameterTwo, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
                rReturn.ParameterTwo = tParameterTwo.intValue;
            }

            // Parameter Three
            if (tEffectType.GetCustomAttributes(typeof(STSNoParameterThreeAttribute), true).Length == 0)
            {
                Rect tRectParameterThree = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tParameterThree = property.FindPropertyRelative("ParameterThree");
                EditorGUI.PropertyField(tRectParameterThree, tParameterThree, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
                rReturn.ParameterThree = tParameterThree.intValue;
            }

            // Offset
            if (tEffectType.GetCustomAttributes(typeof(STSNoOffsetAttribute), true).Length == 0)
            {
                Rect tRectOffset = new Rect(position.x, tY, position.width, tNumberFieldStyle.fixedHeight);
                SerializedProperty tOffset = property.FindPropertyRelative("Offset");
                EditorGUI.PropertyField(tRectOffset, tOffset, false);
                tY += tNumberFieldStyle.fixedHeight + kMarge;
                rReturn.Offset = tOffset.vector2Value;
            }
            // FiveCross
            if (tEffectType.GetCustomAttributes(typeof(STSNoFiveCrossAttribute), true).Length == 0)
            {
                Rect tRectFiveCross = new Rect(position.x, tY, position.width, tPopupFieldStyle.fixedHeight);
                SerializedProperty tFiveCross = property.FindPropertyRelative("FiveCross");
                EditorGUI.PropertyField(tRectFiveCross, tFiveCross, false);
                tY += tPopupFieldStyle.fixedHeight + kMarge;
                rReturn.FiveCross = (STSFiveCross)tFiveCross.intValue;
            }
            // NineCross
            if (tEffectType.GetCustomAttributes(typeof(STSNoNineCrossAttribute), true).Length == 0)
            {
                Rect tRectNineCross = new Rect(position.x, tY, position.width, tPopupFieldStyle.fixedHeight);
                SerializedProperty tNineCross = property.FindPropertyRelative("NineCross");
                EditorGUI.PropertyField(tRectNineCross, tNineCross, false);
                tY += tPopupFieldStyle.fixedHeight + kMarge;
                rReturn.NineCross = (STSNineCross)tNineCross.intValue;
            }

            if (EditorGUI.EndChangeCheck() == true)
            {
                tNewReturn = true;
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

            //Draw local render

            Rect tPreviewRect = new Rect(position.x, tY, position.width, 80.0F);
            if (tNewReturn == true)
            {
                Debug.Log("MUST PREPARE THE EFFECT EXIT");
                rReturn.PrepareEffectExit(tPreviewRect);
            }
            //rReturn.Purcent = LocalPurcent;
            rReturn.Purcent = tPurcent.floatValue;
            // draw white rect
            STSTransitionDrawing.DrawRect(tPreviewRect, Color.white);
            STSTransitionDrawing.DrawCircle(tPreviewRect.center, tPreviewRect.height / 2.0F, 32, Color.red);
            // 
            // TODO : Add image in background
            // draw preview
            rReturn.Draw(tPreviewRect);

            //// test auto animation
            //LocalPurcent += Time.deltaTime;
            //if (LocalPurcent > 1.0F)
            //{
            //    LocalPurcent = 0.0F;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif