using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ResourceType {

  public const string type = "ResourceType";

  public string Key { get; set; }
  public string Name { get; set; }

  public static Dictionary<string, ResourceType> all = new Dictionary<string, ResourceType>();

  public static void Cache (JSONNode json) {
    var resourceType = new ResourceType(json);
    all[resourceType.Key] = resourceType;
    Debug.Log ("Loaded resource type " + resourceType.Name);
  }

  public ResourceType () {

  }

  public ResourceType (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
  }
}
