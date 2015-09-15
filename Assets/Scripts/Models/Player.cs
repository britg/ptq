using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Player : AttributeBase {

  public string name = "Argent";

  public Dictionary<string, Slot> Slots { get; set; }

  public enum State {
    Idling,
    Exploring,
    Targeting,
    Interacting,
    Battling,
    Fleeing
  }

  public Player () {
    Slots = new Dictionary<string, Slot>();
  }

  public State currentState = State.Idling;
  public Vector3 currentDestination;
  public string lastBattleMove;

  public void SetState (State newState) {
    currentState = newState;
  }
}
