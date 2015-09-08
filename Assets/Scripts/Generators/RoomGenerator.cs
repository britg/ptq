using UnityEngine;
using System.Collections;

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
    room = new Room(roomBase);
    room.roomTemplate = roomTemplate;
    room.content = roomTemplate.content;

    GenerateContent();

    return room;
  }

  void GenerateContent () {
    foreach (Tile tile in room.tiles) {
      if (!tile.occupied) {
        var contentKey = tpd.RollMap(room.content);

        if (contentKey == Constants.mobContentKey) {
          GenerateMob(tile);
        }

      }
    }
  }

  void GenerateMob (Tile tile) {
    // convert MobTemplate to a JSONResource
    // pick a random mob template available to this room
    // create a MobGenerator that takes a template, returns a mob
    var mobKey = tpd.RollMap(env.mobChances);

    var mobTemplate = MobTemplate.Get<MobTemplate>(mobKey);
    var mobGenerator = new MobGenerator(mobTemplate);
    var mob = mobGenerator.Generate();
    var randomTile = room.RandomOpenTile();

    mob.position = randomTile.position;

    MobRepository.Save(mob);
    randomTile.Occupy(Constants.mobContentKey, mob.id);
  }

}
