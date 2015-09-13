using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractibleStore : Store {
  static Dictionary<string, Interactible> repo = new Dictionary<string, Interactible>();

  public static Interactible Find (string id) {
    return repo[id];
  }

  public static bool Save (Interactible interactible) {
    repo[interactible.id] = interactible;
    return true;
  }

}
