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
    float playerSpd = player.speed;

    if (playerSpd == 0f) {
      Debug.Log("Player speed is 0!!!");
      return mobIdent;
    }

    //float mobSpd = mob.GetStatValue(Stat.spd);
    float mobSpd = 10f;

    player.initiative += playerSpd;
    mob.initiative += mobSpd;

    if (player.initiative >= requiredInitiative) {
      player.initiative = (player.initiative - requiredInitiative);
      player.lastBattleMove = playerIdent;
      return playerIdent;
    }

    if (mob.initiative >= requiredInitiative) {
      mob.initiative = (mob.initiative - requiredInitiative);
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
