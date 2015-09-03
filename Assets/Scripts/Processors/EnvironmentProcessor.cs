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

    sim.player.currentEnv = Environment.GetEnv(envName);

    // TODO: Either pull the floor map from persistence
    // or generate the map.
    bool persisted = false;
    if (persisted) {
      LoadEnvironment();
    } else {
      GenerateEnvironment();
    }
    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);

    newEvents = new List<PlayerEvent>();
    newEvents = FloorEvents("_enter");
    return newEvents;
  }

  public List<PlayerEvent> FloorEvents (string group) {

    newEvents = new List<PlayerEvent>();

    foreach (var atmTxt in sim.player.currentEnv.events[group]) {
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
    var branch = sim.player.currentEnv.GetBranch(key);
    var branchProcessor = new BranchProcessor(sim, branch);
    newEvents.AddRange(branchProcessor.Start());
  }

  void LoadEnvironment () {

  }

  void GenerateEnvironment () {
    var dunGen = new DunGen();
    sim.player.currentEnv.map = dunGen.CreateDungeon();
    PlacePlayer();
    AddStairs();
  }

  void PlacePlayer () {
    Vector3 pos = sim.player.currentEnv.RandomOpenTile();
    sim.player.currentEnv.playerPos = pos;
  }

  void AddStairs () {

  }

  public List<PlayerEvent> Explore () {
    var newEvents = new List<PlayerEvent>();

    // Look around the room for stuff

    return newEvents;
  }

}
