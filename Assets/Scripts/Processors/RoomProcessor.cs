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

    // Look for nearest stairs
    // Look for nearest interactible
    // Look for nearest mob
    // Look for nearest door

    newEvents.Add (PlayerEvent.Info ("[starting to move]"));
    newEvents.Add(PlayerEvent.Movement(Vector3.forward));
    newEvents.Add(PlayerEvent.Movement(Vector3.right));
    newEvents.Add(PlayerEvent.Movement(Vector3.back));
    newEvents.Add(PlayerEvent.Movement(Vector3.left));
    newEvents.Add (PlayerEvent.Info ("[done moving]"));

    return newEvents;
  }

}
