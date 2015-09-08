using UnityEngine;
using System.Collections;

public class MobGenerator {

  MobTemplate mobTemplate;
  Mob mob;

  public MobGenerator (MobTemplate _mobTemplate) {
    mobTemplate = _mobTemplate;
  }

  public Mob Generate () {
    mob = new Mob();
    mob.template = mobTemplate;
    mob.name = mobTemplate.name;

    // TODO: stats, loot chances, etc.

    return mob;
  }

}
