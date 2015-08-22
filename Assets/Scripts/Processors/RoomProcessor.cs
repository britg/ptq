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
    var roomTemplate = RoomTemplate.all[roomKey];

    List<PlayerEvent> newEvents = new List<PlayerEvent>();

    newEvents.Add (PlayerEvent.PromptChoice(roomTemplate.entranceChoice));

//    newEvents.Add (PlayerEvent.Info ("[Door choice atmosphere text]"));
//
//    var pullLeft = new Choice();
//    pullLeft.label = "Enter";
//    pullLeft.key = Choice.OpenDoor;
//    pullLeft.direction = Choice.Direction.Left;
//
//    var pullRight = new Choice();
//    pullRight.label = "Leave";
//    pullRight.key = Choice.LeaveDoor;
//    pullRight.direction = Choice.Direction.Right;
//
//    var msg = "You consider your next course of action...";
//    newEvents.Add(PlayerEvent.PromptChoice(msg, pullLeft, pullRight));

    return newEvents;
  }

  public List<PlayerEvent> OpenDoor () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Open door]"));
    sim.player.currentRoom = sim.player.currentFloor.RandomRoom();
    return newEvents;
  }

  public List<PlayerEvent> Leave () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Left Room]"));

    return newEvents;
  }

  public List<PlayerEvent> Continue () {
    List<PlayerEvent> newEvents = new List<PlayerEvent>();
    newEvents.Add (PlayerEvent.Info ("[Continue in the room]"));

    return newEvents;
  }
}
