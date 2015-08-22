using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class QuestType {

  public const string type = "QuestType";

  public string Key { get; set; }
  public string Name { get; set; }

  public static Dictionary<string, QuestType> all = new Dictionary<string, QuestType>();

  public static void Cache (JSONNode json) {
    var questType = new QuestType(json);
    all[questType.Key] = questType;
    Debug.Log ("Loaded quest type " + questType.Name);
  }

  public QuestType () {

  }

  public QuestType (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
  }
}
