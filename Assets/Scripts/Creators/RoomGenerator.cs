using UnityEngine;
using System.Collections;

public class RoomGenerator {

  Simulation sim;
  RoomTemplate roomTemplate;
  Floor floor;



  public RoomGenerator (Simulation _sim, RoomTemplate _roomTemplate) {
    sim = _sim;
    roomTemplate = _roomTemplate;
    floor = sim.player.currentFloor;
  }

  public Room CreateRoom () {
    var room = new Room();
    room.roomTemplate = roomTemplate;
    room.content = room.roomTemplate.CascadeContent(floor.content);
    return room;
  }

}
