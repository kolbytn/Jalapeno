using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WorldController : MonoBehaviour
{
    public string[,] GroundMap { get; set; }

    public WorldObject[,] ObjectMap { get; set; }

    public List<Plant> PlantList {get; set;}
    public Actor[] ActorList { get; set; }

    private Player player;
    public Player Player {
        get {
            if (player == null) 
                foreach (Actor actor in ActorList) 
                    if (actor is Player) 
                        player = (Player)actor;

            return player;
        }
    }

    public int Width
    {
        get { return GroundMap.GetUpperBound(0) + 1; }
    }

    public int Height
    {
        get { return GroundMap.GetUpperBound(1) + 1; }
    }

    private static WorldController instance;
    public static WorldController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<WorldController>();
            }
            return instance;
        }
    }

    private SaveManager saveManager;
    private WorldGenerator worldGenerator;
    public Grid WorldGrid { private set; get; }
    public Tilemap WorldTilemap { private set; get; }
    public CameraController WorldCamera { private set; get; }
    public DayCycle DayCycleController { private set; get; }

    void Start()
    {
        saveManager = new GameObject().AddComponent<SaveManager>();
        worldGenerator = new GameObject().AddComponent<WorldGenerator>();
        WorldGrid = GameObject.Find("Grid").GetComponent<Grid>();
        WorldTilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        WorldCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        DayCycleController = GameObject.Find("SkyLight").GetComponent<DayCycle>();
        PlantList = new List<Plant>();
        instance = this;

        worldGenerator.GenerateWorld();
    }

    void Update()
    {
        List<int> toRemove = new List<int>();
        for(int i=0; i<PlantList.Count; i++) {
            Plant plant = PlantList[i];
            bool died = plant.UpdateGrowth();
            if (died) {
                toRemove.Add(i);
            }
        }
        foreach(int i in toRemove) {
            PlantList.RemoveAt(i);
        }
    }

    public Vector3 GetCellLocation(int col, int row)
    {
        return WorldGrid.GetCellCenterWorld(new Vector3Int(col, row, 0));
    }

    public WorldObject GetInteractableAt(int col, int row)
    {
        if (col >= 0 && row >= 0 && col < ObjectMap.GetLength(0) && row < ObjectMap.GetLength(1))
            return ObjectMap[col, row];
        return null;
    }

    public void ChangeTile(int col, int row, Tile newTile)
    {
        WorldTilemap.SetTile(new Vector3Int(col, row, 0), newTile);
    }

    public bool isOccupied(int i, int j) {
        if (i >= ObjectMap.GetLength(0) || i < 0 || j >= ObjectMap.GetLength(1) || j < 0)
            return true;
        return ObjectMap[i, j] != null;
    }

    public Plant AddPlant(GameObject obj, int i, int j) {
        if (worldGenerator.IsBlocked(i, j) || isOccupied(i, j)){
            return null;
        }
        Plant plant = (Plant)worldGenerator.AddObject(obj, i, j, false);
        plant.Init(i, j);
        WorldController.Instance.PlantList.Add(plant);
        return plant;
    }

    public void LoadGameFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string info = File.ReadAllText(filePath);
        saveManager.WorldFromString(info);
        Debug.Log(filePath);
    }

    public void SaveGameToFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string info = saveManager.WorldToString();
        File.WriteAllText(filePath, info);
        Debug.Log(filePath);
    }
}
