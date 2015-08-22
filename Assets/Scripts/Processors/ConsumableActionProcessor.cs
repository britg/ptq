using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsumableActionProcessor {

  Simulation sim;
  PlayerEvent ev;
  string actionKey;
  Consumable consumable;

  Equipment _eq;
  Equipment eq {
    get {
      if (_eq == null) {
        _eq = (Equipment)ev.data[PlayerEvent.equipmentKey];
      }
      return _eq;
    }
  }

  public ConsumableActionProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void HandleAction (PlayerEvent _ev, string _actionKey) {
    ev = _ev;
    actionKey = _actionKey;
    ev.chosenKey = actionKey;
    consumable = (Consumable)ev.data[PlayerEvent.consumableKey];

    if (actionKey == "consume") {
      Consume();
    }

    if (actionKey == "pickup") {
      PickUp();
    }

    NotificationCenter.PostNotification(Constants.OnUpdateStats);
    NotificationCenter.PostNotification(Constants.OnUpdateEvents);
  }

  void PickUp () {
    // TODO: Add to inventory
    ev.Content = "Picked Up";
  }

  void Consume () {
    foreach (KeyValuePair<string, float> statEffect in consumable.statEffects) {
      sim.player.ChangeStat(statEffect.Key, statEffect.Value);
    }

    ev.Content = consumable.usedName;
  }
}
