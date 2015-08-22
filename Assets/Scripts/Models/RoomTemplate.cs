using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class RoomTemplate {

  public const string type = "RoomTemplate";
  public const string entranceChoiceKey = "__ROOM_DOOR_CHOICE__";

  public static Dictionary<string, RoomTemplate> all = new Dictionary<string, RoomTemplate>();

  public string key;
  public Dictionary<string, float> contentOverrides;
  public Dictionary<string, JSONNode> branches;
  public JSONNode entranceChoice;

  public static void Cache (JSONNode json) {
    var roomTemplate = new RoomTemplate(json);
    all[roomTemplate.key] = roomTemplate;
    Debug.Log("Loading room template");
  }

  public RoomTemplate () {

  }

  public RoomTemplate (JSONNode json) {
    key = json["key"].Value;
    contentOverrides = new Dictionary<string, float>();

    var contentJson = json["content"].AsArray;
    foreach (JSONNode item in contentJson) {
      var contentType = item["key"].Value;
      var chance = item["chance"].AsFloat;
      contentOverrides[contentType] = chance;
    }

    entranceChoice = json["entrance_choice"];
//    branches = new Dictionary<string, JSONNode>();
//    var branchesArr = json["branches"].AsArray;
//    foreach (JSONNode branchNode in branchesArr) {
//      branches[branchNode["key"].Value] = branchNode;
//    }
  }

  public Dictionary<string, float> CascadeContent (Dictionary<string, float> content) {
    // Cascade content from Floor
    var final = content;
    foreach (KeyValuePair<string, float> ov in contentOverrides) {
      var key = ov.Key;
      var chance = ov.Value;
      final[key] = chance;
    }

    return final;
  }

}
