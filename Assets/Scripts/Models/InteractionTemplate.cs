using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class InteractionTemplate : JSONResource {
  public const string type = "InteractionTemplate";
  public InteractionTemplate (JSONNode _sourceData) : base(_sourceData) { }

  public string name {
    get {
      return sourceData["name"].Value;
    }
  }

  public int level {
    get {
      return sourceData["level"].AsInt;
    }
  }

  Dictionary<string, List<string>> _eventGroups;
  public Dictionary<string, List<string>> eventGroups {
    get {
      if (_eventGroups == null) {
        _eventGroups = new Dictionary<string, List<string>>();
        var eventsNode = (JSONClass)sourceData["event_groups"];
        foreach (KeyValuePair<string, JSONNode> node in eventsNode) {
          var k = node.Key;
          var eventsArr = (JSONArray)node.Value;
          _eventGroups[k] = new List<string>();
          foreach (JSONNode evNode in eventsArr) {
            var ev = evNode.Value;
            _eventGroups[k].Add(ev);
          }
        }
      }
      return _eventGroups;
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

  public List<string> GetEventGroup (string groupKey) {
    return eventGroups[groupKey];
  }

  public Branch GetBranch (string placeholderKey) {
    var branchKey = tpd.RemoveSubString(placeholderKey, Constants.branchLabel);
    return branches[branchKey];
  }

}
