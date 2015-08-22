using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventTransitionView : EventView {

  Text content;

	// Use this for initialization
	void Start () {
    content = transform.Find("Content").GetComponent<Text>();
    var str = string.Format("[{0}]", playerEvent.Content);
    content.text = str;
	}
	
}
