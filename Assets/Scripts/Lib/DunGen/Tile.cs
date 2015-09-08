using UnityEngine;
using System.Collections;

namespace DunGen {
  public class Tile {

    public enum Type {
      Nothing,
      Blocked,
      Room,
      Corridor,
      Perimeter,
      Entrance,
      Door,
      StairUp,
      StairDown
    }

    public Type type;
    public Vector3 position;
  }
}
