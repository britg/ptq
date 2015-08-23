using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomProcessor  {

  Simulation sim;
  RoomTemplate roomTemplate;
  Room currentRoom;

  public RoomProcessor (Simulation _sim, string roomTemplateKey) {
    sim = _sim;
    roomTemplate = (RoomTemplate)RoomTemplate.cache[roomTemplateKey];
  }

  public RoomProcessor (Simulation _sim, Room _currentRoom) {
    sim = _sim;
    currentRoom = _currentRoom;
  }

  public List<PlayerEvent> CreateAndEnter () {
    var roomGenerator = new RoomGenerator(sim, roomTemplate);
    sim.player.currentRoom = roomGenerator.CreateRoom();
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Chosen room template is " + roomTemplate.key));
    return newEvents;
  }

  public List<PlayerEvent> Continue () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Continue in the room]"));

    return newEvents;
  }
}
