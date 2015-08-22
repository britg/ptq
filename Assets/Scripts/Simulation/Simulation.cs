using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {

  public SimulationConfig config;

  public Player player;

  public void Setup() {
    LoadResources();
    SetupPlayer();
  }

  void LoadResources () {
    config = new SimulationConfig(this);
    config.LoadModels();
  }

  void SetupPlayer () {
    config.CreatePlayer();
  }

}
