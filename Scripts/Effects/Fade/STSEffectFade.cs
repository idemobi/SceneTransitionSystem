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
    /// Represents an effect that applies a fade transition.
    /// </summary>
    [STSEffectName("Fade/Fade basic")]
    [STSTintPrimary("Tint")]
    public class STSEffectFade : STSEffect
    {
        /// <summary>
        /// The name of the basic fade effect used within the Scene Transition System (STS).
        /// This constant holds the identifier string for the fade effect, which is used
        /// when referencing or initializing this specific transition effect in scenes.
        /// The value is "Fade/Fade basic".
        /// </summary>
        public static string K_FADE_NAME = "Fade/Fade basic";

        /// <summary>
        /// Prepares the necessary data to draw the fade effect.
        /// </summary>
        /// <param name="sRect">The rectangle area which the fade effect will be applied to.</param>
        public void Prepare(Rect sRect)
        {
        }

        /// <summary>
        /// Prepares the effect for entering the scene with the specified rectangle dimensions.
        /// </summary>
        /// <param name="sRect">The rectangle dimensions for the effect preparation.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the effect for exiting transitions by setting up
        /// the necessary data based on the provided rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle area to consider for the effect preparation.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the fade effect using a specified rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle in which the effect should be drawn.</param>
        public override void Draw(Rect sRect)
        {
            //STSBenchmark.Start();
            //Debug.Log("Purcent = " + Purcent.ToString("F3"));
            if (Purcent > 0)
            {
                // Do drawing with purcent
                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, Purcent * TintPrimary.a);
                STSDrawQuad.DrawRect(sRect, tFadeColorAlpha);
            }
            //STSBenchmark.Finish();
        }
    }
}