using UnityEngine;
using System.Collections;

public class PlayerProcessor {

  Simulation sim;

  public PlayerProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void TakeTurn () {

    if (sim.newGame) {
      StartNewGame();
    }

    switch (sim.player.currentState) {

    case Player.State.Idling:
    case Player.State.Exploring:
      Explore();
      break;

    case Player.State.Interacting:
      // branch processor
      break;

    case Player.State.Targeting:
      // pathfinding processor
      break;

    case Player.State.Battling:

      break;
    }

//    else if (sim.currentBranch != null) {
//      if (sim.currentChoiceKey != null) {
//        var branchProcessor = new BranchProcessor(sim);
//        branchProcessor.Choose(sim.currentChoiceKey);
//      }
//    }
//
//    else if (sim.currentMob != null) {
//      var battleProcessor = new BattleProcessor(sim);
//      battleProcessor.Continue();
//    }
//    
//    else if (sim.currentInteractible != null) {
//      var interactionProcessor = new InteractionProcessor(sim);
//      interactionProcessor.Continue();
//    }

    sim.EndPlayerTurn();

  }

  void StartNewGame () {
    var envProcessor = new EnvironmentProcessor(sim);

    NotificationCenter.PostNotification(Constants.OnFirstPull);
    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);
    envProcessor.Enter();
  }

  void Explore () {
    var envProcessor = new EnvironmentProcessor(sim);
    envProcessor.Explore();
  }

  void Interact () {

  }

  void Battle () {

  }

}
