using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomProcessor  {

  Simulation sim;
  RoomTemplate roomTemplate;
  Room room;

  public RoomProcessor (Simulation _sim, Room _room) {
    sim = _sim;
    room = _room;
  }

  public void Explore () {

    var targetTile = DiscoverNearestNewContent();

    if (targetTile == null) {
      // look for the nearest door and go to it
      ResetRoomChoice();
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
    Tile nearest = null;
    float dist = Mathf.Infinity;

    foreach (var tile in room.tiles) {
      bool anything = (type == null && tile.contentType != null);
      bool matches = (type != null && tile.contentType == type);
      if (anything || matches) {

        if (sim.discoveredCache.Contains(tile.contentId)) {
          continue;
        }

        if (tile.contentType == Constants.playerContentKey) {
          continue;
        }

        if (Vector3.Distance(tile.position, sim.player.position) < dist) {
          nearest = tile;
        }
      }
    }

    return nearest;
  }

  void ResetRoomChoice () {
    var prompt = "There's nothing new in the room, revisit or leave?";
    sim.AddEvent(PlayerEvent.PromptChoice(prompt,
                                          Choice.SwipeLeft("revisit", "Revisit"),
                                          Choice.SwipeRight("leave", "Leave")));
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
