using UnityEngine;
using System.Collections;

public class EnvironmentRenderer : BaseBehaviour {

  public GameObject playerObj;
  public GameObject wallPrefab;
  public GameObject doorPrefab;
  public GameObject roomPrefab;
  public GameObject corridorPrefab;
  public GameObject mobPrefab;
  public GameObject interactiblePrefab;


  Environment env {
    get {
      return sim.currentEnvironment;
    }
  }

	// Use this for initialization
	void Start () {
    NotificationCenter.AddObserver(this, Constants.OnEnvironmentUpdate);
	}

  void OnEnvironmentUpdate () {
    RenderEnvironment();
  }

  void RenderEnvironment () {
    ClearAll();
    RenderBaseLayer();
    RenderActiveLayer();
    RenderPlayer();
  }

  void ClearAll () {
    foreach (Transform childTransform in transform) {
      Destroy(childTransform);
    }
  }

  void RenderBaseLayer () {
    for (var r = 0; r < env.floor.tiles.GetLength(0); r++) {
      for (var c = 0; c < env.floor.tiles.GetLength(1); c++) {
        var tile = env.floor.tiles[r, c];
        if (tile == DunGen.TileType.Perimeter
            || tile == DunGen.TileType.Nothing
            || tile == DunGen.TileType.Blocked) {
          PlaceObj(wallPrefab, r, c);
        }

        if (tile == DunGen.TileType.Door) {
          PlaceObj(doorPrefab, r, c);
        }

        //if (tile == DunGen.TileType.Room) {
        //  PlaceObj(roomPrefab, r, c);
        //}

        //if (tile == DunGen.TileType.Corridor) {
        //  PlaceObj(corridorPrefab, r, c);
        //}

        // if (tile == DunGen.TileType.Entrance) {
        //  PlaceObj(corridorPrefab, r, c);
        // }

      }
    }
  }

  void RenderActiveLayer () {
    foreach (Room room in env.rooms) {
      RenderRoom(room);
    }
    //foreach (DictionaryEntry e in env.activeLayer) {
    //  Vector3 pos = (Vector3)e.Key;

    //  // tmp implementation
    //  string thing = (string)e.Value;
    //  if (thing == Constants.mobContentKey) {
    //    PlaceObj(mobPrefab, (int)pos.z, (int)pos.x);
    //  } else if (thing == Constants.interactibleContentKey) {
    //    PlaceObj(interactiblePrefab, (int)pos.z, (int)pos.x);
    //  }
    //}
  }

  void RenderRoom (Room room) {
    foreach (Tile tile in room.tiles) {
      RenderTile(tile);
    }
  }

  void RenderTile (Tile tile) {
    if (!tile.occupied) {
      return;
    }

    if (tile.contentType == Constants.mobContentKey) {
      var mob = MobStore.Find(tile.contentId);
      PlaceObj(mobPrefab, mob.position, tile.contentId);
    }
  }

  void RenderPlayer () {
    playerObj.transform.parent = transform.parent;
    playerObj.transform.localPosition = sim.player.position;
  }

  void PlaceObj (GameObject prefab, int r, int c) {
    PlaceObj(prefab, new Vector3(c, prefab.transform.localPosition.y, r));
  }

  void PlaceObj (GameObject prefab, Vector3 pos) {
    PlaceObj(prefab, pos, prefab.name);
  }

  void PlaceObj (GameObject prefab, Vector3 pos, string id) {
    var obj = Instantiate(prefab);
    obj.name = id;
    obj.transform.parent = transform;
    obj.transform.localPosition = pos;
  }

	// Update is called once per frame
	void Update () {

	}
}
