using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SimulationConfig {

  public const string type = "Simulation";

  Simulation sim;
  public Dictionary<string, List<JSONNode>> jsonCache = new Dictionary<string, List<JSONNode>>();

  // Core Config

  // Time
  public float updateIntervalSeconds;
  public int startSeconds;
  public int dayStartHour;
  public int nightStartHour;
  public float initialSpeed;

  // Player
  public List<String> startBuildings = new List<String>();

  // Adventurer
  public int adventurerDefaultLevel;
  public int adventurerDefaultExp;
  public float adventurerDefaultSpeed;

  // Exploration Quests
  public float defaultExplorationRadius;


  // Model config
//  const string CONFIG_PATH = "Assets/Scripts/Simulation/Config";
  string CONFIG_PATH = Application.streamingAssetsPath + "/Config";
  const string EXT = ".json";

  public SimulationConfig (Simulation _sim) {
    sim = _sim;
  }

  public void LoadSelf (JSONNode json) {
    updateIntervalSeconds = json["updateIntervalSeconds"].AsFloat;
    initialSpeed = json["initialSpeed"].AsFloat;
    startSeconds = json["startSeconds"].AsInt;
    foreach(JSONNode arrItem in json["startBuildings"].AsArray) {
      startBuildings.Add(arrItem.Value);
    }

    adventurerDefaultLevel = json["adventurerDefaultLevel"].AsInt;
    adventurerDefaultExp = json["adventurerDefaultExp"].AsInt;
    adventurerDefaultSpeed = json["adventurerDefaultSpeed"].AsFloat;

    defaultExplorationRadius = json["defaultExplorationRadius"].AsFloat;
  }

  public void LoadModels () {
    LoadDirectory(CONFIG_PATH);
    ParseLoadedConfigs();
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
    ConsumableType.type,
    FloorTemplate.type,
    Floor.type,
    RoomTemplate.type,
    MobTemplate.type,
    SimulationConfig.type
  };
  void ParseLoadedConfigs () {
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
      case ConsumableType.type:
        ConsumableType.Cache(config);
        break;
      case FloorTemplate.type:
        FloorTemplate.Cache(config);
        break;
      case Floor.type:
        Floor.Cache(config);
        break;
      case RoomTemplate.type:
        RoomTemplate.Cache(config);
        break;
      case MobTemplate.type:
        MobTemplate.Cache(config);
        break;
      case SimulationConfig.type:
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

  public void LoadExistingPlayer () {
    
  }

  public void CreatePlayer () {
    var playerCreator = new PlayerCreator(sim);
    playerCreator.Create();
  }

}