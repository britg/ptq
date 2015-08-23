using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class BranchResult : JSONResource {

  public BranchResult (JSONNode _sourceData) : base(_sourceData) { }

  List<string> _events;
  public List<string> events {
    get {
      if (_events == null) {
        _events = new List<string>();
        var eventArr = sourceData["events"];
        foreach (JSONNode ev in eventArr.AsArray) {
          _events.Add(ev.Value);
        }
      }

      return _events;
    }
  }

  public string exit {
    get {
      return sourceData["exit"].Value;
    }
  }

}
