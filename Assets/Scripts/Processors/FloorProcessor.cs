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

    sim.player.currentFloor = Floor.GetFloor(floorNum);

    // Either pull the floor map from persistence
    // or generate the map.
    GenerateFloor();
    NotificationCenter.PostNotification(Constants.OnFloorUpdate);

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

  void GenerateFloor () {
    var dunGen = new DunGen();
    sim.player.currentFloor.map = dunGen.CreateDungeon();
    PlacePlayer();
    AddStairs();
  }

  void PlacePlayer () {
    for (var r = 0; r < sim.player.currentFloor.map.GetLength(0); r++) {
      for (var c = 0; c < sim.player.currentFloor.map.GetLength(1); c++) {

      }
    }
  }

  void AddStairs () {
    
  }

}
