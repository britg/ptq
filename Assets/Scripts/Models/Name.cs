using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Name {

  public const string type = "NameList";

  public string First { get; set; }
  public string Last { get; set; }

  public static List<string> firsts = new List<string>();

  public static void Cache (JSONNode json) {
    foreach (JSONNode jsonName in json["first_names"].AsArray) {
      var name = jsonName.Value;
      firsts.Add(name);
    }
  }

  public static string Generate () {
    int rand = (int)Random.Range(0, firsts.Count);
    var first = firsts[rand];

    return string.Format("{0}", first);
  }

  public Name () {

  }

  public Name (JSONNode json) {
  }

}
