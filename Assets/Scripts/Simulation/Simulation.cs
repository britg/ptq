using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {
  public const string type = "Simulation";

  public ResourceLoader resourceLoader;
  public Player player;
  public DunGen.TileType[,] tiles;

  public void Setup() {
    LoadMap();
    LoadResources();
    SetupPlayer();
  }

  void LoadMap () {
    var dunGen = new DunGen();
    tiles = dunGen.CreateDungeon();
  }

  void LoadResources () {
    resourceLoader = new ResourceLoader(this);
    resourceLoader.LoadResources();
  }

  void SetupPlayer () {
    var playerCreator = new PlayerRepository(this);
    playerCreator.Create();
  }

}
