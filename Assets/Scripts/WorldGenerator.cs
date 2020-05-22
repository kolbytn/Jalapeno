﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    
    int width = 100;
    int height = 100;
    int tileCount = 2;
    double waterProbability = .001;
    double rockProbability = .001;
    //double treeProbability = .005;
    double bushProbability = .005;
    double obstacleSizeMean = 10;
    double obstacleSizeStd = 5;
    GameInfo gameInfo;

    void Start()
    {
        gameInfo = GameInfo.Instance;
        gameInfo.GroundMap = new int[width, height];
        gameInfo.ObjectMap = new WorldObject[width, height];
        GenerateWorld();
    }

    public void LoadWorld(string fileName)
    {

    }

    void GenerateWorld()
    {
        bool[,] blocked = new bool[width, height];

        for (int i = -10; i < gameInfo.GroundMap.GetUpperBound(0) + 10; i++)
        {
            for (int j = -10; j < gameInfo.GroundMap.GetUpperBound(1) + 10; j++)
            {
                // Set bounds
                if (i < 0 || i >= gameInfo.GroundMap.GetUpperBound(0) || j < 0 || j >= gameInfo.GroundMap.GetUpperBound(1))
                {
                    if (i == -1 && j > -1 && j < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterLeftTile);
                    }
                    else if (i == gameInfo.GroundMap.GetUpperBound(0) && j > -1 && j < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterRightTile);
                    }
                    else if (j == -1 && i > -1 && i < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterBottomTile);
                    }
                    else if (j == gameInfo.GroundMap.GetUpperBound(1) && i > -1 && i < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterTopTile);
                    }
                    else
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterTile);
                    }
                    continue;
                }

                // Skip if location is used
                if (blocked[i,j])
                {
                    continue;
                }

                // Set ground
                int tileIndex = (int)Math.Floor(UnityEngine.Random.value * tileCount);
                if (tileIndex == tileCount)
                {
                    tileIndex--;
                }
                if (tileIndex == 0)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.BlankTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GrassTile);
                }

                // Add water
                if (UnityEngine.Random.value < waterProbability)
                {
                    Tile[] tiles = new Tile[] { WorldResources.WaterBottomRightTile,
                                                WorldResources.WaterBottomTile, 
                                                WorldResources.WaterBottomLeftTile, 
                                                WorldResources.WaterRightTile,
                                                WorldResources.WaterTile, 
                                                WorldResources.WaterLeftTile,
                                                WorldResources.WaterTopRightTile,
                                                WorldResources.WaterTopTile,
                                                WorldResources.WaterTopLeftTile };
                    GenerateObstacle(tiles, blocked, i, j);
                    continue;
                }

                // Add rock
                if (UnityEngine.Random.value < rockProbability)
                {
                    Tile[] tiles = new Tile[] { WorldResources.RockBottomRightTile,
                                                WorldResources.RockTile,
                                                WorldResources.RockBottomLeftTile,
                                                WorldResources.RockTile,
                                                WorldResources.RockTile,
                                                WorldResources.RockTile,
                                                WorldResources.RockTopRightTile,
                                                WorldResources.RockTile,
                                                WorldResources.RockTopLeftTile };
                    GenerateObstacle(tiles, blocked, i, j);
                    continue;
                }

                // Add tree objects
                //if (UnityEngine.Random.value < treeProbability)
                //{
                //    Vector3 location = tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
                //    GameObject treeObject = Instantiate(treePrefab, location, Quaternion.identity);
                //    blocked[i, j] = true;
                //    gameInfo.ObjectMap[i, j] = treeObject;
                //    continue;
                //}
                else if (UnityEngine.Random.value < bushProbability)
                {
                    Vector3 location = tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
                    BerryBush bushObject = Instantiate(WorldResources.BerryBush, location, Quaternion.identity).GetComponent<BerryBush>();
                    blocked[i, j] = true;
                    if (UnityEngine.Random.value < 0.5)
                    {
                        bushObject.RemoveBerries();
                    }
                    gameInfo.ObjectMap[i, j] = bushObject;
                    continue;
                }
                else
                {
                    gameInfo.ObjectMap[i, j] = null;
                }
            }
        }
    }

    void GenerateObstacle(Tile[] tiles, bool[,] blocked, int locationX, int locationY)
    {
        int sizeX = (int)Math.Floor(Utils.SampleNormal(obstacleSizeMean, obstacleSizeStd));
        int sizeY = (int)Math.Floor(Utils.SampleNormal(obstacleSizeMean, obstacleSizeStd));

        sizeX = locationX + sizeX < blocked.GetUpperBound(0) ? sizeX : blocked.GetUpperBound(0) - locationX;
        sizeY = locationY + sizeY < blocked.GetUpperBound(1) ? sizeY : blocked.GetUpperBound(1) - locationY;

        for (int i = locationX; i < locationX + sizeX; i++)
        {
            for (int j = locationY; j < locationY + sizeY; j++)
            {
                if (i == locationX && j == locationY)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[6]);
                }
                else if (i == locationX + sizeX - 1 && j == locationY)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[8]);
                }
                else if (i == locationX && j == locationY + sizeY - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[0]);
                }
                else if (i == locationX + sizeX - 1 && j == locationY + sizeY - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[2]);
                }
                else if (i == locationX)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[3]);
                }
                else if (j == locationY)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[7]);
                }
                else if (i == locationX + sizeX - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[5]);
                }
                else if (j == locationY + sizeY - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[1]);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tiles[4]);
                }
                blocked[i, j] = true;
            }
        }
    }

    public GameObject GetGameObjectAt(int i, int j)
    {
        return ((MonoBehaviour)gameInfo.ObjectMap[i, j]).gameObject;
    }

    public Vector3 GetCellLocation(int i, int j)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
    }
}
