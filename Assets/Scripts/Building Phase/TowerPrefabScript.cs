using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerPrefabScript : MonoBehaviour {

    public Sprite costImage  { get; set; }
    public string towerName { get; set; }
    public int towerCost    { get; set; }
    public GameObject towerPrefab; //{ get; set; }
    public BuildingPhaseSystemScript buildingPhaseSystem;

	// Use this for initialization
	void Start () {
        // For the prefab, first text is name and second text is cost
        Text[] texts = this.gameObject.GetComponentsInChildren<Text>();
        texts[0].text = towerName;
        texts[1].text = towerCost.ToString();

        Image[] images = this.gameObject.GetComponentsInChildren<Image>();
        images[1].sprite = costImage;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void IsChosen()
    {
        buildingPhaseSystem.selectingGrid.selectedPrefab = towerPrefab;
        buildingPhaseSystem.selectingGrid.ChangeSelected();
    }
}
