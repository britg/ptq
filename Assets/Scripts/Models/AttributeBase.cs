using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AttributeBase {

  public string id;

  public Vector3 position;
  public Dictionary<string, float> attributes;

  public void ChangeAttribute (string key, float delta) {
    attributes[key] += delta;
  }

  public int exp {
    get {
      return (int)attributes[Constants.expAttr];
    }
  }

  public int level {
    get {
      return (int)attributes[Constants.levelAttr];
    }
  }

  public int gold {
    get {
      return (int)attributes[Constants.goldAttr];
    }
  }

  public int currentHp {
    get {
      return (int)attributes[Constants.currentHpAttr];
    }
  }

  public int maxHp {
    get {
      return (int)attributes[Constants.maxHpAttr];
    }
  }

  public int currentAp {
    get {
      return (int)attributes[Constants.currentApAttr];
    }
  }

  public int maxAp {
    get {
      return (int)attributes[Constants.maxApAttr];
    }
  }

  public float attack {
    get {
      return (int)attributes[Constants.attackAttr];
    }
  }

  public float defense {
    get {
      return (int)attributes[Constants.defenseAttr];
    }
  }

  public float speed {
    get {
      return (float)attributes[Constants.speedAttr];
    }
  }

  public float dps {
    get {
      return (float)attributes[Constants.dpsAttr];
    }
  }

  public float crit {
    get {
      return (float)attributes[Constants.critAttr];
    }
  }

  public float sight {
    get {
      return (float)attributes[Constants.sightAttr];
    }
  }

  public float initiative {
    get {
      return (float)attributes[Constants.initiativeAttr];
    }
    set {
      attributes[Constants.initiativeAttr] = value;
    }
  }

}
