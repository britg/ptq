using UnityEngine;
using System.Collections;
using SimpleJSON;

public class JSONResource {

  public JSONNode sourceData {
    get; set;
  }

  string _key;
  public string key {
    get {
      if (_key == null) {
        _key = sourceData["key"].Value;
      }
      return _key;
    }
  }

  public JSONResource (JSONNode _sourceData) {
    sourceData = _sourceData;
  }


}
