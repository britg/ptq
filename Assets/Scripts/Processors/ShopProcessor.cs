using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopProcessor {

  Simulation sim;

  public ShopProcessor (Simulation _sim) {
    sim = _sim;
  }

  public List<PlayerEvent> Continue () {
    var newEvents = new List<PlayerEvent>();
    newEvents.Add(new PlayerEvent("You look around the hastily constructed stalls..."));
    newEvents.Add(new PlayerEvent("List the actions you can take:\nSell your equipment (Blacksmith?)\nPay to heal completely (Healer?)\n"));
    return newEvents;
  }

}
