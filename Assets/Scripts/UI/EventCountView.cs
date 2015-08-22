using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventCountView : MonoBehaviour {

  public Transform feedTrans;
  Text text;

	// Use this for initialization
	void Start () {
    text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
    text.text = string.Format("{0}", feedTrans.childCount);
	}
}
