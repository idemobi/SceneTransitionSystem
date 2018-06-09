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
    [STSEffectName("Clock Block")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    [STSClockwise()]
    [STSEightCross]
    // ***
    public class STSEffectClockBlock : STSEffect
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
                float tPurcent = (Purcent * 8.0F) % 8.0F;

                float tPurcentA = tPurcent;
                float tPurcentB = tPurcent;
                float tPurcentC = tPurcent;
                float tPurcentD = tPurcent;
                float tPurcentE = tPurcent;
                float tPurcentF = tPurcent;
                float tPurcentG = tPurcent;
                float tPurcentH = tPurcent;
                if (Purcent > 0 && Purcent <= 0.125F)
                {
                    tPurcentA = Purcent * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentA =" + tPurcentA);
                }
                else if (Purcent > 0.125F && Purcent <= 0.250F)
                {
                    tPurcentA = 1.0F;
                    tPurcentB = (Purcent - 0.125F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentB =" + tPurcentB);
                }
                else if (Purcent > 0.250F && Purcent <= 0.375F)
                {
                    tPurcentA = 1.0F;
                    tPurcentB = 1.0F;
                    tPurcentC = (Purcent - 0.250F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentC =" + tPurcentC);
                }
                else if (Purcent > 0.375F && Purcent <= 0.500F)
                {
                    tPurcentA = 1.0F;
                    tPurcentB = 1.0F;
                    tPurcentC = 1.0F;
                    tPurcentD = (Purcent - 0.375F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentD =" + tPurcentD);
                }
                else if (Purcent > 0.500F && Purcent <= 0.625F)
                {
                    tPurcentA = 1.0F;
                    tPurcentB = 1.0F;
                    tPurcentC = 1.0F;
                    tPurcentD = 1.0F;
                    tPurcentE = (Purcent - 0.500F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentE =" + tPurcentE);
                }
                else if (Purcent > 0.625F && Purcent <= 0.750F)
                {
                    tPurcentA = 1.0F;
                    tPurcentB = 1.0F;
                    tPurcentC = 1.0F;
                    tPurcentD = 1.0F;
                    tPurcentE = 1.0F;
                    tPurcentF = (Purcent - 0.625F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentF =" + tPurcentF);
                }
                else if (Purcent > 0.750F && Purcent <= 0.875F)
                {
                    tPurcentA = 1.0F;
                    tPurcentB = 1.0F;
                    tPurcentC = 1.0F;
                    tPurcentD = 1.0F;
                    tPurcentE = 1.0F;
                    tPurcentF = 1.0F;
                    tPurcentG = (Purcent - 0.750F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentG =" + tPurcentG.ToString("F3"));
                }
                else
                {
                    tPurcentA = 1.0F;
                    tPurcentB = 1.0F;
                    tPurcentC = 1.0F;
                    tPurcentD = 1.0F;
                    tPurcentE = 1.0F;
                    tPurcentF = 1.0F;
                    tPurcentG = 1.0F;
                    tPurcentH = (Purcent - 0.875F) * 8F;
                    //Debug.Log("Purcent =" + Purcent.ToString("F3"));
                    //Debug.Log("tPurcentH =" + tPurcentH.ToString("F3"));
                }
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
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentA, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tEa = new Vector2(tX , tY + tHeight - tHeightHalf * tPurcentB);
                                        Vector2 tEb = new Vector2(tX , tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tFa = new Vector2(tX , tY + tHeightHalf - tHeightHalf * tPurcentC);
                                        Vector2 tFb = new Vector2(tX , tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentD, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentE, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX+ tWidth, tY + tHeightHalf * tPurcentF);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tBa = new Vector2(tX+ tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX+ tWidth, tY + tHeightHalf + tHeightHalf * tPurcentG);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tCa = new Vector2(tX+ tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX+ tWidth - tWidthHalf * tPurcentH, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentA, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentB);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentC);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentD, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentE, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentF);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentG);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentH, tY + tHeight);
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
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentA, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentB);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentC);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentD, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentE, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentF);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentG);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentH, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentA, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentB);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentC);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentD, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentE, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentF);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentG);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentH, tY);
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
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentA);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentB, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentC, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentD);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentE);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentF, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentG, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentH);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentA);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentB, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentC, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentD);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentE);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentF, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentG, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentH);
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
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentA);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentB, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentC, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentD);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentE);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentF, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentG, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentH);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentA);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentB, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentC, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentD);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentE);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentF, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentG, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentH);
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
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentA);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentB);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentC, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentD, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentE);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentF);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentG, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentH, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentA, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentB, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentC);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentD);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentE, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentF, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentG);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentH);
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
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentA, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentB, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentC);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentD);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentE, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentF, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentG);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentH);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }

                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentA);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentB);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentC, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentD, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentE);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentF);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentG, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentH, tY + tHeight);
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
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentA, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentB, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tAa = new Vector2(tX + tWidth, tY);
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentC);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentD);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentE, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentF, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentG);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentH);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentA);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentB);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentC, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentD, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentE);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentF);
                                        Vector2 tFb = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentG, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentH, tY);
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
                                        Vector2 tAb = new Vector2(tX + tWidth, tY + tHeightHalf * tPurcentA);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tBa = new Vector2(tX + tWidth, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX + tWidth, tY + tHeightHalf + tHeightHalf * tPurcentB);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tCa = new Vector2(tX + tWidth, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidth - tWidthHalf * tPurcentC, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentD, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tEa = new Vector2(tX, tY + tHeight - tHeightHalf * tPurcentE);
                                        Vector2 tEb = new Vector2(tX, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tFa = new Vector2(tX, tY + tHeightHalf - tHeightHalf * tPurcentF);
                                        Vector2 tFb = new Vector2(tX, tY + tHeightHalf);
                                        STSDrawTriangle.DrawTriangle(tFa, tFb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tGa = new Vector2(tX, tY);
                                        Vector2 tGb = new Vector2(tX + tWidthHalf * tPurcentG, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentH, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                }
                                else
                                {
                                    if (Purcent > 0)
                                    {
                                        Vector2 tGa = new Vector2(tX + tWidth, tY);
                                        Vector2 tGb = new Vector2(tX + tWidth - tWidthHalf * tPurcentA, tY);
                                        STSDrawTriangle.DrawTriangle(tGa, tGb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.125F)
                                    {
                                        Vector2 tHa = new Vector2(tX + tWidthHalf, tY);
                                        Vector2 tHb = new Vector2(tX + tWidthHalf - tWidthHalf * tPurcentB, tY);
                                        STSDrawTriangle.DrawTriangle(tHa, tHb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.250F)
                                    {
                                        Vector2 tAa = new Vector2(tX, tY);
                                        Vector2 tAb = new Vector2(tX, tY + tHeightHalf * tPurcentC);
                                        STSDrawTriangle.DrawTriangle(tAa, tAb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.375F)
                                    {
                                        Vector2 tBa = new Vector2(tX, tY + tHeightHalf);
                                        Vector2 tBb = new Vector2(tX, tY + tHeightHalf + tHeightHalf * tPurcentD);
                                        STSDrawTriangle.DrawTriangle(tBa, tBb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.500F)
                                    {
                                        Vector2 tCa = new Vector2(tX, tY + tHeight);
                                        Vector2 tCb = new Vector2(tX + tWidthHalf * tPurcentE, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tCa, tCb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.625F)
                                    {
                                        Vector2 tDa = new Vector2(tX + tWidthHalf, tY + tHeight);
                                        Vector2 tDb = new Vector2(tX + tWidthHalf + tWidthHalf * tPurcentF, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tDa, tDb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.750F)
                                    {
                                        Vector2 tEa = new Vector2(tX + tWidth, tY + tHeight - tHeightHalf * tPurcentG);
                                        Vector2 tEb = new Vector2(tX + tWidth, tY + tHeight);
                                        STSDrawTriangle.DrawTriangle(tEa, tEb, tTt, TintPrimary);
                                    }
                                    if (Purcent > 0.875F)
                                    {
                                        Vector2 tFa = new Vector2(tX + tWidth, tY + tHeightHalf - tHeightHalf * tPurcentH);
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