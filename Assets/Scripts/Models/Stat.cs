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

  public string key { get; set; }
  public StatTemplate template { get; set; }

  public float min;
  public float max;
  public float current;

  public string Abbr {
    get {
      return template.abbr;
    }
  }

  public Stat (string statKey) {
    key = statKey;
    template = (StatTemplate)StatTemplate.cache[statKey];
  }

  public Stat (string statKey, float _min, float _max, float _current) {
    key = statKey;
    template = (StatTemplate)StatTemplate.cache[statKey];
    min = _min;
    max = _max;
    current = _current;
  }

  public Stat (string statKey, float value) {
    key = statKey;
    template = (StatTemplate)StatTemplate.cache[statKey];
    current = value;
    min = 0;
    max = current;
  }

  public float Change (float amount) {
    current = Mathf.Clamp(current + amount, min, max);
    return current;
  }

}
