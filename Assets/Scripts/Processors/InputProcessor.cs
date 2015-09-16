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

  public InputProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void TriggerContinue () {
    Debug.Log("Trigger continue");
    sim.EndPullPrompt();
    TurnProcessor.With(sim).TakeTurn();
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

    var interactionProcessor = new InteractionProcessor(sim);
    interactionProcessor.StoreChoice(choiceKey);

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
