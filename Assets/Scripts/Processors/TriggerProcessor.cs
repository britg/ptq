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
    if (trigger.type == Trigger.Type.NewEnvironment) {
      ChangeEnvironment();
    }

    if (trigger.type == Trigger.Type.PlayerStatChange) {
      ChangePlayerStat();
    }

    if (trigger.type == Trigger.Type.PlayerResourceChange) {
      ChangePlayerResource();
    }

  }

  void ChangeEnvironment () {
    //TODO: update player's floor and all associatd
    //stuff
//    sim.player.tower.floorNum += 1;
  }

  void ChangePlayerStat () {
    var attrKey = (string)trigger.data[Trigger.statKey];
    var amount = (float)trigger.data[Trigger.statChangeAmountKey];

    sim.player.ChangeAttribute(attrKey, amount);

    NotificationCenter.PostNotification(Constants.OnUpdateAttribute);
  }

  void ChangePlayerResource () {
    var resourceKey = (string)trigger.data[Trigger.resourceKey];
    int resourceAmount = (int)trigger.data[Trigger.resourceAmountKey];

    sim.player.ChangeAttribute(resourceKey, resourceAmount);

    NotificationCenter.PostNotification(Constants.OnUpdateAttribute);
  }
}
