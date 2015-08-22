using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Branch : JSONResource {

  public const string type = "Branch";

  public Branch (JSONNode _sourceData) :  base(_sourceData) { }

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



}
