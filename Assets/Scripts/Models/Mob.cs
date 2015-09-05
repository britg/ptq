using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mob {

  public MobTemplate template;
  public string name;
  public int level;

  public Vector3 position;

  public Dictionary<string, float> attributes;

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

    mob.combatProfile = template.combatProfile;

    return mob;
  }

}
