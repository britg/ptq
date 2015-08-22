using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EquipmentType {

  public const string type = "EquipmentType";

  public string Key { get; set; }
  public string Name { get; set; }
  public Dictionary<string, float> StatMultipliers;

  public Dictionary<string, SlotType> SlotTypes { get; set; }
  public SlotType PrimarySlotType {
    get {
      return FirstSlotType();
    }
  }
  public EquipmentDesignation Designation {get; set;}

  public static Dictionary<string, EquipmentType> all = new Dictionary<string, EquipmentType>();

  public static void Cache (JSONNode json) {
    var equipmentType = new EquipmentType(json);
    all[equipmentType.Key] = equipmentType;
    Debug.Log("Loaded equipment type " + equipmentType.Name);
  }

  public EquipmentType () {

  }

  public EquipmentType (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
    Designation = EquipmentDesignation.all[json["designation"].Value];

    StatMultipliers = new Dictionary<string, float>();
    var multipliers = json["stat_multipliers"].AsArray;
    foreach (JSONNode mult in multipliers) {
      string statKey = mult["key"].Value;
      float val = mult["multiplier"].AsFloat;
      StatMultipliers[statKey] = val;
    }

    SlotTypes = new Dictionary<string, SlotType>();
    var slotTypeArr = json["slots"].AsArray;
    
    foreach (JSONNode slotType in slotTypeArr) {
      var key = slotType.Value;
      SlotTypes.Add(key, SlotType.all[key]);
    }
  }

  public SlotType FirstSlotType () {
    foreach (KeyValuePair<string, SlotType> p in SlotTypes) {
      return p.Value;
    }

    return null;
  }

  public float MultiplierForStat (string key) {
    if (StatMultipliers.ContainsKey(key)) {
      return StatMultipliers[key];
    }

    return 1f;
  }
}
