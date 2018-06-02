using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSTransitionTile
    {
        //-------------------------------------------------------------------------------------------------------------
        public Rect Rectangle;
        public float Purcent;
        public float Speed;
        public float StartDelay;
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSTransitionMatrix
    {
        //-------------------------------------------------------------------------------------------------------------
        STSTransitionTile[,] Matrix;
        //-------------------------------------------------------------------------------------------------------------
        public void CreateMatrix(int sLine, int sColumn)
        {
            Matrix = new STSTransitionTile[sLine, sColumn];
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateMatrix(int sLine, int sColumn, Rect sRect)
        {
            float tX = sRect.width / sColumn;
            float tY = sRect.height / sLine;
            Matrix = new STSTransitionTile[sLine, sColumn];
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTransitionTile tTile = GetTile(i, j);
                    tTile.Rectangle = new Rect(i * tX, j * tY, tX, tY);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateMatrix(int sLine, int sColumn, Rect sRect, float sStartDelayFactor)
        {
            float tX = sRect.width / sColumn;
            float tY = sRect.height / sLine;
            Matrix = new STSTransitionTile[sLine, sColumn];
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTransitionTile tTile = GetTile(i, j);
                    tTile.Rectangle = new Rect(i * tX, j * tY, tX, tY);
                    tTile.StartDelay = (i*sColumn +j) * sStartDelayFactor;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public STSTransitionTile GetTile(int sLine, int sColumn)
        {
            return Matrix[sLine, sColumn];
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
