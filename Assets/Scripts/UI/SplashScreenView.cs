using UnityEngine;
using System.Collections;

public class SplashScreenView : MonoBehaviour {

	// Use this for initialization
	void Start () {
    NotificationCenter.AddObserver(this, Constants.OnFirstPull);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnFirstPull () {
    foreach (Transform t in transform) {
      t.gameObject.SetActive(false);
    }
  }
}
