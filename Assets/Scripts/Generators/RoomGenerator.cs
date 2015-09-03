using UnityEngine;
using System.Collections;

public class RoomGenerator {

  Simulation sim;
  RoomTemplate roomTemplate;
  Environment env;



  public RoomGenerator (Simulation _sim, RoomTemplate _roomTemplate) {
    sim = _sim;
    roomTemplate = _roomTemplate;
    env = sim.environment;
  }

  public Room CreateRoom () {
    var room = new Room();
    room.roomTemplate = roomTemplate;
    room.content = room.roomTemplate.CascadeContent(env.content);
    return room;
  }

}
