using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a user interface gauge component with configurable horizontal and vertical fill values.
    /// </summary>
    /// <remarks>
    /// The gauge supports smooth transitions and animations for changing values.
    /// It can expand horizontally and vertically within specified minimum values.
    /// </remarks>
    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    [ExecuteInEditMode]
    public class STSScreenGauge : MonoBehaviour
    {
        /// <summary>
        /// Represents the background image of the screen gauge.
        /// This image is used as the background for the gauge and its size can be adjusted based on the specified
        /// horizontal and vertical expansion values.
        /// </summary>
        [Header("Connect images")] public Image ImageBackground;

        /// <summary>
        /// Represents the fill image component of the screen gauge used for visual display of progress or state in the Scene Transition System.
        /// </summary>
        public Image ImageFill;

        /// <summary>
        /// Represents the image overlay component within the screen gauge UI element.
        /// Used to overlay graphics on top of the primary content of a screen gauge.
        /// </summary>
        public Image ImageOverLay;

        /// <summary>
        /// Determines whether the gauge should expand horizontally.
        /// </summary>
        [Header("Expand zone")] public bool HorizontalExpand = true;

        /// <summary>
        /// The minimum width of the screen gauge when horizontally expanded.
        /// </summary>
        public float HorizontalMin = 30.0f;

        /// <summary>
        /// A boolean variable that determines whether the gauge should expand vertically.
        /// When set to true, the height of the gauge will adjust based on the specified
        /// <see cref="VerticalValue"/> and <see cref="VerticalMin"/> limits.
        /// </summary>
        public bool VerticalExpand = false;

        /// <summary>
        /// The minimum pixel height for the vertical expansion of the screen gauge.
        /// Controls the least amount by which the height of the ImageFill can be reduced when VerticalExpand is enabled.
        /// </summary>
        public float VerticalMin = 30.0f;

        /// <summary>
        /// Gets or sets the horizontal value of the screen gauge.
        /// The value ranges from 0.0F to 1.0F, representing the normalized width
        /// of the fill image relative to the background image.
        /// Validates the value to ensure it stays within the defined range.
        /// </summary>
        [Header("Expand value")] [Range(0.0F, 1.0F)]
        public float HorizontalValue = 1.0F;

        /// <summary>
        /// Represents the vertical fill value of the gauge in the Scene Transition System.
        /// </summary>
        /// <remarks>
        /// The value ranges from 0.0F to 1.0F, where 0.0F represents an empty state and 1.0F represents a fully filled state. The vertical fill value determines how much of the gauge's vertical space is filled.
        /// </remarks>
        [Range(0.0F, 1.0F)] public float VerticalValue = 1.0F;

        /// <summary>
        /// Indicates whether the transition animation should be smooth or instant.
        /// When set to true, the gauge values (HorizontalValue and VerticalValue) change
        /// gradually over time using linear interpolation. When set to false, they
        /// change instantly to their target values.
        /// </summary>
        [Header("Animation")] public bool Smooth = true;

        /// <summary>
        /// Controls the speed of the animation for horizontal and vertical value transitions.
        /// </summary>
        public float Speed = 1.0F;

        /// <summary>
        /// Controls the rate at which the gauge fades in or out when hidden or shown.
        /// The value represents the speed at which the canvas group's alpha is changed
        /// during the hiding or showing animation.
        /// </summary>
        public float SpeedHidden = 0.10F;

        /// <summary>
        /// Indicates whether the screen gauge component is hidden.
        /// </summary>
        private bool Hidden = false;

        /// <summary>
        /// A private field representing the canvas group associated with the STSScreenGauge.
        /// This variable helps in managing the visibility and transparency of the gauge component during transitions.
        /// </summary>
        private CanvasGroup Layer;

        /// <summary>
        /// Initial horizontal value used for animations and transitions
        /// in the STSScreenGauge component.
        /// </summary>
        float HorizontalValueInit = 0.0F;

        /// <summary>
        /// Holds the initial vertical value used for gauge scaling before any changes are applied.
        /// </summary>
        float VerticalValueInit = 0.0F;

        /// <summary>
        /// The target value for the horizontal expansion of the gauge.
        /// The value should be in the range [0.0, 1.0], where 0.0 represents
        /// the minimum expansion and 1.0 represents the maximum expansion.
        /// </summary>
        float HorizontalValueTarget = 1.0F;

        /// <summary>
        /// The target vertical value for the gauge.
        /// Represents the desired vertical filling state of the gauge,
        /// ranging from 0.0 (empty) to 1.0 (full).
        /// This value is used in conjunction with <see cref="Smooth"/> and <see cref="Speed"/>
        /// to animate the transition smoothly if enabled.
        /// </summary>
        float VerticalValueTarget = 1.0F;

        /// <summary>
        /// This private variable is used to keep track of the elapsed time,
        /// primarily for smooth animations of horizontal and vertical values.
        /// It is reset when new target values for horizontal or vertical expansions are set,
        /// and incremented during the Update method to control the interpolation of values over time.
        /// </summary>
        float DeltaTimeCounter = 0.0F;

        /// <summary>
        /// Toggles the hidden state of the gauge.
        /// </summary>
        /// <param name="sValue">A boolean value indicating whether the gauge should be hidden (true) or visible (false).</param>
        public void SetHidden(bool sValue)
        {
            Hidden = sValue;
        }

        /// <summary>
        /// Ensures that the HorizontalValue property stays within the valid range of 0.0 to 1.0.
        /// If the value is greater than 1.0, it will be set to 1.0. If the value is less than 0.0,
        /// it will be set to 0.0.
        /// </summary>
        public void CheckHorizontalValue()
        {
            if (HorizontalValue > 1.0F)
            {
                HorizontalValue = 1.0F;
            }
            else if (HorizontalValue < 0)
            {
                HorizontalValue = 0.0F;
            }
        }

        /// <summary>
        /// Checks the value of the VerticalValue field, ensuring it falls within the range [0.0, 1.0].
        /// If the value exceeds this range, it is clamped to the nearest valid value.
        /// </summary>
        public void CheckVerticalValue()
        {
            if (VerticalValue > 1.0F)
            {
                VerticalValue = 1.0F;
            }
            else if (VerticalValue < 0)
            {
                VerticalValue = 0.0F;
            }
        }

        /// <summary>
        /// Sets the horizontal value of the screen gauge, with an optional parameter to allow regression.
        /// </summary>
        /// <param name="sHorizontalValue">The new horizontal value to set.</param>
        /// <param name="sRegress">Indicates whether regression is allowed. Default is false.</param>
        public void SetHorizontalValue(float sHorizontalValue, bool sRegress = false)
        {
            if (sHorizontalValue < HorizontalValue && sRegress == false)
            {
                sHorizontalValue = HorizontalValue;
            }

            HorizontalValueInit = HorizontalValue;
            HorizontalValueTarget = sHorizontalValue;
            DeltaTimeCounter = 0.0F;
        }

        /// <summary>
        /// Sets the vertical value for the screen gauge and optionally allows regression.
        /// </summary>
        /// <param name="sVerticalValue">The new vertical value to be set, in the range [0.0, 1.0].</param>
        /// <param name="sRegress">If set to false, prevents the vertical value from decreasing. Default is false.</param>
        public void SetVerticalValue(float sVerticalValue, bool sRegress = false)
        {
            if (sVerticalValue < VerticalValue && sRegress == false)
            {
                sVerticalValue = VerticalValue;
            }

            VerticalValueInit = VerticalValue;
            VerticalValueTarget = sVerticalValue;
            DeltaTimeCounter = 0.0F;
        }

        /// <summary>
        /// Unity callback method called when the script instance is being enabled.
        /// Initializes the CanvasGroup component and redraws the screen gauge.
        /// </summary>
        void OnEnable()
        {
            Layer = gameObject.GetComponent<CanvasGroup>();
            ReDraw();
        }

        /// <summary>
        /// Updates the screen gauge's horizontal and vertical values based on the configured settings
        /// and current state, applying smooth transitions if enabled. Also handles the visibility
        /// animation by adjusting the alpha channel of the CanvasGroup.
        /// </summary>
        public void Update()
        {
            if (Application.isPlaying == true)
            {
                if (Smooth == true)
                {
                    DeltaTimeCounter += Time.deltaTime * Speed;
                    HorizontalValue = Mathf.Lerp(HorizontalValueInit, HorizontalValueTarget, DeltaTimeCounter);
                    VerticalValue = Mathf.Lerp(VerticalValueInit, VerticalValueTarget, DeltaTimeCounter);
                    if (Hidden == true && Layer.alpha > 0.0F)
                    {
                        Layer.alpha -= Time.deltaTime * SpeedHidden;
                    }
                    else if (Hidden == false && Layer.alpha < 1.0F)
                    {
                        Layer.alpha += Time.deltaTime * SpeedHidden;
                    }
                }
                else
                {
                    HorizontalValue = HorizontalValueTarget;
                    VerticalValue = VerticalValueTarget;
                }
            }

            ReDraw();
        }

        /// <summary>
        /// Re-calculates and re-draws the sizes and dimensions of the gauge fill image based on the current
        /// horizontal and vertical values. The method modifies the sizeDelta of the RectTransform of the
        /// fill image to reflect the updated dimensions.
        /// </summary>
        void ReDraw()
        {
            if (ImageBackground != null)
            {
                CheckHorizontalValue();
                CheckVerticalValue();
                Rect tRect = ImageBackground.rectTransform.rect;
                float tW = tRect.width;
                if (HorizontalExpand)
                {
                    tW = HorizontalMin + (tRect.width - HorizontalMin) * HorizontalValue;
                }

                float tH = tRect.height;
                if (VerticalExpand)
                {
                    tH = VerticalMin + (tRect.height - VerticalMin) * VerticalValue;
                }

                ImageFill.rectTransform.sizeDelta = new Vector2(tW, tH);
            }
        }
    }
}