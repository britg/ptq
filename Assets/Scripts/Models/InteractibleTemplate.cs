using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class InteractibleTemplate : JSONResource {
  public const string type = "InteractibleTemplate";
  public InteractibleTemplate (JSONNode _sourceData) : base(_sourceData) { }

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

}
