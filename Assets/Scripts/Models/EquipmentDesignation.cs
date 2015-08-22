using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EquipmentDesignation {

  public const string type = "EquipmentDesignation";

  public string Key { get; set; }
  public string Name { get; set; }
  public Dictionary<string, StatType> BaseStats { get; set; }

  public static Dictionary<string, EquipmentDesignation> all = new Dictionary<string, EquipmentDesignation>();

  public static void Cache (JSONNode json) {
    var equipmentDesignation = new EquipmentDesignation(json);
    all[equipmentDesignation.Key] = equipmentDesignation;
    Debug.Log("Loaded equipment designation " + equipmentDesignation.Name);
  }

  public EquipmentDesignation (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;

    BaseStats = new Dictionary<string, StatType>();
    foreach (JSONNode item in json["base_stats"].AsArray) {
      var statKey = item.Value;
      Debug.Log ("Attempting to load base stat " + statKey);
      BaseStats[statKey] = StatType.all[statKey];
    }
  }
}