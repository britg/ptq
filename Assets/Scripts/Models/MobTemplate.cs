using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MobTemplate : JSONResource {
  public const string type = "MobTemplate";
  public MobTemplate (JSONNode _sourceData) : base(_sourceData) { }

  public string name {
    get {
      return sourceData["name"].Value;
    }
  }

  public int level {
    get {
      return sourceData["level"].AsInt;
    }
  }

  public Dictionary<string, RangeAttribute> StatRanges { get; set; }
  public Hashtable combatProfile = new Hashtable();

//  public MobTemplate () {
//
//  }
//
//  public MobTemplate (JSONNode json) {
//    Key = json["key"].Value;
//    name = json["name"].Value;
//    level = json["level"].AsInt;
//    consumableChance = json["consumable_chance"].AsFloat;
//    lootChance = json["loot_chance"].AsFloat;
//    goldChance = json["gold_chance"].AsFloat;
//
//    StatRanges = new Dictionary<string, RangeAttribute>();
//    var stats = json["stats"].AsArray;
//
//    // Loading stats
//    foreach (JSONNode statJson in stats) {
//      var key = statJson["key"].Value;
//      var range = statJson["range"].AsArray;
//
//      StatRanges[key] = new RangeAttribute(range[0].AsFloat, range[1].AsFloat);
//    }
//
//    // Loading combat profile
//    var combat = json["combat"].AsArray;
//    foreach (JSONNode combatNode in combat) {
//      var key = combatNode["key"].Value;
//      var chance = combatNode["chance"].AsFloat;
//
//      combatProfile[key] = chance;
//    }
//
//  }

}
