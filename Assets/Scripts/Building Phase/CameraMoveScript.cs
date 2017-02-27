using UnityEngine;
using System.Collections;

public class CameraMoveScript : MonoBehaviour {

    [SerializeField]
    private float speed;
    private Animator animator;

    [SerializeField]
    private SelectedGridScript selectedGrid;
    [SerializeField]
    private GameObject buildingPhasePrefab;
    [SerializeField]
    private BuildingPhaseSystemScript buildingPhaseSystem;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
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
            }
        }
	}
}
