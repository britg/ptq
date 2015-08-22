using UnityEngine;
using System.Collections;

public class Choice {

  public const string Tower = "tower";
  public const string Shop = "shop";
  public const string Potion = "potion";
  public const string Continue = "continue";
  public const string OpenDoor = "open_door";
  public const string LeaveDoor = "leave_door";

  public enum Direction {
    Left,
    Right
  }

  public string key;
  public string label;
  public Direction direction;

  public static Choice Swipe (string directionStr, string key, string label) {
    if (directionStr == "left") {
      return SwipeLeft(key, label);
    } else {
      return SwipeRight(key, label);
    }
  }

  public static Choice SwipeLeft (string key, string label) {
    var c = new Choice();
    c.key = key;
    c.label = label;
    c.direction = Direction.Left;

    return c;
  }

  public static Choice SwipeRight (string key, string label) {
    var c = new Choice();
    c.key = key;
    c.label = label;
    c.direction = Direction.Right;

    return c;
  }

}
