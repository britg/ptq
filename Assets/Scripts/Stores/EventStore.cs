using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventStore : Store {

  static Dictionary<string, PlayerEvent> cache = new Dictionary<string, PlayerEvent>();

  public static PlayerEvent Find (string id) {
    return cache[id];
  }

  public static bool Save (PlayerEvent playerEvent) {
    cache[playerEvent.Id] = playerEvent;
    return true;
  }

}
