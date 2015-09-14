using UnityEngine;
using System.Collections;

public class GameProcessor {

  Simulation sim;

  public GameProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void TakeTurn () {
    sim.AddEvent(PlayerEvent.Info("enemy turn..."));
    sim.EndGameTurn();
  }
}
