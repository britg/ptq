using UnityEngine;
using System.Collections;

public class GoldGenerator {

  Player player;
  Mob mob;
  Interactible interactible;

  public GoldGenerator (Player _player, Mob _mob) {
    player = _player;
    mob = _mob;
  }

  public GoldGenerator (Player _player, Interactible _interactible) {
    player = _player;
    interactible = _interactible;
  }

  public int Interactible () {
    return interactible.level * Random.Range(1, player.level);
  }

  public int Mob () {
    return mob.level * Random.Range(1, player.level);
  }
	
}
