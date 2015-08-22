using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentModifier {

  public enum Type {
    Prefix,
    Suffix
  }

  public Type type;
  public string name;
  public RangeAttribute levelRange;

  public Dictionary<string, RangeAttribute> statAdditions;

  public static EquipmentModifier Prefix () {
    return new EquipmentModifier(Type.Prefix);
  }

  public static EquipmentModifier Suffix () {
    return new EquipmentModifier(Type.Suffix);
  }

  public EquipmentModifier (Type _type) {
    _type = type;
  }
}
