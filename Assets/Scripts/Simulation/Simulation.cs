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
  public Interaction currentInteraction;
  public Branch currentBranch;
  public string currentChoiceKey;

  string currentEventId;
  public List<PlayerEvent> newEvents;

  public List<string> discoveredCache;

  public bool requiresInput = false;
  public bool promptPull = false;

  public bool newGame {
    get {
      return currentEvent == null;
    }
  }

  public PlayerEvent currentEvent {
    get {
      if (currentEventId != null) {
        return EventStore.Find(currentEventId);
      }
      return null;
    }
  }

  public void Setup() {
    LoadResources();
    LoadDiscoveredCache();
    LoadEvents();
    LoadTurn();
    LoadPlayer();
    LoadEnvironment();
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
    newEvents = new List<PlayerEvent>();

    // TODO: Load some events from persistence
  }

  void LoadTurn () {
    currentTurn = Turn.Type.Player;
    // TODO remember and load from persistence
  }

  void LoadPlayer () {
    var playerStore = new PlayerStore(this);
    if (playerStore.playerPersisted) {
      player = playerStore.Load();
    } else {
      var playerGen = new PlayerGenerator(this);
      player = playerGen.Generate();
    }
  }

  void LoadEnvironment () {
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
    currentEventId = ev.Id;
    newEvents.Add(ev);

    if (ev.requiresInput) {
      requiresInput = true;
    }

    EventStore.Save(ev);
  }

  public void FlushNewEvents () {
    newEvents = new List<PlayerEvent>();
  }

  public void EndPlayerTurn () {
    currentTurn = Turn.Type.Game;
  }

  public void EndGameTurn () {
    currentTurn = Turn.Type.Player;
  }
}
