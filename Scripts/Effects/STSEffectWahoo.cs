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
    /// Represents a special effect with the name "Whaoo" for scene transitions.
    /// </summary>
    [STSEffectNameAttribute("Whaoo")]
    public class STSEffectWhaoo : STSEffect
    {
        /// <summary>
        /// Prepares the given rectangle for rendering effects.
        /// </summary>
        /// <param name="sRect">The source rectangle to prepare.</param>
        public void Prepare(Rect sRect)
        {
        }

        /// <summary>
        /// Prepares the effect for entering the scene transition by setting up necessary data for drawing.
        /// </summary>
        /// <param name="sRect">The rectangle area in which the effect will be prepared and drawn.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the necessary data for the exit effect transition.
        /// </summary>
        /// <param name="sRect">The rectangle representing the screen area where the effect will be rendered.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the transition effect on the given rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle where the effect will be drawn.</param>
        public override void Draw(Rect sRect)
        {
            // Do drawing with purcent
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                float tWidth = sRect.width * Purcent;
                float tHeight = sRect.height * Purcent;
                float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
                float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
                STSDrawQuad.DrawRect(new Rect(tX, tY, tWidth, tHeight), TintPrimary);
            }
            //STSBenchmark.Finish();
        }
    }
}