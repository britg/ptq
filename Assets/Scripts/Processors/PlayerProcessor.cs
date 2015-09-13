using UnityEngine;
using System.Collections;

public class PlayerProcessor {

  Simulation sim;

  public PlayerProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void TakeTurn () {
    var envProcessor = new EnvironmentProcessor(sim);

    if (sim.newGame) {
      StartNewGame();
    }

    if (sim.shouldExplore) {
      sim.newEvents.AddRange(envProcessor.Explore());
    } else {

      if (sim.currentChoiceKey != null) {
        var branchProcessor = new BranchProcessor(sim, sim.currentEvent);
        sim.newEvents.AddRange(branchProcessor.Choose(sim.currentChoiceKey));
      }

      if (sim.currentMob != null) {
        var battleProcessor = new BattleProcessor(sim);
        sim.newEvents.AddRange(battleProcessor.Continue());
      }
      
      if (sim.currentInteractible != null) {
        var interactionProcessor = new InteractionProcessor(sim);
        sim.newEvents.AddRange(interactionProcessor.Continue());
      }
    }
  }

  void StartNewGame () {
    var envProcessor = new EnvironmentProcessor(sim);

    NotificationCenter.PostNotification(Constants.OnFirstPull);
    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);
    envProcessor.Enter();
  }
}
