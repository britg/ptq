using UnityEngine;
using System.Collections;

public class Constants {

  public const float SecondsPerHour = 3600f;

  // Notifications
  public const string OnUpdateFeed = "OnUpdateFeed";
  public const string OnUpdateFeedDone = "OnUpdateFeedDone";
  public const string OnUpdateEvents = "OnUpdateEvents";
  public const string OnUpdateAttribute = "OnUpdateStats";
  public const string OnTakeDamage = "OnTakeDamage";
  public const string OnNewGame = "OnNewGame";
  public const string OnFirstPull = "OnFirstPull";

  public const string OnRenderEvents = "OnRenderEvents";
  public const string OnRenderEventsDone = "OnRenderEventsDone";
  public const string OnTurnChange = "OnTurnChange";
  public const string OnRequireInput = "OnRequireInput";
  public const string OnEnvironmentStart = "OnEnvironmentStart";
  public const string OnEnvironmentUpdate = "OnEnvironmentUpdate";
  public const string OnUpdateFog = "OnUpdateFog";

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
  public const string playerContentKey = "player";
  public const string nothingContentKey = "nothing";
  public const string wallContentKey = "wall";
  public const string doorContentKey = "door";
  public const string mobContentKey = "mob";
  public const string interactibleContentKey = "interactible";
  public const string roomContentKey = "room";

  // Flow Labels
  public const string startEnvKey = "start_env";
  public const string enterKey = "_enter";
  public const string branchLabel = "branch:";
  public const string continueLabel = "continue";
  public const string eventsLabel = "events:";
  public const string eventGroupLabel = "event_group:";
  public const string promptPullLabel = "promp_pull";
  public const string endInteractionLabel = "end_interaction";

  // Attributes
  
  public const string expAttr = "exp";
  public const string levelAttr = "level";
  public const string goldAttr = "gold";
  public const string currentHpAttr = "current_hp";
  public const string maxHpAttr = "max_hp";
  public const string currentApAttr = "current_ap";
  public const string maxApAttr = "max_ap";
  public const string attackAttr = "attack";
  public const string defenseAttr = "defense";
  public const string speedAttr = "speed";
  public const string dpsAttr = "dps";
  public const string critAttr = "crit";
  public const string sightAttr = "sight";
  public const string initiativeAttr = "initiative";
}
