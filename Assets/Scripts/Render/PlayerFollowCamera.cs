using UnityEngine;
using System.Collections;

public class PlayerFollowCamera : MonoBehaviour {

  public Transform playerTransform;

	// Use this for initialization
	void Start () {
    NotificationCenter.AddObserver(this, Constants.OnFloorUpdate);
	}

  void OnFloorUpdate () {
    
  }
	
	// Update is called once per frame
	void Update () {
	  
	}

  void LateUpdate () {

  }

}
