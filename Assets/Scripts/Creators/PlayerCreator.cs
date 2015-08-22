using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerCreator {

  public const string PlayerStatType = "PlayerStat";
  public const string PlayerSlotType = "PlayerSlot";
  public const string PlayerResourceType = "PlayerResource";

  Simulation sim;
  Player player;

  public PlayerCreator (Simulation _sim) {
    sim = _sim;
  }

  public Player Create () {
    player = new Player();
    sim.player = player;
    Bootstrap();
    return player;
  }

  public void Bootstrap () {
    BootstrapResources();
    BootstrapStats();
    BootstrapAllSlots();
  }

  void BootstrapResources () {
    List<JSONNode> resourcesToLoad = sim.config.jsonCache[PlayerResourceType];
    foreach (JSONNode playerResource in resourcesToLoad) {
      var resourceKey = playerResource["resource_key"].Value;
      var amount = playerResource["amount"].AsFloat;
      var resource = new Resource(resourceKey, amount);
      player.Resources[resourceKey] = resource;
    }
  }

  void BootstrapStats () {
    List<JSONNode> statsToLoad = sim.config.jsonCache[PlayerStatType];
    foreach (JSONNode playerStat in statsToLoad) {
      var statKey = playerStat["stat_key"].Value;

      var current = playerStat["current"].AsFloat;
      var min = playerStat["min"].AsFloat;
      var max = playerStat["max"].AsFloat;

      var stat = new Stat(statKey, min, max, current);
      player.Stats[statKey] = stat;
    }
  }

  void BootstrapSlots () {
    List<JSONNode> slotsToLoad = sim.config.jsonCache[PlayerSlotType];
    foreach (JSONNode playerSlot in slotsToLoad) {
      var slotKey = playerSlot["slot_key"].Value;
      var slot = new Slot(slotKey);
      player.Slots[slotKey] = slot;
    }
  }

  void BootstrapAllSlots () {
    foreach (KeyValuePair<string, SlotType> p in SlotType.all) {
      var slot = new Slot(p.Key);
      player.Slots[p.Key] = slot;
    }
  }

}
