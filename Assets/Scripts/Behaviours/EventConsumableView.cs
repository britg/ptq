using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EventConsumableView : EventView {

  public Text title;
  public Text description;

  public Image pullRightIcon;
  public Text pullRightLabel;
  public Image pullLeftIcon;
  public Text pullLeftLabel;
  public Sprite checkSprite;

  Consumable _con;
  public Consumable con {
    get {
      if (_con == null) {
        _con = (Consumable)playerEvent.data[PlayerEvent.consumableKey];
      }
      return _con;
    }
  }

  // Use this for initialization
  void Start () {
    if (playerEvent == null) {
      return;
    }

    UpdateConsumable();
    NotificationCenter.AddObserver(this, Constants.OnUpdateEvents);

  }

  void OnUpdateEvents (Notification n) {
    UpdateConsumable();
    UpdateActions();
  }

  public void UpdateConsumable () {
    if (playerEvent.chosenKey != null) {
      title.color = Color.gray;
    }

    var str = string.Format("[{0}]", playerEvent.Content);
    title.text = str;
  }

  void UpdateActions () {
    if (playerEvent.chosenKey != null) {
      enableLeftAction = false;
      enableRightAction = false;
      pullLeftLabel.text = "";
      pullLeftIcon.sprite = null;
      pullRightLabel.text = "";
      pullRightIcon.sprite = null;
    }
  }


}
