using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsumableGenerator {

  Simulation sim;

  public ConsumableGenerator (Simulation _sim) {
    sim = _sim;
  }

  public Consumable Generate (string consumableTemplateKey) {
    // TODO: do this for realz
    var template = (ConsumableTemplate)ConsumableTemplate.cache[consumableTemplateKey];
    var consumable = new Consumable();
    consumable.key = template.key;
    consumable.name = template.name;
    consumable.usedName = template.usedName;

    consumable.statEffects = new Dictionary<string, float>();
    foreach (KeyValuePair<string, RangeAttribute> statEffect in template.statEffects) {
      var statKey = statEffect.Key;
      var range = statEffect.Value;

      consumable.statEffects[statKey] = Random.Range(range.min, range.max);
    }

    return consumable;
  }
}
