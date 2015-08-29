using UnityEngine;
using System.Collections;

public class FloorRenderer : BaseBehaviour {

  public GameObject playerPrefab;
  public GameObject wallPrefab;
  public GameObject doorPrefab;
  public GameObject roomPrefab;
  public GameObject corridorPrefab;

  Floor floor {
    get {
      return sim.player.currentFloor;
    }
  }

	// Use this for initialization
	void Start () {
    NotificationCenter.AddObserver(this, Constants.OnFloorUpdate);
    if (floor != null) {
      RenderFloor();
    }
	}

  void OnFloorUpdate () {
    RenderFloor();
  }

  void RenderFloor () {
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
    for (var r = 0; r < floor.map.GetLength(0); r++) {
      for (var c = 0; c < floor.map.GetLength(1); c++) {
        var tile = floor.map[r, c];
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

      }
    }
  }

  void RenderPlayer () {

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
