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
    /// STSDrawing provides various static methods for drawing shapes
    /// and lines within a Unity scene. It includes capabilities for drawing
    /// lines, circles, and Bezier curves.
    /// </summary>
    public class STSDrawing
    {
        /// <summary>
        /// A texture used for drawing anti-aliased lines.
        /// </summary>
        private static Texture2D aaLineTex = null;

        /// <summary>
        /// Draws a line between two points with specified color, width, and anti-aliasing option.
        /// </summary>
        private static Texture2D lineTex = null;

        /// <summary>
        /// A private static Material used for performing low-level pixel operations
        /// such as blitting, which is the process of transferring blocks of data from
        /// one place in memory to another.
        /// </summary>
        private static Material blitMaterial = null;

        /// <summary>
        /// A private static <see cref="Material"/> used in the <see cref="STSDrawing"/> class to handle blending operations when drawing.
        /// </summary>
        private static Material blendMaterial = null;

        /// <summary>
        /// Defines the rectangle used for drawing lines.
        /// </summary>
        private static Rect lineRect = new Rect(0, 0, 1, 1);

        /// <summary>
        /// A static Texture2D utilized by the SceneTransitionSystem for drawing operations.
        /// Initialized in the static constructor of the <see cref="STSDrawing"/> class.
        /// </summary>
        static Texture2D kTexture;

        /// <summary>
        /// A static Material instance used for rendering with a specific shader in the Scene Transition System.
        /// </summary>
        static Material tMat;

        /// <summary>
        /// The name of the shader used to create the Material in <c>STSDrawing</c>.
        /// Defaults to "UI/Default".
        /// </summary>
        static string ShaderName = "UI/Default";

        /// <summary>
        /// Provides utility functions for drawing lines, circles, and Bezier curves in Unity.
        /// </summary>
        static STSDrawing()
        {
            Initialize();
            tMat = new Material(Shader.Find(ShaderName));
            if (kTexture == null)
            {
                kTexture = new Texture2D(1, 1);
            }
        }

        /// <summary>
        /// Draws a line between two points with the specified color, width, and anti-aliasing option.
        /// </summary>
        /// <param name="pointA">The starting point of the line.</param>
        /// <param name="pointB">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="width">The width of the line.</param>
        /// <param name="antiAlias">Specifies whether the line should be anti-aliased.</param>
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
        {
#if UNITY_EDITOR
            if (!lineTex)
            {
                Initialize();
            }
#endif
            float dx = pointB.x - pointA.x;
            float dy = pointB.y - pointA.y;
            float len = Mathf.Sqrt(dx * dx + dy * dy);
            if (len < 0.001f)
            {
                return;
            }
            Texture2D tex;
            if (antiAlias)
            {
                width = width * 3.0f;
                tex = aaLineTex;
            }
            else
            {
                tex = lineTex;
            }

            float wdx = width * dy / len;
            float wdy = width * dx / len;

            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.m00 = dx;
            matrix.m01 = -wdx;
            matrix.m03 = pointA.x + 0.5f * wdx;
            matrix.m10 = dy;
            matrix.m11 = wdy;
            matrix.m13 = pointA.y - 0.5f * wdy;
            GL.Clear(true, false, Color.magenta);
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            GUI.color = color;
            GUI.DrawTexture(lineRect, tex);
            GL.PopMatrix();
        }

        /// <summary>
        /// Draws a circle with the specified parameters.
        /// </summary>
        /// <param name="center">The center position of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="width">The width of the circle outline.</param>
        /// <param name="segmentsPerQuarter">The number of segments used to draw one quarter of the circle.</param>
        public static void DrawCircle(Vector2 center, int radius, Color color, float width, int segmentsPerQuarter)
        {
            DrawCircle(center, radius, color, width, false, segmentsPerQuarter);
        }

        /// <summary>
        /// Draws a circle using Bezier curves.
        /// </summary>
        /// <param name="center">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="width">The width of the circle's line.</param>
        /// <param name="antiAlias">Determines whether to apply anti-aliasing to the circle.</param>
        /// <param name="segmentsPerQuarter">The number of segments per quarter of the circle.</param>
        public static void DrawCircle(Vector2 center, int radius, Color color, float width, bool antiAlias, int segmentsPerQuarter)
        {
            float rh = (float)radius / 2;

            Vector2 p1 = new Vector2(center.x, center.y - radius);
            Vector2 p1_tan_a = new Vector2(center.x - rh, center.y - radius);
            Vector2 p1_tan_b = new Vector2(center.x + rh, center.y - radius);

            Vector2 p2 = new Vector2(center.x + radius, center.y);
            Vector2 p2_tan_a = new Vector2(center.x + radius, center.y - rh);
            Vector2 p2_tan_b = new Vector2(center.x + radius, center.y + rh);

            Vector2 p3 = new Vector2(center.x, center.y + radius);
            Vector2 p3_tan_a = new Vector2(center.x - rh, center.y + radius);
            Vector2 p3_tan_b = new Vector2(center.x + rh, center.y + radius);

            Vector2 p4 = new Vector2(center.x - radius, center.y);
            Vector2 p4_tan_a = new Vector2(center.x - radius, center.y - rh);
            Vector2 p4_tan_b = new Vector2(center.x - radius, center.y + rh);

            DrawBezierLine(p1, p1_tan_b, p2, p2_tan_a, color, width, antiAlias, segmentsPerQuarter);
            DrawBezierLine(p2, p2_tan_b, p3, p3_tan_b, color, width, antiAlias, segmentsPerQuarter);
            DrawBezierLine(p3, p3_tan_a, p4, p4_tan_b, color, width, antiAlias, segmentsPerQuarter);
            DrawBezierLine(p4, p4_tan_a, p1, p1_tan_a, color, width, antiAlias, segmentsPerQuarter);
        }

        /// <summary>
        /// Draws a Bezier curve line between specified points with tangents.
        /// </summary>
        /// <param name="start">The starting point of the Bezier curve.</param>
        /// <param name="startTangent">Tangent vector at the starting point.</param>
        /// <param name="end">The ending point of the Bezier curve.</param>
        /// <param name="endTangent">Tangent vector at the ending point.</param>
        /// <param name="color">Color of the Bezier curve.</param>
        /// <param name="width">Width of the Bezier curve.</param>
        /// <param name="antiAlias">If set to true, the line will be anti-aliased.</param>
        /// <param name="segments">Number of segments to divide the Bezier curve into.</param>
        public static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
        {
            Vector2 lastV = CubeBezier(start, startTangent, end, endTangent, 0);
            for (int i = 1; i < segments + 1; ++i)
            {
                Vector2 v = CubeBezier(start, startTangent, end, endTangent, i / (float)segments);
                STSDrawing.DrawLine(lastV, v, color, width, antiAlias);
                lastV = v;
            }
        }


        /// <summary>
        /// Calculates the position of a point along a cubic Bezier curve given the control points and the parameter t.
        /// </summary>
        /// <param name="s">The start point of the Bezier curve.</param>
        /// <param name="st">The first control point of the Bezier curve.</param>
        /// <param name="e">The end point of the Bezier curve.</param>
        /// <param name="et">The second control point of the Bezier curve.</param>
        /// <param name="t">The parameter along the Bezier curve, typically between 0 and 1.</param>
        /// <returns>The position of the point at parameter t on the cubic Bezier curve.</returns>
        private static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
        {
            float rt = 1 - t;
            return rt * rt * rt * s + 3 * rt * rt * t * st + 3 * rt * t * t * et + t * t * t * e;
        }

        /// <summary>
        /// Initializes necessary resources like textures and materials
        /// used for drawing operations.
        /// </summary>
        /// <remarks>
        /// Should be called before any drawing operations to ensure
        /// that all resources are ready and available. This method sets up
        /// the line texture, anti-aliasing line texture, and required materials
        /// for blitting and blending.
        /// </remarks>
        private static void Initialize()
        {
            if (lineTex == null)
            {
                lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                lineTex.SetPixel(0, 1, Color.white);
                lineTex.Apply();
            }

            if (aaLineTex == null)
            {
                // TODO: better anti-aliasing of wide lines with a larger texture? or use Graphics.DrawTexture with border settings
                aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, false);
                aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
                aaLineTex.SetPixel(0, 1, Color.white);
                aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
                aaLineTex.Apply();
            }
            blitMaterial = (Material)typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            blendMaterial = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }
    }
}