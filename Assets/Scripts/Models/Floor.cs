using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Floor : JSONResource {
  public const string type = "Floor";
  public Floor (JSONNode _sourceData) : base(_sourceData) { }

  FloorTemplate _floorTemplate;
  public FloorTemplate floorTemplate {
    get {
      if (_floorTemplate == null) {
        var templateName = sourceData["template"].Value;
        _floorTemplate = FloorTemplate.all[templateName];
      }
      return _floorTemplate;
    }
  }

  Dictionary<string, float> _content;
  public Dictionary<string, float> content {
    get {
      if (_content == null) {
        CascadeContent(sourceData["content"].AsArray);
      }
      return _content;
    }
  }

  List<string> _entranceEvents;
  public List<string> entranceEvents {
    get {
      if (_entranceEvents == null) {
        _entranceEvents = new List<string>();
        var entranceArr = sourceData["entrance_events"].AsArray;
        foreach (JSONNode ent in entranceArr) {
          _entranceEvents.Add(ent.Value);
        }
      }
      return _entranceEvents;
    }
  }

  Dictionary<string, Branch> _branches;
  public Dictionary<string, Branch> branches {
    get {
      if (_branches == null) {
        _branches = new Dictionary<string, Branch>();
        var branchesArr = sourceData["branches"].AsArray;
        foreach (JSONNode b in branchesArr) {
          _branches[b["key"].Value] = new Branch(b);
        }
      }

      return _branches;
    }
  }

  public Dictionary<string, float> consumableChances {
    get {
      return floorTemplate.consumableChances;
    }
  }

  public static Floor GetFloor (int num) {
    return (Floor)cache[num.ToString()];
  }

  void CascadeContent (JSONArray contentJson) {
    // Cascade content from FloorTemplate
    _content = floorTemplate.content;
    foreach (JSONNode item in contentJson) {
      var key = item["key"].Value;
      var chance = item["chance"].AsFloat;
      _content[key] = chance;
    }
  }


  public string Name () {
    return string.Format("{0} {1} Floor", floorTemplate.name, NumberUtilities.AddOrdinal(int.Parse(key)));
  }

  /*
   * Map Stuff
   */

  public DunGen.TileType[,] map;

  public Vector3 playerPos;

  List<Vector3> openTiles;
  public Vector3 RandomOpenTile () {
    if (openTiles == null) {
      ScanOpenTiles();
    }

    return openTiles[Random.Range(0, openTiles.Count -1)];
  }

  void ScanOpenTiles () {
    openTiles = new List<Vector3>();
    for (var r = 0; r < map.GetLength(0); r++) {
      for (var c = 0; c < map.GetLength(1); c++) {
        if (map[r,c] == DunGen.TileType.Room
            || map[r,c] == DunGen.TileType.Corridor) {
          openTiles.Add(new Vector3(c, 0, r));
        }
      }
    }
  }

  public Vector2 stairsUpPos {
    get {
      return Vector2.zero;
    }
  }
  public Vector2 stairsDownPos;

  // TODO: Refactor to cascade mob chances locally
  public Mob RandomMob () {
    return floorTemplate.RandomMob();
  }

  // TODO: Refactor to cascade atmosphere locally
  public string RandomAtmosphereText () {
    return floorTemplate.RandomAtmosphereText();
  }

  // TODO: Refactor to cascade interactible chances locally
  public Interactible RandomInteractible () {
    return floorTemplate.RandomInteractible();
  }

  public RoomTemplate RandomRoomTemplate () {
    var roomTemplateKey = Roll.Hash(floorTemplate.roomTemplateChances);
    return (RoomTemplate)RoomTemplate.cache[roomTemplateKey];
  }

  public Branch GetBranch (string placeholderKey) {
    var branchKey = placeholderKey.Replace("branch:", "");
    return branches[branchKey];
  }
}
