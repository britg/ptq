using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Environment : JSONResource {
  public const string type = "Environment";
  public Environment (JSONNode _sourceData) : base(_sourceData) { }

  public string name {
    get {
      return sourceData["name"].Value;
    }
  }

  EnvironmentTemplate _envTemplate;
  public EnvironmentTemplate envTemplate {
    get {
      if (_envTemplate == null) {
        var templateKey = sourceData["template"].Value;
        _envTemplate = EnvironmentTemplate.Get<EnvironmentTemplate>(templateKey);
      }
      return _envTemplate;
    }
  }

  public string enterInteractionTemplateKey {
    get {
      return sourceData["enter_interaction"].Value;
    }
  }

//  Dictionary<string, List<string>> _events;
//  public Dictionary<string, List<string>> events {
//    get {
//      if (_events == null) {
//        _events = new Dictionary<string, List<string>>();
//        var eventsNode = (JSONClass)sourceData["events"];
//        foreach (KeyValuePair<string, JSONNode> node in eventsNode) {
//          var k = node.Key;
//          var eventsArr = (JSONArray)node.Value;
//          _events[k] = new List<string>();
//          foreach (JSONNode evNode in eventsArr) {
//            var ev = evNode.Value;
//            _events[k].Add(ev);
//          }
//        }
//      }
//      return _events;
//    }
//  }
//
//  Dictionary<string, Branch> _branches;
//  public Dictionary<string, Branch> branches {
//    get {
//      if (_branches == null) {
//        _branches = new Dictionary<string, Branch>();
//        var branchesArr = sourceData["branches"].AsArray;
//        foreach (JSONNode b in branchesArr) {
//          _branches[b["key"].Value] = new Branch(b);
//        }
//      }
//
//      return _branches;
//    }
//  }

  public Dictionary<string, float> consumableChances {
    get {
      return new Dictionary<string, float>();
    }
  }

  Dictionary<string, float> _roomTemplateChances;
  public Dictionary<string, float> roomTemplateChances {
    get {

      if (_roomTemplateChances == null) {
        // TODO: cascade from environment template
        _roomTemplateChances = new Dictionary<string, float>() { { "standard", 100f } };
      }

      return _roomTemplateChances;
    }
  }

  Dictionary<string, float> _mobChances;
  public Dictionary<string, float> mobChances {
    get {
      if (_mobChances == null) {
        // TODO: Cascade from environment template
        _mobChances = envTemplate.mobChances;
      }
      return _mobChances;
    }
  }

  Dictionary<string, float> _interactibleChances;
  public Dictionary<string, float> interactibleChances {
    get {
      if (_interactibleChances == null) {
        // TODO: Cascade from environment template
        _interactibleChances = envTemplate.interactibleChances;
      }
      return _interactibleChances;
    }
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
