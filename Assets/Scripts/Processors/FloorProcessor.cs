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

    // TODO: Either pull the floor map from persistence
    // or generate the map.
    GenerateFloor();
    NotificationCenter.PostNotification(Constants.OnFloorUpdate);

    newEvents = new List<PlayerEvent>();
    newEvents = FloorEvents("_enter");
    return newEvents;
  }

  public List<PlayerEvent> FloorEvents (string group) {

    newEvents = new List<PlayerEvent>();

    foreach (var atmTxt in sim.player.currentFloor.events[group]) {
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
    Vector3 pos = sim.player.currentFloor.RandomOpenTile();
    sim.player.currentFloor.playerPos = pos;
  }

  void AddStairs () {

  }

  public List<PlayerEvent> Explore () {
    var newEvents = new List<PlayerEvent>();

    // Look around the room for stuff

    return newEvents;
  }

}
