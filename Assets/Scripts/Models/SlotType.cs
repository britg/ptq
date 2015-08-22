using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SlotType {

  public const string type = "SlotType";

  public string Key { get; set; }
  public string Name { get; set; }

  public static Dictionary<string, SlotType> all = new Dictionary<string, SlotType>();

  public static void Cache (JSONNode json) {
    var slotType = new SlotType(json);
    all[slotType.Key] = slotType;
    Debug.Log("Loaded slot type " + slotType.Name);
  }

  public SlotType () {

  }

  public SlotType (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
  }
}
