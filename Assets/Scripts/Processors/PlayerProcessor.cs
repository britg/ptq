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

    else if (sim.currentBranch != null) {
      if (sim.currentChoiceKey != null) {
        var branchProcessor = new BranchProcessor(sim);
        branchProcessor.Choose(sim.currentChoiceKey);
      }
    }

    else if (sim.currentMob != null) {
      var battleProcessor = new BattleProcessor(sim);
      battleProcessor.Continue();
    }
    
    else if (sim.currentInteractible != null) {
      var interactionProcessor = new InteractionProcessor(sim);
      interactionProcessor.Continue();
    }

    else {
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
