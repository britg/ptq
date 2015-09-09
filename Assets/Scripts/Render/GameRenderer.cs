using UnityEngine;
using System.Collections;

public class GameRenderer : BaseBehaviour {

  public GameObject playerObj;

  int currentIndex = 0;

  void Start () {
    NotificationCenter.AddObserver(this, Constants.OnRenderEvents);
  }

  void OnRenderEvents () {
    Debug.Log("On render events...");
    currentIndex = 0;
    NextEvent();
  }

  void NextEvent () {
    if (currentIndex >= sim.newEvents.Count) {
      NotifyDone();
      return;
    }

    var ev = sim.newEvents[currentIndex];
    ++currentIndex;

    if (ev.type == PlayerEvent.Type.Movement) {

      Vector3 delta = (Vector3)ev.data[PlayerEvent.movementDeltaKey];
      string mover = (string)ev.data[PlayerEvent.moverKey];

      if (mover == Constants.playerContentKey) {
        MovePlayer(delta);
      } else if (mover == Constants.mobContentKey) {
        string mobId = (string)ev.data[PlayerEvent.moverIdKey];
        MoveMob(mobId, delta);
      }

    } else {
      NextEvent();
    }
  }

  void MovePlayer (Vector3 delta) {
    iTween.MoveBy(playerObj, iTween.Hash(
      "amount", delta,
      "time", 0.3f,
      "oncomplete", "NextEvent",
      "oncompletetarget", gameObject
      )
    );
  }

  void MoveMob (string id, Vector3 delta) {
    var mobObj = GameObject.Find(id);
    iTween.MoveBy(mobObj, iTween.Hash(
      "amount", delta,
      "time", 0.3f,
      "oncomplete", "NextEvent",
      "oncompletetarget", gameObject
      )
    );
  }

  void NotifyDone () {
    NotificationCenter.PostNotification(Constants.OnUpdateFeed);
  }

}
