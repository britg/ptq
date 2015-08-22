using UnityEngine;
using System.Collections;

public class GoldGenerator {

  Player player;
  Mob mob;
  Interactible interactible;
  int playerLvl;

  public GoldGenerator (Player _player, Mob _mob) {
    player = _player;
    mob = _mob;
    playerLvl = (int)player.GetStatValue(Stat.lvl);
  }

  public GoldGenerator (Player _player, Interactible _interactible) {
    player = _player;
    interactible = _interactible;
  }

  public int Interactible () {
    return interactible.level * Random.Range(1, playerLvl);
  }

  public int Mob () {
    return mob.level * Random.Range(1, playerLvl);
  }
	
}
