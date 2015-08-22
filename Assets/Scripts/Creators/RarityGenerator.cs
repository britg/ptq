using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RarityGenerator {

  Simulation sim;

  public RarityGenerator (Simulation _sim) {
    sim = _sim;
  }

  public Rarity Roll () {
    Rarity rarity = Rarity.all["common"];

    float rand = (float)Random.Range(0, 1000);
    float perc = rand / 10f;

    foreach (KeyValuePair<string, Rarity> p in Rarity.all) {
      var r = p.Value;
      if (r.Chance < rarity.Chance && perc <= r.Chance) {
        rarity = r;
      }
    }

    return rarity;
  }

}
