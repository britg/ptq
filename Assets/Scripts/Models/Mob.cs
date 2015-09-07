using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mob : AttributeBase {

  public MobTemplate template;
  public string name;

  public Hashtable combatProfile;
  public float consumableChance;
  public float lootChance;
  public float goldChance;

  public static Mob FromTemplate (MobTemplate template) {
    var mob = new Mob();

    mob.template = template;
    mob.attributes[Constants.levelAttr] = template.level;
    mob.name = template.name;
    mob.consumableChance = template.consumableChance;
    mob.lootChance = template.lootChance;
    mob.goldChance = template.goldChance;

    mob.combatProfile = template.combatProfile;

    return mob;
  }

}
