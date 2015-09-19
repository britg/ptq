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
    env = Environment.Get<Environment>(name);

    GenerateFloor();
    GenerateRooms();
    PlacePlayer();
    AddStairs();

    return env;
  }

  void GenerateFloor () {
    env.floor = DunGen.Floor.Create();
    env.tiles = new Dictionary<Vector3, Tile>();
    for (var r = 0; r < env.floor.tiles.GetLength(0); r++) {
      for (var c = 0; c < env.floor.tiles.GetLength(1); c++) {
        var pos = new Vector3(c, 0, r);
        var tile = new Tile(pos);
        var tileType = env.floor.tiles[r, c];

        switch (tileType) {
        case DunGen.TileType.Perimeter:
        case DunGen.TileType.Blocked:
        case DunGen.TileType.Nothing:
          tile.contentType = Constants.wallContentKey;
          break;

        case DunGen.TileType.Door:
          tile.contentType = Constants.doorContentKey;
          break;
        }

        env.tiles[pos] = tile;
      }
    }
  }

  void GenerateRooms () {
    env.rooms = new List<Room>();
    foreach (DunGen.Room roomBase in env.floor.rooms) {
      GenerateRoom(roomBase);
    }
  }

  void GenerateRoom (DunGen.Room roomBase) {
    // pick a room template from the floor;
    var roomTemplateKey = tpd.RollMap(env.roomTemplateChances);
    var roomTemplate = JSONResource.Get<RoomTemplate>(roomTemplateKey);
    var roomGenerator = new RoomGenerator(env, roomTemplate, roomBase);
    env.rooms.Add(roomGenerator.CreateRoom());
  } 


  void PlacePlayer () {
    var randRoom = tpd.RollList<Room>(env.rooms);
    var randomTile = randRoom.RandomOpenTile();

    sim.player.position = randomTile.position;
    randomTile.Occupy(Constants.playerContentKey, sim.player.id);
    sim.currentRoom = randRoom;
  }

  void AddStairs () {

  }

}
