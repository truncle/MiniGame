﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ELGame
{
    public class Battle
    {
        private static int mapCount = 0;

        //地图信息
        public int mapID;   
        //地图宽高
        public int mapWidth = 0;
        public int mapHeight = 0;

        //地图格子信息
        public Grid[,] mapGrids; 
        List<Grid> normalGrids;
        List<Grid> obstacleGrids;
        
        //战场中铺设格子（信息）
        public void Generate(int width, int height, int obstacle, int gap)
        {
            if (width <= 0 || height <= 0)
                return;

            //地图编号自增
            mapID = mapCount++;
            //记录地图宽高
            mapWidth = width;
            mapHeight = height;
            //生成格子数组
            mapGrids = new Grid[mapWidth, mapHeight];
            //记录普通格子和障碍格子
            normalGrids = new List<Grid>();
            obstacleGrids = new List<Grid>();

            //全部生成为普通格子
            for (int r = 0; r < mapHeight; ++r)
            {
                for (int c = 0; c < mapWidth; ++c)
                {
                    Grid gird = new Grid();
                    gird.localPosition = new Vector3(
                        c * EGameConstL.GridWidth + ((r & 1) > 0 ? (EGameConstL.GridWidth * 0.5f) : 0f),
                        -r * EGameConstL.GridOffsetY,
                        0
                        );

                    //设置格子参数
                    gird.gridPosition = new Vector2Int(r, c);     //位置
                    //初始设置为普通格子
                    SetGridType(gird, GridType.Normal);
                    //保存
                    mapGrids[c, r] = gird;
                }
            }

            //随机一些障碍格子
            DisposeGridUnits(obstacle, gap);
        }

        //设置格子类型
        private void SetGridType(Grid gud, GridType gt)
        {
            switch (gt)
            {
                case GridType.Normal:
                    normalGrids.Add(gud);
                    break;
                case GridType.Obstacle:
                    obstacleGrids.Add(gud);
                    break;
                default:
                    break;
            }
            gud.gridType = gt;
        }

        //放置一些障碍格子
        private void DisposeGridUnits(int obstacle, int gap)
        {
            obstacle = Mathf.Min(mapWidth * mapHeight, obstacle);

            for (int i = 0; i < obstacle; ++i)
            {
                int randomIdx = -1;
                Grid target = null;
                int tryTimes = 999;
                while (tryTimes > 0 && target == null)
                {
                    randomIdx = Random.Range(0, normalGrids.Count);
                    target = normalGrids[randomIdx];
                    //判断距离
                    for (int j = 0; j < obstacleGrids.Count; ++j)
                    {
                        var distance = obstacleGrids[j].Distance(target);
                        if (obstacleGrids[j].Distance(target) < gap)
                        {
                            target = null;
                            break;
                        }
                    }
                    --tryTimes;
                }
                if (target != null)
                {
                    SetGridType(target, GridType.Obstacle);
                    normalGrids.RemoveAt(randomIdx);
                }
                else
                {
                    EUtilityHelperL.LogWarning("Dispose grid unit data warning.");
                }
            }
        }
    }
}