using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomProcessor  {

  Simulation sim;

  public RoomProcessor (Simulation _sim) {
    sim = _sim;
  }

  public List<PlayerEvent> DoorChoice () {

    // TODO: Roll for what type of room this is
    string roomKey = "standard"; 
    var roomTemplate = (RoomTemplate)RoomTemplate.cache[roomKey];

    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.PromptChoice(roomTemplate.entranceBranch));
    return newEvents;
  }

  public List<PlayerEvent> Enter () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Open door]"));
    sim.player.currentRoom = sim.player.currentFloor.RandomRoom();
    return newEvents;
  }

  public List<PlayerEvent> Continue () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Continue in the room]"));

    return newEvents;
  }
}
