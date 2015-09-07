using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Player : AttributeBase {

  public enum Purpose {
    Explore,
    HeadToTarget,
    Flee
  }

  public Dictionary<string, Slot> Slots { get; set; }

  public Player () {
    Slots = new Dictionary<string, Slot>();
  }

  public Purpose purpose = Purpose.Explore;
  public Vector3 currentTarget;
  public Interactible currentInteractible;
  public Mob currentMob;
  public PlayerEvent currentEvent;
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
