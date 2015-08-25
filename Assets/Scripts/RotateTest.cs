using UnityEngine;
using System.Collections;

public class RotateTest : MonoBehaviour {

  public float mult = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    transform.localEulerAngles = transform.localEulerAngles + mult * Vector3.up * Time.deltaTime;
	}
}
