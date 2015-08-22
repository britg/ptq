using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ConsumableTemplate {
  public const string type = "ConsumableTemplate";

  public string Key { get; set; }
  public string Name { get; set; }
  public string UsedName { get; set; }

  public Dictionary<string, RangeAttribute> statEffects;

  public static Dictionary<string, ConsumableTemplate> all = new Dictionary<string, ConsumableTemplate>();

  public static void Cache (JSONNode json) {
    var consumableTemplate = new ConsumableTemplate(json);
    all[consumableTemplate.Key] = consumableTemplate;
    Debug.Log("Loaded consumable type " + consumableTemplate.Name);
  }

  public ConsumableTemplate () {

  }

  public ConsumableTemplate (JSONNode json) {
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
