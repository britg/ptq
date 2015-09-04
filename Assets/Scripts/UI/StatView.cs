using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatView : BaseBehaviour {

  public string statKey;
  public string prefix = "";
  public string suffix = "";
  public bool includeMax = false;

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
    NotificationCenter.AddObserver(this, Constants.OnUpdateAttribute);
	}
	
	// Update is called once per frame
	void Update () {
	}

  void OnUpdateStats () {
    Display();
  }

  void Display () {
    var stat = sim.player.attributes[statKey];
    text.text = string.Format("{0}{1}{2}", prefix, stat, suffix);
  }
}
