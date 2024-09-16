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
    /// Provides methods to draw various quadrilateral shapes and
    /// rectangles with gradient effects in a Unity scene.
    /// </summary>
    public class STSDrawQuad
    {
        /// <summary>
        /// Represents the texture used for drawing operations in the STSDrawQuad class.
        /// </summary>
        static Texture2D kTexture;

        /// <summary>
        /// Represents the static material used for UI rendering operations in the scene transition system.
        /// </summary>
        static Material kMaterialUI;

        /// <summary>
        /// The name of the shader used for UI rendering.
        /// </summary>
        static string kShaderNameUI = "UI/Default";

        /// <summary>
        /// The STSDrawQuad class provides methods to draw quads and rectangles with optional color gradients in Unity.
        /// </summary>
        static STSDrawQuad()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the static fields of the STSDrawQuad class.
        /// This method sets up necessary resources such as materials and textures
        /// for drawing quads in the scene transition system.
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
        /// Draws a quadrilateral on the screen with specified vertices and color.
        /// </summary>
        /// <param name="sA">The first vertex of the quadrilateral.</param>
        /// <param name="sB">The second vertex of the quadrilateral.</param>
        /// <param name="sC">The third vertex of the quadrilateral.</param>
        /// <param name="sD">The fourth vertex of the quadrilateral.</param>
        /// <param name="sColor">The color of the quadrilateral.</param>
        public static void DrawQuad(Vector2 sA, Vector2 sB, Vector2 sC, Vector2 sD, Color sColor)
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
                GL.Begin(GL.QUADS);
                GL.Color(sColor);
                GL.Vertex3(sA.x, sA.y, 0);
                GL.Vertex3(sB.x, sB.y, 0);
                GL.Vertex3(sC.x, sC.y, 0);
                GL.Vertex3(sD.x, sD.y, 0);
                GL.End();
                GL.PopMatrix();
            }
        }

        /// <summary>
        /// Draws a rectangle on the screen with the specified color.
        /// </summary>
        /// <param name="sRect">The rectangle to be drawn, defined in screen coordinates.</param>
        /// <param name="sColor">The color of the rectangle.</param>
        public static void DrawRect(Rect sRect, Color sColor)
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
                // QUADS Method
                GL.Begin(GL.QUADS);
                GL.Color(sColor);
                /*A*/
                GL.Vertex3(sRect.x, sRect.y, 0);
                /*B*/
                GL.Vertex3(sRect.x, sRect.y + sRect.height, 0);
                /*C*/
                GL.Vertex3(sRect.x + sRect.width, sRect.y + sRect.height, 0);
                /*D*/
                GL.Vertex3(sRect.x + sRect.width, sRect.y, 0);

                GL.End();
                GL.PopMatrix();
            }
        }

        /// <summary>
        /// Draws a rectangle with a gradient color effect.
        /// The gradient transitions from the first color to the second color.
        /// </summary>
        /// <param name="sRect">The rectangle to be drawn.</param>
        /// <param name="sColorA">The color at the top of the rectangle.</param>
        /// <param name="sColorB">The color at the bottom of the rectangle.</param>
        public static void DrawRectGradient(Rect sRect, Color sColorA, Color sColorB)
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
                // QUADS Method
                GL.Begin(GL.QUADS);
                GL.Color(sColorA);
                /*A*/
                GL.Vertex3(sRect.x, sRect.y, 0);
                /*B*/
                GL.Color(sColorA);
                GL.Vertex3(sRect.x, sRect.y + sRect.height, 0);
                /*C*/
                GL.Color(sColorB);
                GL.Vertex3(sRect.x + sRect.width, sRect.y + sRect.height, 0);
                /*D*/
                GL.Color(sColorB);
                GL.Vertex3(sRect.x + sRect.width, sRect.y, 0);

                GL.End();
                GL.PopMatrix();
            }
        }

        /// <summary>
        /// Draws a rectangular gradient with the gradient colors converging towards the center.
        /// </summary>
        /// <param name="sRect">The Rect structure that defines the dimensions and position of the rectangle to be drawn.</param>
        /// <param name="sColorA">The initial color to start the gradient from the corners of the rectangle.</param>
        /// <param name="sColorB">The color towards which the gradient converges at the center of the rectangle.</param>
        public static void DrawRectCenterGradient(Rect sRect, Color sColorA, Color sColorB)
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
                // QUADS Method
                GL.Begin(GL.TRIANGLES);

                GL.Color(sColorA);
                GL.Vertex3(sRect.x, sRect.y, 0);
                GL.Vertex3(sRect.x, sRect.y + sRect.height, 0);
                GL.Color(sColorB);
                GL.Vertex3(sRect.x + sRect.width / 2.0F, sRect.y + sRect.height / 2.0F, 0);

                GL.Color(sColorA);
                GL.Vertex3(sRect.x + sRect.width, sRect.y + sRect.height, 0);
                GL.Vertex3(sRect.x + sRect.width, sRect.y, 0);
                GL.Color(sColorB);
                GL.Vertex3(sRect.x + sRect.width / 2.0F, sRect.y + sRect.height / 2.0F, 0);


                GL.Color(sColorA);
                GL.Vertex3(sRect.x, sRect.y, 0);
                GL.Vertex3(sRect.x + sRect.width, sRect.y, 0);
                GL.Color(sColorB);
                GL.Vertex3(sRect.x + sRect.width / 2.0F, sRect.y + sRect.height / 2.0F, 0);

                GL.Color(sColorA);
                GL.Vertex3(sRect.x, sRect.y + sRect.height, 0);
                GL.Vertex3(sRect.x + sRect.width, sRect.y + sRect.height, 0);
                GL.Color(sColorB);
                GL.Vertex3(sRect.x + sRect.width / 2.0F, sRect.y + sRect.height / 2.0F, 0);


                GL.End();
                GL.PopMatrix();
            }
        }
    }
}