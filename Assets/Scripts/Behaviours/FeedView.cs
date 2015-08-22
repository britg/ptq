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
	}

	// Update is called once per frame
	void Update () {

	}

  public void BeginRefresh (RefreshFinishedHandler handler) {
    refreshFinishedHandler = handler;
    pullAnchorTitle.text = pullAnchorWorkingText;
    CullOldEvents();
    Invoke("DoRefresh", refreshDelay);
  }

  void DoRefresh () {
    List<PlayerEvent> newEvents = sim.inputProcessor.Continue();
    List<GameObject> eventObjs = new List<GameObject>();
    foreach (var playerEvent in newEvents) {
      eventObjs.Add(CreatePlayerEventView(playerEvent));
    }
    DisplayNewEvents(eventObjs);
    EndRefresh();
  }

  void EndRefresh () {
    pullAnchorTitle.text = pullAnchorDefaultText;
    refreshFinishedHandler();
  }

  GameObject CreatePlayerEventView (PlayerEvent playerEvent) {
    GameObject prefab = eventPrefab;
    GameObject eventObj;

    if (playerEvent.type == PlayerEvent.Type.Info) {
      prefab = eventInfoPrefab;
    } else if (playerEvent.type == PlayerEvent.Type.Transition) {
      prefab = eventTransitionPrefab;
    } else if (playerEvent.type == PlayerEvent.Type.Equipment) {
      prefab = eventEquipmentPrefab;
    } else if (playerEvent.type == PlayerEvent.Type.Choice) {
      prefab = eventChoicePrefab;
    } else if (playerEvent.type == PlayerEvent.Type.PlayerBasicAttack) {
      prefab = eventPlayerBasicAttackPrefab;
    } else if (playerEvent.type == PlayerEvent.Type.MobBasicAttack) {
      prefab = eventMobBasicAttackPrefab;
    } else if (playerEvent.type == PlayerEvent.Type.Story) {
      prefab = eventStoryPrefab;
    } else if (playerEvent.type == PlayerEvent.Type.Consumable) {
      prefab = eventConsumablePrefab;
    }


    eventObj = (GameObject)Instantiate(prefab);
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
    Debug.Log ("Events to cull " + toCull);
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
