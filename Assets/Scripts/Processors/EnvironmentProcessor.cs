using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public class EnvironmentProcessor {

  Simulation sim;

  public static EnvironmentProcessor With (Simulation sim) {
    return new EnvironmentProcessor(sim);
  }

  public EnvironmentProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void Enter () {
    var interactionTemplateKey = sim.currentEnvironment.enterInteractionTemplateKey;
    var interactionTemplate = JSONResource.Get<InteractionTemplate>(interactionTemplateKey);

    if (interactionTemplate == null) {
      return;
    }

    sim.player.SetState(Player.State.Interacting);
    var interactionGenerator = new InteractionGenerator(interactionTemplate);
    var interaction = interactionGenerator.Generate();
    sim.currentInteraction = interaction;
    var interactionProcessor = new InteractionProcessor(sim);
    interactionProcessor.Start();
  }

}
