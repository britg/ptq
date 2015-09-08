using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentGenerator {

  Simulation sim;
  Environment env;

  public EnvironmentGenerator (Simulation _sim) {
    sim = _sim;
  }

  public Environment Generate (string name) {
    env = Environment.GetEnv(name);

    GenerateFloor();
    PopulateRooms();
    PlacePlayer();
    AddStairs();

    return env;
  }

  void GenerateFloor () {
    env.floor = DunGen.Floor.Create();
  }

  void PopulateRooms () {
    env.rooms = new List<Room>();
    foreach (DunGen.Room roomBase in env.floor.rooms) {
      PopulateRoom(roomBase);
    }
  }

  void PopulateRoom (DunGen.Room roomBase) {
    // pick a room template from the floor;
    var roomTemplateKey = tpd.RollMap(env.roomTemplateChances);
    var roomTemplate = (RoomTemplate)JSONResource.cache[roomTemplateKey];
    var roomGenerator = new RoomGenerator(sim, roomTemplate, roomBase);
    env.rooms.Add(roomGenerator.CreateRoom());
  } 


  void PlacePlayer () {
    var randRoom = tpd.RollList<Room>(env.rooms);
    var randomTile = tpd.RollList<Vector3>(randRoom.baseLayer.tiles);
    //Vector3 pos = env.RandomOpenTile();
    //sim.player.position = pos;
  }

  void AddStairs () {

  }

}
