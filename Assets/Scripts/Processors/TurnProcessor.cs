using UnityEngine;
using System.Collections;

public class TurnProcessor {

  Simulation sim;

  int turnCount = 0;
  int maxTurnCount = 10;

  public TurnProcessor (Simulation _sim) {
    sim = _sim;
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

    if (sim.promptPull) {
      NotificationCenter.PostNotification(Constants.OnRenderEvents);
      return;
    }

    if (sim.idle && turnCount < maxTurnCount) {
      TakeTurn();
    } else {
      NotificationCenter.PostNotification(Constants.OnRenderEvents);
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
