using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

  public string key;
  public RoomTemplate roomTemplate;
  public Dictionary<string, float> content;
  public DunGen.Room baseLayer;
  public List<Tile> tiles;

  public Room (DunGen.Room _baseLayer) {
    baseLayer = _baseLayer;
    InitTiles();
  }

  void InitTiles () {
    tiles = new List<Tile>();
    foreach (Vector3 position in baseLayer.tiles) {
      var tile = new Tile(position);
      tiles.Add(tile);
    }
  }

}
