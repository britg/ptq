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

  public enum Type {
    Info,
    Equipment,
    Consumable,
    Transition,
    Choice,
    PlayerBasicAttack,
    MobBasicAttack,
    Story
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

  public bool blocksContinue {
    get {
      return type == Type.Choice;
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

  private static PlayerEvent PromptChoice (string text, Choice firstChoice, Choice secondChoice) {
    PlayerEvent ev = new PlayerEvent(text);
    ev.type = Type.Choice;
    ev.firstChoice = firstChoice;
    ev.secondChoice = secondChoice;
    ev.conditionsSatisfied = false;
    return ev;
  }

  public static PlayerEvent PromptChoice (JSONNode branch) {
    string text = branch["text"].Value;
    JSONArray choicesArr = branch["choices"].AsArray;
    JSONNode firstNode = choicesArr[0];
    JSONNode secondNode = choicesArr[1];
    var firstChoice = CreateChoice(firstNode);
    var secondChoice = CreateChoice(secondNode);
    var ev = PromptChoice(text, firstChoice, secondChoice);

    ev.data[PlayerEvent.branchKey] = branch;
    return ev;
  }

  public static Choice CreateChoice (JSONNode node) {
    var dirStr = node["pull"].Value;
    var key = node["key"].Value;
    var label = node["label"].Value;
    var choice = Choice.Swipe(dirStr, key, label);
    return choice;
  }

  //public static PlayerEvent PlayerBasicAttack () {
  //}

  public void Update () {
    // shim for when this gets persisted....
  }

}
