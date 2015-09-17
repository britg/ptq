using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplorationProcessor {

  Simulation sim;

  public static ExplorationProcessor With (Simulation sim) {
    return new ExplorationProcessor(sim);
  }

  public ExplorationProcessor (Simulation _sim) {
    sim = _sim;
  }

    public void Explore () {

    var targetTile = DiscoverNearestNewContent();

    if (targetTile == null) {
      ExploreRandomly();
      return;
    }

    if (targetTile.contentType == Constants.interactibleContentKey) {
      HandleInteractibleTile(targetTile);
    }

    if (targetTile.contentType == Constants.mobContentKey) {
      HandleMobTile(targetTile);
    }

  }

  Tile DiscoverNearestNewContent (string type = null) {
    var tiles = TilesToScan();
    Tile nearest = null;
    float dist = Mathf.Infinity;

    foreach (var tile in tiles) {
      bool anything = (type == null && tile.contentType != null);
      bool matches = (type != null && tile.contentType == type);
      if (anything || matches) {

        if (sim.discoveredCache.Contains(tile.contentId)) {
          continue;
        }

        if (tile.contentType == Constants.playerContentKey) {
          continue;
        }

        var tileDistance = Vector3.Distance(tile.position, sim.player.position);

        if (tileDistance > sim.player.sight) {
          continue;
        }

        if (tileDistance < dist) {
          nearest = tile;
        }
      }
    }

    return nearest;
  }

  List<Tile> TilesToScan () {
    if (sim.currentRoom != null) {
      return sim.currentRoom.tiles;
    }
    return new List<Tile>();
  }

  void ExploreRandomly () {

    // Pick an open tile within your periphery and set it as your target


  }

  void TargetDoor () {
    sim.AddEvent(PlayerEvent.Info("Nothing intersting... heading for door"));
  }

  void HandleInteractibleTile (Tile tile) {
    sim.AddEvent(PlayerEvent.Info("Found interactible"));
//    sim.player.currentDestination = tile.position;

    // var pathfindingProcessor = new PathfindingProcessor(sim);
    // pathfindingProcessor.NextTile(from, to)
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

}
