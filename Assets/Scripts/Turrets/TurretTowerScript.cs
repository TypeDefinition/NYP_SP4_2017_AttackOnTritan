using UnityEngine;
using System.Collections;

public class TurretTowerScript : MonoBehaviour {

    public int tileID;
    public GridSystem gridSystem;
    public GameObject explosion;
    private GameObject explosionEffect;

	// Use this for initialization
	void Start () {
        GameObject grid = gridSystem.GetGrid(tileID);
        if (grid != null)
        {
            transform.position = grid.GetComponent<Transform>().position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            explosionEffect = GameObject.Instantiate(explosion);
            explosionEffect.transform.position = transform.position;
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
	}
}
