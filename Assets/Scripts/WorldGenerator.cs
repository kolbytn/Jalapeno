using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public int width = 20;
    public int height = 20;

    int tileCount = 2;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        Tile blank = Resources.Load<Tile>("Tiles/Test/1bit_small_63");
        Tile grass = Resources.Load<Tile>("Tiles/Test/1bit_small_6");
        Tile water_left = Resources.Load<Tile>("Tiles/Test/1bit_small_18");
        Tile water_top = Resources.Load<Tile>("Tiles/Test/1bit_small_25");
        Tile water_right = Resources.Load<Tile>("Tiles/Test/1bit_small_16");
        Tile water_bottom = Resources.Load<Tile>("Tiles/Test/1bit_small_9");
        Tile water = Resources.Load<Tile>("Tiles/Test/1bit_small_17");

        int[,] map = new int[width, height];
        for (int i = -10; i < map.GetUpperBound(0) + 10; i++)
        {
            for (int j = -10; j < map.GetUpperBound(1) + 10; j++)
            {

                // Set bounds
                if (i < 0 || i >= map.GetUpperBound(0) || j < 0 || j >= map.GetUpperBound(1))
                {
                    if (i == -1 && j > -1 && j < map.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_left);
                    }
                    else if (i == map.GetUpperBound(0) && j > -1 && j < map.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_right);
                    }
                    else if (j == -1 && i > -1 && i < map.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_bottom);
                    }
                    else if (j == map.GetUpperBound(1) && i > -1 && i < map.GetUpperBound(1))
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water_top);
                    }
                    else
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), water);
                    }
                    continue;
                }

                // Get random index
                int tileIndex = (int)Math.Floor(UnityEngine.Random.value * tileCount);
                if (tileIndex == tileCount)
                {
                    tileIndex--;
                }

                // Set ground
                if (tileIndex == 0)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), blank);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), grass);
                }
            }
        }
    }
}
