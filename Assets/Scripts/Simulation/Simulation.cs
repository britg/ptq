using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {
  public const string type = "Simulation";

  public ResourceLoader resourceLoader;
  public Player player;

  public void Setup() {
    LoadMap();
    LoadResources();
    SetupPlayer();
  }

  void LoadMap () {
  }

  void LoadResources () {
    resourceLoader = new ResourceLoader(this);
    resourceLoader.LoadResources();
  }

  void SetupPlayer () {
    var playerRepo = new PlayerRepository(this);
    player = playerRepo.LoadOrCreate();
  }

}
