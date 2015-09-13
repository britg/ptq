using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {
  public const string type = "Simulation";

  public ResourceLoader resourceLoader;

  public Player player;
  public Turn.Type currentTurn;
  public Environment currentEnvironment;
  public Room currentRoom;
  public Mob currentMob;
  public Interactible currentInteractible;

  public List<PlayerEvent> recentEvents;
  public List<PlayerEvent> newEvents;

  public List<string> discoveredCache;

  public bool newGame {
    get {
      return currentEvent == null;
    }
  }

  public PlayerEvent currentEvent {
    get {
      if (newEvents.Count > 0) {
        return newEvents[newEvents.Count - 1];
      }

      if (recentEvents.Count > 0) {
        return recentEvents[recentEvents.Count - 1];
      }
      return null;
    }
  }

  public string currentChoiceKey {
    get {
      if (currentEvent != null) {
        return currentEvent.chosenKey;
      }
      return null;
    }
  }

  public bool canContinue {
    get {
      if (currentEvent == null) {
        return true;
      }
      return currentEvent.conditionsSatisfied;
    }
  }

  public bool shouldExplore {
    get {
      return (currentInteractible == null 
        && currentMob == null
        && (currentEvent != null && !currentEvent.blocksContinue)
      );
    }
  }

  public void Setup() {
    LoadResources();
    LoadDiscoveredCache();
    LoadEvents();
    LoadTurn();
    SetupPlayer();
    SetupEnvironment();
  }

  void LoadResources () {
    resourceLoader = new ResourceLoader(this);
    resourceLoader.LoadResources();
  }

  void LoadDiscoveredCache () {
    discoveredCache = new List<string>();
    // TODO: load discovered cache from persistence
  }

  void LoadEvents () {
    recentEvents = new List<PlayerEvent>();
    newEvents = new List<PlayerEvent>();

    // TODO: Load some events from persistence
  }

  void LoadTurn () {
    currentTurn = Turn.Type.Player;
    // TODO remember and load from persistence
  }

  void SetupPlayer () {
    var playerRepo = new PlayerStore(this);
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
      currentEnvironment = envGenerator.Generate(envName);
    }
  }

  public void AddEvent (PlayerEvent ev) {

  }

  public void FlushNewEvents () {
    recentEvents.AddRange(newEvents);
    newEvents = new List<PlayerEvent>();
  }

}
