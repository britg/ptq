using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventMobBasicAttackView : EventView {
  
  public Text actionText;
  public Text damageText;

  Mob mob;
  string statKey;
  float amount;

  void Start () {

    mob = (Mob)playerEvent.data[PlayerEvent.mobKey];

    var trigger = playerEvent.Triggers[0];
    statKey = (string)trigger.data[Trigger.statKey];
    amount = (float)trigger.data[Trigger.statChangeAmountKey];

    UpdateActionText();
    UpdateDamage();
  }
  
  void UpdateActionText () {
    actionText.text = string.Format("[{0}] attacks!", mob.name);
  }
  
  void UpdateDamage () {
    damageText.text = string.Format("{0:0}{1}", amount, statKey);
  }
  
}
