using UnityEngine;
using System.Collections;

public class PostStageScript : MonoBehaviour {

    public int waveCount;
    public GameObject winCanvas;

    [SerializeField]
    private BuildingPhaseSystemScript buildingPhaseSystem;    // The building phase system
    [SerializeField]
    private GameObject buildingPhasePrefab;
    [SerializeField]
    private SelectedGridScript selectingGrid;
    [SerializeField]
    private MonsterSpawner[] spawners;
    [SerializeField]
    private GridSystem theGridSystem;

    // The give up on life way //
    [SerializeField]
    private int[] currency;
    [SerializeField]
    private int[] walls;
    private Animator anim;
    private GameObject backToLevelSelect;
    private float lifeTime;
    private bool startLifeTimeCounter;
    /////////////////////////////

	// Use this for initialization
	void Start () {

        if(buildingPhaseSystem == null)
        {
            Debug.Log("Building phase system is foiled");
            return;
        }
        anim = winCanvas.GetComponent<Animator>();
        backToLevelSelect = GameObject.Find("Main Menu Camera");
        lifeTime = 5f;
        startLifeTimeCounter = false;
        waveCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
        if(startLifeTimeCounter)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                backToLevelSelect.GetComponent<Menu>().GoToLevelSelect();
            }
        }
        else
        {
            for (int i = 0; i < spawners.Length; ++i)
            {
                if (spawners[i].HasActiveMonsters())
                {
                    return;
                } 
                else if (spawners[i].transform.childCount <= waveCount + 1)
                {
                    // Codes to End level happy since you finished all waves in the level
                    anim.SetTrigger("Win");
                    startLifeTimeCounter = true;
                    return;
                }

            }
            BackToBuildingPhase();
            waveCount++;
            this.gameObject.SetActive(false);
        }
     
	}

    public void BackToBuildingPhase()
    {
        if (!buildingPhaseSystem.gameObject.activeInHierarchy)
        {
            buildingPhaseSystem.gameObject.SetActive(true);
        }
        if (!buildingPhasePrefab.gameObject.activeInHierarchy)
        {
            buildingPhasePrefab.gameObject.SetActive(true);
        }
        if (!selectingGrid.gameObject.activeInHierarchy)
        {
            selectingGrid.gameObject.SetActive(true);
        }            

        buildingPhaseSystem.amountToBuildTowers += currency[waveCount];
        buildingPhaseSystem.numberOfBuildableWalls += walls[waveCount];

        buildingPhaseSystem.UpdateText();
        buildingPhaseSystem.selectingGrid.ChangeToOpenPhase();

        theGridSystem.EnableGridCollider(true);
        theGridSystem.RenderGrids(true);
    }
}
