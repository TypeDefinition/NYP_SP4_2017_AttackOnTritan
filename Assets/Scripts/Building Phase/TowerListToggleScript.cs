using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerListToggleScript : MonoBehaviour {

    public SelectedGridScript selectedGrid;
    private Toggle thisToggle;
    public Animator animator;
	// Use this for initialization
	void Start () {
	    if(selectedGrid == null)
        {
            return;
        }
        thisToggle = this.gameObject.GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    public void ChangePhaseMode()
    {
        if (selectedGrid.phaseMode == SelectedGridScript.PHASE_MODE.LIST_OPEN)
        {
            if (thisToggle.isOn)
                return;
            selectedGrid.phaseMode = SelectedGridScript.PHASE_MODE.LIST_CLOSE;
            animator.Play("CloseList");
            selectedGrid.ChangeToClosePhase();
        }
        else if(selectedGrid.phaseMode == SelectedGridScript.PHASE_MODE.LIST_CLOSE)
        {
            if (!thisToggle.isOn)
                return;
            selectedGrid.phaseMode = SelectedGridScript.PHASE_MODE.LIST_OPEN;
            animator.Play("OpenList");
            selectedGrid.ChangeToOpenPhase();
        }
    }
}
