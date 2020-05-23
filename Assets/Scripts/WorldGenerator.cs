using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    readonly int width = 100;
    readonly int height = 100;
    readonly int tileCount = 2;
    readonly double waterProbability = .001;
    readonly double rockProbability = .001;
    //readonly double treeProbability = .005;
    readonly double bushProbability = .005;
    readonly double obstacleSizeMean = 10;
    readonly double obstacleSizeStd = 5;
    bool[,] blocked;
    WorldController worldController;

    void Start()
    {
        blocked = new bool[width, height];
        worldController = WorldController.Instance;
        worldController.GroundMap = new string[width, height];
        worldController.ObjectMap = new WorldObject[width, height];

        GenerateWorld();

        bool placed = false;
        while (!placed)
        {
            int locx = (int)(UnityEngine.Random.value * width);
            int locy = (int)(UnityEngine.Random.value * height);
            if (!blocked[locx, locy])
            {
                Vector3 location = tilemap.GetCellCenterWorld(new Vector3Int(locx, locy, 0));
                PlayerController player = Instantiate(WorldResources.PlayerController, location, Quaternion.identity).GetComponent<PlayerController>();
                worldController.ObjectMap[locx, locy] = player;
                worldController.WorldCamera.ToFollow = player.gameObject;
                player.SetGridLocation(locx, locy);
                placed = true;
            }
        }
    }

    void GenerateWorld()
    {
        for (int i = -10; i <= worldController.GroundMap.GetUpperBound(0) + 10; i++)
        {
            for (int j = -10; j <= worldController.GroundMap.GetUpperBound(1) + 10; j++)
            {
                // Set bounds
                if (i < 0 || i > worldController.GroundMap.GetUpperBound(0) || j < 0 || j > worldController.GroundMap.GetUpperBound(1))
                {
                    if (i == -1 && j > -1 && j < worldController.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterLeftTile);
                    }
                    else if (i == worldController.GroundMap.GetUpperBound(0) + 1 && j > -1 && j <= worldController.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterRightTile);
                    }
                    else if (j == -1 && i > -1 && i <= worldController.GroundMap.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterBottomTile);
                    }
                    else if (j == worldController.GroundMap.GetUpperBound(1) + 1 && i > -1 && i <= worldController.GroundMap.GetUpperBound(1))
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
                    worldController.GroundMap[i, j] = "BlankTile";
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GrassTile);
                    worldController.GroundMap[i, j] = "GrassTile";
                }

                // Add water
                if (UnityEngine.Random.value < waterProbability)
                {
                    string[] tiles = new string[] { "WaterBottomRightTile",
                                                    "WaterBottomTile", 
                                                    "WaterBottomLeftTile", 
                                                    "WaterRightTile",
                                                    "WaterTile", 
                                                    "WaterLeftTile",
                                                    "WaterTopRightTile",
                                                    "WaterTopTile",
                                                    "WaterTopLeftTile" };
                    GenerateObstacle(tiles, blocked, i, j);
                    continue;
                }

                // Add rock
                if (UnityEngine.Random.value < rockProbability)
                {
                    string[] tiles = new string[] { "RockBottomRightTile",
                                                    "RockTile",
                                                    "RockBottomLeftTile",
                                                    "RockTile",
                                                    "RockTile",
                                                    "RockTile",
                                                    "RockTopRightTile",
                                                    "RockTile",
                                                    "RockTopLeftTile" };
                    GenerateObstacle(tiles, blocked, i, j);
                    continue;
                }

                // Add tree objects
                //if (UnityEngine.Random.value < treeProbability)
                //{
                //    Vector3 location = tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
                //    GameObject treeObject = Instantiate(treePrefab, location, Quaternion.identity);
                //    blocked[i, j] = true;
                //    worldController.ObjectMap[i, j] = treeObject;
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
                    worldController.ObjectMap[i, j] = bushObject;
                    continue;
                }
                else
                {
                    worldController.ObjectMap[i, j] = null;
                }
            }
        }
    }

    void GenerateObstacle(string[] tiles, bool[,] blocked, int locationX, int locationY)
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
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[6]));
                    worldController.GroundMap[i, j] = tiles[6];
                }
                else if (i == locationX + sizeX - 1 && j == locationY)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[8]));
                    worldController.GroundMap[i, j] = tiles[8];
                }
                else if (i == locationX && j == locationY + sizeY - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[0]));
                    worldController.GroundMap[i, j] = tiles[0];
                }
                else if (i == locationX + sizeX - 1 && j == locationY + sizeY - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[2]));
                    worldController.GroundMap[i, j] = tiles[2];
                }
                else if (i == locationX)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[3]));
                    worldController.GroundMap[i, j] = tiles[3];
                }
                else if (j == locationY)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[7]));
                    worldController.GroundMap[i, j] = tiles[7];
                }
                else if (i == locationX + sizeX - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[5]));
                    worldController.GroundMap[i, j] = tiles[5];
                }
                else if (j == locationY + sizeY - 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[1]));
                    worldController.GroundMap[i, j] = tiles[1];
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[4]));
                    worldController.GroundMap[i, j] = tiles[4];
                }
                blocked[i, j] = true;
            }
        }
    }

    public GameObject GetGameObjectAt(int i, int j)
    {
        return ((MonoBehaviour)worldController.ObjectMap[i, j]).gameObject;
    }
}
