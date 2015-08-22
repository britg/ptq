using UnityEngine;
using System.Collections;

public class InitiativeProcessor {

  public static string playerIdent = "player";
  public static string mobIdent = "mob";

  static float requiredInitiative = 100f;
  int iterationCount = 0;
  int iterationLimit = 50;

  Player player;
  Mob mob;

  public InitiativeProcessor (Player _player, Mob _mob) {
    player = _player;
    mob = _mob;
  }

  public string NextMove () {
    float playerSpd = player.GetStatValue(Stat.spd);

    if (playerSpd == 0f) {
      Debug.Log("Player speed is 0!!!");
      return mobIdent;
    }

    float mobSpd = mob.GetStatValue(Stat.spd);

    player.currentInitiative += playerSpd;
    mob.currentInitiative += mobSpd;

    if (player.currentInitiative >= requiredInitiative) {
      player.currentInitiative = (player.currentInitiative - requiredInitiative);
      player.lastBattleMove = playerIdent;
      return playerIdent;
    }

    if (mob.currentInitiative >= requiredInitiative) {
      mob.currentInitiative = (mob.currentInitiative - requiredInitiative);
      player.lastBattleMove = mobIdent;
      return mobIdent;
    }

    if (iterationCount >= iterationLimit) {
      Debug.Log("Initiative processor unable to determine initiative");
      Debug.Assert(false);
    }

    ++iterationCount;
    return NextMove();

  }


}
