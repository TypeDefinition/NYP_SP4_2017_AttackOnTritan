using UnityEngine;
using System.Collections;

public class MortarProjectileScript : MonoBehaviour {

    public GameObject radius;
    private float Explosion_radius;
    private int minDamage;
    private int maxDamage;

    MortarProjectileScript(float explosion, int minDamage, int maxDamage)
    {
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.name == "Terrain")
        {
            Collider[] inrange = Physics.OverlapSphere(transform.position, Explosion_radius, LayerMask.GetMask("Monster"));
            foreach (Collider enemy in inrange)
            {
                Health mobHealth = enemy.transform.GetComponent<Health>();
                if (mobHealth != null)
                {
                    mobHealth.DecreaseHealth(Random.Range(minDamage, maxDamage));
                }
            }
            Destroy(this.gameObject);
            GameObject temp = Instantiate(radius);
            temp.transform.position = transform.position;
            temp.transform.localScale = new Vector3(Explosion_radius * 2, 1.0f, Explosion_radius * 2);
            Debug.Log(temp.transform.localScale);
            Destroy(temp, 0.1f);
        }
    }

    public void setRadius(float radius)
    {
        this.Explosion_radius = radius;
    }
    public void setDamage(int minD, int maxD)
    {
        this.minDamage = minD;
        this.maxDamage = maxD;
    }
}
