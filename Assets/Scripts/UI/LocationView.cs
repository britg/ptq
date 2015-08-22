using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationView : BaseBehaviour {

  Text text;

	// Use this for initialization
	void Start () {
    text = GetComponent<Text>();
    Display();

    NotificationCenter.AddObserver(this, Constants.OnUpdateStats);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnUpdateStats () {
    Display();
  }

  void Display () {
    text.text = sim.player.LocationName();
  }
}
