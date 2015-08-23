using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ResourceLoader {

  Simulation sim;
  public Dictionary<string, List<JSONNode>> jsonCache = new Dictionary<string, List<JSONNode>>();

  List<string> parseOrder = new List<string>() {
    // Tier 1
    Rarity.type,
    ResourceType.type,
    QuestType.type,
    StatType.type,
    SlotType.type,
    Name.type,

    // Tier 2
    EquipmentDesignation.type,
    EquipmentType.type,
    ConsumableTemplate.type,
    FloorTemplate.type,
    Floor.type,
    RoomTemplate.type,
    MobTemplate.type,
    Simulation.type
  };


  string CONFIG_PATH = Application.streamingAssetsPath + "/Config";
  const string EXT = ".json";

  public ResourceLoader (Simulation _sim) {
    sim = _sim;
  }

  public void LoadSelf (JSONNode json) {
  }

  public void LoadResources () {
    LoadDirectory(CONFIG_PATH);
    ParseLoadedResources();
  }

  public void LoadDirectory (string dir) {

    foreach (string subdir in Directory.GetDirectories(dir)) {
      LoadDirectory(subdir);
    }

    foreach (string filename in Directory.GetFiles(dir)) {
      LoadFile(filename);
    }
  }

  public void LoadFile (string filename) {
    var fileInfo = new FileInfo(filename);
    var ext = fileInfo.Extension;

    if (ext == EXT) {
      string contents = File.ReadAllText(filename);
      ParseContents(contents);
    } else {
    }

  }

  public void ParseContents (string json) {
    var parsed = JSON.Parse(json);
    var type = parsed["type"].Value;
    if (!jsonCache.ContainsKey(type)) {
      jsonCache[type] = new List<JSONNode>();
    }
    jsonCache[type].Add(parsed);
  }


  void ParseLoadedResources () {
    foreach (string type in parseOrder) {
      ParseLoadedType(type);
    }
  }

  void ParseLoadedType (string type) {
    var configs = jsonCache[type];
    foreach (JSONNode config in configs) {
      ParseSingleConfig(config);
    }
  }

  void ParseSingleConfig (JSONNode config) {

    var type = config["type"].Value;
    switch (type) {
      case ResourceType.type:
        ResourceType.Cache(config);
        break;
      case QuestType.type:
        QuestType.Cache(config);
        break;
      case StatType.type:
        StatType.Cache(config);
        break;
      case SlotType.type:
        SlotType.Cache(config);
        break;
      case EquipmentDesignation.type:
        EquipmentDesignation.Cache(config);
        break;
      case EquipmentType.type:
        EquipmentType.Cache(config);
        break;
      case ConsumableTemplate.type:
        JSONResource.Cache<ConsumableTemplate>(config);
        break;
      case FloorTemplate.type:
        FloorTemplate.Cache(config);
        break;
      case Floor.type:
        JSONResource.Cache<Floor>(config);
        break;
      case RoomTemplate.type:
        JSONResource.Cache<RoomTemplate>(config);
        break;
      case MobTemplate.type:
        MobTemplate.Cache(config);
        break;
      case Simulation.type:
        LoadSelf(config);
      break;
      case Name.type:
        Name.Cache(config);
        break;
      case Rarity.type:
        Rarity.Cache(config);
        break;
      default:
        Debug.LogWarning(string.Format("Failed to load {0} {1}", config["type"], config["key"]));
      break;
    }

  }

}