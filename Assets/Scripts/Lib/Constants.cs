using UnityEngine;
using System.Collections;

public class Constants {

  public const float SecondsPerHour = 3600f;

  // Notifications
  public const string OnTavernCreated = "OnTavernCreated";
  public const string OnAdventurerCreated = "OnAdventurerCreated";
  public const string OnEnterPendingSelection = "OnEnterPendingSelection";
  public const string OnWorldSelection = "OnWorldSelection";
  public const string OnUpdateEvents = "OnUpdateEvents";
  public const string OnUpdateStats = "OnUpdateStats";
  public const string OnTakeDamage = "OnTakeDamage";
  public const string OnNewGame = "OnNewGame";
  public const string OnFirstPull = "OnFirstPull";

  // Layers
  public const string GroundLayer = "Ground";

  // Choices
  public const string c_Equip = "equip";
  public const string c_Pickup = "pickup";
  public const string c_Shop = "shop";
  public const string c_Tower = "tower";
  public const string c_OpenDoor = "open_door";
  public const string c_LeaveDoor = "leave_door";

  // Content
  public const string mobContentKey = "mob";
  public const string interactibleContentKey = "interactible";
  public const string roomContentKey = "room";

}
