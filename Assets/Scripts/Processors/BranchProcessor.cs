using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchProcessor {

  Simulation sim;
  Branch branch;

  public BranchProcessor (Simulation _sim, Branch _branch) {
    sim = _sim;
    branch = _branch;
  }

  public BranchProcessor (Simulation _sim, PlayerEvent ev) {
    sim = _sim;
    branch = (Branch)ev.data[PlayerEvent.branchKey];
  }

  public List<PlayerEvent> Start () {
    var ev = PlayerEvent.PromptChoice(branch);
    return new List<PlayerEvent>() { ev };
  }

  public List<PlayerEvent> Choose (string choiceKey) {
    var newEvents = new List<PlayerEvent>();
    var res = branch.results[choiceKey];
    foreach (string evTxt in res.events) {
      newEvents.Add(PlayerEvent.Story(evTxt));
    }

    return newEvents;
  }

}
