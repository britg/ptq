using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mob {

  public MobTemplate template;
  public string name;
  public int level;

  public Dictionary<string, Stat> Stats { get; set; }
  public Hashtable combatProfile;
  public float currentInitiative;
  public float consumableChance;
  public float lootChance;
  public float goldChance;

  public static Mob FromTemplate (MobTemplate template) {
    var mob = new Mob();

    mob.template = template;
    mob.level = template.level;
    mob.name = template.name;
    mob.consumableChance = template.consumableChance;
    mob.lootChance = template.lootChance;
    mob.goldChance = template.goldChance;

    mob.Stats = new Dictionary<string, Stat>();
    foreach (KeyValuePair<string, RangeAttribute> pair in template.StatRanges) {
      var statKey = pair.Key;
      var range = pair.Value;
      mob.Stats[statKey] = new Stat(statKey, Random.Range(range.min, range.max));
    }
    mob.combatProfile = template.combatProfile;

    return mob;
  }

  public Stat GetStat (string key) {
    var mobStat = new Stat(key, 0f);
    if (Stats.ContainsKey(key)) {
      mobStat = Stats[key];
    } else {
      Stats[key] = mobStat;
    }

    return mobStat;
  }

  public float GetStatValue (string key) {
    var stat = GetStat(key);
    return stat.current;
  }

  public void ChangeStat (string key, float amount) {
    var s = GetStat(key);
    s.Change(amount);
  }

}
