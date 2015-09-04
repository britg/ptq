using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Player {

  public Vector3 position;
  public Dictionary<string, Slot> Slots { get; set; }

  public Dictionary<string, float> attributes;


  public Player () {
    Slots = new Dictionary<string, Slot>();
    encounteredMobKeys = new List<string>();
  }

  public void ChangeAttribute (string key, float delta) {
    attributes[key] += delta;
  }

  public int gold {
    get {
      return (int)attributes[Constants.goldAttr];
    }
  }

  public int level {
    get {
      return (int)attributes[Constants.levelAttr];
    }
  }

  public int currentHp {
    get {
      return (int)attributes[Constants.currentHpAttr];
    }
  }

  public int speed {
    get {
      return (int)attributes[Constants.speedAttr];
    }
  }

  public float dps {
    get {
      return (float)attributes[Constants.dpsAttr];
    }
  }

  public float crit {
    get {
      return (float)attributes[Constants.critAttr];
    }
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
