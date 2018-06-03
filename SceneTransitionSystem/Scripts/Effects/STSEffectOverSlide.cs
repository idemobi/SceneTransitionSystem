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
            // Do drawing with purcent
            switch (FiveCross)
            {
                case STSFiveCross.Right:
                    {
                        STSTransitionDrawing.DrawQuad(new Rect(sRect.x + sRect.width, sRect.y, -sRect.width * Purcent, sRect.height), TintPrimary);
                    }
                    break;
                case STSFiveCross.Bottom:
                    {
                        STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y + sRect.height, sRect.width, -sRect.height * Purcent), TintPrimary);
                    }
                    break;
                case STSFiveCross.Top:
                    {
                        STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y, sRect.width, sRect.height * Purcent), TintPrimary);
                    }
                    break;
                case STSFiveCross.Center:
                    {
                        float tWidth = sRect.width * Purcent;
                        float tHeight = sRect.height * Purcent;
                        float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
                        float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
                        STSTransitionDrawing.DrawQuad(new Rect(tX, tY, tWidth, tHeight), TintPrimary);
                    }
                    break;
                default:
                case STSFiveCross.Left:
                    {
                        STSTransitionDrawing.DrawQuad(new Rect(sRect.x, sRect.y, sRect.width * Purcent, sRect.height), TintPrimary);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================