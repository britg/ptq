using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Floor : JSONResource {
  public const string type = "Floor";
  public Floor (JSONNode _sourceData) : base(_sourceData) { }

  FloorTemplate _floorTemplate;
  public FloorTemplate floorTemplate {
    get {
      if (_floorTemplate == null) {
        var templateName = sourceData["template"].Value;
        _floorTemplate = FloorTemplate.all[templateName];
      }
      return _floorTemplate;
    }
  }

  Dictionary<string, float> _content;
  public Dictionary<string, float> content {
    get {
      if (_content == null) {
        CascadeContent(sourceData["content"].AsArray);
      }
      return _content;
    }
  }

  List<string> _entranceEvents;
  public List<string> entranceEvents {
    get {
      if (_entranceEvents == null) {
        _entranceEvents = new List<string>();
        var entranceArr = sourceData["entrance_events"].AsArray;
        foreach (JSONNode ent in entranceArr) {
          _entranceEvents.Add(ent.Value);
        }
      }
      return _entranceEvents;
    }
  }

  public Dictionary<string, JSONNode> branches;

  public Dictionary<string, float> consumableChances {
    get {
      return floorTemplate.consumableChances;
    }
  }

  public static Floor GetFloor (int num) {
    return (Floor)cache[num.ToString()];
  }

  //public Floor (JSONNode json) {
  //  num = json["number"].AsInt;
  //  floorTemplate = FloorTemplate.all[json["template"].Value];

  //  entranceEvents = new List<string>();
  //  var entranceArr = json["entrance_events"].AsArray;
  //  foreach (JSONNode ent in entranceArr) {
  //    entranceEvents.Add(ent.Value);
  //  }

  //  branches = new Dictionary<string, JSONNode>();
  //  var branchesArr = json["branches"].AsArray;
  //  foreach (JSONNode branchNode in branchesArr) {
  //    branches[branchNode["key"].Value] = branchNode;
  //  }

  //  CascadeContent(json["content"].AsArray);
  //}

  void CascadeContent (JSONArray contentJson) {
    // Cascade content from FloorTemplate
    _content = floorTemplate.content;
    foreach (JSONNode item in contentJson) {
      var key = item["key"].Value;
      var chance = item["chance"].AsFloat;
      _content[key] = chance;
    }
  }


  public string Name () {
    return string.Format("{0} {1} Floor", floorTemplate.name, NumberUtilities.AddOrdinal(int.Parse(key)));
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
