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
        _instance = Setting.Get<Setting>("main");
      }
      return _instance;
    }
  }

  public static object Get (string key) {
    return instance.sourceData[key].Value;
  }

  public JSONClass playerStartAttributes {
    get {
      return (JSONClass)sourceData["player_start_attributes"];
    }
  }

}
