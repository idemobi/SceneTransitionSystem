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
    [STSEffectName("Close effect")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    //[STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Division Number", 1, 30)]
    [STSTwoCross()]
    // ***
    public class STSEffectClose : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        private STSMatrix Matrix;
        //-------------------------------------------------------------------------------------------------------------
        public void Prepare(Rect sRect)
        {
            //Debug.Log("STSEffectFadeLine Prepare()");
            ParameterOne = 1;
            if (ParameterTwo < 1)
            {
                ParameterTwo = 1;
            }
            Matrix = new STSMatrix();

            if (Clockwise == STSClockwise.Clockwise)
            {
                Matrix.CreateMatrix(ParameterOne, ParameterTwo, sRect);
            }
            else
            {
                Matrix.CreateMatrix(ParameterTwo,ParameterOne, sRect);
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
                float tPurcentP = Purcent / 2.0F;
                if (TwoCross == STSTwoCross.Horizontal)
                {
                    foreach (STSTile tTile in Matrix.TilesList)
                    {
                        Rect tA = new Rect(tTile.Rectangle.x, tTile.Rectangle.y, tTile.Rectangle.width * tPurcentP, tTile.Rectangle.height);
                        Rect tB = new Rect(tTile.Rectangle.x + tTile.Rectangle.width, tTile.Rectangle.y, -tTile.Rectangle.width * tPurcentP, tTile.Rectangle.height);
                        STSDrawQuad.DrawRect(tA, TintPrimary);
                        STSDrawQuad.DrawRect(tB, TintPrimary);
                    }
                }
                else
                {
                    foreach (STSTile tTile in Matrix.TilesList)
                    {
                        Rect tA = new Rect(tTile.Rectangle.x, tTile.Rectangle.y, tTile.Rectangle.width, tTile.Rectangle.height * tPurcentP);
                        Rect tB = new Rect(tTile.Rectangle.x, tTile.Rectangle.y + tTile.Rectangle.height, tTile.Rectangle.width, -tTile.Rectangle.height * tPurcentP);
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