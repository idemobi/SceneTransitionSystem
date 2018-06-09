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
    [STSEffectName("Inter Block")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    // ***
    public class STSEffectInterBlock : STSEffect
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
                float tWidthPurcent = Matrix.TilesList[0].Rectangle.width * Purcent;
                float tWidth = Matrix.TilesList[0].Rectangle.width;
                float tHeight = Matrix.TilesList[0].Rectangle.height / 2.0F;
                foreach (STSTile tTile in  Matrix.TilesList)
                {
                    Rect tA = new Rect(tTile.Rectangle.x, tTile.Rectangle.y, tWidthPurcent, tHeight);
                    Rect tB = new Rect(tTile.Rectangle.x+tWidth, tTile.Rectangle.y+tHeight, -tWidthPurcent, tHeight);
                    STSDrawQuad.DrawRect(tA, TintPrimary);
                    STSDrawQuad.DrawRect(tB, TintPrimary);
                }
            }
            //STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================