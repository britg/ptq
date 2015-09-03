using UnityEngine;
using System.Collections;

public class EnvironmentRenderer : BaseBehaviour {

  public GameObject playerObj;
  public GameObject wallPrefab;
  public GameObject doorPrefab;
  public GameObject roomPrefab;
  public GameObject corridorPrefab;

  Environment env {
    get {
      return sim.environment;
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
    RenderPlayer();
  }

  void ClearAll () {
    foreach (Transform childTransform in transform) {
      Destroy(childTransform);
    }
  }

  void RenderBaseLayer () {
    for (var r = 0; r < env.baseLayer.GetLength(0); r++) {
      for (var c = 0; c < env.baseLayer.GetLength(1); c++) {
        var tile = env.baseLayer[r, c];
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

  void RenderPlayer () {
    playerObj.transform.parent = transform.parent;
    playerObj.transform.localPosition = env.playerPos;
  }

  void PlaceObj (GameObject prefab, int r, int c) {
    var obj = Instantiate(prefab);
    obj.transform.parent = transform;
    obj.transform.localPosition = new Vector3(c, 0, r);
  }

	// Update is called once per frame
	void Update () {

	}
}
