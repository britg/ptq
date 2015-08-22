using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventInfoView : EventView {

  public Text description;

  // Use this for initialization
  void Start () {
    description.text = playerEvent.Content;
  }

}
