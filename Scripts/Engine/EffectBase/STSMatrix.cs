using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents a tile in the scene transition system.
    /// </summary>
    public class STSTile
    {
        /// <summary>
        /// Defines a rectangular area within the tile, represented as a UnityEngine.Rect structure.
        /// This property is used to set or get the dimensions and position of the rectangle
        /// associated with the STSTile object, enabling operations like drawing or transforming
        /// rectangular bounds within various scene transition effects.
        /// </summary>
        public Rect Rectangle;

        /// <summary>
        /// Represents the percentage value associated with the STSTile object.
        /// This value is used to determine the proportion or completion level
        /// of the tile's transition or effect within the scene transition system.
        /// </summary>
        public float Purcent;

        /// <summary>
        /// Represents the speed at which a tile in the scene transition system moves.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Specifies the delay before starting the transition for a specific tile.
        /// The delay is calculated based on the tile's position within the matrix
        /// and a given start delay factor, which ensures staggered transitions.
        /// </summary>
        public float StartDelay;
    }

    /// <summary>
    /// The STSMatrix class provides functionality to manage a matrix structure
    /// composed of STSTile objects. It allows initialization of the matrix
    /// with different configurations, retrieval of tiles, and manipulation of
    /// an internal list of tiles for various purposes.
    /// </summary>
    public class STSMatrix
    {
        /// <summary>
        /// A 2D array representing a matrix of STSTile objects.
        /// Used within the Scene Transition System to manage and manipulate the layout of tiles
        /// for transitions and animations.
        /// </summary>
        public STSTile[,] Matrix;

        /// <summary>
        /// A list containing instances of <c>STSTile</c> used in various effects to perform
        /// operations on tiles within the scene transition system matrix.
        /// </summary>
        public List<STSTile> TilesList;

        /// <summary>
        /// Represents the total count of tiles within the matrix of scene transitions.
        /// This value is used extensively within various effect classes to determine
        /// the number of tiles to be drawn or manipulated based on a percentage.
        /// </summary>
        public float TileCount = 0;

        /// <summary>
        /// Represents the number of lines in the STSMatrix. This variable is primarily used to define the dimensions
        /// of the matrix and is set during the creation of the matrix.
        /// </summary>
        public int Line;

        /// <summary>
        /// Represents the number of columns in the STSMatrix.
        /// </summary>
        public int Column;

        /// <summary>
        /// Creates a matrix of STSTile objects with specified dimensions.
        /// </summary>
        /// <param name="sLine">The number of lines (rows) in the matrix.</param>
        /// <param name="sColumn">The number of columns in the matrix.</param>
        public void CreateMatrix(int sLine, int sColumn)
        {
            //STSBenchmark.Start();
            Matrix = new STSTile[sLine, sColumn];
            TilesList = new List<STSTile>();
            TileCount = 0;
            Line = sLine;
            Column = sColumn;
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTile tTile = new STSTile();
                    Matrix[i, j] = tTile;
                    TilesList.Add(tTile);
                    TileCount++;
                }
            }
            //STSBenchmark.Finish();
        }

        /// Creates a matrix of tiles based on the specified number of lines and columns,
        /// and positions the tiles within the given rectangle.
        /// <param name="sLine">Number of lines in the matrix.</param>
        /// <param name="sColumn">Number of columns in the matrix.</param>
        /// <param name="sRect">Rectangle defining the area in which to place the tiles.</param>
        public void CreateMatrix(int sLine, int sColumn, Rect sRect)
        {
            //STSBenchmark.Start();
            float tX = sRect.width / sColumn;
            float tY = sRect.height / sLine;
            Matrix = new STSTile[sLine, sColumn];
            TilesList = new List<STSTile>();
            TileCount = 0;
            Line = sLine;
            Column = sColumn;
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTile tTile = new STSTile(); //GetTile(i, j);
                    tTile.Rectangle = new Rect(sRect.x + j * tX, sRect.y + i * tY, tX, tY);
                    Matrix[i, j] = tTile;
                    TilesList.Add(tTile);
                    TileCount++;
                }
            }
            //STSBenchmark.Finish();
        }

        /// <summary>
        /// Creates a matrix of STSTile objects with specified number of lines and columns.
        /// </summary>
        /// <param name="sLine">The number of lines in the matrix.</param>
        /// <param name="sColumn">The number of columns in the matrix.</param>
        /// <param name="sRect">The rectangular area within which the matrix will be created.</param>
        /// <param name="sStartDelayFactor">The factor for calculating the start delay for each tile.</param>
        public void CreateMatrix(int sLine, int sColumn, Rect sRect, float sStartDelayFactor)
        {
            //STSBenchmark.Start();
            float tX = sRect.width / sColumn;
            float tY = sRect.height / sLine;
            Matrix = new STSTile[sLine, sColumn];
            TilesList = new List<STSTile>();
            TileCount = 0;
            Line = sLine;
            Column = sColumn;
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTile tTile = new STSTile(); //GetTile(i, j);
                    tTile.Rectangle = new Rect(i * tX, j * tY, tX, tY);
                    tTile.StartDelay = (i * sColumn + j) * sStartDelayFactor;
                    Matrix[i, j] = tTile;
                    TilesList.Add(tTile);
                    TileCount++;
                }
            }
            //STSBenchmark.Finish();
        }

        /// <summary>
        /// Retrieves a tile from the matrix at the specified line and column indices.
        /// </summary>
        /// <param name="sLine">The line index of the tile to retrieve.</param>
        /// <param name="sColumn">The column index of the tile to retrieve.</param>
        /// <returns>The tile at the specified line and column indices.</returns>
        public STSTile GetTile(int sLine, int sColumn)
        {
            //Debug.Log("sLine = " + sLine +" sColumn = " + sColumn);
            //Debug.Log("Line = " + Matrix.GetLength(0) + " Column = " + Matrix.GetLength(1));
            return Matrix[sLine, sColumn];
        }

        /// Shuffles the elements in the TilesList randomly.
        /// This method randomly rearranges the elements in the `TilesList`.
        /// It removes each tile and re-inserts it at a random position
        /// within the list. This operation can help in creating varied
        /// scene transition effects where the order of tiles changes
        /// unpredictably.
        public void ShuffleList()
        {
            //STSBenchmark.Start();
            int tCount = TilesList.Count;
            for (int i = 0; i < tCount; i++)
            {
                STSTile tTile = TilesList[i];
                TilesList.Remove(tTile);
                TilesList.Insert(Random.Range(0, tCount - 1), tTile);
            }
            //STSBenchmark.Finish();
        }

        /// <summary>
        /// Orders the list of STSTile objects based on the specified direction and clockwise settings.
        /// </summary>
        /// <param name="sDirection">The direction specified as one of the STSFourCross values.</param>
        /// <param name="sClosckwise">The clockwise orientation specified as one of the STSClockwise values.</param>
        public void OrderList(STSFourCross sDirection, STSClockwise sClosckwise)
        {
            //STSBenchmark.Start();
            switch (sDirection)
            {
                case STSFourCross.Bottom:
                    OrderList(STSNineCross.Bottom, sClosckwise);
                    break;
                case STSFourCross.Top:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSFourCross.Left:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSFourCross.Right:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
            }
            //STSBenchmark.Finish();
        }

        /// <summary>
        /// Orders the list based on the specified five-cross direction and clockwise setting.
        /// </summary>
        /// <param name="sDirection">The direction from the STSFiveCross enumeration specifying Top, Bottom, Right, Left, or Center.</param>
        /// <param name="sClosckwise">The clockwise setting from the STSClockwise enumeration specifying Clockwise or Counterclockwise.</param>
        public void OrderList(STSFiveCross sDirection, STSClockwise sClosckwise)
        {
            //STSBenchmark.Start();
            switch (sDirection)
            {
                case STSFiveCross.Bottom:
                    OrderList(STSNineCross.Bottom, sClosckwise);
                    break;
                case STSFiveCross.Top:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSFiveCross.Left:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSFiveCross.Right:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSFiveCross.Center:
                    OrderList(STSNineCross.Center, sClosckwise);
                    break;
            }
            //STSBenchmark.Finish();
        }

        /// <summary>
        /// Orders the list based on the specified direction and clockwise settings using the STSEightCross enumeration.
        /// </summary>
        /// <param name="sDirection">The direction to order the list, specified by the STSEightCross enumeration.</param>
        /// <param name="sClosckwise">The clockwise setting, specified by the STSClockwise enumeration.</param>
        public void OrderList(STSEightCross sDirection, STSClockwise sClosckwise)
        {
            //STSBenchmark.Start();
            switch (sDirection)
            {
                case STSEightCross.Bottom:
                    OrderList(STSNineCross.Bottom, sClosckwise);
                    break;
                case STSEightCross.Top:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSEightCross.Left:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSEightCross.Right:
                    OrderList(STSNineCross.Top, sClosckwise);
                    break;
                case STSEightCross.TopLeft:
                    OrderList(STSNineCross.TopLeft, sClosckwise);
                    break;
                case STSEightCross.TopRight:
                    OrderList(STSNineCross.TopRight, sClosckwise);
                    break;
                case STSEightCross.BottomLeft:
                    OrderList(STSNineCross.BottomLeft, sClosckwise);
                    break;
                case STSEightCross.BottomRight:
                    OrderList(STSNineCross.BottomRight, sClosckwise);
                    break;
            }
            //STSBenchmark.Finish();
        }


        /// <summary>
        /// Orders the tiles in a list based on the specified direction and clockwise orientation.
        /// </summary>
        /// <param name="sDirection">The direction in which to order the tiles.</param>
        /// <param name="sClockwise">The orientation in which to order the tiles, either clockwise or counterclockwise.</param>
        public void OrderList(STSNineCross sDirection, STSClockwise sClockwise)
        {
            //STSBenchmark.Start();
            TilesList = new List<STSTile>();
            switch (sDirection)
            {
                case STSNineCross.Bottom:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.Top:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.Left:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.Right:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.Center:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.TopLeft:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.TopRight:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.BottomLeft:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
                case STSNineCross.BottomRight:
                    if (sClockwise == STSClockwise.Clockwise)
                    {
                    }
                    else
                    {
                    }

                    break;
            }
            //STSBenchmark.Finish();
        }
    }
}