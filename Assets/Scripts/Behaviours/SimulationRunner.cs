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
    //var floorProcessor = new FloorProcessor(sim);
    //floorProcessor.EnterFloor(1);
    // NotificationCenter.PostNotification(Constants.OnFloorUpdate);
  }

}
