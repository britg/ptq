using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

  public GameObject feedView;
  public GameObject equipmentView;
  public GameObject inventoryView;
  public GameObject questView;
  public GameObject mapView;
  public GameObject miniMapView;

  public GameObject feedMenuView;
  public GameObject characterMenuView;

  public GameObject statusView;

  void Awake () {
    NotificationCenter.AddObserver(this, Constants.OnFirstPull);
  }

	// Use this for initialization
	void Start () {
	//    SwitchToCharacterView();
	//SwitchToFeedView();
	  OnNewGame();
	}

	// Update is called once per frame
	void Update () {

	}

  public void OnNewGame () {
    feedMenuView.SetActive(false);
    characterMenuView.SetActive(false);
    statusView.SetActive(false);
    mapView.SetActive(false);
    miniMapView.SetActive(false);
  }

  public void OnFirstPull () {
    SwitchToFeedView();
  }

  public void OnCharacterButtonTapped () {
    SwitchToCharacterView();
  }

  public void OnFeedButtonTapped () {
    SwitchToFeedView();
  }

  public void OnInventoryButtonTapped () {
    SwitchToInventoryView();
  }

  public void OnQuestButtonTapped () {
    SwitchToQuestView();
  }

  void ActivateCharacterMenu () {
    feedMenuView.SetActive(false);
    characterMenuView.SetActive(true);
  }

  void ActivateFeedMenu () {
    feedMenuView.SetActive(true);
    characterMenuView.SetActive(false);
  }

  void SwitchToCharacterView () {
    ActivateCharacterMenu();

    equipmentView.SetActive(true);

    feedView.SetActive(false);
    inventoryView.SetActive(false);
    questView.SetActive(false);
    mapView.SetActive(false);
    miniMapView.SetActive(false);
  }

  void SwitchToFeedView () {
//    ActivateCharacterMenu();
//    statusView.SetActive(true);
    feedView.SetActive(true);
    characterMenuView.SetActive(false);

    mapView.SetActive(false);
    miniMapView.SetActive(true);
    statusView.SetActive(false);
    equipmentView.SetActive(false);
    inventoryView.SetActive(false);
    questView.SetActive(false);
  }

  void SwitchToQuestView () {
    ActivateCharacterMenu();
    statusView.SetActive(true);

    questView.SetActive(true);

    feedView.SetActive(false);
    equipmentView.SetActive(false);
    inventoryView.SetActive(false);
    mapView.SetActive(false);
    miniMapView.SetActive(false);
  }

  void SwitchToInventoryView () {
    ActivateCharacterMenu();
    statusView.SetActive(true);

    inventoryView.SetActive(true);

    feedView.SetActive(false);
    equipmentView.SetActive(false);
    questView.SetActive(false);
    mapView.SetActive(false);
    miniMapView.SetActive(false);
  }

  public void SwitchToMapView () {
    mapView.SetActive(true);
    statusView.SetActive(true);
    characterMenuView.SetActive(true);

    feedView.SetActive(false);
    equipmentView.SetActive(false);
    questView.SetActive(false);
    inventoryView.SetActive(false);
    miniMapView.SetActive(false);
  }


}
