using UnityEngine;
using System.Collections;

public class InteractibleGenerator {

  InteractibleTemplate interactibleTemplate;
  Interactible interactible;

  public InteractibleGenerator (InteractibleTemplate _interactibleTemplate) {
    interactibleTemplate = _interactibleTemplate;
  }

  public Interactible Generate () {
    interactible = new Interactible();
    interactible.id = System.Guid.NewGuid().ToString();
    interactible.template = interactibleTemplate;
    interactible.name = interactibleTemplate.name;

    // TODO: stats, loot chances, etc.

    return interactible;
  }
}
