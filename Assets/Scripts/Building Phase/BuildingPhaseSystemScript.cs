using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingPhaseSystemScript : MonoBehaviour {

    public SelectedGridScript selectingGrid;        // The grid that does the selection for the building phase
    public GameObject[] towerPrefabs;               // The towers that players can choose to plant

    public int amountToBuildTowers { get; set; }
    public int numberOfBuildableWalls { get; set; }

    // Buttons to check
    public Button sellTurretButton;
    public Button sellWallButton;
    public Button upgradeButton;
    public Button nextWaveButton;

    public Text wall;
    public Text currency;
    public Text updateCost;

    [SerializeField]
    private int startGameWalls;
    [SerializeField]
    private int startGameCurrency;

    // Use this for initialization
    void Start()
    {
        numberOfBuildableWalls = startGameWalls;
        amountToBuildTowers = startGameCurrency;
        if (selectingGrid == null)
        {
            Debug.Log("Building phase system no selecting script");
            return;
        }
        if (towerPrefabs.Length <= 0)
        {
            return;
        }
        selectingGrid.selectedPrefab = towerPrefabs[0];
        if (selectingGrid == null)
            print("selecting grid not initialised");
        selectingGrid.ChangeSelected();
        selectingGrid.ChangeToOpenPhase();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SellTurret()
    {
        amountToBuildTowers += selectingGrid.DestroySelectedTurret();
        UpdateText();
    }

    public void SellWall()
    {
        amountToBuildTowers += selectingGrid.DestroySelectedWall();
        numberOfBuildableWalls += 1;
        UpdateText();
    }

    public void UpgradeTurret()
    {
        amountToBuildTowers -= selectingGrid.UpgradeSelectedTurret();
        selectingGrid.CheckTowerUpdate();
        UpdateText();
    }

    public void SelectedTowerButton()
    {
        sellTurretButton.gameObject.SetActive(true);
        sellWallButton.gameObject.SetActive(true);
        selectingGrid.CheckTowerUpdate();
    }

    public void SelectedWallButton()
    {
        sellTurretButton.gameObject.SetActive(false);
        sellWallButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(false);
    }

    public void SelectedNothingButton()
    {
        sellTurretButton.gameObject.SetActive(false);
        sellWallButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
    }

    public void DisableAllButtons()
    {
        sellTurretButton.gameObject.SetActive(false);
        sellWallButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        nextWaveButton.gameObject.SetActive(false);
    }

    public void UpdateText()
    {
        wall.text = numberOfBuildableWalls.ToString();
        currency.text = amountToBuildTowers.ToString();
    }
}
