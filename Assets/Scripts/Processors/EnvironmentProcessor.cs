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

  public List<PlayerEvent> EnterEnvironment (string envName) {

    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);

    newEvents = new List<PlayerEvent>();
    newEvents = FloorEvents("_enter");
    return newEvents;
  }

  public List<PlayerEvent> FloorEvents (string group) {

    newEvents = new List<PlayerEvent>();

    foreach (var atmTxt in sim.environment.events[group]) {
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
    var branch = sim.environment.GetBranch(key);
    var branchProcessor = new BranchProcessor(sim, branch);
    newEvents.AddRange(branchProcessor.Start());
  }

  public List<PlayerEvent> Explore () {
    var newEvents = new List<PlayerEvent>();

    // Look around the room for stuff
    newEvents.Add(PlayerEvent.Info("exploring..."));

    return newEvents;
  }

}
