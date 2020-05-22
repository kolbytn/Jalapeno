using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Reflection;

public class GameInfo : MonoBehaviour
{
    [Serializable]
    private struct GameInfoSerializable
    {
        public int width;
        public int height;
        public ObjectInfo[] objectArray;
        public int[] groundArray;

        [Serializable]
        public struct ObjectInfo
        {
            public string type;
            public int locx;
            public int locy;
            public string info;
        }
    }

    private int width;
    private int height;

    private int[,] groundMap;
    public int[,] GroundMap
    {
        get
        {
            return groundMap;
        }
        set
        {
            width = value.GetUpperBound(0);
            height = value.GetUpperBound(1);
            groundMap = value;
        }
    }

    private IWorldObject[,] objectMap;
    public IWorldObject[,] ObjectMap
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

        IWorldObject[] objects = Utils.Flatten2dArray<IWorldObject>(objectMap);
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
        info.groundArray = Utils.Flatten2dArray<int>(groundMap);
        info.width = width;
        info.height = height;

        return JsonUtility.ToJson(info);
    }

    void InfoFromJson(string json)
    {
        foreach (IWorldObject obj in Utils.Flatten2dArray<IWorldObject>(objectMap))
        {
            if (obj != null)
            {
                Destroy(obj.GetGameObject());
            }
        }
        objectMap = new IWorldObject[width, height];

        GameInfoSerializable info = JsonUtility.FromJson<GameInfoSerializable>(json);

        objectMap = new IWorldObject[info.width, info.height];
        foreach (GameInfoSerializable.ObjectInfo obj in info.objectArray) 
        {
            Vector3 location = new Vector3Int(obj.locx, obj.locy, 0);
            GameObject prefab = WorldResources.GetGameObject(obj.type);
            GameObject gameObject = Instantiate(prefab, location, Quaternion.identity);
            Type objectType = Type.GetType(obj.type);
            MethodInfo method = typeof(GameObject).GetMethod(nameof(GameObject.GetComponent), Type.EmptyTypes);
            MethodInfo generic = method.MakeGenericMethod(objectType);
            IWorldObject worldObject = (IWorldObject)generic.Invoke(gameObject, null);
            worldObject.ObjectFromString(obj.info);
            objectMap[obj.locx, obj.locy] = worldObject;
        }

        groundMap = Utils.Reshape2dArray<int>(info.groundArray, info.width, info.height);
        width = info.width;
        height = info.height;
    }
}
