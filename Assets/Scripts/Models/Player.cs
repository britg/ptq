using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Player : AttributeBase {

  public string name = "Argent";

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
  public Vector3 currentDestination;
  public string lastBattleMove;

}
