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
    /// The STSDrawLine class provides functionality for drawing lines in Unity scenes.
    /// </summary>
    public class STSDrawLine
    {
        /// <summary>
        /// A static texture used for drawing operations within the
        /// SceneTransitionSystem. Initialized as a 1x1 Texture2D if not already set.
        /// </summary>
        static Texture2D kTexture;

        /// <summary>
        /// A static material used for drawing lines in the Scene Transition System.
        /// </summary>
        static Material kMaterial;

        /// <summary>
        /// The name of the shader used for drawing lines in the scene transition system.
        /// </summary>
        static string kShaderName = "UI/Default";

        /// <summary>
        /// Represents a utility class responsible for drawing lines on the screen.
        /// </summary>
        static STSDrawLine()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the necessary resources for drawing operations, such as the material and texture.
        /// </summary>
        /// <remarks>
        /// Ensures that the material and texture are properly created and assigned before any drawing operations are performed.
        /// This method is automatically called during the static initializer and also conditionally before drawing operations.
        /// </remarks>
        static void Initialize()
        {
            if (kMaterial == null)
            {
                kMaterial = new Material(Shader.Find(kShaderName));
            }

            if (kTexture == null)
            {
                kTexture = new Texture2D(1, 1);
            }
        }


        /// <summary>
        /// Draws multiple lines on the screen.
        /// </summary>
        /// <param name="sPoints">An array of Vector2 points defining the end points of the lines to be drawn.</param>
        /// <param name="sColor">The color of the lines.</param>
        /// <param name="sWwidth">The width of the lines.</param>
        /// <param name="sAntiAlias">A boolean indicating whether to apply anti-aliasing.</param>
        public static void DrawLines(Vector2[] sPoints, Color sColor, float sWwidth, bool sAntiAlias)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                Initialize();
#endif
                GL.Clear(true, false, Color.magenta);
                GL.PushMatrix();
                kMaterial.SetPass(0);
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
        /// Draws a line between two points on the screen.
        /// </summary>
        /// <param name="sA">The starting point of the line.</param>
        /// <param name="sB">The ending point of the line.</param>
        /// <param name="sColor">The color of the line.</param>
        /// <param name="sWidth">The width of the line.</param>
        /// <param name="sAntiAlias">Determines if anti-aliasing is applied to the line.</param>
        public static void DrawLine(Vector2 sA, Vector2 sB, Color sColor, float sWidth, bool sAntiAlias)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                Initialize();
#endif
                GL.Clear(true, false, Color.magenta);
                GL.PushMatrix();
                kMaterial.SetPass(0);
                GL.LoadPixelMatrix();
                GL.Begin(GL.LINES);
                GL.Color(sColor);
                GL.Vertex3(sA.x, sA.y, 0);
                GL.Vertex3(sB.x, sB.y, 0);
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}