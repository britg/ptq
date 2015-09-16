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
      BeginExploring();
      break;

    case Player.State.Exploring:
      Explore();
      break;

    case Player.State.Interacting:
      Interact();
      break;

    case Player.State.Pathfinding:
      Pathfind();
      break;

    case Player.State.Battling:
      Battle();
      break;
    }

  }

  void StartNewGame () {
    NotificationCenter.PostNotification(Constants.OnFirstPull);
    NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);
    EnvironmentProcessor.With(sim).Enter();
  }

  void BeginExploring () {
    sim.AddEvent(PlayerEvent.Story(string.Format("{0} ventures forth...", sim.player.name)));
    ExplorationProcessor.With(sim).Explore();
  }

  void Explore () {
    ExplorationProcessor.With(sim).Explore();
  }

  void Pathfind () {

  }

  void Interact () {
    var interactionProcessor = new InteractionProcessor(sim);
    interactionProcessor.Continue();
  }

  void Battle () {

  }

}
