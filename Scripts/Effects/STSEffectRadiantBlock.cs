using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a radiant block effect for scene transitions.
    /// </summary>
    [STSEffectName("Circle/Radiant Block")]
    // *** Active some parameters in inspector
    [STSTintPrimary()]
    [STSTintSecondary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    // ***
    public class STSEffectRadiantBlock : STSEffect
    {
        /// <summary>
        /// Represents an instance of the STSMatrix class used within the STSEffectRadiantBlock class.
        /// Primarily used for managing a grid of tiles and their various properties and states.
        /// </summary>
        private STSMatrix Matrix;

        /// <summary>
        /// Prepares the matrix for rendering the effect with the specified parameters.
        /// </summary>
        /// <param name="sRect">The rectangular area in which the matrix will be created.</param>
        public void Prepare(Rect sRect)
        {
            if (ParameterOne < 1)
            {
                ParameterOne = 1;
            }

            if (ParameterTwo < 1)
            {
                ParameterTwo = 1;
            }

            Matrix = new STSMatrix();
            Matrix.CreateMatrix(ParameterOne, ParameterTwo, sRect);
        }

        /// <summary>
        /// Prepares the necessary data for rendering the effect's entrance.
        /// This method is typically called before the effect starts entering.
        /// </summary>
        /// <param name="sRect">The rectangular area within which the effect will be rendered.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the effect for exiting the scene transition.
        /// Initializes and sets up necessary data required for drawing the effect.
        /// </summary>
        /// <param name="sRect">The rectangular area defining the dimensions for the effect.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the radiant block effect on the provided rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle on which the effect will be drawn.</param>
        public override void Draw(Rect sRect)
        {
            if (Purcent > 0)
            {
                float tPurcent = Purcent * 2;
                Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tPurcent * TintPrimary.a);
                Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);

                foreach (STSTile tTile in Matrix.TilesList)
                {
                    STSDrawQuad.DrawRectCenterGradient(tTile.Rectangle, tFadeColorAlpha, tColorLerp);
                }
            }
        }
    }
}