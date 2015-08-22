using UnityEngine;
using System.Collections;

public class Slot {
  
  public SlotType Type { get; set; }
  public Equipment Equipment { get; set; }

  public Slot (string slotKey) {
    Debug.Log ("Attempting to instantiate stat of type " + slotKey);
    Type = SlotType.all[slotKey];
  }
  
}
