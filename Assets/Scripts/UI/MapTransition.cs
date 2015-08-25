using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapTransition : MonoBehaviour {

  public bool open = false;
  float originalUVRectHeight = 0f;
  float originalRenderHeight = 0f;
  float originalCameraHeight = 0f;
  float originalCameraSize = 0f;

  public RawImage renderImage;
  public Camera renderCamera;
  public float mapViewCameraSize = 12f;

  public ScreenManager screenManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void OpenMap () {
    if (open) {
      return;
    }

    open = true;

    var uvRect = renderImage.uvRect;
    originalUVRectHeight = uvRect.height;
    uvRect.height = (float)Screen.height / (float)Screen.width;
    renderImage.uvRect = uvRect;
    var rectTrans = renderImage.rectTransform;
    var rect = rectTrans.rect;
    originalRenderHeight = rect.height;
    renderImage.rectTransform.sizeDelta = new Vector2(0f, 568f);

    var cameraRect = renderCamera.rect;
    originalCameraHeight = cameraRect.height;
    cameraRect.height = 1f;
    renderCamera.rect = cameraRect;

    originalCameraSize = renderCamera.orthographicSize;
    renderCamera.orthographicSize = mapViewCameraSize;

    screenManager.SwitchToMapView();
  }

  public void CloseMap () {
    if (!open) {
      return;
    }
    open = false;
    var uvRect = renderImage.uvRect;
    uvRect.height = originalUVRectHeight;
    renderImage.uvRect = uvRect;
    var rect = renderImage.rectTransform.rect;
    renderImage.rectTransform.sizeDelta = new Vector2(0, originalRenderHeight);

    var cameraRect = renderCamera.rect;
    cameraRect.height = originalCameraHeight;
    renderCamera.rect = cameraRect;

    renderCamera.orthographicSize = originalCameraSize;
  }
}
