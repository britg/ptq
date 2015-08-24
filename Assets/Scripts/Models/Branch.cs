using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Branch : JSONResource {
  public const string type = "Branch";
  public Branch (JSONNode _sourceData) :  base(_sourceData) { }


  List<string> _introEvents;
  public List<string> introEvents {
	get {
	  if (_introEvents == null) {
      var introArr = sourceData["intro_events"].AsArray;
      _introEvents = new List<string>();
      foreach (JSONNode ev in introArr) {
        _introEvents.Add(ev.Value);
      }
    }

    return _introEvents;
  }

  string _text;
  public string text {
    get {
      if (_text == null) {
        _text = sourceData["text"].Value;
      }
      return _text;
    }
  }

  Choice _firstChoice;
  public Choice firstChoice {
    get {
      if (_firstChoice == null) {
        _firstChoice = Choice.Create(sourceData["choices"].AsArray[0]);
      }
      return _firstChoice;
    }
  }

  Choice _secondChoice;
  public Choice secondChoice {
    get {
      if (_secondChoice == null) {
        _secondChoice = Choice.Create(sourceData["choices"].AsArray[1]);
      }
      return _secondChoice;
    }
  }

  Dictionary<string, BranchResult> _results;
  public Dictionary<string, BranchResult> results {
    get {
      if (_results == null) {
        _results = new Dictionary<string, BranchResult>();
        var resultsArr = sourceData["results"].AsArray;
        foreach (JSONNode resultNode in resultsArr) {
          var res = new BranchResult(resultNode);
          _results[res.key] = res;
        }
      }

      return _results;
    }
  }





}
