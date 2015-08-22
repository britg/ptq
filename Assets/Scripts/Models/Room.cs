using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

  public enum Type {
    Standard
  }

  public string key;
  public Type type;
  public RoomTemplate roomTemplate;
  public Dictionary<string, float> content;

  public Room () {

  }

  public Room (Type _type) {
    type = _type;
  }



}
