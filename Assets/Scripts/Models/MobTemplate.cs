using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MobTemplate {

  public const string type = "MobTemplate";

  public string Key;
  public string name;
  public int level;
  public float consumableChance;
  public float lootChance;
  public float goldChance;

  public Dictionary<string, RangeAttribute> StatRanges { get; set; }
  public Hashtable combatProfile = new Hashtable();

  public static Dictionary<string, MobTemplate> all = new Dictionary<string, MobTemplate>();

  public static void Cache (JSONNode json) {
    var mobTemplate = new MobTemplate(json);
    all[mobTemplate.Key] = mobTemplate;
    Debug.Log("Loaded mob template " + mobTemplate.Key);
  }

  public MobTemplate () {

  }

  public MobTemplate (JSONNode json) {
    Key = json["key"].Value;
    name = json["name"].Value;
    level = json["level"].AsInt;
    consumableChance = json["consumable_chance"].AsFloat;
    lootChance = json["loot_chance"].AsFloat;
    goldChance = json["gold_chance"].AsFloat;

    StatRanges = new Dictionary<string, RangeAttribute>();
    var stats = json["stats"].AsArray;

    // Loading stats
    foreach (JSONNode statJson in stats) {
      var key = statJson["key"].Value;
      var range = statJson["range"].AsArray;

      StatRanges[key] = new RangeAttribute(range[0].AsFloat, range[1].AsFloat);
    }

    // Loading combat profile
    var combat = json["combat"].AsArray;
    foreach (JSONNode combatNode in combat) {
      var key = combatNode["key"].Value;
      var chance = combatNode["chance"].AsFloat;

      combatProfile[key] = chance;
    }

  }

}
