using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatProcessor  {

  public const string basicAttack = "basic";
  public const string specialAttack = "special";

  Simulation sim;
  Player player;
  Mob mob;

  public PlayerCombatProcessor (Simulation _sim) {
    sim = _sim;
    player = sim.player;
    mob = sim.currentMob;
  }

  public void TakeAction () {

    // Default action is to take a swing
    var chances = iTween.Hash(
      basicAttack, 75f/*,
      specialAttack, 25
      */
    );

    var chosen = tpd.RollMap(chances);

    if (chosen == basicAttack) {
      BasicAttack();
    }
  }

  void BasicAttack () {

    // Calc damage
    HitType hitType = HitType.Hit;
    var damage = AddVariance(player.dps);

    if (tpd.RollPercent(ChanceToMiss())) {
      hitType = HitType.Miss;
      damage = 0f;
    } else if (tpd.RollPercent(ChanceToGlance ())) {
      hitType = HitType.Glance;
      damage *= 0.5f;
    } else if (tpd.RollPercent(ChanceToCrit())) {
      hitType = HitType.Crit;
      damage *= 2f;
    }

    // TODO: Adjust value up and down for def
    //mob.ChangeStat(Stat.hp, -damage);

    var ev = new PlayerEvent();
    ev.type = PlayerEvent.Type.PlayerBasicAttack;
    ev.data[PlayerEvent.mobKey] = mob;
    ev.data[PlayerEvent.damageKey] = damage;
    ev.data[PlayerEvent.hitTypeKey] = hitType;

    sim.AddEvent(ev);
  }

  /*
   * Compare speeds
   */
  float ChanceToMiss () {
    var playerSpd = player.speed;
    //var mobSpd = mob.Stats[Stat.spd].current;
    var mobSpd = 10f;

    float baseChance = 10f;
    var diff = playerSpd - mobSpd;

    return baseChance - diff;
  }

  float ChanceToGlance () {
    var playerSpd = player.speed;
    //var mobSpd = mob.Stats[Stat.spd].current;
    var mobSpd = 10f;

    float baseChance = 20f;
    var diff = playerSpd - mobSpd;

    return baseChance - diff;
  }

  float ChanceToCrit () {
    var playerCrit = player.crit;
    // TODO: do some other stuff?

    return playerCrit;
  }

  float AddVariance (float damage) {
    var rand = Random.Range(-.2f, .2f);

    return damage + (damage * rand);
  }

}
