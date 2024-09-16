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
    /// Represents an octagonal random scene transition effect.
    /// </summary>
    [STSEffectName("Octo/Octo Random")]
    [STSNoSmallPreview]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    // ***
    public class STSEffectOctoRandom : STSEffect
    {
        /// <summary>
        /// Represents a matrix used within the scene transition system effects. The matrix
        /// is utilized for organizing and managing tiles that represent various parts of
        /// the scene. It supports functions like matrix creation, shuffling, and ordering
        /// which are critical for producing dynamic visual transitions.
        /// </summary>
        private STSMatrix Matrix;

        /// <summary>
        /// Prepares the effect by creating and shuffling a matrix based on the specified rectangle dimensions
        /// and the effect's parameters.
        /// </summary>
        /// <param name="sRect">The rectangle defining the area for the matrix.</param>
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
        /// Prepares the transition effect for entering a new scene. This method is called to set up any necessary data before the actual transition begins.
        /// </summary>
        /// <param name="sRect">The rectangle area of the screen where the transition effect will be applied.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectEnter()");
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the necessary data for the effect exit transition.
        /// </summary>
        /// <param name="sRect">A rectangle representing the screen area to prepare for the effect exit.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectExit()");
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the transition effect on the screen based on the current progress percentage.
        /// </summary>
        /// <param name="sRect">The rectangle defining the area where the effect should be drawn.</param>
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
                    STSDrawCircle.DrawCircle(tTile.Rectangle.center, tTile.Rectangle.width, 2, TintPrimary);
                }

                // Draw Alpha tile
                if (tIndex < Matrix.TileCount)
                {
                    STSTile tTileAlpha = Matrix.TilesList[tIndex];
                    float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                    //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                    //Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha*TintPrimary.a);
                    //STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);

                    STSDrawCircle.DrawCircle(tTileAlpha.Rectangle.center, tTileAlpha.Rectangle.width * tAlpha, 2, TintPrimary);
                }
            }
            //STSBenchmark.Finish();
        }
    }
}