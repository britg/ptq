using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class StatType {

  public const string type = "StatType";

  public string Key { get; set; }
  public string Name { get; set; }
  public string Abbr { get; set; }

  public static Dictionary<string, StatType> all = new Dictionary<string, StatType>();

  public static void Cache (JSONNode json) {
    var statType = new StatType(json);
    all[statType.Key] = statType;
    Debug.Log("Loaded stat type " + statType.Name);
  }

  public StatType () {

  }

  public StatType (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
    Abbr = json["abbr"].Value;
  }
}
