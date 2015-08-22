using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EventEquipmentView : EventView {

  public Text title;
  public Text description;

  public Image pullRightIcon;
  public Text pullRightLabel;
  public Image pullLeftIcon;
  public Text pullLeftLabel;
  public Sprite checkSprite;

  Equipment _eq;
  public Equipment eq {
    get {
      if (_eq == null) {
        _eq = (Equipment)playerEvent.data[PlayerEvent.equipmentKey];
      }
      return _eq;
    }
  }

	// Use this for initialization
	void Start () {
    if (playerEvent == null) {
      return;
    }

    UpdateEquipment();
    NotificationCenter.AddObserver(this, Constants.OnUpdateEvents);

	}

  void OnUpdateEvents (Notification n) {
    UpdateEquipment();
    UpdateActions();
  }

  public void UpdateEquipment () {
    if (eq == null) {
      title.color = Color.gray;
    } else {
      var str = string.Format("[{0}]", playerEvent.Content); 
      title.text = str;
      title.color = eq.Rarity.Color;
      description.text = StatsString();
    }
  }

  string StatsString () {
    string str = "";
    foreach (KeyValuePair<string, Stat> p in eq.Stats) {
      var stat = p.Value;
      var pol = "+";
      if (stat.current < 0f) {
        pol = "-";
      }

      // TODO: Calc diff and show diffs instead of absolute value
      str += string.Format("{0}{1:0.0} {2} ", pol, stat.current, stat.Key);
    }

    return str;
  }

  void UpdateActions () {
    if (playerEvent.chosenKey != null) {

      // Pulled left
      if (rightActionView.actionName == playerEvent.chosenKey) {
        pullLeftIcon.sprite = checkSprite;
        pullLeftLabel.text = "Equipped";
        pullRightIcon.sprite = null;
        pullRightLabel.text = "";
      } 
      
      // pulled right
      else {
        pullRightIcon.sprite = checkSprite;
        pullRightLabel.text = "Picked Up";
        pullLeftIcon.sprite = null;
        pullLeftLabel.text = "";
      }

      enableLeftAction = false;
      enableRightAction = false;

    }
  }


}
