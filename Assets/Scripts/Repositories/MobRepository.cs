using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobRepository : Repository {

  static Dictionary<string, Mob> repo = new Dictionary<string, Mob>();

  public static Mob Find (string id) {
    return repo[id];
  }

  public static bool Save (Mob mob) {
    repo[mob.id] = mob;
    return true;
  }
}
