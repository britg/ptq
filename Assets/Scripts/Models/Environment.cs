using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Environment : JSONResource {
  public const string type = "Environment";
  public Environment (JSONNode _sourceData) : base(_sourceData) { }

  public string name {
    get {
      return sourceData["name"].Value;
    }
  }

  EnvironmentTemplate _envTemplate;
  public EnvironmentTemplate envTemplate {
    get {
      if (_envTemplate == null) {
        var templateName = sourceData["template"].Value;
        _envTemplate = EnvironmentTemplate.all[templateName];
      }
      return _envTemplate;
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

  Dictionary<string, List<string>> _events;
  public Dictionary<string, List<string>> events {
    get {
      if (_events == null) {
        _events = new Dictionary<string, List<string>>();
        var eventsNode = (JSONClass)sourceData["events"];
        foreach (KeyValuePair<string, JSONNode> node in eventsNode) {
          var k = node.Key;
          var eventsArr = (JSONArray)node.Value;
          _events[k] = new List<string>();
          foreach (JSONNode evNode in eventsArr) {
            var ev = evNode.Value;
            _events[k].Add(ev);
          }
        }
      }
      return _events;
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
      return envTemplate.consumableChances;
    }
  }

  public static Environment GetEnv (string name) {
    return (Environment)cache[name];
  }

  void CascadeContent (JSONArray contentJson) {
    // Cascade content from EnvironmentTemplate
    _content = envTemplate.content;
    foreach (JSONNode item in contentJson) {
      var key = item["key"].Value;
      var chance = item["chance"].AsFloat;
      _content[key] = chance;
    }
  }


  public string Name () {
    return string.Format("{0}", name);
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
    return envTemplate.RandomMob();
  }

  // TODO: Refactor to cascade atmosphere locally
  public string RandomAtmosphereText () {
    return envTemplate.RandomAtmosphereText();
  }

  // TODO: Refactor to cascade interactible chances locally
  public Interactible RandomInteractible () {
    return envTemplate.RandomInteractible();
  }

  public RoomTemplate RandomRoomTemplate () {
    var roomTemplateKey = tpd.RollMap(envTemplate.roomTemplateChances);
    return (RoomTemplate)RoomTemplate.cache[roomTemplateKey];
  }

  public Branch GetBranch (string placeholderKey) {
    var branchKey = placeholderKey.Replace("branch:", "");
    return branches[branchKey];
  }
}
