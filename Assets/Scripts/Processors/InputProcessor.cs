using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class InputProcessor {

  Simulation sim;
  Player player {
    get {
      return sim.player;
    }
  }

  public bool canContinue {
    get {
      if (player.currentEvent == null) {
        return true;
      }
      return player.currentEvent.conditionsSatisfied;
    }
  }

  public InputProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void Continue () {
    Debug.Log("Continuing...");
    sim.newEvents = new List<PlayerEvent>();

    if (!player.currentlyOccupied) {
      var envProcessor = new EnvironmentProcessor(sim);

      if (player.currentEvent == null) {
        NotificationCenter.PostNotification(Constants.OnFirstPull);
        NotificationCenter.PostNotification(Constants.OnEnvironmentUpdate);
        sim.newEvents.AddRange(envProcessor.Enter());
      } else {
        sim.newEvents.AddRange(envProcessor.Explore());
      }
    }

    if (player.currentChoiceKey != null) {
      var branchProcessor = new BranchProcessor(sim, player.currentEvent);
      sim.newEvents.AddRange(branchProcessor.Choose(player.currentChoiceKey));
    }

    if (player.currentMob != null) {
      var battleProcessor = new BattleProcessor(sim);
      sim.newEvents.AddRange(battleProcessor.Continue());
    }
    
    if (player.currentInteractible != null) {
      var interactionProcessor = new InteractionProcessor(sim);
      sim.newEvents.AddRange(interactionProcessor.Continue());
    }

    if (sim.newEvents.Count > 0) {
      player.currentEvent = sim.newEvents[sim.newEvents.Count - 1];
    }

    NotificationCenter.PostNotification(Constants.OnRenderEvents);

  }

  public void TriggerEvent (PlayerEvent ev) {
    foreach (Trigger trigger in ev.Triggers) {
      var triggerProcessor = new TriggerProcessor(sim, trigger);
      triggerProcessor.Process();
      ev.hasTriggered = true;
      ev.Update();
    }
  }

  public void TriggerAction (PlayerEvent ev, string actionName) {
    Debug.Log ("Trigger action " + actionName + " for event " + ev.Content);

    if (ev.type == PlayerEvent.Type.Equipment) {
      var equipmentActionProcessor = new EquipmentActionProcessor(sim);
      equipmentActionProcessor.HandleAction(ev, actionName);
    }

    if (ev.type == PlayerEvent.Type.Consumable) {
      var consumableActionProcessor = new ConsumableActionProcessor(sim);
      consumableActionProcessor.HandleAction(ev, actionName);
    }

    NotificationCenter.PostNotification(Constants.OnUpdateEvents);
  }

  public void TriggerChoice (PlayerEvent ev, string choiceKey) {
    Debug.Log ("Trigger choice " + choiceKey + " for event " + ev.Content);

    // TODO: Refactor into a choice processor when necessary
    ev.chosenKey = choiceKey;
    ev.conditionsSatisfied = true;

    NotificationCenter.PostNotification(Constants.OnUpdateEvents);
  }


  List<PlayerEvent> Dev_RandomLoot () {
    var list = new List<PlayerEvent>();
    var eqGen = new EquipmentGenerator(sim);
    var rand = Random.Range(7, 15);
    var eRand = Random.Range(1, 4);
    for (int i = 0; i < rand; i++) {
      var eq = eqGen.Generate();
      var ev = PlayerEvent.Loot(eq);

      for (int j = 0; j < eRand; j++) {
        //        list.Add(fill);
      }
      list.Add(ev);
    }

    return list;
  }

}
