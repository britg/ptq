using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerGenerator {

  public const string PlayerSlotType = "PlayerSlot";

  Simulation sim;
  Player player;

  public PlayerGenerator (Simulation _sim) {
    sim = _sim;
  }

  public Player Generate () {
    player = new Player();
    GenerateStats();
    BootstrapAllSlots();

    return player;
  }

  void GenerateStats () {
    var startAtts = (JSONClass)Setting.instance.sourceData["player_start_attributes"];
    player.attributes = new Dictionary<string, float>();
    foreach (KeyValuePair<string, JSONNode> e in startAtts) {
      player.attributes[e.Key] = e.Value.AsFloat;
    }
  }
  
  void BootstrapAllSlots () {
    foreach (KeyValuePair<string, SlotType> p in SlotType.all) {
      var slot = new Slot(p.Key);
      player.Slots[p.Key] = slot;
    }
  }
}
