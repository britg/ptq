using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FeedView : BaseBehaviour {

  public delegate void RefreshFinishedHandler();
  RefreshFinishedHandler refreshFinishedHandler;

  public GameObject pullAnchor;
  public GameObject eventPrefab;
  public GameObject eventStoryPrefab;
  public GameObject eventInfoPrefab;
  public GameObject eventTransitionPrefab;
  public GameObject eventEquipmentPrefab;
  public GameObject eventChoicePrefab;
  public GameObject eventPlayerBasicAttackPrefab;
  public GameObject eventMobBasicAttackPrefab;
  public GameObject eventConsumablePrefab;

  public int numEvents = 20;
  public float refreshDelay = 1f;
  public string pullAnchorDefaultText = "Pull to Quest";
  public string pullAnchorWorkingText = "Questing...";

  Text pullAnchorTitle;

	// Use this for initialization
	void Start () {
    pullAnchorTitle = pullAnchor.transform.Find("Title").GetComponent<Text>();
    pullAnchorTitle.text = pullAnchorDefaultText;
    NotificationCenter.AddObserver(this, Constants.OnUpdateEvents);
    NotificationCenter.AddObserver(this, Constants.OnUpdateFeed);
	}

	// Update is called once per frame
	void Update () {

	}

  public void BeginRefresh (RefreshFinishedHandler handler) {
    refreshFinishedHandler = handler;
    pullAnchorTitle.text = pullAnchorWorkingText;
    CullOldEvents();
    Invoke ("ProcessGame", 0.3f);
//    ProcessGame();
  }

  void ProcessGame () {
    var inputProcessor = new InputProcessor(sim);
    inputProcessor.TriggerContinue();
  }

  public void OnUpdateFeed () {
    List<GameObject> eventObjs = new List<GameObject>();
    foreach (var playerEvent in sim.newEvents) {
      if (playerEvent.isFeedEvent) {
        var evObj = CreatePlayerEventView(playerEvent);
        if (evObj != null) {
          eventObjs.Add(evObj);
        }
      }
    }
    DisplayNewEvents(eventObjs);
    EndRefresh();
  }

  void EndRefresh () {
    NotificationCenter.PostNotification(Constants.OnUpdateFeedDone);
    pullAnchorTitle.text = pullAnchorDefaultText;
    refreshFinishedHandler();
  }

  GameObject CreatePlayerEventView (PlayerEvent playerEvent) {
    GameObject prefab = eventPrefab;
    GameObject eventObj = null;

    switch (playerEvent.type) {

    case PlayerEvent.Type.Info:
      eventObj = InstantiatePrefab(eventInfoPrefab, playerEvent);
      break;
    case PlayerEvent.Type.Transition:
      eventObj = InstantiatePrefab(eventTransitionPrefab, playerEvent);
      break;
    case PlayerEvent.Type.Equipment:
      eventObj = InstantiatePrefab(eventEquipmentPrefab, playerEvent);
      break;
    case PlayerEvent.Type.Choice:
      eventObj = InstantiatePrefab(eventChoicePrefab, playerEvent);
      break;
    case PlayerEvent.Type.PlayerBasicAttack:
      eventObj = InstantiatePrefab(eventPlayerBasicAttackPrefab, playerEvent);
      break;
    case PlayerEvent.Type.MobBasicAttack:
      eventObj = InstantiatePrefab(eventMobBasicAttackPrefab, playerEvent);
      break;
    case PlayerEvent.Type.Story:
      eventObj = InstantiatePrefab(eventStoryPrefab, playerEvent);
      break;
    case PlayerEvent.Type.Consumable:
      eventObj = InstantiatePrefab(eventConsumablePrefab, playerEvent);
      break;
    default:
      break;

    }

    return eventObj;
  }

  GameObject InstantiatePrefab (GameObject prefab, PlayerEvent playerEvent) {
    var eventObj = (GameObject)Instantiate(prefab);
    eventObj.GetComponent<EventView>().playerEvent = playerEvent;
    return eventObj;
  }

  void DisplayNewEvents (List<GameObject> newEvents) {
    foreach (GameObject eventObj in newEvents) {
      eventObj.transform.SetParent(transform, false);
      eventObj.transform.SetSiblingIndex(1);
    }
  }

  void CullOldEvents () {
    var toCull = transform.childCount - numEvents;
    if (toCull < 1) {
      return;
    }

    for (int i = 0; i < toCull; i++) {
      CullEventFromLast(i);
    }
  }

  void CullEventFromLast (int fromLast) {

    // 1. Get current height with content.
    // 2. Remove content, and set preferred height to true and to the previous height
    // 3. Disable Vertical layout group and remove the component

    var lastEventTrans = transform.GetChild(transform.childCount - (1 + fromLast));
    lastEventTrans.name = "Culled";
    var height = lastEventTrans.GetComponent<RectTransform>().sizeDelta.y;
    for (int i = 0; i < lastEventTrans.childCount; i++) {
      var child = lastEventTrans.GetChild(i);
      Destroy(child.gameObject);
    }

    var layoutElem = lastEventTrans.GetComponent<LayoutElement>();
    layoutElem.preferredHeight = height;

    var layoutGroup = lastEventTrans.GetComponent<VerticalLayoutGroup>();
    Destroy(layoutGroup);

    var eventInfoView = lastEventTrans.GetComponent<EventView>();
    Destroy(eventInfoView);

  }

  void OnUpdateEvents (Notification n) {
    pullAnchorTitle.text = pullAnchorDefaultText;
  }

}
