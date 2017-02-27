using UnityEngine;
using System.Collections;

public class StartStageScript : MonoBehaviour {

    [SerializeField]
    private PostStageScript postStageScript;
    private int waveCount;
    [SerializeField]
    private MonsterSpawner[] spawners;

	// Use this for initialization
	void Start () {
        if (spawners.Length <= 0)
        {
            print("No spawners");
            return;
        }
        if(postStageScript == null)
        {
            print("No post stage script");
            return;
        }
        waveCount = postStageScript.GetComponent<PostStageScript>().waveCount;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void StartStage()
    {
        waveCount = postStageScript.GetComponent<PostStageScript>().waveCount;
        for (int i = 0; i < spawners.Length; ++i)
        {
            if (spawners[i].transform.childCount <= waveCount)
                break;
            spawners[i].StartStage(waveCount);
        }
        if(!postStageScript.gameObject.activeInHierarchy)
        {
            postStageScript.gameObject.SetActive(true);
        }
    }
}
