﻿//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSDrawCircle
    {
        //-------------------------------------------------------------------------------------------------------------
        static Texture2D kTexture;
        static Material KMaterialUI;
        static string kShaderNameUI = "UI/Default";
        //-------------------------------------------------------------------------------------------------------------
        static STSDrawCircle()
        {
            Initialize();
        }
        //-------------------------------------------------------------------------------------------------------------
        static void Initialize()
        {
            if (KMaterialUI == null)
            {
                KMaterialUI = new Material(Shader.Find(kShaderNameUI));
            }
            if (kTexture == null)
            {
                kTexture = new Texture2D(1, 1);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawCircle(Vector2 sCenter, float sRadius, uint sSegmentPerQuarter, Color sColor)
        {
            if (sSegmentPerQuarter < 1)
            {
                sSegmentPerQuarter = 1;
            }
            uint tTriangles = (sSegmentPerQuarter+1) * 4 * 3;
            Vector2[] tList = new Vector2[tTriangles];
            // Create Circle points triangles around this center
            // Put in DrawTriangles methods
            int tCounter = 0;
            float tRadIncrement = Mathf.PI / (2.0F * (float)sSegmentPerQuarter);
            //Debug.Log("tRadIncrement " + tRadIncrement);
            //Debug.Log("cos " + Mathf.Cos(tRadIncrement));
            //Debug.Log("sin " + Mathf.Sin(tRadIncrement));
            // Add First Segment
            tList[tCounter++] = sCenter;
            tList[tCounter++] = new Vector2(sCenter.x +sRadius, sCenter.y );
            Vector2 tOriginalPoint= new Vector2(sCenter.x + Mathf.Cos(tRadIncrement) * sRadius, sCenter.y - Mathf.Sin(tRadIncrement) * sRadius);
            Vector2 tNextPoint = tOriginalPoint;
            tList[tCounter++] = tNextPoint;
            uint tSeg = (sSegmentPerQuarter * 4)-1;
            for (int i = 1; i <= tSeg; i++)
            {
                float tR = tRadIncrement * i;
                //Debug.Log("tRadIncrement <" +i+">"+ tR.ToString());
                //Debug.Log("cos " + Mathf.Cos(tR));
                //Debug.Log("sin " + Mathf.Sin(tR));
                // Add next Segment
                tList[tCounter++] = sCenter;
                tList[tCounter++] = tNextPoint;
                tNextPoint = new Vector2(sCenter.x + Mathf.Cos(tR) * sRadius, sCenter.y - Mathf.Sin(tR) * sRadius);
                tList[tCounter++] = tNextPoint;
            }
            tList[tCounter++] = sCenter;
            tList[tCounter++] = tNextPoint;
            tList[tCounter++] = new Vector2(sCenter.x + sRadius, sCenter.y);
            STSDrawTriangle.DrawTriangles(tList, sColor);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================