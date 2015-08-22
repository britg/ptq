using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {

  public SimulationConfig config;

  public Map map;
  public Player player;
  public List<Quest> questList = new List<Quest>();

  public InputProcessor inputProcessor;

  float previousSpeed;
  public float CurrentSpeed { get; set; }
  public GameTime GameTime { get; set; }
  public float UpdateDelta { get; set; }

  public void Setup() {
    SetupConfig();
    inputProcessor = new InputProcessor(this);
    SetupGameTime();
    SetupMap();
    SetupPlayer();
    SetupProcessors();
  }

  void SetupConfig () {
    config = new SimulationConfig(this);
    config.LoadModels();
  }

  void SetupGameTime() {
    CurrentSpeed = config.initialSpeed;
    GameTime = new GameTime(config);
  }

  void SetupMap () {
    map = new Map();
  }

  void SetupPlayer () {
    config.CreatePlayer();
  }

  void SetupProcessors () {

  }

  public void Start() {
  }

  public void Update(float deltaTime) {
    UpdateDelta = deltaTime * CurrentSpeed;
    GameTime.AddTime(UpdateDelta);

   
  }


  public void Pause() {
    previousSpeed = CurrentSpeed;
    CurrentSpeed = 0f;
  }

  public void Resume() {
    CurrentSpeed = previousSpeed;
  }

  public void AdjustCurrentSpeed(float value) {
    CurrentSpeed = value;
  }

  public void CreateExplorationQuest (Vector3 location) {
  }

}
