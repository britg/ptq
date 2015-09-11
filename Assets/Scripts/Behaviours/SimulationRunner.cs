using UnityEngine;
using System.Collections;

public class SimulationRunner : MonoBehaviour {

  public Simulation sim;

  void Awake () {
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = 60;
    sim = new Simulation();
    sim.Setup();
  }

  void Start () {
    NotificationCenter.AddObserver(this, Constants.OnUpdateFeedDone);
  }

  void OnUpdateFeedDone () {
    sim.FlushNewEvents();
  }

}
