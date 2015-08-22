using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Floor {

  public const string type = "Floor";

  public static Dictionary<int, Floor> all = new Dictionary<int, Floor>();

  public int num;
  public FloorTemplate floorTemplate;
  public Dictionary<string, float> content;
  public List<string> entranceEvents;
  public Dictionary<string, JSONNode> branches;

  public Dictionary<string, float> consumableChances {
    get {
      return floorTemplate.consumableChances;
    }
  }

  public static Floor GetFloor (int num) {
    return all[num];
  }

  public static void Cache (JSONNode json) {
    var floor = new Floor(json);
    all[floor.num] = floor;
    Debug.Log("Loaded floor " + floor.num);
  }

  public Floor () {

  }

  public Floor (JSONNode json) {
    num = json["number"].AsInt;
    floorTemplate = FloorTemplate.all[json["template"].Value];

    entranceEvents = new List<string>();
    var entranceArr = json["entrance_events"].AsArray;
    foreach (JSONNode ent in entranceArr) {
      entranceEvents.Add(ent.Value);
    }

    branches = new Dictionary<string, JSONNode>();
    var branchesArr = json["branches"].AsArray;
    foreach (JSONNode branchNode in branchesArr) {
      branches[branchNode["key"].Value] = branchNode;
    }

    CascadeContent(json["content"].AsArray);
  }

  void CascadeContent (JSONArray contentJson) {
    // Cascade content from FloorTemplate
    content = floorTemplate.content;
    foreach (JSONNode item in contentJson) {
      var key = item["key"].Value;
      var chance = item["chance"].AsFloat;
      content[key] = chance;
    }
  }


  public string Name () {
    return string.Format("{0} {1} Floor", floorTemplate.name, NumberUtilities.AddOrdinal(num));
  }

  // TODO: Refactor to cascade mob chances locally
  public Mob RandomMob () {
    return floorTemplate.RandomMob();
  }

  // TODO: Refactor to cascade atmosphere locally
  public string RandomAtmosphereText () {
    return floorTemplate.RandomAtmosphereText();
  }

  // TODO: Refactor to cascade interactible chances locally
  public Interactible RandomInteractible () {
    return floorTemplate.RandomInteractible();
  }

  public Room RandomRoom () {
    var roomTemplateKey = Roll.Hash(floorTemplate.roomTemplateChances);
    var roomGenerator = new RoomGenerator(roomTemplateKey, this);
    return roomGenerator.CreateRoom();
  }

}
