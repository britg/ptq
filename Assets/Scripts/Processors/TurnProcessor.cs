using UnityEngine;
using System.Collections;

public class TurnProcessor {

  Simulation sim;

  int turnCount = 0;
  int maxTurnCount = 10;

  public TurnProcessor (Simulation _sim) {
    sim = _sim;
  }

  public static TurnProcessor With (Simulation sim) {
    return new TurnProcessor(sim);
  }

  public void TakeTurn () {
    ++turnCount;

    if (sim.currentTurn == Turn.Type.Player) {
      var playerProcessor = new PlayerProcessor(sim);
      playerProcessor.TakeTurn();
    } else if (sim.currentTurn == Turn.Type.Game) {
      var gameProcessor = new GameProcessor(sim);
      gameProcessor.TakeTurn();
    }

    bool atTurnLimit = (turnCount >= maxTurnCount);

    if (sim.requiresInput || atTurnLimit) {
      NotificationCenter.PostNotification(Constants.OnUpdateFog);
      NotificationCenter.PostNotification(Constants.OnRenderEvents);
      return;
    } else {
      TakeTurn();
    }

  }

  public void Change () {
    if (sim.currentTurn == Turn.Type.Player) {
      sim.currentTurn = Turn.Type.Game;
    } else {
      sim.currentTurn = Turn.Type.Player;
    }
  }

}
