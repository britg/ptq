using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionProcessor {

  Simulation sim;
  Branch branch;

  public InteractionProcessor (Simulation _sim) {
    sim = _sim;
    branch = sim.currentBranch;
  }

  public void Start () {
    CreateEvents(Constants.enterKey);
  }

  public void Continue () {
    if (sim.currentChoiceKey != null) {
      ExecuteChoice(sim.currentChoiceKey);
      sim.currentChoiceKey = null;
    }
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

    var ev = PlayerEvent.PromptChoice(branch);
    sim.AddEvent(ev);
  }

  public void StoreChoice (string choiceKey) {
    // TODO: Refactor into a choice processor when necessary
    var ev = sim.currentEvent;
    ev.chosenKey = choiceKey;
    sim.currentChoiceKey = choiceKey;
    sim.requiresInput = false;
  }

  public void ExecuteChoice (string choiceKey) {

    var fullChoiceKey = "BranchResult-" + choiceKey;
    var res = branch.results[fullChoiceKey];

    foreach (string evTxt in res.events) {
      sim.AddEvent(PlayerEvent.Story(evTxt));
    }

    // TODO: Parse loot key

    if (res.thenToEventGroup) {
      var eventsKey = tpd.RemoveSubString(res.thenTo, Constants.eventGroupLabel);
      CreateEvents(eventsKey);
    }
    
    if (res.thenToPromptPull) {
      sim.PromptPull();
    }

    if (res.thenToEndInteraction) {
      sim.PromptPull();
      End();
    }
  }

  public void End () {
    sim.currentInteraction = null;
    sim.player.currentState = Player.State.Idling;
  }


}
