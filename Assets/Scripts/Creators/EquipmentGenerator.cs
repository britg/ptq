using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentGenerator {

  Simulation sim;

  public EquipmentGenerator (Simulation _sim) {
    sim = _sim;
  }

  public Equipment Generate () {
    List<string> keyList = new List<string>(EquipmentType.all.Keys);
    var randInt = Random.Range(0, keyList.Count - 1);
    var eqTypeKey = keyList[randInt];
    return Generate(eqTypeKey);
  }

  public Equipment Generate (string eqTypeKey) {
    var eqType = EquipmentType.all[eqTypeKey];
    return Generate(eqType);
  }

  public Equipment Generate (EquipmentType type) {
    var e = new Equipment();
    e.Type = type;
    e.SlotType = type.PrimarySlotType;
    e.Name = type.Name;
    var rarityGen = new RarityGenerator(sim);
    e.Rarity = rarityGen.Roll();
    AssignAttributes(e);

    return e;
  }

  void AssignAttributes (Equipment e) {

    // Get base stat from equipment designation
    foreach (KeyValuePair<string, StatType> pair in e.Designation.BaseStats) {
      var statKey = pair.Key;
      var statType = pair.Value;

      float val = BaseStatValue(statKey);

      float eqTypeMultiplier = EquipmentTypeMultiplier(e.Type, statKey);
      val *= eqTypeMultiplier;

      float rarityMultiplier = RarityMultiplier(e.Rarity, Stat.SignForValue(val));
      val *= rarityMultiplier;

      // Prefix
      var prefixRoll = Roll.Percent(e.Rarity.prefixChance);
      if (prefixRoll) {
        e.Prefix = EquipmentModifier.Prefix();
        ApplyModifier(e, e.Prefix);
      }

      // Suffix
      var suffixRoll = Roll.Percent(e.Rarity.suffixChance);
      if (suffixRoll) {
        e.Suffix = EquipmentModifier.Suffix();
        ApplyModifier(e, e.Suffix);
      }

      e.Stats[statType.Key] = new Stat(statType.Key, val);
    }
  }

  float BaseStatValue (string statKey) {
    float lvl = sim.player.GetStatValue(Stat.lvl);
    // TODO: Somehow get base values.
    float val = Random.Range(lvl, lvl + 1f);
    return val;
  }

  float EquipmentTypeMultiplier (EquipmentType eqType, string statKey) {
    return eqType.MultiplierForStat(statKey);
  }

  float RarityMultiplier (Rarity rarity, Stat.Sign sign) {
    var rarityMult = Roll.Range(rarity.statMultiplierRange);
    if (sign == Stat.Sign.Negative) {
      rarityMult = 1f / rarityMult;
    }
    return rarityMult;
  }

  EquipmentModifier PrefixGenerator () {
    var prefix = EquipmentModifier.Prefix();
    return prefix;
  }

  void ApplyModifier (Equipment e, EquipmentModifier mod) {
    // Apply stats
    // Chang name
  }

}
