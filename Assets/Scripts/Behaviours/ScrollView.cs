using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollView : BaseBehaviour, IEndDragHandler {

  enum State {
    Reset,
    Pulling,
    Refreshing,
    Paused
  }

  public GameObject eventList;
  public GameObject pullTriggerView;
  public Text pullTriggerText;
  FeedView feedView;
  ScrollRect scrollRect;
  Rect screenRect;

  public float pullTriggerMargin = 0.1f;
  State state = State.Reset;

	// Use this for initialization
	void Start () {
    scrollRect = GetComponent<ScrollRect>();
    feedView = eventList.GetComponent<FeedView>();
    pullTriggerText = pullTriggerView.transform.Find("Title").GetComponent<Text>();
    screenRect = new Rect(0f, 0f, 
                          Screen.width + 1, 
                          Screen.height - Screen.height*pullTriggerMargin);
	}
	
	// Update is called once per frame
	void Update () {
	}

  void LateUpdate () {
    DetectVisibleTrigger();
  }

  void DetectVisibleTrigger () {

    if (state != State.Reset) {
      return;
    } 

    Vector3[] corners = new Vector3[4];

    pullTriggerView.GetComponent<RectTransform>().GetWorldCorners(corners);

    foreach (Vector3 corner in corners) {
      if (!screenRect.Contains(corner)) {
        return;
      }
    }

    InitiateRefresh();
  }


  public void OnEndDrag (PointerEventData data) {
    if (state == State.Pulling) {
      Reset();
    }
  }

  void Reset () {
    state = State.Reset;
    scrollRect.vertical = true;
    Invoke("MakeElastic", 0.1f);
  }

  void MakeElastic () {
    scrollRect.movementType = ScrollRect.MovementType.Elastic;
  }

  void InitiateRefresh () {
    if (!sim.inputProcessor.canContinue) {
      pullTriggerText.text = "You must make a choice to continue...";
      return;
    }
    Debug.Log ("Initiating refresh...");
    state = State.Refreshing;
    scrollRect.movementType = ScrollRect.MovementType.Clamped;
    scrollRect.vertical = false;
    feedView.BeginRefresh(OnRefreshFinished);
  }

  void OnRefreshFinished () {
    NotificationCenter.PostNotification(Constants.OnUpdateEvents);
    Debug.Log ("Refresh finished");
    Invoke ("Reset", 0.1f);
  }


}
