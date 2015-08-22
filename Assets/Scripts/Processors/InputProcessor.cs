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

  public List<PlayerEvent> Continue () {

    List<PlayerEvent> newEvents = new List<PlayerEvent>();

    if (player.currentFloor == null) {
      NotificationCenter.PostNotification(Constants.OnFirstPull);
      var floorProcessor = new FloorProcessor(sim);
      newEvents.AddRange(floorProcessor.EnterFloor(1));
    }

//    if (player.currentChoice == Choice.OpenDoor) {
//      var roomProcessor = new RoomProcessor(sim);
//      newEvents.AddRange(roomProcessor.OpenDoor());
//      player.currentChoice = null;
//    }

    if (player.currentChoiceKey != null) {
      Debug.Log ("Current choice key " + player.currentChoiceKey);
    }

    if (player.currentRoom != null) {
      var roomProcessor = new RoomProcessor(sim);
      newEvents.AddRange(roomProcessor.Continue());
    }

    if (player.currentMob != null) {
      var battleProcessor = new BattleProcessor(sim);
      newEvents.AddRange(battleProcessor.Continue());
    }
    
    if (player.currentInteractible != null) {
      var interactionProcessor = new InteractionProcessor(sim);
      newEvents.AddRange(interactionProcessor.Continue());
    }

    if (newEvents.Count > 0) {
      player.currentEvent = newEvents[newEvents.Count - 1];
    }

    if (!player.currentlyOccupied) {
      var towerProcessor = new TowerProcessor(sim);
      newEvents.AddRange(towerProcessor.Continue());
    }

    if (newEvents.Count > 0) {
      player.currentEvent = newEvents[newEvents.Count - 1];
    }

    return newEvents;
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
