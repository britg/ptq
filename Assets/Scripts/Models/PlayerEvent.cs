using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerEvent {

  public const string equipmentKey = "equipment";
  public const string consumableKey = "consumable";
  public const string mobKey = "mob";
  public const string damageKey = "damage";
  public const string hitTypeKey = "hitType";
  public const string branchKey = "branchKey";
  public const string movementDeltaKey = "delta";
  public const string moverKey = "mover";
  public const string moverIdKey = "mover";


  public enum Type {
    Info,
    Story,
    Equipment,
    Consumable,
    Transition,
    Choice,
    PlayerBasicAttack,
    MobBasicAttack,
    Movement
  }

  public string Id { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }
  public Type type = Type.Info;

  // data can contain:
  //    equipment
  //    consumable
  public Hashtable data = new Hashtable();
  public List<Trigger> Triggers = new List<Trigger>();

  public Choice firstChoice;
  public Choice secondChoice;

  public string chosenKey;

  public bool hasTriggered = false;
  public bool conditionsSatisfied = true;

  public bool hasActions {
    get {
      return (type == Type.Equipment || type == Type.Consumable);
    }
  }

  public bool hasChoices {
    get {
      return type == Type.Choice;
    }
  }

  public bool requiresInput {
    get {
      return type == Type.Choice;
    }
  }

  public bool endsTurn {
    get {
      return (type != Type.Info && type != Type.Story);
    }
  }

  public bool isFeedEvent {
    get {
      return (type != Type.Movement);
    }
  }

  public PlayerEvent () {
    Id = System.Guid.NewGuid().ToString();
  }

  public PlayerEvent (string content) {
    Id = System.Guid.NewGuid().ToString();
    Content = content;
    type = Type.Info;
  }

  public static PlayerEvent Info (string text) {
    PlayerEvent ev = new PlayerEvent(text);
    ev.type = Type.Info;
    return ev;
  }

  public static PlayerEvent Story (string text) {
    PlayerEvent ev = new PlayerEvent(text);
    ev.type = Type.Story;
    return ev;
  }

  public static PlayerEvent Transition (string name) {
    PlayerEvent ev = new PlayerEvent(name);
    ev.type = Type.Transition;

    return ev;
  }

  public static PlayerEvent Loot (Equipment e) {
    PlayerEvent ev = new PlayerEvent(e.Name);
    ev.type = Type.Equipment;
    ev.data[equipmentKey] = e;
    return ev;
  }

  public static PlayerEvent Consumable (Consumable consumable) {
    PlayerEvent ev = new PlayerEvent(consumable.name);
    ev.type = Type.Consumable;
    ev.data[consumableKey] = consumable;
    return ev;
  }

  public static PlayerEvent PromptChoice (string text, Choice firstChoice, Choice secondChoice) {
    PlayerEvent ev = new PlayerEvent(text);
    ev.type = Type.Choice;
    ev.firstChoice = firstChoice;
    ev.secondChoice = secondChoice;
    ev.conditionsSatisfied = false;
    return ev;
  }

  public static PlayerEvent PromptChoice (Branch branch) {
    // TODO: Implement using branch stuff
    var ev = PromptChoice(branch.text, branch.firstChoice, branch.secondChoice);
    ev.data[PlayerEvent.branchKey] = branch;
    return ev;
  }

  public static PlayerEvent Movement (Vector3 delta) {
    var ev = new PlayerEvent();
    ev.type = Type.Movement;
    ev.data[moverKey] = Constants.playerContentKey;
    ev.data[movementDeltaKey] = delta;
    return ev;
  }

  public static PlayerEvent Movement (Vector3 delta, string mobId) {
    var ev = new PlayerEvent();
    ev.type = Type.Movement;
    ev.data[moverKey] = Constants.mobContentKey;
    ev.data[moverIdKey] = mobId;
    ev.data[movementDeltaKey] = delta;
    return ev;
  }

  //public static PlayerEvent PlayerBasicAttack () {
  //}

  public void Update () {
    // shim for when this gets persisted....
  }

}
