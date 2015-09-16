using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interaction {

  public InteractionTemplate template;

  public string id;
  public string name;
  public int level;

  public Vector3 position;

  public List<string> GetEventGroup (string groupKey) {
    return template.GetEventGroup(groupKey);
  }

  public Branch GetBranch (string branchKey) {
    return template.GetBranch(branchKey);
  }

}
