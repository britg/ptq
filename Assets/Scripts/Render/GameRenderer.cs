using UnityEngine;
using System.Collections;

public class GameRenderer : BaseBehaviour {

  int currentIndex = 0;
  public GameObject playerObj;

  void Start () {
    NotificationCenter.AddObserver(this, Constants.OnRenderEvents);
  }

  void OnRenderEvents () {
    currentIndex = 0;
    NextEvent();
  }

  void NextEvent () {
    if (currentIndex >= sim.newEvents.Count) {
      NotifyDone();
      return;
    }

    var ev = sim.newEvents[currentIndex];

    iTween.MoveBy(playerObj, iTween.Hash(
      "amount", new Vector3(-1, 0, 0),
      "time", 0.3f,
      "oncomplete", "NextEvent",
      "oncompletetarget", gameObject
      )
    );

    ++currentIndex;
  }

  void NotifyDone () {
    NotificationCenter.PostNotification(Constants.OnUpdateFeed);
  }

}
