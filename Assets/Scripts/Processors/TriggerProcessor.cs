using UnityEngine;
using System.Collections;

public class TriggerProcessor {

  Simulation sim;
  Trigger trigger;

  public TriggerProcessor (Simulation _sim, Trigger _trigger) {
    sim = _sim;
    trigger = _trigger;
  }

  public void Process () {
    if (trigger.type == Trigger.Type.NewFloor) {
      ChangeFloor();
    }

    if (trigger.type == Trigger.Type.PlayerStatChange) {
      ChangePlayerStat();
    }

    if (trigger.type == Trigger.Type.PlayerResourceChange) {
      ChangePlayerResource();
    }

  }

  void ChangeFloor () {
    //TODO: update player's floor and all associatd
    //stuff
//    sim.player.tower.floorNum += 1;
  }

  void ChangePlayerStat () {
    var statKey = (string)trigger.data[Trigger.statKey];
    var amount = (float)trigger.data[Trigger.statChangeAmountKey];

    sim.player.ChangeStat(statKey, amount);

    if (statKey == Stat.hp && amount < 0) {
      NotificationCenter.PostNotification(Constants.OnTakeDamage);
    }

    NotificationCenter.PostNotification(Constants.OnUpdateStats);
  }

  void ChangePlayerResource () {
    var resourceKey = (string)trigger.data[Trigger.resourceKey];
    int resourceAmount = (int)trigger.data[Trigger.resourceAmountKey];

    sim.player.ChangeResource(resourceKey, resourceAmount);

    NotificationCenter.PostNotification(Constants.OnUpdateStats);
  }
}
