using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchProcessor {

  Simulation sim;
  Branch branch;

  public BranchProcessor (Simulation _sim) {
    sim = _sim;
    branch = sim.currentBranch;
  }

  public void Start () {
    var ev = PlayerEvent.PromptChoice(branch);
    sim.AddEvent(ev);
  }

  public void Choose (string choiceKey) {
    var fullChoiceKey = "BranchResult-" + choiceKey;
    var res = branch.results[fullChoiceKey];
    foreach (string evTxt in res.events) {
      sim.AddEvent(PlayerEvent.Story(evTxt));
    }

    if (res.thenToContinue) {
      sim.player.SetState(Player.State.Idling);
      sim.currentBranch = null;
      return;
    }

    if (res.thenToEvents) {
      var interactionProcessor = new InteractionProcessor(sim);
      var eventsKey = tpd.RemoveSubString(res.thenTo, "event_group:");
      interactionProcessor.CreateEvents(eventsKey);
    }

    if (res.thenToPromptPull) {
      sim.promptPull = true;
      return;
    }
  }

}
