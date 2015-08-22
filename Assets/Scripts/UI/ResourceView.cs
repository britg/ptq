using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceView : BaseBehaviour {
  
  public string resourceKey;
  public string prefix = "";
  public string suffix = "";

  Resource _resource;
  Resource resource {
    get {
      if (_resource == null) {
        Debug.Log ("Attempting to load stat " + resourceKey);
        _resource = sim.player.Resources[resourceKey];
      }
      return _resource;
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
    var res = sim.player.Resources[resourceKey];
    text.text = string.Format("{0}{1}{2}", prefix, res.Amount, suffix);
  }
}
