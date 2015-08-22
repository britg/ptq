using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatView : BaseBehaviour {

  public string statKey;
  public string prefix = "";
  public string suffix = "";
  public bool includeMax = false;

  Stat _stat;
  Stat stat {
    get {
      if (_stat == null) {
        Debug.Log ("Attempting to load stat " + statKey);
        _stat = sim.player.Stats[statKey];
      }
      return _stat;
    }
  }

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
    var val = sim.player.GetStatValue(statKey);
    string txt = "";

    if (includeMax) {
      txt = string.Format("{0:0}/{1}", val, stat.max);
    } else {
      txt = string.Format("{0:0}", val);
    }
    text.text = string.Format("{0}{1}{2}", prefix, txt, suffix);
  }
}
