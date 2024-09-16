using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSEffectCurveFadeGradient class defines a specific scene transition effect
    /// characterized by a gradient that fades in accordance to an animation curve.
    /// </summary>
    /// <remarks>
    /// This effect transitions between two tints (primary and secondary) based on the progression
    /// defined by an animation curve. The effect's alpha component is proportionally adjusted
    /// by the primary tint's alpha value.
    /// </remarks>
    /// <example>
    /// The effect interpolates color values between TintPrimary and TintSecondary, giving a smooth
    /// fade transition as per the curve's evaluation. It then applies an alpha blend based on the
    /// evaluated curve percentage multiplied by the TintPrimary's alpha.
    /// </example>
    [STSEffectName("Fade/Fade gradient curve")]
    [STSTintPrimary()]
    [STSTintSecondary()]
    [STSAnimationCurve()]
    public class STSEffectCurveFadeGradient : STSEffect
    {
        /// <summary>
        /// Draws the fade gradient curve effect within a specified rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle within which the effect is drawn.</param>
        public override void Draw(Rect sRect)
        {
            CurvePurcent = Curve.Evaluate(Purcent);
            if (Purcent > 0)
            {
                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, CurvePurcent);
                Color tFadeColorAlpha = new Color(tColorLerp.r, tColorLerp.g, tColorLerp.b, CurvePurcent * TintPrimary.a);
                STSDrawQuad.DrawRect(sRect, tFadeColorAlpha);
            }
        }
    }
}