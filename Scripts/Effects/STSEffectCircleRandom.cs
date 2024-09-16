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
    /// The STSEffectCircleRandom class defines a circle effect with random transitions.
    /// </summary>
    [STSEffectName("Circle/Circle Random")]
    [STSNoSmallPreview]
    [STSNoBigPreview]
    [STSTintPrimary("Tint")]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    public class STSEffectCircleRandom : STSEffect
    {
        /// <summary>
        /// Represents the matrix used to manage and store tiles for various effects in the Scene Transition System.
        /// This matrix is responsible for creating, shuffling, and ordering tiles, which are used in the transition effects.
        /// </summary>
        private STSMatrix Matrix;

        /// <summary>
        /// Prepares the effect by validating parameters and creating a shuffled matrix based on the given rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle within which the effect should be prepared.</param>
        public void Prepare(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine Prepare()");
            if (ParameterOne < 1)
            {
                ParameterOne = 1;
            }

            if (ParameterTwo < 1)
            {
                ParameterTwo = 1;
            }

            Matrix = new STSMatrix();
            Matrix.CreateMatrix(ParameterOne, ParameterTwo, sRect);
            Matrix.ShuffleList();
        }

        /// <summary>
        /// Prepares necessary data for drawing the enter effect with a random circle pattern.
        /// </summary>
        /// <param name="sRect">The rectangle representing the screen area where the effect will be applied.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectEnter()");
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the effect for exiting the current scene transition.
        /// </summary>
        /// <param name="sRect">The rectangle that defines the screen area for the effect.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectExit()");
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the transition effect based on a circle pattern with a random order of tiles.
        /// </summary>
        /// <param name="sRect">The rectangle within which the effect is drawn.</param>
        public override void Draw(Rect sRect)
        {
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                int tIndex = (int)Mathf.Floor(Purcent * Matrix.TileCount);
                //Debug.Log("tIndex = " + tIndex + " on TileCount) = "+TileCount);
                // draw all fill tiles
                for (int i = 0; i < tIndex; i++)
                {
                    STSTile tTile = Matrix.TilesList[i];
                    //STSTransitionDrawing.DrawRect(tTile.Rectangle, TintPrimary);
                    STSDrawCircle.DrawCircle(tTile.Rectangle.center, tTile.Rectangle.width, 32, TintPrimary);
                }

                // Draw Alpha tile
                if (tIndex < Matrix.TileCount)
                {
                    STSTile tTileAlpha = Matrix.TilesList[tIndex];
                    float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                    //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                    //Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha*TintPrimary.a);
                    //STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                    STSDrawCircle.DrawCircle(tTileAlpha.Rectangle.center, tTileAlpha.Rectangle.width * tAlpha, 32, TintPrimary);
                }
            }
            //STSBenchmark.Finish();
        }
    }
}