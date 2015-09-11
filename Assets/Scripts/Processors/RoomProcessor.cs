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

  public List<PlayerEvent> Explore () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();

    var targetTile = DiscoverNearestNewContent();

    if (targetTile == null) {
      // look for the nearest door and go to it
    }

    if (targetTile.contentType == Constants.interactibleContentKey) {
      sim.player.currentDestination = targetTile.position;
    }

    // TODO: Look for nearest stairs
    // Look for nearest interactible
    // Look for nearest mob
    // Look for nearest door

    newEvents.Add(PlayerEvent.Movement(Vector3.forward));

    return newEvents;
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

        if (Vector3.Distance(tile.position, sim.player.position) < dist) {
          nearest = tile;
        }
      }
    }

    return nearest;
  }

  List<PlayerEvent> MobTilePrompt (Tile targetTile) {
    var promptEvents = new List<PlayerEvent>();

    var mob = MobRepository.Find(targetTile.contentId);
    var prompt = string.Format("You notice [{0}] before it notices you...", mob.name);
    promptEvents.Add(PlayerEvent.PromptChoice(prompt,
                                              Choice.SwipeLeft("attack", "Attack"),
                                              Choice.SwipeRight("ignore", "Ignore")));

    return promptEvents;
  }

}
