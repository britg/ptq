using UnityEngine;
using System.Collections;

public class PlayerProcessor {

  Simulation sim;

  public PlayerProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void TakeTurn () {

    if (sim.newGame) {
      StartNewGame();
    }

    switch (sim.player.currentState) {

    case Player.State.Idling:
      BeginExploring();
      break;

    case Player.State.Exploring:
      Explore();
      break;

    case Player.State.Interacting:
      Interact();
      break;

    case Player.State.Pathfinding:
      Pathfind();
      break;

    case Player.State.Battling:
      Battle();
      break;
    }

  }

  void StartNewGame () {
    NotificationCenter.PostNotification(Constants.OnFirstPull);
    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);
    EnvironmentProcessor.With(sim).Enter();
    sim.MarkGameStarted();
  }

  void BeginExploring () {
    sim.AddEvent(PlayerEvent.Story(string.Format("{0} ventures forth...", sim.player.name)));
    sim.player.SetState(Player.State.Exploring);
    Explore();
  }

  void Explore () {
    var contentTile = ScanForNewContent();
    if (contentTile != null) {
      PromptContent(contentTile);
    } else {
      ExplorationProcessor.With(sim).Explore();
    }
  }

  void Pathfind () {
    var contentTile = ScanForNewContent();
    if (contentTile != null) {
      PromptContent(contentTile);
    } else {
      ExplorationProcessor.With(sim).Pathfind();
    }
  }

  Tile ScanForNewContent () {
    var tile = ExplorationProcessor.With(sim).DiscoverNearestNewContent();
    return tile;
  }

  void PromptContent (Tile targetTile) {
    if (targetTile.contentType == Constants.interactibleContentKey) {
      HandleInteractibleTile(targetTile);
    }

    if (targetTile.contentType == Constants.mobContentKey) {
      HandleMobTile(targetTile);
    }
  }

  void HandleInteractibleTile (Tile tile) {
    sim.AddEvent(PlayerEvent.Info("Found interactible"));
    var interactible = InteractibleStore.Find(tile.contentId);
    var prompt = string.Format("You see {0}", interactible.name);
    sim.AddEvent(PlayerEvent.PromptChoice(prompt,
                                          Choice.SwipeLeft("investigate", "Investigate"),
                                          Choice.SwipeRight("ignore", "Ignore")));
    sim.discoveredCache.Add(interactible.id);
  }

  void HandleMobTile (Tile tile) {
    //TODO: If the mob is low enough level, just go towards it.
    PromptMobChoices(tile);
  }

  void PromptMobChoices (Tile tile) {
    var mob = MobStore.Find(tile.contentId);
    var prompt = string.Format("You notice [{0}] before it notices you...", mob.name);
    sim.AddEvent(PlayerEvent.PromptChoice(prompt,
                                          Choice.SwipeLeft("attack", "Attack"),
                                          Choice.SwipeRight("ignore", "Ignore")));
    sim.discoveredCache.Add(mob.id);
  }

  void Interact () {
    var interactionProcessor = new InteractionProcessor(sim);
    interactionProcessor.Continue();
  }

  void Battle () {

  }

}
