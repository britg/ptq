using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public class EnvironmentProcessor {

  Simulation sim;
  List<PlayerEvent> newEvents;

  public EnvironmentProcessor (Simulation _sim) {
    sim = _sim;
  }

  public List<PlayerEvent> Enter () {
    newEvents = new List<PlayerEvent>();
    newEvents = Events(Constants.enterKey);
    return newEvents;
  }

  public List<PlayerEvent> Events (string group) {

    newEvents = new List<PlayerEvent>();

    foreach (var atmTxt in sim.currentEnvironment.events[group]) {
      if (DetectBranch(atmTxt)) {
        ExecuteBranch(atmTxt);
      } else {
        newEvents.Add(PlayerEvent.Story(atmTxt));
      }
    }

    return newEvents;
  }


  bool DetectBranch (string txt) {
    return tpd.BeginsWith(txt, Constants.branchLabel);
  }

  void ExecuteBranch (string key) {
    var branch = sim.currentEnvironment.GetBranch(key);
    var branchProcessor = new BranchProcessor(sim, branch);
    newEvents.AddRange(branchProcessor.Start());
  }

  public List<PlayerEvent> Explore () {
    var newEvents = new List<PlayerEvent>();

    if (sim.currentRoom != null) {
      var roomProcessor = new RoomProcessor(sim, sim.currentRoom);
      newEvents.AddRange(roomProcessor.Explore());
    }


    // If nothing is found, just pick a direction or use the last
    // direction you were going.


    return newEvents;
  }

}
