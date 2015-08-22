using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerStateView : BaseBehaviour {

  Text _text;
  Text text {
    get {
      if (_text == null) {
        _text = GetComponent<Text>();
      }
      return _text;
    }
  }


  // Use this for initialization
  void Start () {
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
    text.text = string.Format("{0}", sim.player.currentFloor.num);
  }
}
