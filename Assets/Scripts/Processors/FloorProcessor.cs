using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public class FloorProcessor {

  Simulation sim;
  List<PlayerEvent> newEvents;

  public FloorProcessor (Simulation _sim) {
    sim = _sim;
  }

  public List<PlayerEvent> EnterFloor (int floorNum) {

    sim.player.currentFloor = Floor.all[floorNum];

    newEvents = new List<PlayerEvent>();

    foreach (var atmTxt in sim.player.currentFloor.entranceEvents) {
      if (DetectBranch(atmTxt)) {
        ExecuteBranch(atmTxt);
      } else {
        newEvents.Add(PlayerEvent.Story(atmTxt));
      }
    }
    return newEvents;
  }

  bool DetectBranch (string txt) {
    return txt.Substring(0, 2) == "__";
  }

  void ExecuteBranch (string key) {
    JSONNode branch = sim.player.currentFloor.branches[key];
    newEvents.Add(PlayerEvent.PromptChoice(branch));
  }

}
