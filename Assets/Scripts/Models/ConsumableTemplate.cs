using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ConsumableTemplate : JSONResource {
  public const string type = "ConsumableTemplate";
  public ConsumableTemplate (JSONNode _sourceData) : base(_sourceData) { }

  public string name {
    get {
      return sourceData["name"].Value;
    }
  }

  public string usedName {
    get {
      return sourceData["used_name"].Value;
    }
  }

  Dictionary<string, RangeAttribute> _statEffects;
  public Dictionary<string, RangeAttribute> statEffects {
    get {
      if (_statEffects == null) {
        ParseStatEffects();
      }
      return _statEffects;
    }
  }

  void ParseStatEffects () {
    _statEffects = new Dictionary<string, RangeAttribute>();
    var statEffectsJson = sourceData["stat_effects"].AsArray;
    foreach (JSONNode statEffectJson in statEffectsJson) {
      var statKey = statEffectJson["key"].Value;
      var min = statEffectJson["range"].AsArray[0].AsFloat;
      var max = statEffectJson["range"].AsArray[1].AsFloat;

      _statEffects[statKey] = new RangeAttribute(min, max);
    }
  }

}
