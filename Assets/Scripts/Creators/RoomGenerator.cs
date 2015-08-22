using UnityEngine;
using System.Collections;

public class RoomGenerator {

  string roomTemplateKey;
  Floor floor;

  public RoomGenerator (string _roomTemplateKey, Floor _floor) {
    roomTemplateKey = _roomTemplateKey;
    floor = _floor;
  }

  public Room CreateRoom () {
    var room = new Room();
    room.roomTemplate = RoomTemplate.all[roomTemplateKey];
    room.content = room.roomTemplate.CascadeContent(floor.content);
    return room;
  }

}
