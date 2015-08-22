using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentActionProcessor {

  Simulation sim;
  PlayerEvent ev;
  string actionKey;

  Equipment _eq;
  Equipment eq {
    get {
      if (_eq == null) {
        _eq = (Equipment)ev.data[PlayerEvent.equipmentKey];
      }
      return _eq;
    }
  }

  public EquipmentActionProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void HandleAction (PlayerEvent _ev, string _actionKey) {
    ev = _ev;
    actionKey = _actionKey;

    if (actionKey == Constants.c_Pickup) {
      ev.chosenKey = Constants.c_Pickup;
      PickUp(ev);
    } else if (actionKey == Constants.c_Equip) {
      ev.chosenKey = Constants.c_Equip;
      Equip(ev);
    }
  }

  public void PickUp (PlayerEvent ev) {
    Debug.Log ("Loot processor is picking up " + eq.Name);
  }

  public void Equip (PlayerEvent ev) {
    Debug.Log ("Loot processor is equipping " + eq.Name);
    SlotType slotType = eq.SlotType;
    Slot playerSlot = sim.player.Slots[slotType.Key];
    var prevEquipment = playerSlot.Equipment;
    playerSlot.Equipment = eq;

    SubtractPrevEquipment(prevEquipment);
    AddNewEquipment(eq);

    NotificationCenter.PostNotification(Constants.OnUpdateStats);
  }

  void SubtractPrevEquipment (Equipment prev) {
    if (prev == null) {
      return;
    }

    foreach (KeyValuePair<string, Stat> pair in prev.Stats) {
      var stat = pair.Value;
      var playerStat = sim.player.GetStat(stat.Key);
      playerStat.Change(-stat.current);
    }
  }

  void AddNewEquipment (Equipment eq) {
    foreach (KeyValuePair<string, Stat> pair in eq.Stats) {
      var stat = pair.Value;
      var playerStat = sim.player.GetStat(stat.Key);
      playerStat.Change(stat.current);
    }
  }
}
