﻿using System;
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
    [STSEffectName("Fade Gradient")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSTintSecondary()]
    // ***
    public class STSEffectFadeGradient : STSEffect
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void Draw(Rect sRect)
        {
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                // Do drawing with purcent
                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                Color tFadeColorAlpha = new Color(tColorLerp.r, tColorLerp.g, tColorLerp.b, Purcent* TintPrimary.a);
                STSDrawQuad.DrawRect(sRect, tFadeColorAlpha);

                //Color tFadeColorAlphaA = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, Purcent * TintPrimary.a);

                //Color tFadeColorAlphaB = new Color(TintSecondary.r, TintSecondary.g, TintSecondary.b, Purcent * TintSecondary.a);
                //STSDrawQuad.DrawRectGradient(sRect, tFadeColorAlphaA,tFadeColorAlphaB);
            }
            //STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================