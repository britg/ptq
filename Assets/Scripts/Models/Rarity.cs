using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Rarity {

  public const string trash = "trash";
  public const string common = "common";
  public const string uncommon = "uncommon";
  public const string rare = "rare";
  public const string epic = "epic";

  public const string type = "Rarity";

  public string Key { get; set; }
  public string Name { get; set; }
  public float Chance { get; set; }
  public Color Color { get; set; }
  public RangeAttribute statMultiplierRange;
  public float prefixChance;
  public float suffixChance;

  public static Dictionary<string, Rarity> all = new Dictionary<string, Rarity>();

  public static void Cache (JSONNode json) {
    var rarity = new Rarity(json);
    all[rarity.Key] = rarity;
    Debug.Log("Loaded rarity type " + rarity.Name);
  }

  public Rarity () {

  }

  public Rarity (JSONNode json) {
    Key = json["key"].Value;
    Name = json["name"].Value;
    Chance = json["chance"].AsFloat;
    Color = ColorUtilities.HexToColor(json["color"].Value);

    var multipliers = json["multiplier"].AsArray;
    statMultiplierRange = new RangeAttribute(multipliers[0].AsFloat,
                                             multipliers[1].AsFloat);

    var modifierChances = json["modifier_chances"].AsArray;
    prefixChance = modifierChances[0].AsFloat;
    suffixChance = modifierChances[1].AsFloat;
  }

}
