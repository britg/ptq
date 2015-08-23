﻿using UnityEngine;
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

    sim.player.currentFloor = Floor.GetFloor(floorNum);

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
    Debug.Log("branch is " + txt + " " + txt.Substring(0, 6));
    return txt.Substring(0, 7) == "branch:";
  }

  void ExecuteBranch (string key) {
    var branch = sim.player.currentFloor.GetBranch(key);
    var branchProcessor = new BranchProcessor(sim, branch);
    newEvents.AddRange(branchProcessor.Start());
  }

}
