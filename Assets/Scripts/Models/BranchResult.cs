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

  public string thenTo {
    get {
      return sourceData["then_to"].Value;
    }
  }

  public bool thenToContinue {
    get {
      return thenTo == "continue";
    }
  }

  public bool thenToRoom {
    get {
      return tpd.BeginsWith(thenTo, "room:");
    }
  }

  public bool thenToEvents {
    get {
      return tpd.BeginsWith(thenTo, "events:");
    }
  }

  public bool thenToPromptPull {
    get {
      return thenTo == "prompt_pull";
    }
  }

}
