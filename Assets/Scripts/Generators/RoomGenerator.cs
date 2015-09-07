using UnityEngine;
using System.Collections;

public class RoomGenerator {

  Simulation sim;
  RoomTemplate roomTemplate;
  Environment env;
  DunGen.Room roomBase;

  public RoomGenerator (Simulation _sim, RoomTemplate _roomTemplate, DunGen.Room _roomBase) {
    sim = _sim;
    roomTemplate = _roomTemplate;
    env = sim.environment;
    roomBase = _roomBase;
  }

  public Room CreateRoom () {
    var room = new Room();
    room.roomTemplate = roomTemplate;
    room.content = roomTemplate.content;
    room.baseLayer = roomBase;

    GenerateContent();

    return room;
  }

  void GenerateContent () {
    
  }

}
