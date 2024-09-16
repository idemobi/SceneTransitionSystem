using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSEffectPreview class is an EditorWindow used to preview scene transition effects within the Unity Editor.
    /// </summary>
    public class STSEffectPreview : EditorWindow
    {
        /// <summary>
        /// Represents a scene transition effect in the Scene Transition System.
        /// This variable is used to manage the lifecycle and actions related to the transition effect,
        /// including preparation, execution, pausing, stopping, and resetting the effect.
        /// </summary>
        STSEffect Effect = null;

        /// Duration of the effect in seconds.
        /// Controls how long the STSEffect will play when triggered in the STSEffectPreview class.
        /// This value is set to a default of 1.0F.
        float Duration = 1.0F;

        /// <summary>
        /// Indicates whether the STSEffectPreview is currently playing an effect.
        /// </summary>
        bool IsPlaying = false;

        /// <summary>
        /// The time in seconds since the last frame.
        /// Used for calculating frame-based animations and effects.
        /// </summary>
        float Delta = 0.0F;

        /// <summary>
        /// A Texture2D object used as the background for the scene transition effect preview.
        /// </summary>
        Texture2D Background = null;

        /// <summary>
        /// Indicates whether to disable the preview in the STSEffectPreview editor window.
        /// When set to true, the preview of the effect will not be shown.
        /// </summary>
        public bool NoPreview = false;

        /// <summary>
        /// A <see cref="GUIStyle"/> instance used in the STSEffectPreview class to style the label displayed when no preview is available.
        /// </summary>
        static GUIStyle tNoPreviewFieldStyle;

        /// <summary>
        /// Index of the selected preview effect in the STSEffectPreview window.
        /// </summary>
        int SelectedPreview = 0;

        /// <summary>
        /// Holds the position and dimensions of the last known Rect of the STSEffectPreview window.
        /// </summary>
        Rect LastPosition;

        /// <summary>
        /// Represents a static instance of the STSEffectPreview editor window.
        /// </summary>
        public static STSEffectPreview kEffectPreview;

        /// <summary>
        /// Displays the effect preview window, creating it if it does not already exist.
        /// </summary>
        /// <returns>
        /// The <see cref="STSEffectPreview"/> instance representing the effect preview window.
        /// </returns>
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

        /// <summary>
        /// Method executed when the STSEffectPreview editor window is destroyed.
        /// This method ensures that the static reference to the editor window instance (kEffectPreview)
        /// is set to null, allowing for proper cleanup and garbage collection.
        /// </summary>
        private void OnDestroy()
        {
            kEffectPreview = null;
        }

        /// <summary>
        /// Initializes necessary styles and settings for the effect preview window.
        /// </summary>
        void Start()
        {
            //Debug.Log("Start");
            tNoPreviewFieldStyle = new GUIStyle(EditorStyles.boldLabel);
            tNoPreviewFieldStyle.alignment = TextAnchor.MiddleCenter;
            tNoPreviewFieldStyle.normal.textColor = Color.red;
            LastPosition = position;
        }

        /// <summary>
        /// Represents the time elapsed since the last editor frame update.
        /// This variable is crucial to determine time-based modifications/actions
        /// when previewing effects in the editor.
        /// </summary>
        double editorDeltaTime = 0f;

        /// <summary>
        /// Stores the timestamp of the last recorded point in time since the application started.
        /// Used to measure elapsed time between updates or events within the effect preview.
        /// </summary>
        double lastTimeSinceStartup = 0f;

        /// <summary>
        /// Updates the editorDeltaTime based on the time since the last call to this method.
        /// </summary>
        /// <remarks>
        /// This method calculates the delta time in the editor by comparing the current time
        /// since startup with the time recorded during the last call. This is used to manage
        /// time-dependent functionalities within the editor.
        /// </remarks>
        private void SetEditorDeltaTime()
        {
            if (lastTimeSinceStartup == 0f)
            {
                lastTimeSinceStartup = EditorApplication.timeSinceStartup;
            }

            editorDeltaTime = EditorApplication.timeSinceStartup - lastTimeSinceStartup;
            lastTimeSinceStartup = EditorApplication.timeSinceStartup;
        }

        /// <summary>
        /// Updates the state of the effect preview. This method is called regularly to update the
        /// effect's progress if it is currently playing. If the effect is playing, it increments
        /// the elapsed time (Delta) and recalculates the effect's percentage of completion.
        /// It stops the playback once the elapsed time exceeds the specified duration.
        /// If the effect is available, it estimates the percentage completion and triggers a repaint.
        /// </summary>
        void Update()
        {
            //Debug.Log("Update");
            if (IsPlaying == true)
            {
                SetEditorDeltaTime();
                Delta += (float)editorDeltaTime;
                //Debug.Log("Update IsPlaying == true Delta = " + Delta.ToString("F3") + "  /" + Duration.ToString("F3"));
                if (Delta <= Duration)
                {
                    if (Effect != null)
                    {
                        Effect.EstimatePurcent();
                        Repaint();
                        //Debug.Log("Update effect Purcent = " + Effect.Purcent.ToString("F3"));
                        // Repaint();
                    }
                    else
                    {
                        //Debug.Log("Update effect is null");
                        IsPlaying = false;
                    }
                }
                else
                {
                    //Debug.Log("Update play is finish");
                    IsPlaying = false;
                }
            }
        }

        /// <summary>
        /// Checks if there has been a change in the window's size or position.
        /// If any change is detected, it updates the stored position, prepares
        /// the effect, and repaints the window.
        /// </summary>
        public void CheckResize()
        {
            if (LastPosition.x != position.x || LastPosition.y != position.y || LastPosition.width != position.width || LastPosition.height != position.height)
            {
                LastPosition = position;
                EffectPrepare();
                Repaint();
            }
        }

        /// <summary>
        /// Prepares the effect for rendering in the preview window.
        /// </summary>
        /// <remarks>
        /// This method checks if the effect is not currently playing and,
        /// if an effect is assigned, it calls the effect's PrepareEffectExit method with the
        /// current dimensions of the preview window. This ensures the effect is properly
        /// set up for display.
        /// </remarks>
        public void EffectPrepare()
        {
            if (IsPlaying == false)
            {
                if (Effect != null)
                {
                    Effect.PrepareEffectExit(new Rect(0, 0, position.width, position.height));
                }
            }
        }

        /// <summary>
        /// Runs the transition effect over a specified duration.
        /// </summary>
        /// <param name="sDuration">The duration over which the effect will run.</param>
        public void EffectRun(float sDuration)
        {
            //Debug.Log("EffectRun sDuration" + sDuration.ToString("F3"));
            if (Effect != null)
            {
                //Debug.Log("EffectRun is not null");
                Effect.Purcent = 0;
                Duration = sDuration;
                IsPlaying = true;
                Delta = 0.0F;
                //Debug.Log("Update IsPlaying == true Delta = " + Delta.ToString("F3") + "  /" + Duration.ToString("F3"));
            }
        }

        /// <summary>
        /// Sets the specified effect to be used in the preview.
        /// </summary>
        /// <param name="sEffect">The STSEffect to be set for the preview.</param>
        public void SetEffect(STSEffect sEffect)
        {
            if (IsPlaying == false)
            {
                //Debug.Log("SetEffect");
                if (sEffect != Effect)
                {
                    Effect = sEffect;
                    EffectPrepare();
                }
            }
        }

        /// <summary>
        /// Sets the percentage value for the current effect.
        /// </summary>
        /// <param name="sPurcent">The percentage value to set for the effect.</param>
        public void SetEffectPurcent(float sPurcent)
        {
            if (IsPlaying == false)
            {
                if (Effect != null)
                {
                    Effect.Purcent = sPurcent;
                }
            }
        }

        /// <summary>
        /// The OnGUI method is responsible for rendering the GUI elements within the
        /// STSEffectPreview window. It handles resizing, background rendering, and
        /// effect execution. This method checks for a resized window and adjusts the
        /// layout accordingly. It also manages the display of background textures and
        /// the rendering of preview effects based on user selection.
        /// </summary>
        void OnGUI()
        {
            //Debug.Log("OnGUI");
            CheckResize();
            Rect ThisRect = new Rect(0, 0, position.width, position.height);
            if (Background == null)
            {
                //Background = AssetDatabase.LoadAssetAtPath<Texture2D>(STSFindPackage.PathOfPackage("/Scripts/Editor/Resources/STSPreviewA.png"));
                STSDrawQuad.DrawRect(ThisRect, Color.white);
                STSDrawCircle.DrawCircle(ThisRect.center, ThisRect.height / 2.0F, 64, Color.black);
            }

            if (Background != null)
            {
                GUI.DrawTexture(ThisRect, Background);
            }

            if (NoPreview == true)
            {
                GUI.Label(ThisRect, new GUIContent(STSConstants.K_NO_BIG_PREVIEW), tNoPreviewFieldStyle);
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
                //Debug.Log("effect is drawinf with purcent " + Effect.Purcent);
                Effect.EstimateCurvePurcent();
                Effect.Draw(ThisRect);
            }
            else
            {
                //Debug.Log("effect is null");
            }
        }
    }
}