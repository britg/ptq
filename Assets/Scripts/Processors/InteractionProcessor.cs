using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionProcessor {

  Simulation sim;

  public InteractionProcessor (Simulation _sim) {
    sim = _sim;
  }

  public List<PlayerEvent> StartInteraction (Interactible interactible) {
    var newEvents = new List<PlayerEvent>();
    sim.player.currentInteractible = interactible;

    newEvents.Add (PlayerEvent.Info ("[DEV] You find " + interactible.name + " and must make a choice about it."));

    // DEV
    newEvents.Add (PlayerEvent.Info ("[DEV] ending interaction...."));
    sim.player.currentInteractible = null;

    return newEvents;
  }

  public List<PlayerEvent> Continue () {
    var newEvents = new List<PlayerEvent>();
    return newEvents;
  }
}
