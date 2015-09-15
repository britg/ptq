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

}
