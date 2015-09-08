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

}
