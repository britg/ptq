using UnityEngine;
using System.Collections;

public class Trigger {

  public const string damageKey = "damage";
  public const string targetKey = "target";
  public const string statKey = "stat";
  public const string statChangeAmountKey = "statChangeAmount";
  public const string statChangeTargetKey = "statChangeTarget";

  public const string resourceKey = "resource";
  public const string resourceAmountKey = "resourceAmount";

  public enum Type {
    NewFloor,
    PlayerStatChange,
    PlayerResourceChange
  }

  public Type type { get; set; }
  public Hashtable data = new Hashtable();

  public Trigger (Type _type) {
    type = _type;
  }

}
