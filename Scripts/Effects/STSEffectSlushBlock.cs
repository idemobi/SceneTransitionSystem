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
    /// Represents the "Slush Block" effect for scene transitions in the Scene Transition System.
    /// Inherits from the STSEffect base class.
    /// </summary>
    [STSEffectName("Slush Block")]
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    [STSClockwise()]
    public class STSEffectSlushBlock : STSEffect
    {
        /// <summary>
        /// Represents a matrix used in the "Slush Block" transitions effect within the SceneTransitionSystem.
        /// </summary>
        /// <remarks>
        /// This matrix is responsible for organizing and managing the tiles involved in the slush block effect,
        /// including creating and ordering the tiles.
        /// </remarks>
        private STSMatrix Matrix;

        /// <summary>
        /// Prepares the slush block effect by initializing the matrix with the specified parameters and rectangular area.
        /// </summary>
        /// <param name="sRect">The rectangular area within which the matrix will be created.</param>
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
        }

        /// <summary>
        /// Prepares the effect for entering the scene transition phase.
        /// </summary>
        /// <param name="sRect">The rectangle area where the effect will be drawn.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectEnter()");
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// Prepares the current effect for exiting the transition using the specified rectangle.
        /// <param name="sRect">The rectangle defining the area for the effect exit preparation.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectExit()");
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the current state of the slush block transition effect.
        /// </summary>
        /// <param name="sRect">The rectangle within which the effect should be drawn.</param>
        public override void Draw(Rect sRect)
        {
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                float tWidthPurcent = Matrix.TilesList[0].Rectangle.width * Purcent;
                float tWidth = Matrix.TilesList[0].Rectangle.width;
                float tHeight = Matrix.TilesList[0].Rectangle.height;
                if (Clockwise == STSClockwise.Clockwise)
                {
                    foreach (STSTile tTile in Matrix.TilesList)
                    {
                        Vector2 tAa = new Vector2(tTile.Rectangle.x, tTile.Rectangle.y);
                        Vector2 tAb = new Vector2(tTile.Rectangle.x, tTile.Rectangle.y + tHeight);
                        Vector2 tAc = new Vector2(tTile.Rectangle.x + tWidthPurcent, tTile.Rectangle.y);

                        Vector2 tBa = new Vector2(tTile.Rectangle.x + tWidth, tTile.Rectangle.y);
                        Vector2 tBb = new Vector2(tTile.Rectangle.x + tWidth, tTile.Rectangle.y + tHeight);
                        Vector2 tBc = new Vector2(tTile.Rectangle.x + tWidth - tWidthPurcent, tTile.Rectangle.y + tHeight);

                        STSDrawTriangle.DrawTriangle(tAa, tAb, tAc, TintPrimary);
                        STSDrawTriangle.DrawTriangle(tBa, tBb, tBc, TintPrimary);
                    }
                }
                else
                {
                    foreach (STSTile tTile in Matrix.TilesList)
                    {
                        Vector2 tAa = new Vector2(tTile.Rectangle.x, tTile.Rectangle.y);
                        Vector2 tAb = new Vector2(tTile.Rectangle.x, tTile.Rectangle.y + tHeight);
                        Vector2 tAc = new Vector2(tTile.Rectangle.x + tWidthPurcent, tTile.Rectangle.y + tHeight);

                        Vector2 tBa = new Vector2(tTile.Rectangle.x + tWidth, tTile.Rectangle.y);
                        Vector2 tBb = new Vector2(tTile.Rectangle.x + tWidth, tTile.Rectangle.y + tHeight);
                        Vector2 tBc = new Vector2(tTile.Rectangle.x + tWidth - tWidthPurcent, tTile.Rectangle.y);

                        STSDrawTriangle.DrawTriangle(tAa, tAb, tAc, TintPrimary);
                        STSDrawTriangle.DrawTriangle(tBa, tBb, tBc, TintPrimary);
                    }
                }
            }
            //STSBenchmark.Finish();
        }
    }
}