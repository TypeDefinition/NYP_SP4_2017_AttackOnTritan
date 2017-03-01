using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PostStageScript : MonoBehaviour {

    public int waveCount;
    public GameObject winCanvas;
    public GameObject bossCanvas;
    public Button continueButton;

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
    private Animator anim2;
    private GameObject backToLevelSelect;
    private bool startLifeTimeCounter;
    private int maxStages;
    private bool checkBoss;
    /////////////////////////////

	// Use this for initialization
	void Start () {

        if(buildingPhaseSystem == null)
        {
            Debug.Log("Building phase system is foiled");
            return;
        }
        anim = winCanvas.GetComponent<Animator>();
        anim2 = bossCanvas.GetComponent<Animator>();
        backToLevelSelect = GameObject.Find("Main Menu Camera");
        startLifeTimeCounter = false;
        waveCount = 0;
        maxStages = 0;
        checkBoss = false;

        for (int i = 0; i < spawners.Length; ++i)
        {
            maxStages = Mathf.Max(maxStages, spawners[i].transform.childCount);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        if(!startLifeTimeCounter)
        {
            for (int i = 0; i < spawners.Length; ++i)
            {
                if (spawners[i].HasActiveMonsters())
                {
                    return;
                } 
            }
            if (maxStages <= waveCount + 1)
            {
                // Codes to End level happy since you finished all waves in the level
                anim.SetTrigger("Win");
                startLifeTimeCounter = true;
                return;
            }

            for (int i = 0; i < spawners.Length; ++i)
            {
                if (!checkBoss && spawners[i].HasBoss(waveCount + 1))
                {
                    checkBoss = true;
                    anim2.SetTrigger("BossHere");
                }
            }
            if (checkBoss && anim2.GetCurrentAnimatorStateInfo(0).IsName("KillMessageState"))
            {
                print("HI");
                anim2.enabled = false;
                BackToBuildingPhase();
                waveCount++;
                checkBoss = false;
                this.gameObject.SetActive(false);
            }
            else if(!checkBoss)
            {
                BackToBuildingPhase();
                waveCount++;
                checkBoss = false;
                this.gameObject.SetActive(false);
            }
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

    public void VictoryScreen()
    {
        if(startLifeTimeCounter)
            backToLevelSelect.GetComponent<Menu>().GoToLevelSelect();
    }
}
