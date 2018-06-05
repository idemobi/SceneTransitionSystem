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
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSTintSecondary()]
    [STSNineCross("From side")]
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
                            STSDrawQuad.DrawRect(new Rect(sRect.x, sRect.y + sRect.height, sRect.width * Purcent, -sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.BottomRight:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x + sRect.width, sRect.y + sRect.height, -sRect.width * Purcent, -sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.TopLeft:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.TopRight:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.Right:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height), tColorLerp);
                        }
                        break;
                    case STSNineCross.Bottom:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x, sRect.y + sRect.height, sRect.width, -sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.Top:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x, sRect.y, sRect.width, sRect.height * Purcent), tColorLerp);
                        }
                        break;
                    case STSNineCross.Center:
                        {
                            float tWidth = sRect.width * Purcent;
                            float tHeight = sRect.height * Purcent;
                            float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
                            float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
                            STSDrawQuad.DrawRect(new Rect(tX, tY, tWidth, tHeight), tColorLerp);
                        }
                        break;
                    default:
                    case STSNineCross.Left:
                        {
                            STSDrawQuad.DrawRect(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height), tColorLerp);
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