using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ConsumableType {
  public const string type = "ConsumableType";

  public string Key { get; set; }
  public string Name { get; set; }
  public string UsedName { get; set; }

  public Dictionary<string, RangeAttribute> statEffects;

  public static Dictionary<string, ConsumableType> all = new Dictionary<string, ConsumableType>();

  public static void Cache (JSONNode json) {
    var consumableType = new ConsumableType(json);
    all[consumableType.Key] = consumableType;
    Debug.Log("Loaded consumable type " + consumableType.Name);
  }

  public ConsumableType () {

  }

  public ConsumableType (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
    UsedName = json["used_name"].Value;

    statEffects = new Dictionary<string, RangeAttribute>();
    var statEffectsJson = json["stat_effects"].AsArray;
    foreach (JSONNode statEffectJson in statEffectsJson) {
      var statKey = statEffectJson["key"].Value;
      var min = statEffectJson["range"].AsArray[0].AsFloat;
      var max = statEffectJson["range"].AsArray[1].AsFloat;

      statEffects[statKey] = new RangeAttribute(min, max);
    }

  }

  public Consumable Consumable () {
    var consumable = new Consumable();
    consumable.key = Key;
    consumable.name = Name;
    consumable.usedName = UsedName;

    consumable.statEffects = new Dictionary<string, float>();
    foreach (KeyValuePair<string, RangeAttribute> statEffect in statEffects) {
      var statKey = statEffect.Key;
      var range = statEffect.Value;

      consumable.statEffects[statKey] = Random.Range(range.min, range.max);
    }

    return consumable;
  }
}
