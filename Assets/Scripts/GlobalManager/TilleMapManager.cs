using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager Instance;
    public Dictionary<Tilemap, RuleTile> tileDictionary;
    public Dictionary<Vector3Int, bool> tileCellDictionary;
    public Dictionary<Vector3Int, Plant> tilePlantDictionary;
    public Tilemap tilemap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        tileCellDictionary = new Dictionary<Vector3Int, bool>();
        tilePlantDictionary = new Dictionary<Vector3Int, Plant>();
    }

    private void Start()
    {
        tileDictionary = new Dictionary<Tilemap, RuleTile>();
        foreach(Transform transform in this.transform)
        {
            Tile tile =  transform.GetComponent<Tile>();
            if (tile != null)
            {
                RuleTile ruleTile = tile.ruleTile;
                Tilemap tilemap = tile.GetComponent<Tilemap>();
                tileDictionary[tilemap] = ruleTile;
            }
        }
    }

    public void SetTile(Tilemap tilemap, Vector3Int tilePosition)
    {
        tileDictionary.TryGetValue(tilemap, out RuleTile ruleTile);
        if (ruleTile != null)
        {
            tilemap.SetTile(tilePosition, ruleTile);
            tileCellDictionary[tilePosition] = false;
        }
    }

    public void DeleteTile(Tilemap tilemap, Vector3Int tilePosition)
    {
        tileDictionary.TryGetValue(tilemap, out RuleTile ruleTile);
        if (ruleTile != null)
        {
            tilemap.SetTile(tilePosition, null);
            tileCellDictionary.Remove(tilePosition);
        }
    }


    public List<TileSaveData> GetTileData()
    {
        List<TileSaveData> tileData = new List<TileSaveData>();

        foreach (var tile in tileCellDictionary)
        {
            tileData.Add(new TileSaveData { tilePosition = tile.Key, tilePlant = tile.Value });

        }

        return tileData;
    }

    public void SetTileData(List<TileSaveData> tileSaveData)
    {
        foreach(var tile in tileCellDictionary)
        {
            tilemap.SetTile(tile.Key, null);
        }

        tileCellDictionary = new Dictionary<Vector3Int, bool>();

        foreach (TileSaveData tileData in tileSaveData)
        {
            SetTile(tilemap, tileData.tilePosition);
            tileCellDictionary[tileData.tilePosition] = tileData.tilePlant;
        }

    }


}
