using UnityEngine;
using System.Collections;

public class FloorRenderer : BaseBehaviour {

  public GameObject wallPrefab;
  public GameObject doorPrefab;
  public GameObject roomPrefab;
  public GameObject corridorPrefab;

	// Use this for initialization
	void Start () {
    for (var r = 0; r < sim.tiles.GetLength(0); r++) {
      for (var c = 0; c < sim.tiles.GetLength(1); c++) {
        var tile = sim.tiles[r, c];
        if (tile == DunGen.TileType.Perimeter 
            || tile == DunGen.TileType.Nothing
            || tile == DunGen.TileType.Blocked) {
          PlaceObj(wallPrefab, r, c);
        }

        if (tile == DunGen.TileType.Door) {
          PlaceObj(doorPrefab, r, c);
        }

//        if (tile == DunGen.TileType.Room) {
//          PlaceObj(roomPrefab, r, c);
//        }
//
//        if (tile == DunGen.TileType.Corridor) {
//          PlaceObj(corridorPrefab, r, c);
//        }

      }
    }
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
