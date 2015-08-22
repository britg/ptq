using UnityEngine;
using System.Collections;

public class DamageShake : MonoBehaviour {

  public float amount = 100f;
  public float duration = 0.3f;

  void Start () {
    NotificationCenter.AddObserver(this, Constants.OnTakeDamage);
  }

  void OnTakeDamage () {
    Shake();
  }

  void Shake () {
    iTween.ShakePosition(gameObject, Vector3.right*amount, duration);
  }
}
