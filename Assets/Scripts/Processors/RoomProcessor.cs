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
    newEvents.Add (PlayerEvent.Info ("[Continue in the room]"));

    return newEvents;
  }

}
