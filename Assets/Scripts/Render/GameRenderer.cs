using UnityEngine;
using System.Collections;

public class GameRenderer : BaseBehaviour {

  public GameObject playerObj;

  int currentIndex = 0;

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
    ++currentIndex;

    switch (ev.type) {

    case PlayerEvent.Type.Movement:
      RenderMoveEvent(ev);
      break;

    case PlayerEvent.Type.DiscoverTile:
      RevealTile(ev);
      break;

    default:
      NextEvent();
      break;
    }

  }

  void RenderMoveEvent (PlayerEvent ev) {
    Vector3 delta = (Vector3)ev.data[PlayerEvent.movementDeltaKey];
    string mover = (string)ev.data[PlayerEvent.moverKey];

    if (mover == Constants.playerContentKey) {
      MovePlayer(delta);
    } else if (mover == Constants.mobContentKey) {
      string mobId = (string)ev.data[PlayerEvent.moverIdKey];
      MoveMob(mobId, delta);
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

  void RevealTile (PlayerEvent ev) {
    Tile tile = (Tile)ev.data[PlayerEvent.tileKey];
    tile.tileObj.SetActive(false);
    NextEvent();
  }

  void NotifyDone () {
    NotificationCenter.PostNotification(Constants.OnRenderEventsDone);
    NotificationCenter.PostNotification(Constants.OnUpdateFeed);
  }

}
