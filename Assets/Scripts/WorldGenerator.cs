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

        Debug.Log("finished");
    }

    void GenerateWorld()
    {
        Tile blank = Resources.Load<Tile>("Tiles/Test/1bit_small_63");
        Tile ground = Resources.Load<Tile>("Tiles/Test/1bit_small_6");

        int[,] map = new int[width, height];
        for (int i = 0; i < map.GetUpperBound(0); i++)
        {
            for (int j = 0; j < map.GetUpperBound(1); j++)
            {
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
                    tilemap.SetTile(new Vector3Int(i, j, 0), ground);
                }
            }
        }
    }
}
