using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchProcessor {

  Simulation sim;
  Branch branch;

  public BranchProcessor (Simulation _sim, Branch _branch) {
    sim = _sim;
    branch = _branch;
  }

  public BranchProcessor (Simulation _sim, PlayerEvent ev) {
    sim = _sim;
    branch = (Branch)ev.data[PlayerEvent.branchKey];
  }

  public List<PlayerEvent> Start () {
    var ev = PlayerEvent.PromptChoice(branch);
    return new List<PlayerEvent>() { ev };
  }

  public List<PlayerEvent> Choose (string choiceKey) {
    var newEvents = new List<PlayerEvent>();
    var res = branch.results[choiceKey];
    foreach (string evTxt in res.events) {
      newEvents.Add(PlayerEvent.Story(evTxt));
    }

    if (res.thenToContinue) {
      return newEvents;
    }

    if (res.thenToRoom) {
      var roomTemplateKey = res.thenTo.Replace("room:", "");
      var roomProcessor = new RoomProcessor(sim, roomTemplateKey);
      newEvents.AddRange(roomProcessor.CreateAndEnter());
    }

    if (res.thenToEvents) {
      var envProcessor = new EnvironmentProcessor(sim);
      var eventsKey = res.thenTo.Replace("events:", "");
      newEvents.AddRange(envProcessor.Events(eventsKey));
    }

    return newEvents;
  }

}
