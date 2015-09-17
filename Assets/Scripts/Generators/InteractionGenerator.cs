using UnityEngine;
using System.Collections;

public class InteractionGenerator {

  InteractionTemplate interactibleTemplate;
  Interaction interactible;

  public InteractionGenerator (InteractionTemplate _interactibleTemplate) {
    interactibleTemplate = _interactibleTemplate;
  }

  public static Interaction Generate (InteractionTemplate template) {
    return new InteractionGenerator(template).Generate();
  }

  public Interaction Generate () {
    interactible = new Interaction();
    interactible.id = System.Guid.NewGuid().ToString();
    interactible.template = interactibleTemplate;
    interactible.name = interactibleTemplate.name;

    // TODO: stats, loot chances, etc.

    return interactible;
  }
}
