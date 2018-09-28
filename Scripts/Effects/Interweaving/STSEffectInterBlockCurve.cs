//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [STSEffectName("Interweaving/Inter Block Curve")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    [STSTwoCross]
    [STSAnimationCurve()]
    // ***
    public class STSEffectInterBlockCurve : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        private STSMatrix Matrix;
        //-------------------------------------------------------------------------------------------------------------
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
            if (TwoCross == STSTwoCross.Horizontal)
            {
                Matrix.CreateMatrix(ParameterOne, ParameterTwo, sRect);
            }
            else
            {
                Matrix.CreateMatrix(ParameterTwo, ParameterOne, sRect);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PrepareEffectEnter(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectEnter()");
            // Prepare your datas to draw
            Prepare(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PrepareEffectExit(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine PrepareEffectExit()");
            // Prepare your datas to draw
            Prepare(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Draw(Rect sRect)
        {
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                CurvePurcent = Curve.Evaluate(Purcent);
                if (TwoCross == STSTwoCross.Horizontal)
                {
                    float tWidthPurcent = Matrix.TilesList[0].Rectangle.width * CurvePurcent;
                    float tWidth = Matrix.TilesList[0].Rectangle.width;
                    float tHeight = Matrix.TilesList[0].Rectangle.height / 2.0F;
                    foreach (STSTile tTile in Matrix.TilesList)
                    {
                        Rect tA = new Rect(tTile.Rectangle.x, tTile.Rectangle.y, tWidthPurcent, tHeight);
                        Rect tB = new Rect(tTile.Rectangle.x + tWidth, tTile.Rectangle.y + tHeight, -tWidthPurcent, tHeight);
                        STSDrawQuad.DrawRect(tA, TintPrimary);
                        STSDrawQuad.DrawRect(tB, TintPrimary);
                    }
                }
                else
                {
                    float tHeightPurcent = Matrix.TilesList[0].Rectangle.height * CurvePurcent;
                    float tWidth = Matrix.TilesList[0].Rectangle.width / 2.0F;
                    float tHeight = Matrix.TilesList[0].Rectangle.height;
                    foreach (STSTile tTile in Matrix.TilesList)
                    {
                        Rect tA = new Rect(tTile.Rectangle.x, tTile.Rectangle.y, tWidth, tHeightPurcent);
                        Rect tB = new Rect(tTile.Rectangle.x + tWidth, tTile.Rectangle.y + tHeight, tWidth, -tHeightPurcent);
                        STSDrawQuad.DrawRect(tA, TintPrimary);
                        STSDrawQuad.DrawRect(tB, TintPrimary);
                    }

                }
            }
            //STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================