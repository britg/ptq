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

    foreach (DunGen.Room roomBase in env.floor.rooms) {
      PopulateRoom(roomBase);
    }
    //env.activeLayer = new Hashtable();

    //int i = 0;
    //List<int> toRemove = new List<int>();
    //foreach(var pos in env.openTiles) {
    //  string contentKey = tpd.RollMap(env.content);
    //  if (contentKey == Constants.nothingContentKey) {
    //    continue;
    //  }

    //  if (contentKey == Constants.mobContentKey) {
    //    env.activeLayer[pos] = contentKey; // TODO: actually generate the mob here?
    //    toRemove.Add(i);
    //  } else if (contentKey == Constants.interactibleContentKey) {
    //    env.activeLayer[pos] = contentKey; // TODO: actually generate the mob here?
    //    toRemove.Add(i);
    //  }

    //  ++i;
    //}

    //foreach (int j in toRemove) {
    //  env.openTiles.RemoveAt(j);
    //}
  }

  void PopulateRoom (DunGen.Room roomBase) {

  }


  void PlacePlayer () {
    //Vector3 pos = env.RandomOpenTile();
    //sim.player.position = pos;
  }

  void AddStairs () {

  }

}
