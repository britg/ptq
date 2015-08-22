using UnityEngine;
using System.Collections;

public class ExperienceProcessor {

  Player player;
  Mob mob;

  public ExperienceProcessor (Player _player, Mob _mob) {
    player = _player;
    mob = _mob;
  }

  public float ExperienceGain () {
    // TODO: Experience based on mob
    return 10f;
  }
}
