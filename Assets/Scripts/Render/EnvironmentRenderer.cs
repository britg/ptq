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
	}

  void OnEnvironmentUpdate () {
    RenderEnvironment();
  }

  void OnUpdateFog () {
  }

  void RenderEnvironment () {
    ClearAll();
    RenderGame();
    RenderPlayer();
  }

  void ClearAll () {
    foreach (Transform childTransform in transform) {
      Destroy(childTransform.gameObject);
    }
  }

  void RenderGame () {
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

      case Constants.mobContentKey:
        AddContent(floorPrefab, tileObj);
        AddContent(mobPrefab, tileObj);
        break;
      
      case Constants.interactibleContentKey:
        AddContent(floorPrefab, tileObj);
        AddContent(interactiblePrefab, tileObj);
        break;

      default:
        AddContent(floorPrefab, tileObj);
        break;
      }

      tile.SetTileObj(tileObj);
      tileObjs[pos] = tileObj;

    }
  }

  void RenderPlayer () {
    playerObj.transform.parent = transform.parent;
    playerObj.transform.localPosition = sim.player.position;
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
