using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Player : AttributeBase {

  public Dictionary<string, Slot> Slots { get; set; }

  public Player () {
    Slots = new Dictionary<string, Slot>();
    encounteredMobKeys = new List<string>();
  }

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

}
