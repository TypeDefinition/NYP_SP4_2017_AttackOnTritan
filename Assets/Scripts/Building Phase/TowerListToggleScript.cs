using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerListToggleScript : MonoBehaviour {

    public SelectedGridScript selectedGrid;
    private Toggle thisToggle;
    public Animator animator;
    public GameObject canvasUI;
    private Canvas cameraUI;
	// Use this for initialization
	void Start () {
	    if(selectedGrid == null)
        {
            return;
        }
        thisToggle = this.gameObject.GetComponent<Toggle>();
        cameraUI = canvasUI.GetComponent<Canvas>();
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
            #if UNITY_ANDROID == true
                cameraUI.enabled = true;
            #endif
            selectedGrid.ChangeToClosePhase();
        }
        else if(selectedGrid.phaseMode == SelectedGridScript.PHASE_MODE.LIST_CLOSE)
        {
            if (!thisToggle.isOn)
                return;
            selectedGrid.phaseMode = SelectedGridScript.PHASE_MODE.LIST_OPEN;
            animator.Play("OpenList");
            #if UNITY_ANDROID == true
                cameraUI.enabled = false;
            #endif
            selectedGrid.ChangeToOpenPhase();
        }
    }

    public void Reset()
    {
        thisToggle.isOn = true;
        selectedGrid.phaseMode = SelectedGridScript.PHASE_MODE.LIST_OPEN;
        selectedGrid.ChangeToOpenPhase();
        #if UNITY_ANDROID == true
            cameraUI.enabled = true;
        #endif
    }
}
