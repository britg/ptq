using UnityEngine;
using System.Collections;

public class Tile {

  public int row;
  public int col;
  public Vector3 position;

  public string contentType;
  public string contentId;

  public bool visible = false;

  public bool occupied {
    get {
      return contentType != null;
    }
  }

  public Tile (Vector3 _position) {
    position = _position;
    row = (int)position.z;
    col = (int)position.x;
    contentType = null;
    contentId = null;
  }

  public void Occupy (string type, string id) {
    contentType = type;
    contentId = id;
  }

  public void SetVisible () {
    visible = true;
  }

}
