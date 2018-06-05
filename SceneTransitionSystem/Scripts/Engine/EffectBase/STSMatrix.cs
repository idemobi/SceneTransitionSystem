﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSTile
    {
        //-------------------------------------------------------------------------------------------------------------
        public Rect Rectangle;
        public float Purcent;
        public float Speed;
        public float StartDelay;
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class STSMatrix
    {
        //-------------------------------------------------------------------------------------------------------------
        public STSTile[,] Matrix;
        public List<STSTile> TilesList;
        public float TileCount = 0;
        //-------------------------------------------------------------------------------------------------------------
        public void CreateMatrix(int sLine, int sColumn)
        {
            STSBenchmark.Start();
            Matrix = new STSTile[sLine, sColumn];
            TilesList = new List<STSTile>();
            TileCount = 0;
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
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateMatrix(int sLine, int sColumn, Rect sRect)
        {
            STSBenchmark.Start();
            float tX = sRect.width / sColumn;
            float tY = sRect.height / sLine;
            Matrix = new STSTile[sLine, sColumn];
            TilesList = new List<STSTile>();
            TileCount = 0;
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTile tTile = new STSTile();//GetTile(i, j);
                    tTile.Rectangle = new Rect(sRect.x+j * tX,sRect.y + i * tY, tX, tY);
                    Matrix[i, j] = tTile;
                    TilesList.Add(tTile);
                    TileCount++;
                }
            }
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateMatrix(int sLine, int sColumn, Rect sRect, float sStartDelayFactor)
        {
            STSBenchmark.Start();
            float tX = sRect.width / sColumn;
            float tY = sRect.height / sLine;
            Matrix = new STSTile[sLine, sColumn];
            TilesList = new List<STSTile>();
            TileCount = 0;
            for (int i = 0; i < sLine; i++)
            {
                for (int j = 0; j < sColumn; j++)
                {
                    STSTile tTile = new STSTile();//GetTile(i, j);
                    tTile.Rectangle = new Rect(i * tX, j * tY, tX, tY);
                    tTile.StartDelay = (i * sColumn + j) * sStartDelayFactor;
                    Matrix[i, j] = tTile;
                    TilesList.Add(tTile);
                    TileCount++;
                }
            }
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public STSTile GetTile(int sLine, int sColumn)
        {
            //Debug.Log("sLine = " + sLine +" sColumn = " + sColumn);
            //Debug.Log("Line = " + Matrix.GetLength(0) + " Column = " + Matrix.GetLength(1));
            return Matrix[sLine, sColumn];
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShuffleList()
        {
            STSBenchmark.Start();
            int tCount = TilesList.Count;
            for (int i = 0; i <tCount; i++)
            {
                STSTile tTile = TilesList[i];
                TilesList.Remove(tTile);
                TilesList.Insert(Random.Range(0, tCount - 1), tTile);
            }
            STSBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================