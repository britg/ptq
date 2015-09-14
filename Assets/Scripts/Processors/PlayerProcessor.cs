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

    if (sim.currentBranch != null) {
      if (sim.currentChoiceKey != null) {
        var branchProcessor = new BranchProcessor(sim);
        branchProcessor.Choose(sim.currentChoiceKey);
      }
    }

    if (sim.currentMob != null) {
      var battleProcessor = new BattleProcessor(sim);
      battleProcessor.Continue();
    }
    
    if (sim.currentInteractible != null) {
      var interactionProcessor = new InteractionProcessor(sim);
      interactionProcessor.Continue();
    }

    if (sim.idle) {
      envProcessor.Explore();
    }

    sim.EndPlayerTurn();

  }

  void StartNewGame () {
    var envProcessor = new EnvironmentProcessor(sim);

    NotificationCenter.PostNotification(Constants.OnFirstPull);
    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);
    envProcessor.Enter();
  }
}
