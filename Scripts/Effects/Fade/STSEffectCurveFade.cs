using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a fade effect with an animation curve for scene transitions in the Scene Transition System.
    /// </summary>
    [STSEffectName("Fade/Fade curve")]
    [STSTintPrimary("Tint")]
    [STSAnimationCurve()]
    public class STSEffectCurveFade : STSEffect
    {
        /// <summary>
        /// Prepares the effect with the given rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle used for preparing the effect.</param>
        public void Prepare(Rect sRect)
        {
        }

        /// <summary>
        /// Prepares the necessary data for rendering the effect upon entering the scene transition.
        /// </summary>
        /// <param name="sRect">The rectangular region within which to prepare the effect.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the effect for its exit phase using the specified rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle defining the area for the effect.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the transition effect based on the evaluated animation curve and the provided rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle area where the effect will be drawn.</param>
        public override void Draw(Rect sRect)
        {
            // Add curve percent calculate
            CurvePurcent = Curve.Evaluate(Purcent);
            if (Purcent > 0)
            {
                // Do drawing with purcent
                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, CurvePurcent * TintPrimary.a);
                STSDrawQuad.DrawRect(sRect, tFadeColorAlpha);
            }
        }
    }
}