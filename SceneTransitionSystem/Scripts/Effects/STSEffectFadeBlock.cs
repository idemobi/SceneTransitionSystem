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
    [STSEffectName("Fade Block")]
    // *** Remove some parameters in inspector
    [STSNoTintSecondary]
    [STSNoTexturePrimary]
    [STSNoTextureSecondary]
    //[STSNoParameterOne]
    //[STSNoParameterTwo]
    [STSNoParameterThree]
    [STSNoOffset]
    //[STSNoFiveCross]
    [STSNoNineCross]
    // ***
    [STSParameterOneEntitle("Line Number", 0, 10)]
    [STSParameterTwoEntitle("Column Number", 0, 10)]
    // ***
    public class STSEffectFadeBlock : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        private STSTransitionMatrix Matrix;
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
            Matrix = new STSTransitionMatrix();
            Matrix.CreateMatrix(ParameterOne, ParameterTwo, sRect);
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
                //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                int tIndex = (int)Mathf.Floor(Purcent * Matrix.TileCount);
                //Debug.Log("tIndex = " + tIndex + " on TileCount) = "+TileCount);
                // draw all fill tiles
                int tLine = 0;
                int tColumn = 0;
                switch (FiveCross)
                {
                    case STSFiveCross.Left:
                        {
                            for (int i = 0; i < tIndex; i++)
                            {
                                tColumn = (int)Mathf.Floor((float)i / (float)ParameterOne);
                                tLine = (int)((float)i % ((float)ParameterOne));
                                //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                                STSTransitionTile tTile = Matrix.GetTile(tLine, tColumn);
                                STSTransitionDrawing.DrawRect(tTile.Rectangle, TintPrimary);
                            }
                            // Draw Alpha tile
                            if (tIndex < Matrix.TileCount)
                            {
                                tColumn = (int)Mathf.Floor((float)tIndex / (float)ParameterOne);
                                tLine = (int)((float)tIndex % ((float)ParameterOne));
                                //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLineAlpha + " tColumnAlpha = " + tColumnAlpha);
                                STSTransitionTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                                float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha);
                                STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                            }
                        }
                        break;
                    case STSFiveCross.Right:
                        {
                            for (int i = 0; i < tIndex; i++)
                            {
                                tColumn = (int)Mathf.Ceil(ParameterTwo - (float)i / (float)ParameterOne) - 1;
                                tLine = (int)((float)i % ((float)ParameterOne));
                                //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                                STSTransitionTile tTile = Matrix.GetTile(tLine, tColumn);
                                STSTransitionDrawing.DrawRect(tTile.Rectangle, TintPrimary);
                            }
                            // Draw Alpha tile
                            if (tIndex < Matrix.TileCount)
                            {
                                tColumn = (int)Mathf.Ceil(ParameterTwo - (float)tIndex / (float)ParameterOne) - 1;
                                tLine = (int)((float)tIndex % ((float)ParameterOne));
                                //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLineAlpha + " tColumnAlpha = " + tColumnAlpha);
                                STSTransitionTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                                float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha);
                                STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                            }
                        }
                        break;
                    case STSFiveCross.Top:
                        {
                            for (int i = 0; i < tIndex; i++)
                            {
                                tLine = (int)Mathf.Floor((float)i / (float)ParameterTwo);
                                tColumn = (int)((float)i % ((float)ParameterTwo));
                                //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                                STSTransitionTile tTile = Matrix.GetTile(tLine, tColumn);
                                STSTransitionDrawing.DrawRect(tTile.Rectangle, TintPrimary);
                            }
                            // Draw Alpha tile
                            if (tIndex < Matrix.TileCount)
                            {
                                tLine = (int)Mathf.Floor((float)tIndex / (float)ParameterTwo);
                                tColumn = (int)((float)tIndex % ((float)ParameterTwo));
                                //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLineAlpha + " tColumnAlpha = " + tColumnAlpha);
                                STSTransitionTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                                float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha);
                                STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                            }
                        }
                        break;
                    case STSFiveCross.Bottom:
                        {
                            for (int i = 0; i < tIndex; i++)
                            {
                                tLine = (int)Mathf.Ceil(ParameterOne - (float)i / (float)ParameterTwo) - 1;
                                tColumn = (int)((float)i % ((float)ParameterTwo));
                                //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                                STSTransitionTile tTile = Matrix.GetTile(tLine, tColumn);
                                STSTransitionDrawing.DrawRect(tTile.Rectangle, TintPrimary);
                            }
                            // Draw Alpha tile
                            if (tIndex < Matrix.TileCount)
                            {
                                tLine = (int)Mathf.Ceil(ParameterOne - (float)tIndex / (float)ParameterTwo) - 1;
                                tColumn = (int)((float)tIndex % ((float)ParameterTwo));
                                //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLine + " tColumnAlpha = " + tColumn);
                                STSTransitionTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                                float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha* TintPrimary.a);
                                STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                            }
                        }
                        break;
                }
            }
            //STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================