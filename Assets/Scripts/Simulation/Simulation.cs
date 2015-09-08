using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {
  public const string type = "Simulation";

  public ResourceLoader resourceLoader;
  public Player player;
  public Environment environment;
  public Room room;

  public List<PlayerEvent> recentEvents;

  public void Setup() {
    LoadResources();
    SetupPlayer();
    SetupEnvironment();
  }

  void LoadResources () {
    resourceLoader = new ResourceLoader(this);
    resourceLoader.LoadResources();
  }

  void SetupPlayer () {
    var playerRepo = new PlayerRepository(this);
    if (playerRepo.playerPersisted) {
      player = playerRepo.Load();
    } else {
      var playerGen = new PlayerGenerator(this);
      player = playerGen.Generate();
    }
  }

  void SetupEnvironment () {
    // TODO: Either pull the floor map from persistence
    // or generate the map.
    bool persisted = false;
    if (persisted) {

    } else {
      var envName = Setting.Get(Constants.startEnvKey).ToString();
      var envGenerator = new EnvironmentGenerator(this);
      environment = envGenerator.Generate(envName);
    }
  }

}
