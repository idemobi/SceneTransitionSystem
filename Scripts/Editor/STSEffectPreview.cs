using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSEffectPreview : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_ENVIRONMENT_SYNC, false, 62)]
        //-------------------------------------------------------------------------------------------------------------
        STSEffect Effect = null;
        Texture2D Background = null;
        public bool NoPreview = false;
        static GUIStyle tNoPreviewFieldStyle;
        int SelectedPreview = 0;
        Rect LastPosition;
        //-------------------------------------------------------------------------------------------------------------
        public static STSEffectPreview kEffectPreview;
        //-------------------------------------------------------------------------------------------------------------
        public static STSEffectPreview EffectPreviewShow()
        {
            if (kEffectPreview == null)
            {
                kEffectPreview = EditorWindow.GetWindow(typeof(STSEffectPreview)) as STSEffectPreview;
            }
            kEffectPreview.ShowUtility();
            //kEffectPreview.Focus();
            return kEffectPreview;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            kEffectPreview = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Debug.Log("Start");
            tNoPreviewFieldStyle = new GUIStyle(EditorStyles.boldLabel);
            tNoPreviewFieldStyle.alignment = TextAnchor.MiddleCenter;
            tNoPreviewFieldStyle.normal.textColor = Color.red;
            LastPosition = position;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Update()
        {
            //Debug.Log("Update");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckResize()
        {
            if (LastPosition.x != position.x || LastPosition.y != position.y || LastPosition.width != position.width || LastPosition.height != position.height)
            {
                LastPosition = position;
                EffectPrepare();
                Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EffectPrepare()
        {
            if (Effect != null)
            {
                Effect.PrepareEffectExit(new Rect(0, 0, position.width, position.height));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetEffect(STSEffect sEffect)
        {
            Debug.Log("SetEffect");
            if (sEffect != Effect)
            {
                Effect = sEffect;
                EffectPrepare();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetEffectPurcent(float sPurcent)
        {
            if (Effect != null)
            {
                Effect.Purcent = sPurcent;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnGUI()
        {
            CheckResize();
            Debug.Log("OnGUI");
            if (Background == null)
            {
                Background = AssetDatabase.LoadAssetAtPath<Texture2D>(STSFindPackage.PathOfPackage("/Scripts/Editor/Resources/STSPreviewA.png"));
            }
            Rect ThisRect = new Rect(0, 0, position.width, position.height);
            if (Background != null)
            {
                GUI.DrawTexture(ThisRect, Background);
            }
            if (NoPreview == true)
            {
                GUI.Label(ThisRect, new GUIContent("no preview available"), tNoPreviewFieldStyle);
            }
            int tSelectedPreviewNew = EditorGUILayout.IntPopup(SelectedPreview, new string[] { "A", "B", "C", "D", "…" }, new int[] { 0, 1, 2, 3, 999 });
            if (tSelectedPreviewNew != SelectedPreview)
            {
                SelectedPreview = tSelectedPreviewNew;
                if (SelectedPreview == 0)
                {
                    Background = AssetDatabase.LoadAssetAtPath<Texture2D>(STSFindPackage.PathOfPackage("/Scripts/Editor/Resources/STSPreviewA.png"));
                }
                else if (SelectedPreview == 1)
                {
                    Background = AssetDatabase.LoadAssetAtPath<Texture2D>(STSFindPackage.PathOfPackage("/Scripts/Editor/Resources/STSPreviewB.png"));
                }
                else if (SelectedPreview == 2)
                {
                    Background = AssetDatabase.LoadAssetAtPath<Texture2D>(STSFindPackage.PathOfPackage("/Scripts/Editor/Resources/STSPreviewC.png"));
                }
                else if (SelectedPreview == 3)
                {
                    Background = AssetDatabase.LoadAssetAtPath<Texture2D>(STSFindPackage.PathOfPackage("/Scripts/Editor/Resources/STSPreviewD.png"));
                }
                else
                {
                    Background = null;
                }
            }
            if (Effect != null)
            {
                Debug.Log("effect is drawinf with purcent " + Effect.Purcent);
                Effect.Draw(ThisRect);
            }
            else
            {
                Debug.Log("effect is null");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
