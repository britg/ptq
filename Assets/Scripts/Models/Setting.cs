using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Setting : JSONResource {

  public const string type = "Setting";
  public Setting (JSONNode _sourceData) : base(_sourceData) { }

  public static object Get (string key) {
    var setting = (Setting)Setting.cache["main"];
    return setting.sourceData[key].Value;
  }

}
