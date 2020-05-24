using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    readonly int width = 100;
    readonly int height = 100;
    readonly int tileCount = 2;
    readonly double waterProbability = .001;
    readonly double rockProbability = .001;
    readonly double treeProbability = .005;
    readonly double bushProbability = .005;
    readonly double obstacleSizeMean = 10;
    readonly double obstacleSizeStd = 5;
    bool[,] blocked;

    public void GenerateWorld()
    {
        blocked = new bool[width, height];
        WorldController.Instance.GroundMap = new string[width, height];
        WorldController.Instance.ObjectMap = new WorldObject[width, height];

        IterateWorld();

        bool placed = false;
        while (!placed)
        {
            int locx = (int)(UnityEngine.Random.value * width);
            int locy = (int)(UnityEngine.Random.value * height);
            if (!blocked[locx, locy])
            {
                Vector3 location = WorldController.Instance.WorldTilemap.GetCellCenterWorld(new Vector3Int(locx, locy, 0));
                PlayerController player = Instantiate(WorldResources.PlayerController, location, Quaternion.identity).GetComponent<PlayerController>();
                WorldController.Instance.ObjectMap[locx, locy] = player;
                WorldController.Instance.WorldCamera.ToFollow = player.gameObject;
                player.SetGridLocation(locx, locy);
                placed = true;
            }
        }
    }

    void IterateWorld()
    {
        for (int i = -10; i <= WorldController.Instance.GroundMap.GetUpperBound(0) + 10; i++)
        {
            for (int j = -10; j <= WorldController.Instance.GroundMap.GetUpperBound(1) + 10; j++)
            {
                // Set bounds
                if (i < 0 || i > WorldController.Instance.GroundMap.GetUpperBound(0) || j < 0 || j > WorldController.Instance.GroundMap.GetUpperBound(1))
                {
                    if (i == -1 && j > -1 && j < WorldController.Instance.GroundMap.GetUpperBound(1))
                    {
                        WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterLeftTile);
                    }
                    else if (i == WorldController.Instance.GroundMap.GetUpperBound(0) + 1 && j > -1 && j <= WorldController.Instance.GroundMap.GetUpperBound(1))
                    {
                        WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterRightTile);
                    }
                    else if (j == -1 && i > -1 && i <= WorldController.Instance.GroundMap.GetUpperBound(1))
                    {
                        WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterBottomTile);
                    }
                    else if (j == WorldController.Instance.GroundMap.GetUpperBound(1) + 1 && i > -1 && i <= WorldController.Instance.GroundMap.GetUpperBound(1))
                    {
                        WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterTopTile);
                    }
                    else
                    {
                        WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.WaterTile);
                    }
                    continue;
                }

                // Skip if location is used
                if (blocked[i, j])
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
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.BlankTile);
                    WorldController.Instance.GroundMap[i, j] = "BlankTile";
                }
                else
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GrassTile);
                    WorldController.Instance.GroundMap[i, j] = "GrassTile";
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
                if (UnityEngine.Random.value < treeProbability)
                {
                    AddObject(WorldResources.WoodTree, i, j);
                    continue;
                }
                else if (UnityEngine.Random.value < bushProbability)
                {
                    WorldObject worldObject = AddObject(WorldResources.BerryBush, i, j);
                    if (UnityEngine.Random.value < 0.5)
                    {
                        ((BerryBush)worldObject).RemoveBerries();
                    }
                    continue;
                }
            }
        }
    }

    WorldObject AddObject(GameObject obj, int i, int j)
    {
        Vector3 location = WorldController.Instance.WorldTilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));
        WorldObject worldObject = Instantiate(obj, location, Quaternion.identity).GetComponent<WorldObject>();
        blocked[i, j] = true;
        WorldController.Instance.ObjectMap[i, j] = worldObject;
        return worldObject;
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
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[6]));
                    WorldController.Instance.GroundMap[i, j] = tiles[6];
                }
                else if (i == locationX + sizeX - 1 && j == locationY)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[8]));
                    WorldController.Instance.GroundMap[i, j] = tiles[8];
                }
                else if (i == locationX && j == locationY + sizeY - 1)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[0]));
                    WorldController.Instance.GroundMap[i, j] = tiles[0];
                }
                else if (i == locationX + sizeX - 1 && j == locationY + sizeY - 1)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[2]));
                    WorldController.Instance.GroundMap[i, j] = tiles[2];
                }
                else if (i == locationX)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[3]));
                    WorldController.Instance.GroundMap[i, j] = tiles[3];
                }
                else if (j == locationY)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[7]));
                    WorldController.Instance.GroundMap[i, j] = tiles[7];
                }
                else if (i == locationX + sizeX - 1)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[5]));
                    WorldController.Instance.GroundMap[i, j] = tiles[5];
                }
                else if (j == locationY + sizeY - 1)
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[1]));
                    WorldController.Instance.GroundMap[i, j] = tiles[1];
                }
                else
                {
                    WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(tiles[4]));
                    WorldController.Instance.GroundMap[i, j] = tiles[4];
                }
                blocked[i, j] = true;
            }
        }
    }
}
