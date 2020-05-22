using System;
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
        gameInfo.ObjectMap = new IWorldObject[width, height];
        GenerateWorld();
    }

    public void LoadWorld(string fileName)
    {

    }

    void GenerateWorld()
    {
        GameObject treePrefab = Resources.Load<GameObject>("Prefabs/tree");
        GameObject bushPrefab = Resources.Load<GameObject>("Prefabs/BerryBush");

        Tile blank = Resources.Load<Tile>("Tiles/Test/1bit_small_63");
        Tile grass = Resources.Load<Tile>("Tiles/Test/1bit_small_6");

        Tile water_left = Resources.Load<Tile>("Tiles/Test/1bit_small_18");
        Tile water_top = Resources.Load<Tile>("Tiles/Test/1bit_small_25");
        Tile water_right = Resources.Load<Tile>("Tiles/Test/1bit_small_16");
        Tile water_bottom = Resources.Load<Tile>("Tiles/Test/1bit_small_9");
        Tile water_top_right = Resources.Load<Tile>("Tiles/Test/1bit_small_24");
        Tile water_top_left = Resources.Load<Tile>("Tiles/Test/1bit_small_26");
        Tile water_bottom_right = Resources.Load<Tile>("Tiles/Test/1bit_small_8");
        Tile water_bottom_left = Resources.Load<Tile>("Tiles/Test/1bit_small_10");
        Tile water = Resources.Load<Tile>("Tiles/Test/1bit_small_17");

        Tile rock_top_right = Resources.Load<Tile>("Tiles/Test/1bit_small_31");
        Tile rock_top_left = Resources.Load<Tile>("Tiles/Test/1bit_small_30");
        Tile rock_bottom_right = Resources.Load<Tile>("Tiles/Test/1bit_small_39");
        Tile rock_bottom_left = Resources.Load<Tile>("Tiles/Test/1bit_small_38");
        Tile rock = Resources.Load<Tile>("Tiles/Test/1bit_small_29");

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
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_left);
                    }
                    else if (i == gameInfo.GroundMap.GetUpperBound(0) && j > -1 && j < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_right);
                    }
                    else if (j == -1 && i > -1 && i < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_bottom);
                    }
                    else if (j == gameInfo.GroundMap.GetUpperBound(1) && i > -1 && i < gameInfo.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_top);
                    }
                    else
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water);
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
                    tilemap.SetTile(new Vector3Int(i, j, 0), blank);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), grass);
                }

                // Add water
                if (UnityEngine.Random.value < waterProbability)
                {
                    Tile[] tiles = new Tile[] { water_bottom_right, water_bottom, water_bottom_left, water_right, water, water_left, water_top_right, water_top, water_top_left };
                    GenerateObstacle(tiles, blocked, i, j);
                    continue;
                }

                // Add rock
                if (UnityEngine.Random.value < rockProbability)
                {
                    Tile[] tiles = new Tile[] { rock_bottom_right, rock, rock_bottom_left, rock, rock, rock, rock_top_right, rock, rock_top_left };
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
                    BerryBush bushObject = Instantiate(bushPrefab, location, Quaternion.identity).GetComponent<BerryBush>();
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
