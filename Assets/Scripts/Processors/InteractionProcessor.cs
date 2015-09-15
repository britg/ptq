using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionProcessor {

  Simulation sim;

  public InteractionProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void StartInteraction (Interaction interactible) {
    sim.currentInteractible = interactible;

    sim.AddEvent(PlayerEvent.Info ("[DEV] You find " + interactible.name + " and must make a choice about it."));

    // DEV
    sim.AddEvent(PlayerEvent.Info ("[DEV] ending interaction...."));
    sim.currentInteractible = null;
  }

  public void Continue () {
  }
}
