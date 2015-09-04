using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceView : BaseBehaviour {
  
  public string resourceKey;
  public string prefix = "";
  public string suffix = "";
  
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
    var res = sim.player.attributes[resourceKey];
    text.text = string.Format("{0}{1}{2}", prefix, res, suffix);
  }
}
