using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldView : BaseBehaviour {

  Text text;

	// Use this for initialization
	void Start () {
    text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
    var goldAmount = (int)sim.player.Resources[Resource.Gold].Amount;
    text.text = string.Format("{0}g", goldAmount);
	}
}
