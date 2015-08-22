using UnityEngine;
using System.Collections;

public class Resource {

  public const string Gold = "gold";

  public ResourceType ResourceType { get; set; }
  public float Amount { get; set; }

  public Resource () {

  }

  public Resource (string resourceKey, float amount) {
    ResourceType = ResourceType.all[resourceKey];
    Amount = amount;
  }
}
