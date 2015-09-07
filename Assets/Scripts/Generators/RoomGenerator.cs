using UnityEngine;
using System.Collections;

public class RoomGenerator {

  Simulation sim;
  RoomTemplate roomTemplate;
  Environment env;
  DunGen.Room roomBase;

  public RoomGenerator (Simulation _sim, RoomTemplate _roomTemplate, DunGen.Room _roomTiles) {
    sim = _sim;
    roomTemplate = _roomTemplate;
    env = sim.environment;
    roomBase = _roomTiles;
  }

  public Room CreateRoom () {
    var room = new Room();
    room.roomTemplate = roomTemplate;
    room.content = roomTemplate.content;
    return room;
  }

}
