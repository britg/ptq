using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {
  public const string type = "Simulation";

  public ResourceLoader resourceLoader;
  public Player player;

  public void Setup() {
    LoadResources();
    SetupPlayer();
  }

  void LoadResources () {
    resourceLoader = new ResourceLoader(this);
    resourceLoader.LoadResources();
  }

  void SetupPlayer () {
    var playerCreator = new PlayerCreator(this);
    playerCreator.Create();
  }

}
