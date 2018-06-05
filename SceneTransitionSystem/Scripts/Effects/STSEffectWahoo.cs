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
    [STSEffectNameAttribute("Whaoo")]
    // *** Remove some parameters in inspector
    // *** Active some parameters in inspector
    //[STSTintPrimary()]
    //[STSTintSecondary()]
    //[STSNineCross("From side")]
    // ***
    public class STSEffectWhaoo : STSEffect
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
            //STSBenchmark.Start();
            if (Purcent > 0)
            {
                float tWidth = sRect.width * Purcent;
                float tHeight = sRect.height * Purcent;
                float tX = sRect.position.x + (sRect.width - tWidth) / 2.0F;
                float tY = sRect.position.y + (sRect.height - tHeight) / 2.0F;
                STSDrawing.DrawRect(new Rect(tX, tY, tWidth, tHeight), TintPrimary);
            }
            //STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================