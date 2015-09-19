using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

  public string key;
  public RoomTemplate roomTemplate;
  public Dictionary<string, float> content;
  public List<Tile> tiles;

  public Room () {
//    InitTiles();
  }

//  void InitTiles () {
//    tiles = new List<Tile>();
//    foreach (Vector3 position in baseLayer.tiles) {
//      var tile = new Tile(position);
//      tiles.Add(tile);
//    }
//  }

  public Tile RandomOpenTile () {
    var test = tpd.RollList<Tile>(tiles);
    if (test.occupied) {
      return RandomOpenTile();
    }
    return test;
  }

}
