using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    public string[,] GroundMap { get; set; }

    public WorldObject[,] ObjectMap { get; set; }

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

    void Start()
    {
        saveManager = new GameObject().AddComponent<SaveManager>();
        worldGenerator = new GameObject().AddComponent<WorldGenerator>();
        WorldGrid = GameObject.Find("Grid").GetComponent<Grid>();
        WorldTilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        WorldCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        instance = this;

        worldGenerator.GenerateWorld();
    }

    public Vector3 GetCellLocation(int col, int row)
    {
        // TODO WorldController should be initialized first to avoid this
        if (WorldGrid == null)
        {
            return new Vector3(0, 0, 0);
        }
        return WorldGrid.GetCellCenterWorld(new Vector3Int(col, row, 0));
    }

    public void ChangeTile(int col, int row, Tile newTile)
    {
        WorldTilemap.SetTile(new Vector3Int(col, row, 0), newTile);
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
