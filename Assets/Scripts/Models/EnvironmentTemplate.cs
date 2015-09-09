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
      if (_mobChances == null) {
        ExtractChances("interactibles", ref _mobChances);
      }
      return _mobChances;
    }
  }


  //  public string name;
  //  public string key;
  //  public RangeAttribute roomCountRange;
  //  public List<string> atmosphereText;
  //  public Dictionary<string, float> content;
  //  public Dictionary<string, float> roomTemplateChances;
  //  public Dictionary<string, float> mobChances;
  //  public Dictionary<string, float> consumableChances;
  //  public Dictionary<string, float> interactibleChances;
  //
  //  public static Dictionary<string, EnvironmentTemplate> all = new Dictionary<string, EnvironmentTemplate>();
  //
  //  public static void Cache (JSONNode json) {
  //    var envTemplate = new EnvironmentTemplate(json);
  //    all[envTemplate.key] = envTemplate;
  //    Debug.Log("Loaded environment template " + envTemplate.name);
  //  }
  //
  //  public EnvironmentTemplate () {
  //
  //  }
  //
  //  public EnvironmentTemplate (JSONNode json) {
  //
  //    name = json["name"].Value;
  //    key = json["key"].Value;
  //
  //    atmosphereText = new List<string>();
  //
  //    var atmArr = json["atmosphere"].AsArray;
  //    foreach (JSONNode atmNode in atmArr) {
  //      atmosphereText.Add(atmNode.Value);
  //    }
  //
  //    var roomCountArr = json["room_count"].AsArray;
  //    roomCountRange = new RangeAttribute(roomCountArr[0].AsInt, roomCountArr[1].AsInt);
  //
  //    var contentArr = json["content"].AsArray;
  //    content = new Dictionary<string, float>();
  //    ExtractChances(contentArr, ref content);
  //
  //    var roomArr = json["rooms"].AsArray;
  //    roomTemplateChances = new Dictionary<string, float>();
  //    ExtractChances(roomArr, ref roomTemplateChances);
  //
  //    var mobArr = json["mobs"].AsArray;
  //    mobChances = new Dictionary<string, float>();
  //    ExtractChances(mobArr, ref mobChances);
  //
  //    var consumablesArr = json["consumables"].AsArray;
  //    consumableChances = new Dictionary<string, float>();
  //    ExtractChances(consumablesArr, ref consumableChances);
  //
  //    var interactibleArr = json["interactibles"].AsArray;
  //    interactibleChances = new Dictionary<string, float>();
  //    ExtractChances(interactibleArr, ref interactibleChances);
  //  }
  //
  //  void ExtractChances (JSONArray arr, ref Dictionary<string, float> dict) {
  //    foreach (JSONNode node in arr) {
  //      var key = node["key"].Value;
  //      var chance = node["chance"].AsFloat;
  //      dict[key] = chance;
  //    }
  //  }

}
