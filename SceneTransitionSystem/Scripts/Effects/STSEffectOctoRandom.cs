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
    [STSEffectName("Octo Random")]
    // *** Remove some parameters in inspector
    [STSNoTintSecondary]
    [STSNoTexturePrimary]
    [STSNoTextureSecondary]
    //[STSNoParameterOne]
    //[STSNoParameterTwo]
    [STSNoParameterThree]
    [STSNoOffset]
    [STSNoFiveCross]
    [STSNoNineCross]
    // ***
    [STSParameterOneEntitle("Line Number", 0, 10)]
    [STSParameterTwoEntitle("Column Number", 0, 10)]
    // ***
    public class STSEffectOctoRandom : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        private STSTransitionMatrix Matrix;
        //-------------------------------------------------------------------------------------------------------------
        public void Prepare(Rect sRect)
        {
            Debug.Log("STSEffectFadeLine Prepare()");
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
            Matrix.ShuffleList();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PrepareEffectEnter(Rect sRect)
        {
            Debug.Log("STSEffectFadeLine PrepareEffectEnter()");
            // Prepare your datas to draw
            Prepare(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PrepareEffectExit(Rect sRect)
        {
            Debug.Log("STSEffectFadeLine PrepareEffectExit()");
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
                for (int i = 0; i < tIndex; i++)
                {
                    STSTransitionTile tTile = Matrix.TilesList[i];
                    //STSTransitionDrawing.DrawRect(tTile.Rectangle, TintPrimary);
                    STSTransitionDrawing.DrawCircle(tTile.Rectangle.center, tTile.Rectangle.width, 2, TintPrimary);
                }
                // Draw Alpha tile
                if (tIndex < Matrix.TileCount)
                {
                    STSTransitionTile tTileAlpha = Matrix.TilesList[tIndex];
                    float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                    //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                    //Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha*TintPrimary.a);
                    //STSTransitionDrawing.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);

                    STSTransitionDrawing.DrawCircle(tTileAlpha.Rectangle.center,tTileAlpha.Rectangle.width*tAlpha,2, TintPrimary);
                }
            }
            //STSBenchmark.Finish();
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================