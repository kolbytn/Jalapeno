using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Always update the resource member name when renaming classes
public static class WorldResources
{
    public static GameObject WoodTree = Resources.Load<GameObject>("Prefabs/tree");
    public static GameObject BerryBush = Resources.Load<GameObject>("Prefabs/BerryBush");
    public static GameObject Player = Resources.Load<GameObject>("Prefabs/Player");
    public static GameObject HighlightPrefab = Resources.Load<GameObject>("Prefabs/HighlightPrefab");


    public static Tile BlankTile = Resources.Load<Tile>("Tiles/Test/1bit_small_63");
    public static Tile GrassTile = Resources.Load<Tile>("Tiles/Test/1bit_small_6");

    public static Tile WaterLeftTile = Resources.Load<Tile>("Tiles/Test/1bit_small_18");
    public static Tile WaterTopTile = Resources.Load<Tile>("Tiles/Test/1bit_small_25");
    public static Tile WaterRightTile = Resources.Load<Tile>("Tiles/Test/1bit_small_16");
    public static Tile WaterBottomTile = Resources.Load<Tile>("Tiles/Test/1bit_small_9");
    public static Tile WaterTopRightTile = Resources.Load<Tile>("Tiles/Test/1bit_small_24");
    public static Tile WaterTopLeftTile = Resources.Load<Tile>("Tiles/Test/1bit_small_26");
    public static Tile WaterBottomRightTile = Resources.Load<Tile>("Tiles/Test/1bit_small_8");
    public static Tile WaterBottomLeftTile = Resources.Load<Tile>("Tiles/Test/1bit_small_10");
    public static Tile WaterTile = Resources.Load<Tile>("Tiles/Test/1bit_small_17");

    public static Tile RockTopRightTile = Resources.Load<Tile>("Tiles/Test/1bit_small_31");
    public static Tile RockTopLeftTile = Resources.Load<Tile>("Tiles/Test/1bit_small_30");
    public static Tile RockBottomRightTile = Resources.Load<Tile>("Tiles/Test/1bit_small_39");
    public static Tile RockBottomLeftTile = Resources.Load<Tile>("Tiles/Test/1bit_small_38");
    public static Tile RockTile = Resources.Load<Tile>("Tiles/Test/1bit_small_29");


    public static GameObject GetGameObject(string name)
    {
        return (GameObject)Type.GetType("WorldResources").GetField(name)?.GetValue(null);
    }

    public static Tile GetTile(string name)
    {
        return (Tile)Type.GetType("WorldResources").GetField(name)?.GetValue(null);
    }
}
