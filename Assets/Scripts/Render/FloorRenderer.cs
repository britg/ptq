using UnityEngine;
using System.Collections;

public class FloorRenderer : BaseBehaviour {

  public GameObject wallPrefab;

	// Use this for initialization
	void Start () {
    for (var r = 0; r < sim.tiles.GetLength(0); r++) {
      for (var c = 0; c < sim.tiles.GetLength(1); c++) {
        var tile = sim.tiles[r, c];
        if (tile == DunGen.TileType.Perimeter 
            || tile == DunGen.TileType.Nothing
            || tile == DunGen.TileType.Blocked) {
          var wallObj = Instantiate(wallPrefab);
          wallObj.transform.parent = transform;
          wallObj.transform.localPosition = new Vector3(c, 0, r);
        }
      }
    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
