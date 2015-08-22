using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimeView : BaseBehaviour {

  Text text;

  // Use this for initialization
  void Start() {
    text = GetComponent<Text>();
  }

  // Update is called once per frame
  void Update() {
    text.text = sim.GameTime.ToString();
  }
}
