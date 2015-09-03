using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerRepository {

  Simulation sim;
  Player player;

  public PlayerRepository (Simulation _sim) {
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
    player.currentInitiative = 0;

    // player.currentFloor - currently loaded at InputProcessor->Continue
    // but once persistence happens, it should be loaded from here
  }

}
