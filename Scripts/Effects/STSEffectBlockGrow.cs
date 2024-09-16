using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSEffectBlockGrow class defines an effect where blocks grow in a specified pattern
    /// during a scene transition. This effect is customizable with various parameters
    /// like the number of rows and columns of blocks, the direction from which blocks will grow,
    /// and the block's primary tint color.
    /// </summary>
    [STSEffectName("Block Grow")]
    [STSTintPrimary()]
    [STSParameterOne("Line Number", 1, 30)]
    [STSParameterTwo("Column Number", 1, 30)]
    [STSNineCross("Block From")]
    [STSClockwise("Block Direction")]
    public class STSEffectBlockGrow : STSEffect
    {
        /// <summary>
        /// Represents a matrix used within the scene transition system that holds tiles
        /// and provides functionality to create, order, and shuffle its contents.
        /// </summary>
        private STSMatrix Matrix;

        /// <summary>
        /// Prepares the matrix for the block grow effect by initializing the matrix with the specified
        /// number of lines and columns, and shuffling the list of tiles.
        /// </summary>
        /// <param name="sRect">The rectangle defining the area for the matrix.</param>
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
            Matrix.ShuffleList();
        }

        /// <summary>
        /// Prepares the effect for entering, using the specified rectangular area.
        /// </summary>
        /// <param name="sRect">The rectangle representing the area to prepare the effect in.</param>
        public override void PrepareEffectEnter(Rect sRect)
        {
            Prepare(sRect);
        }

        /// <summary>
        /// Prepares the exit effect for a scene transition.
        /// </summary>
        /// <param name="sRect">The rectangle defining the area of the effect.</param>
        public override void PrepareEffectExit(Rect sRect)
        {
            Prepare(sRect);
        }

        /// <summary>
        /// Draws the transition effect on the screen based on the current transition percentage.
        /// </summary>
        /// <param name="sRect">Defines the rectangle area where the effect should be drawn.</param>
        public override void Draw(Rect sRect)
        {
            if (Purcent > 0)
            {
                //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, Purcent);
                int tIndex = (int)Mathf.Floor(Purcent * Matrix.TileCount);
                //Debug.Log("tIndex = " + tIndex + " on TileCount) = "+TileCount);
                // draw all fill tiles
                int tLine = 0;
                int tColumn = 0;
                switch (FiveCross)
                {
                    case STSFiveCross.Left:
                    {
                        for (int i = 0; i < tIndex; i++)
                        {
                            tColumn = (int)Mathf.Floor((float)i / (float)ParameterOne);
                            tLine = (int)((float)i % ((float)ParameterOne));
                            //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                            STSTile tTile = Matrix.GetTile(tLine, tColumn);
                            STSDrawQuad.DrawRect(tTile.Rectangle, TintPrimary);
                        }

                        // Draw Alpha tile
                        if (tIndex < Matrix.TileCount)
                        {
                            tColumn = (int)Mathf.Floor((float)tIndex / (float)ParameterOne);
                            tLine = (int)((float)tIndex % ((float)ParameterOne));
                            //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLineAlpha + " tColumnAlpha = " + tColumnAlpha);
                            STSTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                            float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                            //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                            Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha);
                            STSDrawQuad.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                        }
                    }
                        break;
                    case STSFiveCross.Right:
                    {
                        for (int i = 0; i < tIndex; i++)
                        {
                            tColumn = (int)Mathf.Ceil(ParameterTwo - (float)i / (float)ParameterOne) - 1;
                            tLine = (int)((float)i % ((float)ParameterOne));
                            //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                            STSTile tTile = Matrix.GetTile(tLine, tColumn);
                            STSDrawQuad.DrawRect(tTile.Rectangle, TintPrimary);
                        }

                        // Draw Alpha tile
                        if (tIndex < Matrix.TileCount)
                        {
                            tColumn = (int)Mathf.Ceil(ParameterTwo - (float)tIndex / (float)ParameterOne) - 1;
                            tLine = (int)((float)tIndex % ((float)ParameterOne));
                            //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLineAlpha + " tColumnAlpha = " + tColumnAlpha);
                            STSTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                            float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                            //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                            Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha);
                            STSDrawQuad.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                        }
                    }
                        break;
                    case STSFiveCross.Top:
                    {
                        for (int i = 0; i < tIndex; i++)
                        {
                            tLine = (int)Mathf.Floor((float)i / (float)ParameterTwo);
                            tColumn = (int)((float)i % ((float)ParameterTwo));
                            //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                            STSTile tTile = Matrix.GetTile(tLine, tColumn);
                            STSDrawQuad.DrawRect(tTile.Rectangle, TintPrimary);
                        }

                        // Draw Alpha tile
                        if (tIndex < Matrix.TileCount)
                        {
                            tLine = (int)Mathf.Floor((float)tIndex / (float)ParameterTwo);
                            tColumn = (int)((float)tIndex % ((float)ParameterTwo));
                            //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLineAlpha + " tColumnAlpha = " + tColumnAlpha);
                            STSTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                            float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                            //Color tColorLerp = Color.Lerp(TintSecondary, TintPrimary, tAlpha);
                            Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha);
                            STSDrawQuad.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                        }
                    }
                        break;
                    case STSFiveCross.Bottom:
                    {
                        for (int i = 0; i < tIndex; i++)
                        {
                            tLine = (int)Mathf.Ceil(ParameterOne - (float)i / (float)ParameterTwo) - 1;
                            tColumn = (int)((float)i % ((float)ParameterTwo));
                            //Debug.Log("index = "+i+"/"+tIndex+"/ "+TileCount+" ---> loop tLine ="+tLine +" tColumn = " +tColumn);
                            STSTile tTile = Matrix.GetTile(tLine, tColumn);
                            STSDrawQuad.DrawRect(tTile.Rectangle, TintPrimary);
                        }

                        // Draw Alpha tile
                        if (tIndex < Matrix.TileCount)
                        {
                            tLine = (int)Mathf.Ceil(ParameterOne - (float)tIndex / (float)ParameterTwo) - 1;
                            tColumn = (int)((float)tIndex % ((float)ParameterTwo));
                            //Debug.Log("index = " + tIndex + "/" + tIndex + "/ " + TileCount + " ---> loop tLineAlpha =" + tLine + " tColumnAlpha = " + tColumn);
                            STSTile tTileAlpha = Matrix.GetTile(tLine, tColumn);
                            float tAlpha = (Purcent * Matrix.TileCount) - (float)tIndex;
                            Color tFadeColorAlpha = new Color(TintPrimary.r, TintPrimary.g, TintPrimary.b, tAlpha * TintPrimary.a);
                            STSDrawQuad.DrawRect(tTileAlpha.Rectangle, tFadeColorAlpha);
                        }
                    }
                        break;
                }
            }
        }
    }
}