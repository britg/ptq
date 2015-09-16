using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EnvironmentTemplate : JSONResource {
  public const string type = "EnvironmentTemplate";
  public EnvironmentTemplate (JSONNode _sourceData) : base(_sourceData) { }

  Dictionary<string, float> _mobChances;
  public Dictionary<string, float> mobChances {
    get {
      if (_mobChances == null) {
        ExtractChances("mobs", ref _mobChances);
      }
      return _mobChances;
    }
  }

  Dictionary<string, float> _interactibleChances;
  public Dictionary<string, float> interactibleChances {
    get {
      if (_interactibleChances == null) {
        ExtractChances("interactibles", ref _interactibleChances);
      }
      return _interactibleChances;
    }
  }


}
