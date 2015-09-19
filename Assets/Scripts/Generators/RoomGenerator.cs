using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator {

  RoomTemplate roomTemplate;
  Environment env;
  DunGen.Room roomBase;
  Room room;

  public RoomGenerator (Environment _env, RoomTemplate _roomTemplate, DunGen.Room _roomBase) {
    roomTemplate = _roomTemplate;
    env = _env;
    roomBase = _roomBase;
  }

  public Room CreateRoom () {
    room = new Room();
    room.id = roomBase.id.ToString();
    room.tiles = InitTiles();
    room.roomTemplate = roomTemplate;
    room.content = roomTemplate.content;

    GenerateContent();

    return room;
  }

  List<Tile> InitTiles () {
    var roomTiles = new List<Tile>();
    foreach (Vector3 position in roomBase.tiles) {
      var tile = env.tiles[position];
      roomTiles.Add(tile);
    }

    return roomTiles;
  }

  void GenerateContent () {
    foreach (Tile tile in room.tiles) {
      if (!tile.occupied) {
        var contentKey = tpd.RollMap(room.content);

        if (contentKey == Constants.mobContentKey) {
          GenerateMob(tile);
        } else if (contentKey == Constants.interactibleContentKey) {
          GenerateInteractible(tile);
        }

      }
    }
  }

  void GenerateMob (Tile tile) {
    var mobTemplateKey = tpd.RollMap(env.mobChances);

    var mobTemplate = JSONResource.Get<MobTemplate>(mobTemplateKey);
    var mobGenerator = new MobGenerator(mobTemplate);
    var mob = mobGenerator.Generate();
    var randomTile = room.RandomOpenTile();

    mob.position = randomTile.position;

    MobStore.Save(mob);
    randomTile.Occupy(Constants.mobContentKey, mob.id);
  }

  void GenerateInteractible (Tile tile) {
    var interactibleTemplateKey = tpd.RollMap(env.interactibleChances);
    var interactibleTemplate = JSONResource.Get<InteractionTemplate>(interactibleTemplateKey);
    var interactibleGenerator = new InteractionGenerator(interactibleTemplate);
    var interactible = interactibleGenerator.Generate();
    var randomTile = room.RandomOpenTile();
    interactible.position = randomTile.position;

    InteractibleStore.Save(interactible);
    randomTile.Occupy(Constants.interactibleContentKey, interactible.id);
  }

}
