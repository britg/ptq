using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour {

  Simulation sim;
  Environment env;

  public EnvironmentGenerator (Simulation _sim) {
    _sim = sim;
  }

  public Environment Generate (string name) {
    env = Environment.GetEnv(name);

    var dunGen = new DunGen();
    env.baseLayer = dunGen.CreateDungeon();

    PlacePlayer();
    AddStairs();

    return env;
  }

  void PlacePlayer () {
    Vector3 pos = env.RandomOpenTile();
    env.playerPos = pos;
  }

  void AddStairs () {

  }

}
