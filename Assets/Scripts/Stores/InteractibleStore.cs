using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractibleStore : Store {
  static Dictionary<string, Interaction> repo = new Dictionary<string, Interaction>();

  public static Interaction Find (string id) {
    return repo[id];
  }

  public static bool Save (Interaction interactible) {
    repo[interactible.id] = interactible;
    return true;
  }

}
