using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Reflection;
using UnityEngine.Tilemaps;

public class GameInfo : MonoBehaviour
{
    [Serializable]
    private struct GameInfoSerializable
    {
        public int width;
        public int height;
        public ObjectInfo[] objectArray;
        public string[] groundArray;

        [Serializable]
        public struct ObjectInfo
        {
            public string type;
            public float locx;
            public float locy;
            public string info;
        }
    }

    private int width;
    private int height;

    private string[,] groundMap;
    public string[,] GroundMap
    {
        get
        {
            return groundMap;
        }
        set
        {
            width = value.GetUpperBound(0) + 1;
            height = value.GetUpperBound(1) + 1;
            groundMap = value;
        }
    }

    private WorldObject[,] objectMap;
    public WorldObject[,] ObjectMap
    {
        get
        {
            return objectMap;
        }
        set
        {
            objectMap = value;
        }
    }

    private static GameInfo instance;
    public static GameInfo Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<GameInfo>();
                instance = gameObject.GetComponent<GameInfo>();
            }
            return instance;
        }
    }

    public Vector3 GetCellLocation(int i, int j)
    {
        // finding the grid every time we call this has got to be very inefficient
        // this is called 9 times every tick
        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();
        return grid.GetCellCenterWorld(new Vector3Int(i, j, 0));
    }

    public void LoadGameFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = File.ReadAllText(filePath);
        InfoFromJson(jsonData);
        Debug.Log(filePath);
    }

    public void SaveGameToFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = InfoToJson();
        File.WriteAllText(filePath, jsonData);
        Debug.Log(filePath);
    }

    string InfoToJson()
    {
        GameInfoSerializable info;

        WorldObject[] objects = Utils.Flatten2dArray<WorldObject>(objectMap);
        List<GameInfoSerializable.ObjectInfo> objectList = new List<GameInfoSerializable.ObjectInfo>();
        for (int i = 0; i < objects.Length; i++) 
        {
            if (objects[i] != null)
            {
                GameInfoSerializable.ObjectInfo objectInfo;
                objectInfo.type = objects[i].GetType().ToString();
                objectInfo.locx = objects[i].GetLocationX();
                objectInfo.locy = objects[i].GetLocationY();
                objectInfo.info = objects[i].ObjectToString();
                objectList.Add(objectInfo);
            }
        }

        info.objectArray = objectList.ToArray();
        info.groundArray = Utils.Flatten2dArray<string>(groundMap);
        info.width = width;
        info.height = height;

        return JsonUtility.ToJson(info);
    }

    void InfoFromJson(string json)
    {
        foreach (WorldObject obj in Utils.Flatten2dArray<WorldObject>(objectMap))
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
        objectMap = new WorldObject[width, height];

        GameInfoSerializable info = JsonUtility.FromJson<GameInfoSerializable>(json);

        objectMap = new WorldObject[info.width, info.height];
        foreach (GameInfoSerializable.ObjectInfo obj in info.objectArray) 
        {
            Vector3 location = new Vector3(obj.locx, obj.locy, 0);
            GameObject prefab = WorldResources.GetGameObject(obj.type);
            WorldObject worldObject = Instantiate(prefab, location, Quaternion.identity).GetComponent<WorldObject>();
            worldObject.ObjectFromString(obj.info);
            objectMap[(int)obj.locx, (int)obj.locy] = worldObject;

            if (obj.type == "PlayerController")
            {
                CameraController camera = GameObject.Find("MainCamera").GetComponent<CameraController>();
                camera.ToFollow = worldObject.gameObject;
            }
        }

        groundMap = Utils.Reshape2dArray<string>(info.groundArray, info.width, info.height);
        width = info.width;
        height = info.height;

        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        for (int i = 0; i <= groundMap.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= groundMap.GetUpperBound(1); j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(groundMap[i, j]));
            }
        }
    }
}
