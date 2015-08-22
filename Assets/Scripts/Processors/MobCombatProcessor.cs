using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobCombatProcessor {

  public const string basicAttack = "basic";

  Player player;
  Mob mob;

  public MobCombatProcessor (Player _player, Mob _mob) {
    player = _player;
    mob = _mob;
  }

  public List<PlayerEvent> TakeAction () {
    var newEvents = new List<PlayerEvent>();
    var action = Roll.Hash(mob.combatProfile);

    if (action == basicAttack) {
      newEvents.AddRange(BasicAttack());
    }
    
    return newEvents;
  }

  List<PlayerEvent> BasicAttack () {
    
    var newEvents = new List<PlayerEvent>();
    
    // Calc damage
    // TODO: Miss chance?
    // TODO: Crit chance
    // TODO: Adjust value up and down for def
    var damage = mob.GetStatValue(Stat.dps);

    var ev = new PlayerEvent();
    ev.type = PlayerEvent.Type.MobBasicAttack;
    ev.data[PlayerEvent.mobKey] = mob;
    ev.data[PlayerEvent.damageKey] = damage;

    var trigger = new Trigger(Trigger.Type.PlayerStatChange);
    trigger.data[Trigger.statKey] = Stat.hp;
    trigger.data[Trigger.statChangeAmountKey] = -damage;

    ev.Triggers.Add(trigger);

    newEvents.Add(ev);
    return newEvents;
  }
}
