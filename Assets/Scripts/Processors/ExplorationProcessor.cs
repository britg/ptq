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

    // Pick an open tile within your periphery and set it as your target
    List<Vector3> dirs = new List<Vector3>() {
      Vector3.forward,
      Vector3.back,
      Vector3.left,
      Vector3.right
    };
    
    var randomDir = tpd.RollList(dirs);
    var destDelta = randomDir * sim.player.sight;

    sim.MovePlayer(randomDir);
  }

  public Tile DiscoverNearestNewContent (string type = null) {
    var tiles = TilesToScan();
    Tile nearest = null;
    float dist = Mathf.Infinity;

    foreach (var tile in tiles) {

      var tileDistance = Vector3.Distance(tile.position, sim.player.position);

      if (tileDistance > sim.player.sight) {
        continue;
      } else {
        sim.AddDiscoveredTile(tile.position);
        tile.Discover();
      }

      bool anything = (type == null && tile.contentType != null);
      bool matches = (type != null && tile.contentType == type);
      if (anything || matches) {


        if (sim.discoveredObjects.Contains(tile.contentId)) {
          continue;
        }

        if (tile.contentType == Constants.playerContentKey) {
          continue;
        }


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

  void TargetDoor () {
    sim.AddEvent(PlayerEvent.Info("Nothing intersting... heading for door"));
  }

  public void Pathfind () {

  }

}
