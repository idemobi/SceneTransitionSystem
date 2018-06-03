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
    [STSEffectName("Over Slide Gradient")]
    // *** Remove some parameters in inspector
    //[STSNoTintSecondary]
    [STSNoTexturePrimary]
    [STSNoTextureSecondary]
    [STSNoParameterOne]
    [STSNoParameterTwo]
    [STSNoParameterThree]
    [STSNoOffset]
    [STSNoFiveCross]
    // ***
    public class STSEffectOverSlideGradient : STSEffect
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
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                // Do drawing with purcent
                switch (NineCross)
                {
                    case STSNineCross.BottomLeft:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x, sRect.y + sRect.height, sRect.width * Purcent, -sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.BottomRight:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x + sRect.width, sRect.y + sRect.height, -sRect.width * Purcent, -sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.TopLeft:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.TopRight:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.Right:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height), tColorLerp);
                        }
                        break;
                    case STSNineCross.Bottom:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x, sRect.y + sRect.height, sRect.width, -sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.Top:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x, sRect.y, sRect.width, sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.Center:
                        {
                            float tWidth = sRect.width * Purcent;
                            float tHeight = sRect.height * Purcent;
                            float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
                            float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
                            STSTransitionDrawing.DrawRect(new Rect(tX, tY, tWidth, tHeight), tColorLerp);
                        }
                        break;
                    default:
                    case STSNineCross.Left:
                        {
                            STSTransitionDrawing.DrawRect(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height), tColorLerp);
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