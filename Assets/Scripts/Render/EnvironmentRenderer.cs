using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentRenderer : BaseBehaviour {

  public GameObject playerObj;
  public GameObject tilePrefab;
  public GameObject floorPrefab;
  public GameObject wallPrefab;
  public GameObject doorPrefab;
  public GameObject roomPrefab;
  public GameObject corridorPrefab;
  public GameObject mobPrefab;
  public GameObject interactiblePrefab;
  public GameObject fogPrefab;

  Dictionary<Vector3, GameObject> tileObjs;


  Environment env {
    get {
      return sim.currentEnvironment;
    }
  }

	// Use this for initialization
	void Start () {
    NotificationCenter.AddObserver(this, Constants.OnEnvironmentUpdate);
    NotificationCenter.AddObserver(this, Constants.OnUpdateFog);
	}

  void OnEnvironmentUpdate () {
    RenderEnvironment();
  }

  void OnUpdateFog () {
  }

  void RenderEnvironment () {
    ClearAll();
    RenderBaseLayer();
    RenderActiveLayer();
    RenderPlayer();
  }

  void ClearAll () {
    foreach (Transform childTransform in transform) {
      Destroy(childTransform.gameObject);
    }
  }

  void RenderBaseLayer () {
    tileObjs = new Dictionary<Vector3, GameObject>();
    foreach (KeyValuePair<Vector3, Tile> pair in env.tiles) {
      var pos = pair.Key;
      var tile = pair.Value;
      GameObject tileObj = PlaceObj(tilePrefab, pos);
      tileObj.name = pos.ToString();

//      var fogObj = PlaceObj(fogPrefab, pos);
//      fogMap[fogObj.transform.localPosition] = fogObj;
//      fogObj.transform.parent = fogParent.transform;

      switch (tile.contentType) {
      case Constants.wallContentKey:
        AddContent(wallPrefab, tileObj);
        break;
      case Constants.doorContentKey:
        AddContent(doorPrefab, tileObj);
        break;
      default:
        AddContent(floorPrefab, tileObj);
        break;
      }

      tileObjs[pos] = tileObj;

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

//    if (!tile.visible) {
//      PlaceObj(fogPrefab, tile.position);
//      return;
//    }

    if (!tile.occupied) {
      return;
    }

    if (tile.contentType == Constants.mobContentKey) {
      var mob = MobStore.Find(tile.contentId);
      PlaceObj(mobPrefab, mob.position, tile.contentId);
    }

    if (tile.contentType == Constants.interactibleContentKey) {
      var interaction = InteractibleStore.Find(tile.contentId);
      PlaceObj(interactiblePrefab, interaction.position, tile.contentId);
    }
  }

  void RenderPlayer () {
    playerObj.transform.parent = transform.parent;
    playerObj.transform.localPosition = sim.player.position;
  }

  void UpdateNewlyVisibleTiles () {
    RenderRoom(sim.currentRoom);
  }

  GameObject PlaceObj (GameObject prefab, int r, int c) {
    return PlaceObj(prefab, new Vector3(c, prefab.transform.localPosition.y, r));
  }

  GameObject PlaceObj (GameObject prefab, Vector3 pos) {
    return PlaceObj(prefab, pos, prefab.name);
  }

  GameObject PlaceObj (GameObject prefab, Vector3 pos, string id) {
    var obj = Instantiate(prefab);
    obj.name = id;
    obj.transform.parent = transform;
    obj.transform.localPosition = pos;
    return obj;
  }

  void AddContent (GameObject prefab, GameObject tileObj) {
    var originalPos = prefab.transform.position;
    var tileContent = PlaceObj(prefab, tileObj.transform.position);
    tileContent.transform.parent = tileObj.transform;
    tileContent.transform.localPosition = originalPos;
  }
}
