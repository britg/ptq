using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Setting : JSONResource {

  public const string type = "Setting";
  public Setting (JSONNode _sourceData) : base(_sourceData) { }

  static Setting _instance;
  public static Setting instance {
    get {
      if (_instance == null) {
        _instance = (Setting)JSONResource.cache["main"];
      }
      return _instance;
    }
  }

  public static object Get (string key) {
    return instance.sourceData[key].Value;
  }

}
