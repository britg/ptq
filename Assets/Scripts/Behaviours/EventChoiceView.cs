using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EventChoiceView : EventView {

  public Text title;
  public Text pullRightLabel;
  public Text pullLeftLabel;

  Sprite originalPullRightSprite;
  Sprite originalPullLeftSprite;
  public Image pullRightIcon;
  public Image pullLeftIcon;
  public Sprite checkSprite;

  Choice leftChoice;
  Choice rightChoice;

  // Use this for initialization
  void Start () {
    if (playerEvent == null) {
      return;
    }

    //leftActionConfirmation = null;
    //rightActionConfirmation = null;

    NotificationCenter.AddObserver(this, Constants.OnUpdateEvents);
    title.text = playerEvent.Content;

    AssignChoice(playerEvent.firstChoice);
    AssignChoice(playerEvent.secondChoice);

    leftActionView.actionName = rightChoice.key;
    rightActionView.actionName = leftChoice.key;

    pullLeftLabel.text = leftChoice.label;
    pullRightLabel.text = rightChoice.label;

    originalPullLeftSprite = pullLeftIcon.sprite;
    originalPullRightSprite = pullRightIcon.sprite;
  }

  void AssignChoice (Choice choice) {

    if (choice.direction == Choice.Direction.Left) {
      leftChoice = choice;
    } else {
      rightChoice = choice;
    }

  }

  void OnUpdateEvents (Notification n) {
    if (playerEvent.chosenKey != null) {

      if (leftChoice.key == playerEvent.chosenKey) {
        pullLeftIcon.sprite = checkSprite;
        pullRightIcon.sprite = originalPullRightSprite;
      } else {
        pullRightIcon.sprite = checkSprite;
        pullLeftIcon.sprite = originalPullLeftSprite;
      }

    }

    if (!isLastEvent()) {
      enableLeftAction = false;
      enableRightAction = false;
    }
  }

}
