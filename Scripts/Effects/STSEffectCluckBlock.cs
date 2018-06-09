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
    [STSEffectName("Cluck Block")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    [STSClockwise()]
    [STSEightCross]
    // ***
    public class STSEffectCluckBlock : STSEffect
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
            //Debug.Log("STSEffectFadeLine Draw()");
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                float tWidth = Matrix.TilesList[0].Rectangle.width;
                float tHeight = Matrix.TilesList[0].Rectangle.height;
                float tWidthHalf = tWidth / 2.0F;
                float tHeightHalf = tHeight / 2.0F;
                foreach (STSTile tTile in Matrix.TilesList)
                {
                    float tX = tTile.Rectangle.x;
                    float tY = tTile.Rectangle.y;
                    Vector2 tTt = new Vector2(tX + tWidthHalf, tY + tHeightHalf);
                    switch (EightCross)
                    {
                        case STSEightCross.Bottom:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tEa = new Vector2(tX , tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX , tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tFa = new Vector2(tX , tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX , tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX+ tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tBa = new Vector2(tX+ tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX+ tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tCa = new Vector2(tX+ tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX+ tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.Top:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.Left:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.Right:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.BottomLeft:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.BottomRight:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {

                                    if (Purcent > 0)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }

                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.TopLeft:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
                        case STSEightCross.TopRight:
                            {
                                if (Clockwise == STSClockwise.Clockwise)
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * Purcent, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * Purcent);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * Purcent, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * Purcent);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * Purcent);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                }
                            }
                            break;
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