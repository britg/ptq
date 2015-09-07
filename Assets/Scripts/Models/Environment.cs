using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Environment : JSONResource {
  public const string type = "Environment";
  public Environment (JSONNode _sourceData) : base(_sourceData) { }


  public static Environment GetEnv (string name) {
    return (Environment)cache[name];
  }

  public string name {
    get {
      return sourceData["name"].Value;
    }
  }

  EnvironmentTemplate _envTemplate;
  public EnvironmentTemplate envTemplate {
    get {
      if (_envTemplate == null) {
        var templateName = sourceData["template"].Value;
        _envTemplate = EnvironmentTemplate.all[templateName];
      }
      return _envTemplate;
    }
  }

  Dictionary<string, List<string>> _events;
  public Dictionary<string, List<string>> events {
    get {
      if (_events == null) {
        _events = new Dictionary<string, List<string>>();
        var eventsNode = (JSONClass)sourceData["events"];
        foreach (KeyValuePair<string, JSONNode> node in eventsNode) {
          var k = node.Key;
          var eventsArr = (JSONArray)node.Value;
          _events[k] = new List<string>();
          foreach (JSONNode evNode in eventsArr) {
            var ev = evNode.Value;
            _events[k].Add(ev);
          }
        }
      }
      return _events;
    }
  }

  Dictionary<string, Branch> _branches;
  public Dictionary<string, Branch> branches {
    get {
      if (_branches == null) {
        _branches = new Dictionary<string, Branch>();
        var branchesArr = sourceData["branches"].AsArray;
        foreach (JSONNode b in branchesArr) {
          _branches[b["key"].Value] = new Branch(b);
        }
      }

      return _branches;
    }
  }

  public Dictionary<string, float> consumableChances {
    get {
      return envTemplate.consumableChances;
    }
  }

  public Branch GetBranch (string placeholderKey) {
    var branchKey = placeholderKey.Replace(Constants.branchLabel, "");
    return branches[branchKey];
  }

  public string Name () {
    return string.Format("{0}", name);
  }

  /*
   * Map Stuff
   */

  public DunGen.Floor floor;
  public List<Room> rooms;
  public List<Hallway> hallways;

}
