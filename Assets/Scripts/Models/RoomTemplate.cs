﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class RoomTemplate : JSONResource {
  public const string type = "RoomTemplate";
  public RoomTemplate (JSONNode _sourceData) : base(_sourceData) { }

  Dictionary<string, float> _contentOverrides;
  public Dictionary<string, float> contentOverrides {
    get {
      if (_contentOverrides == null) {
        _contentOverrides = new Dictionary<string, float>();
        var contentJson = sourceData["content"].AsArray;
        foreach (JSONNode item in contentJson) {
          var contentType = item["key"].Value;
          var chance = item["chance"].AsFloat;
          _contentOverrides[contentType] = chance;
        }
      }
      return _contentOverrides;
    }
  }

  Branch _entranchBranch;
  public Branch entranceBranch {
    get {
      if (_entranchBranch == null) {
        _entranchBranch = new Branch(sourceData["entrance_branch"]);
      }
      return _entranchBranch;
    }
  }

  //public RoomTemplate (JSONNode json) {
  //  key = json["key"].Value;
  //  contentOverrides = new Dictionary<string, float>();

  //  var contentJson = json["content"].AsArray;
  //  foreach (JSONNode item in contentJson) {
  //    var contentType = item["key"].Value;
  //    var chance = item["chance"].AsFloat;
  //    contentOverrides[contentType] = chance;
  //  }

  //  entranceBranch = new Branch(json["entrance_branch"]);

  //}

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
