using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FloorTemplate {

  public const string type = "FloorTemplate";

  public string name;
  public string key;
  public RangeAttribute roomCountRange;
  public List<string> atmosphereText;
  public Dictionary<string, float> content;
  public Dictionary<string, float> roomTemplateChances;
  public Dictionary<string, float> mobChances;
  public Dictionary<string, float> consumableChances;
  public Dictionary<string, float> interactibleChances;

  public static Dictionary<string, FloorTemplate> all = new Dictionary<string, FloorTemplate>();

  public static void Cache (JSONNode json) {
    var floorTemplate = new FloorTemplate(json);
    all[floorTemplate.key] = floorTemplate;
    Debug.Log("Loaded floor template " + floorTemplate.name);
  }

  public FloorTemplate () {

  }

  public FloorTemplate (JSONNode json) {

    name = json["name"].Value;
    key = json["key"].Value;

    atmosphereText = new List<string>();

    var atmArr = json["atmosphere"].AsArray;
    foreach (JSONNode atmNode in atmArr) {
      atmosphereText.Add(atmNode.Value);
    }

    var roomCountArr = json["room_count"].AsArray;
    roomCountRange = new RangeAttribute(roomCountArr[0].AsInt, roomCountArr[1].AsInt);

    var contentArr = json["content"].AsArray;
    content = new Dictionary<string, float>();
    ExtractChances(contentArr, ref content);

    var roomArr = json["rooms"].AsArray;
    roomTemplateChances = new Dictionary<string, float>();
    ExtractChances(roomArr, ref roomTemplateChances);

    var mobArr = json["mobs"].AsArray;
    mobChances = new Dictionary<string, float>();
    ExtractChances(mobArr, ref mobChances);

    var consumablesArr = json["consumables"].AsArray;
    consumableChances = new Dictionary<string, float>();
    ExtractChances(consumablesArr, ref consumableChances);

    var interactibleArr = json["interactibles"].AsArray;
    interactibleChances = new Dictionary<string, float>();
    ExtractChances(interactibleArr, ref interactibleChances);
  }

  void ExtractChances (JSONArray arr, ref Dictionary<string, float> dict) {
    foreach (JSONNode node in arr) {
      var key = node["key"].Value;
      var chance = node["chance"].AsFloat;
      dict[key] = chance;
    }
  }

  public string RandomAtmosphereText () {
    int rand = Random.Range(0, atmosphereText.Count - 1);
    return atmosphereText[rand];
  }

  public Mob RandomMob () {
    var mobKey = Roll.Hash(mobChances);
    MobTemplate template = MobTemplate.all[mobKey];
    Mob mob = Mob.FromTemplate(template);

    return mob;
  }

  public Interactible RandomInteractible () {
    var interactible = new Interactible();
    interactible.name = "skeletal remains";
    return interactible;

    /*  This should be refactored to InteractibleTemplate

    var interactibleKey = Roll.Hash(interactibleChances);
    Interactible interactible = 
    */
  }

}
