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
    [STSEffectName("Over Slide")]
    // *** Remove some parameters in inspector
    [STSNoTintSecondary]
    [STSNoTexturePrimary]
    [STSNoTextureSecondary]
    [STSNoParameterOne]
    [STSNoParameterTwo]
    [STSNoParameterThree]
    [STSNoOffset]
    [STSNoFiveCross]
    // ***
    public class STSEffectOverSlide : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        public void Prepare(Rect sRect)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PrepareEffectExit(Rect sRect)
        {
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
                // Do drawing with purcent
                switch (NineCross)
                {
                    case STSNineCross.BottomLeft:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y + sRect.height, sRect.width * Purcent, -sRect.height * Purcent), TintPrimary);
                        }
                        break;
                    case STSNineCross.BottomRight:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x + sRect.width, sRect.y + sRect.height, -sRect.width * Purcent, -sRect.height * Purcent), TintPrimary);
                        }
                        break;
                    case STSNineCross.TopLeft:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height * Purcent), TintPrimary);
                        }
                        break;
                    case STSNineCross.TopRight:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height * Purcent), TintPrimary);
                        }
                        break;
                    case STSNineCross.Right:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height), TintPrimary);
                        }
                        break;
                    case STSNineCross.Bottom:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y + sRect.height, sRect.width, -sRect.height * Purcent), TintPrimary);
                        }
                        break;
                    case STSNineCross.Top:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y, sRect.width, sRect.height * Purcent), TintPrimary);
                        }
                        break;
                    case STSNineCross.Center:
                        {
                            float tWidth = sRect.width * Purcent;
                            float tHeight = sRect.height * Purcent;
                            float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
                            float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
                            STSTransitionDrawing.DrawQuad(new Rect(tX, tY, tWidth, tHeight), TintPrimary);
                        }
                        break;
                    default:
                    case STSNineCross.Left:
                        {
                            STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height), TintPrimary);
                        }
                        break;
                }
            }
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================