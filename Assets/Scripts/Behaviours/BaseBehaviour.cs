using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBehaviour : MonoBehaviour {

  SimulationRunner _runner;
  SimulationRunner runner {
    get {
      if (_runner == null) {
        _runner = GameObject.Find("Simulation").GetComponent<SimulationRunner>();
      }
      return _runner;
    }
  }

  Simulation _sim;
  protected Simulation sim {
    get {
      if (_sim == null) {
        _sim = runner.GetSim();
      }
      return _sim;
    }
  }

  protected void Pause () {
    runner.Pause();
  }

}
