using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a scene transition effect that applies a fade gradient.
    /// </summary>
    [STSEffectName("Fade/Fade gradient")]
    [STSTintPrimary()]
    [STSTintSecondary()]
    public class STSEffectFadeGradient : STSEffect
    {
        /// <summary>
        /// Draws a fading gradient effect within a specified rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle within which the gradient will be drawn.</param>
        public override void Draw(Rect sRect)
        {
            if (Purcent > 0)
            {
                // Do drawing with purcent
                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                Color tFadeColorAlpha = new Color(tColorLerp.r, tColorLerp.g, tColorLerp.b, Purcent * TintPrimary.a);
                STSDrawQuad.DrawRect(sRect, tFadeColorAlpha);
            }
        }
    }
}