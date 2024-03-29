﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerStore {

  Simulation sim;
  Player player;

  public PlayerStore (Simulation _sim) {
    sim = _sim;
  }

  public bool playerPersisted = false;

  public Player Load () {
    player = new Player();
    LoadState();
    return player;
  }

  // TODO: Load from real persistence
  void LoadState () {
    player.initiative = 0;

    // player.currentFloor - currently loaded at InputProcessor->Continue
    // but once persistence happens, it should be loaded from here
  }

}
