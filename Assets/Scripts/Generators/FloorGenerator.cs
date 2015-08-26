using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorGenerator {

  int[,] dungeonLayout = new int[,] { { 1, 1, 1, }, { 1, 0, 1 }, { 1, 1, 1 } };
  int corridorLayout = 50;

  int NOTHING = 0x00000000;
  int BLOCKED = 0x00000001;
  int ROOM = 0x00000002;
  int CORRIDOR = 0x00000004;

  int PERIMETER   = 0x00000010;
  int ENTRANCE = 0x00000020;
  int ROOM_ID = 0x0000FFC0;

  int ARCH = 0x00010000;
  int DOOR = 0x00020000;
  int LOCKED = 0x00040000;
  int TRAPPED = 0x00080000;
  int SECRET = 0x00100000;
  int PORTC = 0x00200000;
  int STAIR_DN = 0x00400000;
  int STAIR_UP = 0x00800000;

  uint LABEL = 0xFF000000;

  int OPENSPACE {
    get {
      return ROOM | CORRIDOR;
    }
  }

  int DOORSPACE {
    get {
      return ARCH | DOOR | LOCKED | TRAPPED | SECRET | PORTC;
    }
  }

  long ESPACE {
    get {
      return ENTRANCE | DOORSPACE | 0xFF000000;
    }
  }

  int STAIRS {
    get {
      return STAIR_DN | STAIR_UP;
    }
  }

  int BLOCK_ROOM {
    get {
      return BLOCKED | ROOM;
    }
  }

  int BLOCK_CORR {
    get {
      return BLOCKED | PERIMETER | CORRIDOR;
    }
  }

  int BLOCK_DOOR {
    get {
      return BLOCKED | DOORSPACE;
    }
  }

  Dictionary<string, int> di = new Dictionary<string, int>() {
    { "north", -1 }, { "south", 1 }, {"west", 0 }, {"east", 0 }
  };

  Dictionary<string, int> dj = new Dictionary<string, int>() {
    { "north", 0 }, { "south", 0 }, {"west", -1 }, {"east", 1 }
  };

  Dictionary<string, string> opposite = new Dictionary<string, string>() {
    {"north", "south" },
    
    {"west", "east" },
    {"east", "west" }
  };

  Hashtable defaultOpts = new Hashtable() {
    {"n_rows", 39 },
    {"n_cols", 39 },
    {"dungeon_layout", "None" },
    {"room_min", 3 },
    {"room_max", 9 },
    {"room_layout", "Scattered" },
    {"corridor_layout", "Bent" },
    {"remove_deadends", 100 },
    {"add_stairs", 2 },
    {"map_style", "Standard" },
    {"cell_size",  18 }
  };

  int[,] cells;
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

  Hashtable proto;

  public int[,] CreateDungeon (Hashtable _opts) {
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

    cells = new int[n_rows, n_cols];
    cells = InitCells(cells);
    cells = PackRooms(cells);

    return cells;
  }

  int[,] InitCells (int[,] _cells) {

    for (var r = 0; r <= n_rows; r++) {
      for (var c = 0; c <= n_cols; c++) {
        _cells[r,c] = NOTHING;
      }
    }

    return _cells;
  }

  int[,] PackRooms (int[,] _cells) {

    for (var i = 0; i < n_i; i++) {
      var r = (i * 2) + 1;

      for (var j = 0; j < n_j; j++) {
        var c = (j * 2) + 1;

        if ((_cells[r,c] & ROOM) != 0) {
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

  int[,] PlaceRoom (int[,] _cells, Hashtable proto) {

    proto = SetRoom(proto);
    
    return _cells;
  }

  Hashtable SetRoom (Hashtable _proto) {

    bool heightDefined = _proto.ContainsKey("height");
    bool widthDefined = _proto.ContainsKey("width");

    if (!heightDefined) {

    }

    if (!widthDefined) {

    }



    return _proto;
  }

  int[,] OpenRooms (int[,] _cells) {

    return _cells;
  }

  int[,] LabelRooms (int[,] _cells) {

    return _cells;
  }

  int[,] Corridors (int[,] _cells) {

    return _cells;
  }

  int[,] EmplaceStairs (int[,] _cells) {

    return _cells;
  }

  int[,] CleanDungeon (int[,] _cells) {

    return _cells;
  }


}