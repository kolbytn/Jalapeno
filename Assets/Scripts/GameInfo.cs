using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class GameInfo
{
    [Serializable]
    private struct GameInfoSerializable
    {
        public int width;
        public int height;
        public int[,] map;
        public GameObject[] objects;
    }

    private GameInfoSerializable info;

    public int[,] Map
    {
        get
        {
            return info.map;
        }
        set
        {
            info.width = value.GetUpperBound(0);
            info.height = value.GetUpperBound(1);
            info.map = value;
        }
    }

    public GameObject[] Objects
    {
        private get
        {
            return info.objects;
        }
        set
        {
            info.objects = value;
        }
    }

    private static GameInfo instance;
    public static GameInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameInfo();
            }
            return instance;
        }
    }

    public GameInfo()
    {
        info = new GameInfoSerializable();
        info.width = 0;
        info.height = 0;
        info.map = new int[0,0];
        info.objects = new GameObject[0];
    }

    public void LoadGameFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = File.ReadAllText(filePath);
        info = JsonUtility.FromJson<GameInfoSerializable>(jsonData);
    }

    public void SaveGameToFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = JsonUtility.ToJson(info, true);
        File.WriteAllText(filePath, jsonData);
    }
}
