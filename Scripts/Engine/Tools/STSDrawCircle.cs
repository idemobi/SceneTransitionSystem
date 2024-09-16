using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SceneTransitionSystem
{
    /// <summary>
    /// Provides functionality to draw a circle with specified parameters.
    /// </summary>
    public class STSDrawCircle
    {
        /// <summary>
        /// A static texture used within the Scene Transition System for
        /// rendering graphical elements. The texture is initialized
        /// within the Initialize method if it is null.
        /// </summary>
        static Texture2D kTexture;

        /// <summary>
        /// A static Material instance used for rendering UI elements in the Scene Transition System.
        /// </summary>
        static Material kMaterialUI;

        /// <summary>
        /// Represents the name of the shader used for UI elements in the Scene Transition System.
        /// </summary>
        static string kShaderNameUI = "UI/Default";

        /// <summary>
        /// Provides functionality to draw a circle in a Unity scene.
        /// </summary>
        static STSDrawCircle()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the necessary resources for the STSDrawCircle class.
        /// This method ensures that the required material and texture are created and
        /// properly assigned if they do not already exist.
        /// </summary>
        static void Initialize()
        {
            if (kMaterialUI == null)
            {
                kMaterialUI = new Material(Shader.Find(kShaderNameUI));
            }

            if (kTexture == null)
            {
                kTexture = new Texture2D(1, 1);
            }
        }

        /// <summary>
        /// Draws a circle on the screen using GL drawing functions.
        /// </summary>
        /// <param name="sCenter">The center position of the circle.</param>
        /// <param name="sRadius">The radius of the circle.</param>
        /// <param name="sSegmentPerQuarter">The number of segments per quarter of the circle. Higher values result in a smoother circle.</param>
        /// <param name="sColor">The color of the circle.</param>
        public static void DrawCircle(Vector2 sCenter, float sRadius, uint sSegmentPerQuarter, Color sColor)
        {
            if (sSegmentPerQuarter < 1)
            {
                sSegmentPerQuarter = 1;
            }

            uint tTriangles = (sSegmentPerQuarter + 1) * 4 * 3;
            Vector2[] tList = new Vector2[tTriangles];
            int tCounter = 0;
            float tRadIncrement = Mathf.PI / (2.0F * (float)sSegmentPerQuarter);
            tList[tCounter++] = sCenter;
            tList[tCounter++] = new Vector2(sCenter.x + sRadius, sCenter.y);
            Vector2 tOriginalPoint = new Vector2(sCenter.x + Mathf.Cos(tRadIncrement) * sRadius, sCenter.y - Mathf.Sin(tRadIncrement) * sRadius);
            Vector2 tNextPoint = tOriginalPoint;
            tList[tCounter++] = tNextPoint;
            uint tSeg = (sSegmentPerQuarter * 4) - 1;
            for (int i = 1; i <= tSeg; i++)
            {
                float tR = tRadIncrement * i;
                tList[tCounter++] = sCenter;
                tList[tCounter++] = tNextPoint;
                tNextPoint = new Vector2(sCenter.x + Mathf.Cos(tR) * sRadius, sCenter.y - Mathf.Sin(tR) * sRadius);
                tList[tCounter++] = tNextPoint;
            }

            tList[tCounter++] = sCenter;
            tList[tCounter++] = tNextPoint;
            tList[tCounter++] = new Vector2(sCenter.x + sRadius, sCenter.y);


#if UNITY_EDITOR
            Initialize();
#endif
            GL.Clear(true, false, Color.magenta);
            GL.PushMatrix();
            kMaterialUI.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.TRIANGLES);
            GL.Color(sColor);
            foreach (Vector2 tV in tList)
            {
                GL.Vertex3(tV.x, tV.y, 0);
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}