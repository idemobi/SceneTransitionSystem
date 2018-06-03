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
    [STSEffectName("Fade Line Gradient")]
    // *** Remove some parameters in inspector
    //[STSNoTintSecondary]
    [STSNoTexturePrimary]
    [STSNoTextureSecondary]
    //[STSNoParameterOne]
    [STSNoParameterTwo]
    [STSNoParameterThree]
    [STSNoOffset]
    //[STSNoFiveCross]
    [STSNoNineCross]
    // ***
    [STSParameterOneEntitle("Line Number", 0, 10)]
    // ***
    public class STSEffectFadeLineGradient : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        private STSTransitionMatrix Matrix;
        //-------------------------------------------------------------------------------------------------------------
        public void Prepare(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine Prepare()");
            if (ParameterOne<1)
            {
                ParameterOne = 1;
            }
            Matrix = new STSTransitionMatrix();
            switch (FiveCross)
            {
                case STSFiveCross.Top:
                    {
                        Matrix.CreateMatrix(ParameterOne, 1, sRect);
                    }
                    break;
                case STSFiveCross.Bottom:
                    {
                        Matrix.CreateMatrix(ParameterOne, 1, sRect);
                    }
                    break;
                case STSFiveCross.Left:
                    {
                        Matrix.CreateMatrix(1, ParameterOne, sRect);
                    }
                    break;
                case STSFiveCross.Right:
                    {
                        Matrix.CreateMatrix(1, ParameterOne, sRect);
                    }
                    break;
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
            STSBenchmark.Start();
            if (Purcent > 0)
            {
                //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                int tIndex = (int)Mathf.Ceil(Purcent * ParameterOne);
                //Debug.Log("tIndex = " + tIndex);
                for (int i = 0; i < tIndex - 1; i++)
                {
                    STSTransitionTile tTile = null;
                    switch (FiveCross)
                    {
                        case STSFiveCross.Top:
                            {
                                tTile = Matrix.GetTile(i, 0);
                            }
                            break;
                        case STSFiveCross.Bottom:
                            {
                                tTile = Matrix.GetTile(ParameterOne - i - 1, 0);
                            }
                            break;
                        case STSFiveCross.Left:
                            {
                                tTile = Matrix.GetTile(0, i);
                            }
                            break;
                        case STSFiveCross.Right:
                            {
                                tTile = Matrix.GetTile(0, ParameterOne - i - 1);
                            }
                            break;
                    }
                    //Debug.Log("tTile.Rectangle = "+i+"   "+ tTile.Rectangle.ToString());
                    STSTransitionDrawing.DrawQuad(tTile.Rectangle, TintPrimary);
                }
                if (tIndex <= ParameterOne && tIndex >= 0)
                {
                    STSTransitionTile tTileAlpha = null;
                    switch (FiveCross)
                    {
                        case STSFiveCross.Top:
                            {
                                tTileAlpha = Matrix.GetTile(tIndex - 1, 0);
                            }
                            break;
                        case STSFiveCross.Bottom:
                            {
                                tTileAlpha = Matrix.GetTile(ParameterOne - tIndex, 0);
                            }
                            break;
                        case STSFiveCross.Left:
                            {
                                tTileAlpha = Matrix.GetTile(0, tIndex - 1);
                            }
                            break;
                        case STSFiveCross.Right:
                            {
                                tTileAlpha = Matrix.GetTile(0, ParameterOne - tIndex);
                            }
                            break;
                    }
                    float tAlpha = (Purcent * (float)ParameterOne) - (float)tIndex + 1;
                    Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                    Color tFadeColorAlpha = new Color(tColorLerp.r, tColorLerp.g, tColorLerp.b, tAlpha);
                    STSTransitionDrawing.DrawQuad(tTileAlpha.Rectangle, tFadeColorAlpha);
                }
            }
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================