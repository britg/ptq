using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DunGen {

  public enum TileType {
    Nothing,
    Blocked,
    Room,
    Corridor,
    Perimeter,
    Entrance,
    Door,
    StairUp,
    StairDown
  }

  int[,] dungeonLayout = new int[,] { { 1, 1, 1, }, { 1, 0, 1 }, { 1, 1, 1 } };
  int corridorLayout = 50;

  Dictionary<string, int> di = new Dictionary<string, int>() {
    { "north", -1 }, { "south", 1 }, {"west", 0 }, {"east", 0 }
  };

  Dictionary<string, int> dj = new Dictionary<string, int>() {
    { "north", 0 }, { "south", 0 }, {"west", -1 }, {"east", 1 }
  };

  List<string> dj_dirs = new List<string>() { "north", "south", "west", "east" };

  Dictionary<string, string> opposite = new Dictionary<string, string>() {
    {"north", "south" },
    
    {"west", "east" },
    {"east", "west" }
  };

  Hashtable defaultOpts = new Hashtable() {
    {"n_rows", 39 },
    {"n_cols", 39 },
    {"dungeon_layout", "None" },
    {"room_min", 5 },
    {"room_max", 11 },
    {"room_layout", "Packed" },
    {"corridor_layout", "Bent" },
    {"remove_deadends", 100 },
    {"add_stairs", 2 },
    {"map_style", "Standard" },
    {"cell_size",  18 }
  };

  TileType[,] cells;
  Hashtable rooms;
  Hashtable opts;
  int n_i;
  int n_j;
  int n_rows;
  int n_cols;
  int max_row;
  int max_col;
  int n_rooms;
  int room_min;
  int room_max;
  int room_base;
  int room_radix;
  int last_room_id = 0;

  Hashtable proto;

  public TileType[,] CreateDungeon () {
    return CreateDungeon(defaultOpts);
  }

  public TileType[,] CreateDungeon (Hashtable _opts) {
    // TODO: Merge default opts with opts;
    opts = defaultOpts;

    n_i = (int)((int)opts["n_rows"] / 2);
    n_j = (int)((int)opts["n_cols"] / 2);
    n_rows = n_i * 2;
    n_cols = n_j * 2;
    max_row = n_rows - 1;
    max_col = n_cols - 1;
    n_rooms = 0;
    room_min = (int)opts["room_min"];
    room_max = (int)opts["room_max"];
    room_base = (int)((room_min + 1) / 2);
    room_radix = (int)((room_max - room_min) / 2) + 1;

    rooms = new Hashtable();
    cells = new TileType[n_rows+1, n_cols+1];
    cells = InitCells(cells);
    cells = PackRooms(cells);
    cells = OpenRooms(cells, rooms);
    cells = CreateCorridors(cells);

    Debug.Log (cells);

    return cells;
  }

  TileType[,] InitCells (TileType[,] _cells) {

    for (var r = 0; r <= n_rows; r++) {
      for (var c = 0; c <= n_cols; c++) {
        _cells[r,c] = TileType.Nothing;
      }
    }

    return _cells;
  }

  TileType[,] PackRooms (TileType[,] _cells) {

    for (var i = 0; i < n_i; i++) {
      var r = (i * 2) + 1;

      for (var j = 0; j < n_j; j++) {
        var c = (j * 2) + 1;

        if (_cells[r,c] == TileType.Room) {
          continue;
        }

        if ((i == 0 || j == 0) && Random.Range(0, 2) == 0) {
          continue;
        }

        Hashtable proto = new Hashtable() {
          {"i", i }, {"j", j }
        };
        _cells = PlaceRoom(_cells, proto);

      }
    }

    return _cells;
  }

  TileType[,] PlaceRoom (TileType[,] _cells, Hashtable proto) {

    proto = SetRoom(proto);

    var r1 = ((int)proto["i"] * 2) + 1;
    var c1 = ((int)proto["j"] * 2) + 1;
    var r2 = (((int)proto["i"] + (int)proto["height"]) * 2) - 1;
    var c2 = (((int)proto["j"] + (int)proto["width"]) * 2) - 1;

    if (r1 < 1 || r2 > max_row) {
      return _cells;
    }

    if (c1 < 1 || c2 > max_col) {
      return _cells;
    }

    bool hit = RoomCollision(_cells, r1, c1, r2, c2);
    if (hit) {
      return _cells;
    }

    int room_id = ++n_rooms;
    last_room_id = room_id;

    for (var r = r1; r <= r2; r++) {
      for (var c = c1; c <= c2; c++) {
        if (_cells[r, c] == TileType.Entrance) {
//          _cells[r, c] &= ~ ESPACE;
        } else if (_cells[r, c] == TileType.Perimeter) {

        }
        _cells[r, c] = TileType.Room;
      }
    }

    rooms[room_id] = new Hashtable () {
      {"id", room_id}, {"row", r1}, {"col", c1},
      {"north", r1}, {"south", r2}, {"west", c1}, {"east", c2}
    };

    // Perimeters
    for (var r = r1-1; r <= r2+1; r++) {
      if (_cells[r, c1-1] != TileType.Room && _cells[r, c1-1] != TileType.Entrance) {
        _cells[r, c1-1] = TileType.Perimeter;
      }
      if (_cells[r, c2+1] != TileType.Room && _cells[r, c2+1] != TileType.Entrance) {
        _cells[r, c2+1] = TileType.Perimeter;
      }
    }

    for (var c = c1-1; c <= c2+1; c++) {
      if (_cells[r1-1, c] != TileType.Room && _cells[r1-1, c] != TileType.Entrance) {
        _cells[r1-1, c] = TileType.Perimeter;
      }
      if (_cells[r2+1, c] != TileType.Room && _cells[r2+1, c] != TileType.Entrance) {
        _cells[r2+1, c] = TileType.Perimeter;
      }
    }

    return _cells;
  }

  Hashtable SetRoom (Hashtable _proto) {

    bool heightDefined = _proto.ContainsKey("height");
    bool widthDefined = _proto.ContainsKey("width");
    bool iDefined = _proto.ContainsKey("i");
    bool jDefined = _proto.ContainsKey("j");

    if (!heightDefined) {
      if (iDefined) {
        var a = n_i - room_base - (int)_proto["i"];
        if (a < 0) {
          a = 0;
        }

        var r = (a < room_radix) ? a : room_radix;
        _proto["height"] = (int)Random.Range(0, r) + room_base;
      } else {
        _proto["height"] = (int)Random.Range(0, room_radix) + room_base;
      }
    }

    if (!widthDefined) {
      if (jDefined) {
        var a = n_j - room_base - (int)_proto["j"];
        if (a < 0) {
          a = 0;
        }

        var r = (a < room_radix) ? a : room_radix;
        _proto["width"] = (int)Random.Range(0, r) + room_base;
      } else {
        _proto["width"] = (int)Random.Range(0, room_radix) + room_base;
      }
    }

    if (!iDefined) {
      _proto["i"] = Random.Range(0, n_i - (int)_proto["height"]);
    }

    if (!jDefined) {
      _proto["j"] = Random.Range(0, n_j - (int)_proto["width"]);
    }

    return _proto;
  }

  bool RoomCollision (TileType[,] _cells, int r1, int c1, int r2, int c2) {

    for (int r = r1; r <= r2; r++) {
      for (int c = c1; c <= c2; c++) {

        if (_cells[r, c] == TileType.Blocked) {
          return true;
        }

        if (_cells[r, c] == TileType.Room) {
          return true;
        }
      }
    }

    return false;
  }

  TileType[,] OpenRooms (TileType[,] _cells, Hashtable _rooms) {

    foreach (DictionaryEntry entry in _rooms) {
      var room_id = (int)entry.Key;
      var room = (Hashtable)entry.Value;

      _cells = OpenRoom(_cells, room);
    }

    return _cells;
  }

  TileType[,] OpenRoom (TileType[,] _cells, Hashtable room) {

    var sills = DoorSills(_cells, room);
//    int n_opens = AllocateOpens(_cells, room);
    int n_opens = Random.Range (1, 3);

    for (var i = 0; i < n_opens; i++) {
      var rand = Random.Range (0, sills.Count-1);
      var sill = sills[rand];
      var door_r = (int)sill["door_r"];
      var door_c = (int)sill["door_c"];
      _cells[door_r, door_c] = TileType.Door;
    }

    return _cells;
  }

  List<Hashtable> DoorSills (TileType[,] _cells, Hashtable room) {

    var list = new List<Hashtable>();

    if ((int)room["north"] >= 3) {
      for (var c = (int)room["west"]; c <= (int)room["east"]; c += 2) {
        var sill = CheckSill(_cells, room, (int)room["north"], c, "north");
        if (sill != null) {
          list.Add(sill);
        }
      }
    }

    if ((int)room["south"] <= (n_rows - 3)) {
      for (var c = (int)room["west"]; c <= (int)room["east"]; c += 2) {
        var sill = CheckSill(_cells, room, (int)room["south"], c, "south");
        if (sill != null) {
          list.Add(sill);
        }
      }
    }

    if ((int)room["west"] >= 3) {
      for (var r = (int)room["north"]; r <= (int)room["south"]; r += 2) {
        var sill = CheckSill(_cells, room, r, (int)room["west"], "west");
        if (sill != null) {
          list.Add(sill);
        }
      }
    }

    if ((int)room["east"] <= n_cols - 3) {
      for (var r = (int)room["north"]; r <= (int)room["south"]; r+= 2) {
        var sill = CheckSill(_cells, room, r, (int)room["east"], "east");
        if (sill != null) {
          list.Add(sill);
        }
      }
    }

    return list;
  }

  Hashtable CheckSill (TileType[,] _cells, Hashtable room, int sill_r, int sill_c, string dir) {

    var door_r = sill_r + di[dir];
    var door_c = sill_c + dj[dir];

    var door_cell = _cells[door_r, door_c];
    if (door_cell != TileType.Perimeter) {
      return null;
    }

    var out_r = door_r + di[dir];
    var out_c = door_c + dj[dir];
    var out_cell = _cells[out_r, out_c];
    if (out_cell == TileType.Blocked) {
      return null;
    }

    if (out_cell == TileType.Room) {
      // get the room id of the cell and 
      // return null if out is into the same room
    }

    var sill = new Hashtable() {
      {"sill_r", sill_r},
      {"sill_c", sill_c},
      {"dir", dir},
      {"door_r", door_r},
      {"door_c", door_c},
      {"out_id", 0} // temp
    };

    return sill;
  }

  int AllocateOpens (TileType[,] _cells, Hashtable room) {

    var room_h = (((int)room["south"] - (int)room["north"]) / 2) + 1;
    var room_w = (((int)room["east"] - (int)room["west"]) / 2) + 1;
    var f = (int)Mathf.Sqrt((float)room_w * (float)room_h);

    return (f + (int)Random.Range(0, f));
  }

  TileType[,] CreateCorridors (TileType[,] _cells) {

    for (var i = 1; i < n_i; i++) {
      var r = (i * 2) + 1;
      for (var j = 1; j < n_j; j++) {
        var c = (j * 2) + 1;

        if (_cells[r, c] == TileType.Corridor) {
          continue;
        }

        _cells = CreateTunnel(_cells, i, j, null);
      }
    }
    return _cells;
  }

  TileType[,] CreateTunnel (TileType[,] _cells, int i, int j, string last_dir) {
    var dirs = TunnelDirections(_cells, last_dir);

    foreach (string dir in dirs) {
      if (OpenTunnel(ref _cells, i, j, dir)) {
        var next_i = i + di[dir];
        var next_j = j + dj[dir];

        _cells = CreateTunnel(_cells, next_i, next_j, last_dir);
      }
    }

    return _cells;
  }

  List<string> TunnelDirections (TileType[,] _cells, string lastDirection) {
    var dirs = (List<string>)Shuffle(dj_dirs);

    if (lastDirection != null) {
      if (Roll.Percent(corridorLayout)) {
        dirs.Insert(0, lastDirection);
      }
    }

    return dirs;
  }

  bool OpenTunnel (ref TileType[,] _cells, int i, int j, string dir) {
    var this_r = (i * 2) + 1;
    var this_c = (j * 2) + 1;
    var next_r = ((i + di[dir]) * 2) + 1;
    var next_c = ((j + dj[dir]) * 2) + 1;
    var mid_r = (this_r + next_r) / 2;
    var mid_c = (this_c + next_c) / 2;

    if (SoundTunnel(_cells, mid_r, mid_c, next_r, next_c)) {
      _cells = DelveTunnel(_cells, this_r, this_c, next_r, next_c);
      return true;
    }

    return false;
  }

  bool SoundTunnel (TileType[,] _cells, int mid_r, int mid_c, int next_r, int next_c) {

    if (next_r < 0 || next_r > n_rows) {
      return false;
    }

    if (next_c < 0 || next_c > n_cols) {
      return false;
    }

    var rList = new List<int>() { mid_r, next_r };
    rList.Sort();
    var r1 = rList[0];
    var r2 = rList[1];

    var cList = new List<int>() { mid_c, next_c };
    cList.Sort();
    var c1 = cList[0];
    var c2 = cList[1];

    for (var r = r1; r <= r2; r++) {
      for (var c = c1; c <= c2; c++) {
        if (_cells[r, c] == TileType.Blocked || _cells[r, c] == TileType.Perimeter || _cells[r, c] == TileType.Corridor) {
          return false;
        }
      }
    }

    return true;
  }

  TileType[,] DelveTunnel (TileType[,] _cells, int this_r, int this_c, int next_r, int next_c) {

    var rList = new List<int>() { this_r, next_r };
    rList.Sort();
    var r1 = rList[0];
    var r2 = rList[1];
    
    var cList = new List<int>() { this_c, next_c };
    cList.Sort();
    var c1 = cList[0];
    var c2 = cList[1];

    for (var r = r1; r <= r2; r++) {
      for (var c = c1; c <= c2; c++) {
        _cells[r,c] = TileType.Corridor;
      }
    }

    return _cells;
  }

  int[,] EmplaceStairs (int[,] _cells) {

    return _cells;
  }

  int[,] CleanDungeon (int[,] _cells) {

    return _cells;
  }


  public IList<T> Shuffle<T>(IList<T> list) {
    int n = list.Count;
    while (n > 1) {
      n--;
      int k = Random.Range(0, n + 1);
      T value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
    return list;
  }

}