using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Player {

  public Vector3 position;

  public Dictionary<string, Resource> Resources { get; set; }
  public Dictionary<string, Stat> Stats { get; set; }
  public Dictionary<string, Slot> Slots { get; set; }

  public Dictionary<string, float> attributes;

  public Interactible currentInteractible;
  public Mob currentMob;
  public PlayerEvent currentEvent;
  public List<string> encounteredMobKeys;
  public float currentInitiative;
  public string lastBattleMove;

  public bool currentlyOccupied {
    get {
      return (currentInteractible != null 
        || currentMob != null
        || (currentEvent != null && currentEvent.blocksContinue)
      );
    }
  }

  public string currentChoiceKey {
    get {
      if (currentEvent == null || currentEvent.type != PlayerEvent.Type.Choice) {
        return null;
      } else {
        return currentEvent.chosenKey;
      }
    }
  }

  public Player () {
    Resources = new Dictionary<string, Resource>();
    Stats = new Dictionary<string, Stat>();
    Slots = new Dictionary<string, Slot>();
    encounteredMobKeys = new List<string>();
  }

  public Stat GetStat (string key) {
    var playerStat = new Stat(key, 0f);
    if (Stats.ContainsKey(key)) {
      playerStat = Stats[key];
    } else {
      Stats[key] = playerStat;
    }

    return playerStat;
  }

  public float GetStatValue (string key) {
    var stat = GetStat(key);
    return stat.current;
  }

  public void ChangeStat (string key, float amount) {
    var s = GetStat(key);
    s.Change(amount);
    Stats[key] = s;
  }

  public void ChangeResource (string key, int amount) {
    var r = Resources[key];
    r.Amount += amount;
    Resources[key] = r;
  }

}
