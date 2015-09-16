using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public class EnvironmentProcessor {

  Simulation sim;

  public EnvironmentProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void Enter () {
    sim.player.SetState(Player.State.Interacting);
    var interactionTemplateKey = sim.currentEnvironment.enterInteractionTemplateKey;
    var interactionTemplate = JSONResource.Get<InteractionTemplate>(interactionTemplateKey);

    if (interactionTemplate == null) {
      return;
    }

    var interactionGenerator = new InteractionGenerator(interactionTemplate);
    var interaction = interactionGenerator.Generate();
    sim.currentInteraction = interaction;
    var interactionProcessor = new InteractionProcessor(sim);
    interactionProcessor.Start();
  }

  public void Explore () {

    if (sim.currentRoom != null) {
      var roomProcessor = new RoomProcessor(sim, sim.currentRoom);
      roomProcessor.Explore();
    }

  }

}
