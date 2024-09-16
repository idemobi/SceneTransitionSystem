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
    /// The STSDrawTriangle class provides static methods for drawing triangles using the Unity Graphics Library (GL).
    /// </summary>
    public class STSDrawTriangle
    {
        /// <summary>
        /// A static instance of Texture2D used within the STSDrawTriangle class.
        /// </summary>
        static Texture2D kTexture;

        /// <summary>
        /// A static Material object used by the SceneTransitionSystem to render UI elements.
        /// This material is created using the "UI/Default" shader if it has not been initialized.
        /// </summary>
        static Material kMaterialUI;

        /// <summary>
        /// Specifies the name of the shader used for UI rendering.
        /// </summary>
        static string kShaderNameUI = "UI/Default";

        /// A class that provides methods to draw triangles in a Unity UI context.
        /// /
        static STSDrawTriangle()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the necessary resources for drawing triangles, such as the texture and material.
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
        /// Draws a series of triangles defined by the given points with the specified color.
        /// </summary>
        /// <param name="sPoints">An array of Vector2 points defining the vertices of the triangles to be drawn.</param>
        /// <param name="sColor">The color to be used for drawing the triangles.</param>
        public static void DrawTriangles(Vector2[] sPoints, Color sColor)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                Initialize();
#endif
                GL.Clear(true, false, Color.magenta);
                GL.PushMatrix();
                kMaterialUI.SetPass(0);
                GL.LoadPixelMatrix();
                GL.Begin(GL.TRIANGLES);
                GL.Color(sColor);
                foreach (Vector2 tV in sPoints)
                {
                    GL.Vertex3(tV.x, tV.y, 0);
                }

                GL.End();
                GL.PopMatrix();
            }
        }

        /// <summary>
        /// Draws a triangle on the screen using the given vertices and color.
        /// Only executes during the repaint event.
        /// </summary>
        /// <param name="sA">The first vertex of the triangle.</param>
        /// <param name="sB">The second vertex of the triangle.</param>
        /// <param name="sC">The third vertex of the triangle.</param>
        /// <param name="sColor">The color to fill the triangle.</param>
        public static void DrawTriangle(Vector2 sA, Vector2 sB, Vector2 sC, Color sColor)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                Initialize();
#endif
                GL.Clear(true, false, Color.magenta);
                GL.PushMatrix();
                kMaterialUI.SetPass(0);
                GL.LoadPixelMatrix();
                GL.Begin(GL.TRIANGLES);
                GL.Color(sColor);
                GL.Vertex3(sA.x, sA.y, 0);
                GL.Vertex3(sB.x, sB.y, 0);
                GL.Vertex3(sC.x, sC.y, 0);
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}