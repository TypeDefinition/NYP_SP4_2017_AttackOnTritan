using UnityEngine;
using System.Collections;

public class CameraTriggerBuildPhase : MonoBehaviour {

    private Animator animator;

    [SerializeField]
    private SelectedGridScript selectedGrid;
    [SerializeField]
    private GameObject buildingPhasePrefab;
    [SerializeField]
    private BuildingPhaseSystemScript buildingPhaseSystem;
    [SerializeField]
    private CameraController cameraController;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!selectedGrid.gameObject.activeInHierarchy && 
            !buildingPhasePrefab.gameObject.activeInHierarchy &&
            !buildingPhaseSystem.gameObject.activeInHierarchy)
        {
            if (animator.enabled == true && animator.GetCurrentAnimatorStateInfo(0).IsName("Dead Camera"))
            {
                animator.enabled = false;
                selectedGrid.gameObject.SetActive(true);
                buildingPhasePrefab.gameObject.SetActive(true);
                buildingPhaseSystem.gameObject.SetActive(true);
                GetComponent<CameraMovement>().enabled = true;
                cameraController.enabled = true;
            }
        }
	}
}
