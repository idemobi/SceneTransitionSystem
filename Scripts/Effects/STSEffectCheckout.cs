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
    /// Represents a "Checkout" scene transition effect in the Scene Transition System.
    /// </summary>
    [STSEffectNameAttribute("Checkout")]
    public class STSEffectCheckout : STSEffect
    {
        /// <summary>
        /// Prepares any necessary data for the transition effect using the given rectangular area.
        /// </summary>
        /// <param name="sRect">The rectangular area used to prepare the transition effect.</param>
        public void Prepare(Rect sRect)
        {
        }

        /// <summary>
        /// Prepares the effect for entering transition by setting up the necessary data for rendering.
        /// </summary>
        /// <param name="sRect">The rectangular area where the effect will be drawn.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// Prepares the effect for exiting by preparing the necessary data for drawing.
        /// <param name="sRect">The rectangle area where the effect will be prepared and drawn.</param>
        /// /
        public override void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the current effect within the specified rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle in which to draw the effect.</param>
        public override void Draw(Rect sRect)
        {
            // Do drawing with purcent
            float tWidth = sRect.width * Purcent;
            float tHeight = sRect.height * Purcent;
            int tRadius = (int)Mathf.Max(tWidth, tHeight);
            //float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
            //float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
            STSDrawing.DrawCircle(sRect.center, tRadius, TintPrimary, tWidth, ParameterOne);
        }
    }
}