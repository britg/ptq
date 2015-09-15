using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public class EnvironmentProcessor {

  Simulation sim;

  public EnvironmentProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void Enter () {
    sim.player.SetState(Player.State.Interacting);
    Events(Constants.enterKey);
  }

  public void Events (string group) {

    foreach (var atmTxt in sim.currentEnvironment.events[group]) {
      if (DetectBranch(atmTxt)) {
        ExecuteBranch(atmTxt);
      } else {
        sim.AddEvent(PlayerEvent.Story(atmTxt));
      }
    }
  }


  bool DetectBranch (string txt) {
    return tpd.BeginsWith(txt, Constants.branchLabel);
  }

  void ExecuteBranch (string key) {
    var branch = sim.currentEnvironment.GetBranch(key);
    sim.currentBranch = branch;
    var branchProcessor = new BranchProcessor(sim);
    branchProcessor.Start();
  }

  public void Explore () {

    if (sim.currentRoom != null) {
      var roomProcessor = new RoomProcessor(sim, sim.currentRoom);
      roomProcessor.Explore();
    }

    // If nothing is found, just pick a direction or use the last
    // direction you were going.
  }

}
