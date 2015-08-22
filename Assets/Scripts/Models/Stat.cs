using UnityEngine;
using System.Collections;

public class Stat {

  public enum Sign {
    Positive,
    Negative
  }

  public static string hp = "hp";
  public static string ap = "ap";
  public static string xp = "exp";
  public static string dps = "dps";
  public static string def = "def";
  public static string spd = "spd";
  public static string luck = "luck";
  public static string res = "res";
  public static string crit = "crit";
  public static string lvl = "level";


  public static Sign SignForValue (float val) {
    if (val < 0) {
      return Sign.Negative;
    }
    return Sign.Positive;
  }


  public string Key { get; set; }

  public StatType Type { get; set; }

  // Need to refactor to min/max/current!
  public float min;
  public float max;
  public float current;

  //public float Min { get; set; }
  //public float Max { get; set; }
  //public float Base { get; set; }
  //public float CurrentChange { get; set; }
  //public float Value { get; set; }

  public string Abbr {
    get {
      return Type.Abbr;
    }
  }

  public Stat (string statKey) {
    Key = statKey;
    Type = StatType.all[statKey];
  }

  public Stat (string statKey, float _min, float _max, float _current) {
    Key = statKey;
    Type = StatType.all[statKey];
    min = _min;
    max = _max;
    current = _current;
  }

  public Stat (string statKey, float value) {
    Key = statKey;
    Type = StatType.all[statKey];
    current = value;
    min = 0;
    max = current;
  }

  public float Change (float amount) {
    current = Mathf.Clamp(current + amount, min, max);
    return current;
  }

}
