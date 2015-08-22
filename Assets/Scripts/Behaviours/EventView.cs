using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventView : BaseBehaviour {

  public bool enableLeftAction = false;
  public bool enableRightAction = false;
  public float swipeFactor = 0.5f;

  public ActionView leftActionView;
  public ActionView rightActionView;

  float leftActionWidth;
  float rightActionWidth;

  PlayerEvent _playerEvent;
  public PlayerEvent playerEvent {
    get {
      return _playerEvent;
    }
    set {
      SetPlayerEvent(value);
    }
  }

  bool hasTriggered = false;
  Rect screenRect;
  RectTransform rectTrans;

  void Awake () {
    screenRect = new Rect(0f, 0f, Screen.width + 1, Screen.height + 1);
    rectTrans = GetComponent<RectTransform>();
    Invoke("GetActionWidths", 1f);
  }

  void GetActionWidths () {
    if (leftActionView != null) {
      leftActionWidth = leftActionView.gameObject.GetComponent<RectTransform>().rect.width;
    }

    if (rightActionView != null) {
      rightActionWidth = rightActionView.gameObject.GetComponent<RectTransform>().rect.width;
    }
  }

  void SetPlayerEvent (PlayerEvent ev) {
    _playerEvent = ev;
    if (leftActionView != null) {
      leftActionView.playerEvent = ev;
    }
    if (rightActionView != null) {
      rightActionView.playerEvent = ev;
    }
  }

  // Update is called once per frame
  void Update () {
    DetectVisibleTrigger();
    DetectHorizontalSwipe();
  }

  void DetectVisibleTrigger () {

    if (hasTriggered) {
      return;
    } else {

      if (playerEvent == null || playerEvent.Triggers.Count < 1) {
        hasTriggered = true;
        return;
      }
    }

    Vector3[] corners = new Vector3[4];

    this.GetComponent<RectTransform>().GetWorldCorners(corners);

    foreach (Vector3 corner in corners) {
      if (!screenRect.Contains(corner)) {
        return;
      }
    }

    hasTriggered = true;
    sim.inputProcessor.TriggerEvent(playerEvent);
  }

  
  void DetectHorizontalSwipe () {

    if (!enableLeftAction && !enableRightAction) {
      return;
    }
    
    if (Input.GetMouseButtonDown(0) && DetectInBounds()) {
      CaptureStartOfHorizontalSwipe(Input.mousePosition);
    }
    
    if (Input.GetMouseButton(0) && isSwiping) {
      CaptureHorizontalSwipe(Input.mousePosition);
    }
    
    if (Input.GetMouseButtonUp(0) && isSwiping) {
      ResetHorizontalSwipe();
    }
  }
  
  bool DetectInBounds () {
    Vector3[] corners = new Vector3[4];
    rectTrans.GetWorldCorners(corners);

    var pos = Input.mousePosition;
    return (pos.y > corners[0].y && pos.y < corners[1].y);
  }
  
  Vector3 hSwipeStart;
  Vector3 originalPos;
  bool isSwiping = false;
  bool triggeredThisSession = false;
  void CaptureStartOfHorizontalSwipe (Vector3 startPos) {
    if (isSwiping) {
      return;
    }
    hSwipeStart = startPos;
    originalPos = rectTrans.localPosition;
    isSwiping = true;
  }
  
  void CaptureHorizontalSwipe (Vector3 currPosition) {
    var delta = currPosition - hSwipeStart;
    var pos = rectTrans.localPosition;

    bool rightAllowed = (delta.x < 0 && enableRightAction);
    bool leftAllowed = (delta.x > 0 && enableLeftAction);

    if (rightAllowed || leftAllowed) {
      pos.x += delta.x * swipeFactor;
      rectTrans.localPosition = pos;
      hSwipeStart = currPosition;

      if (pos.x > 0 && pos.x > leftActionWidth) {
        leftActionView.hasTriggered = true;
      }

      if (pos.x < 0 && pos.x < -rightActionWidth) {
        rightActionView.hasTriggered = true;
      }

      if (!triggeredThisSession) {
        HandleActionTrigger();
      }
    }
  }
  
  void ResetHorizontalSwipe () {
    iTween.MoveTo(gameObject, iTween.Hash (
      "position", originalPos, 
      "isLocal", true,
      "oncomplete", "EndHorizontalSwipe"
    ));
  }

  void EndHorizontalSwipe () {
    triggeredThisSession = false;
    leftActionView.hasTriggered = false;
    rightActionView.hasTriggered = false;
    isSwiping = false;
  }

  void HandleActionTrigger () {

    string actionName = null;

    if (enableLeftAction && leftActionView.hasTriggered) {
      ResetHorizontalSwipe();
      actionName = leftActionView.actionName;
    }

    if (enableRightAction && rightActionView.hasTriggered) {
      ResetHorizontalSwipe();
      actionName = rightActionView.actionName;
    }

    if (actionName != null) {
      triggeredThisSession = true;
      if (playerEvent == null) {
        return;
      }

      if (playerEvent.hasActions) {
        sim.inputProcessor.TriggerAction(playerEvent, actionName);
      }

      if (playerEvent.hasChoices) {
        sim.inputProcessor.TriggerChoice(playerEvent, actionName);
      }
    }
  }

  public bool isLastEvent () {
    int i = 0;
    foreach (Transform child in transform.parent) {
      if (transform == child) {
        break;
      }
      i++;
    }

    Debug.Log ("Current position is " + i);
    return i == 1;

  }
}
