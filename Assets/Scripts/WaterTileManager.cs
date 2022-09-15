using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTileManager : MonoBehaviour
{
    public GameObject StartTile;

    public List<WaterTile> Tiles = new List<WaterTile>();
    public Transform Ship;

    void Start()
    {
        Tiles.Add(new WaterTile() { Tile = StartTile, Position = new Vector2(0, 0)});
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(0, 1) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(1, 1) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(1, 0) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(1, -1) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(0, -1) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(-1, -1) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(-1, 0) });
        Tiles.Add(new WaterTile() { Tile = Instantiate(StartTile, transform), Position = new Vector2(-1, 1) });

        foreach (WaterTile tile in Tiles)
        {
            tile.Tile.transform.position = new Vector3(tile.Position.x * 2000, 0, tile.Position.y * 2000);
        }
    }

    void Update()
    {
        
    }

    public class WaterTile
    {
        public GameObject Tile;
        public Vector2 Position;
    }
}
