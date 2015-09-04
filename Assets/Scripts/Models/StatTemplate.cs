using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class StatTemplate : JSONResource {

  public const string type = "StatTemplate";
  public StatTemplate (JSONNode _sourceData) : base(_sourceData) { }

  public string abbr {
    get {
      return sourceData["abbr"].Value;
    }
  }
}
