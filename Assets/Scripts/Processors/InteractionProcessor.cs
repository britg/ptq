using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionProcessor {

  Simulation sim;

  public InteractionProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void Start () {
    CreateEvents(Constants.enterKey);
  }

  public void Continue () {
  }

  public void CreateEvents (string group) {

    var eventGroup = sim.currentInteraction.GetEventGroup(group);
    foreach (var atmTxt in eventGroup) {
      if (DetectBranch(atmTxt)) {
        ExecuteBranch(atmTxt);
      } else {
        sim.AddEvent(PlayerEvent.Story(atmTxt));
      }
    }
  }


  bool DetectBranch (string txt) {
    return tpd.BeginsWith(txt, Constants.branchLabel);
  }

  void ExecuteBranch (string key) {
    var branch = sim.currentInteraction.GetBranch(key);
    sim.currentBranch = branch;
    var branchProcessor = new BranchProcessor(sim);
    branchProcessor.Start();
  }

}
