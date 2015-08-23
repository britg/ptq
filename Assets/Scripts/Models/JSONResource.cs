﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JSONResource {

  //public static Dictionary<string, JSONResource> cache = new Dictionary<string, JSONResource>();
  public static Hashtable cache = new Hashtable();

  public static void Cache<T>(JSONNode _sourceData) where T : JSONResource {
    T resource = (T)Activator.CreateInstance(typeof(T), new object[] { _sourceData });
    cache[resource.key] = resource; 
    //Debug.Log("cached an instance of " + resource + " " + resource.key);
  }


  public JSONNode sourceData {
    get; set;
  }

  string _key;
  public string key {
    get {
      if (_key == null) {
        _key = sourceData["key"].Value;
      }
      return _key;
    }
  }

  public JSONResource (JSONNode _sourceData) {
    sourceData = _sourceData;
  }


}
