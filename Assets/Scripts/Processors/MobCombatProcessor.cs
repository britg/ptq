using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobCombatProcessor {

  public const string basicAttack = "basic";

  Simulation sim;
  Player player;
  Mob mob;

  public MobCombatProcessor (Simulation _sim) {
    sim = _sim;
    player = sim.player;
    mob = sim.currentMob;
  }

  public void TakeAction () {
    var action = tpd.RollMap(mob.combatProfile);

    if (action == basicAttack) {
      BasicAttack();
    }
    
  }

  void BasicAttack () {
    
    // Calc damage
    // TODO: Miss chance?
    // TODO: Crit chance
    // TODO: Adjust value up and down for def
    //var damage = mob.GetStatValue(Stat.dps);
    var damage = 10f;

    var ev = new PlayerEvent();
    ev.type = PlayerEvent.Type.MobBasicAttack;
    ev.data[PlayerEvent.mobKey] = mob;
    ev.data[PlayerEvent.damageKey] = damage;

    var trigger = new Trigger(Trigger.Type.PlayerStatChange);
    //trigger.data[Trigger.statKey] = Stat.hp;
    trigger.data[Trigger.statChangeAmountKey] = -damage;

    ev.Triggers.Add(trigger);

    sim.AddEvent(ev);
  }
}
