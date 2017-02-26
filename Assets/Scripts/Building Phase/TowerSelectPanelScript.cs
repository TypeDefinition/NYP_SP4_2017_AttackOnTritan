using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TowerSelectPanelScript : MonoBehaviour {
    // Our dearest building phase system
    public BuildingPhaseSystemScript buildingPhaseSystem;
    // Our dearest prefab for tower selection
    public GameObject towerSelectPrefab;
    // Content of scrollRect
    public GameObject content;
    // The group of toggles
    public ToggleGroup towerToggleGroup;
    // To store the tower prefabs from buildiPhaseSystem
    public List<Sprite> sprites;

    private GameObject[] towerPrefabs;

	// Use this for initialization
	void Start () {
        if (buildingPhaseSystem == null) 
        { 
            print("No building system"); return; 
        }
        if(!towerSelectPrefab)
        {
            print("No tower selected prefab"); return;
        }
        if (!towerToggleGroup)
        {
            print("No tower toggle group"); return;
        }
        // Store it into a gameobject array in this class
        towerPrefabs = buildingPhaseSystem.towerPrefabs;
        if(towerPrefabs.Length <= 0)
        {
            print("No towers;"); return;
        }

        for (int i = 0; i < towerPrefabs.Length; ++i)
        {
            GameObject towerGO = GameObject.Instantiate(towerSelectPrefab);
            TowerPrefabScript towerGOScript = towerGO.GetComponent<TowerPrefabScript>();
            towerGOScript.towerName = towerPrefabs[i].name;
            towerGOScript.towerPrefab = towerPrefabs[i];
            towerGOScript.buildingPhaseSystem = buildingPhaseSystem;

            if (towerPrefabs[i].CompareTag("Wall"))
            {
                towerGOScript.towerCost = 1;
                towerGOScript.costImage = sprites[0];
            }
            else
            {
                towerGOScript.towerCost = 1000 + i;
                towerGOScript.costImage = sprites[1];
            }
            Transform parentTransform = gameObject.transform.parent;

            towerGO.GetComponent<Toggle>().group = towerToggleGroup;
            towerGO.transform.SetParent(content.transform);
            towerGO.transform.localScale = content.transform.localScale;    
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
