using UnityEngine;
using System.Collections;

public class PlasmaScript : TurretScript {

    private GameObject target;
    private Vector3 direction;
    private float rotateSpd;

    [SerializeField]
    private int[] towerCost;

    // Use this for initialization
    protected override void Start()
    {
        //onGrid; 
        base.Start();
        minAttackDamage = 10;
        maxAttackDamage = 15;
        attackSpeed = 0.5f;
        proximity = 4f;
        direction = new Vector3(0, 0, 0);
        rotateSpd = 3f;
        attackStyle = ATTSTYLE.FURTHEST_FIRST;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target)
        {
            // Gets Vector3 direction from traget
            direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), Time.deltaTime * rotateSpd);
            Health mobHealth = target.transform.GetComponent<Health>();
            if (mobHealth.GetCurrentHealth() <= 0)
            {
                target = null;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * rotateSpd);
        }
    }

    protected override Collider[] EnemiesInAttackRadius()
    {
        if (base.EnemiesInAttackRadius() == null)
            target = null;
        return base.EnemiesInAttackRadius();
    }

    protected override void Attacking(Collider[] enemies)
    {
        switch (attackStyle)
        {
            case ATTSTYLE.NEAREST_FIRST:
                {
                    float nearestDistance = proximity;
                    foreach (Collider enemy in enemies)
                    {
                        if ((transform.position - enemy.transform.position).magnitude < nearestDistance)
                        {
                            nearestDistance = (enemy.transform.position - transform.position).magnitude;
                            Health mobHealth = enemy.transform.GetComponent<Health>();
                            if (mobHealth.GetCurrentHealth() > 0)
                            {
                                target = enemy.transform.gameObject;
                            }
                        }
                    }
                    break;
                }

            case ATTSTYLE.FURTHEST_FIRST:
                {
                    float longestDistance = 0f;
                    foreach (Collider enemy in enemies)
                    {
                        if ((transform.position - enemy.transform.position).magnitude >= longestDistance)
                        {
                            longestDistance = (enemy.transform.position - transform.position).magnitude;
                            Health mobHealth = enemy.transform.GetComponent<Health>();
                            if (mobHealth.GetCurrentHealth() > 0)
                            {
                                target = enemy.transform.gameObject;
                            }
                        }
                    }
                    break;
                }
        }

        Debug.DrawRay(transform.position, direction, new Color(1, 0, 1), 10);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, proximity))
        {
            Health mobHealth = hit.transform.GetComponent<Health>();
            if (mobHealth != null)
            {
                mobHealth.DecreaseHealth(Random.Range(minAttackDamage, maxAttackDamage));
            }
        }
    }

    public override void LevelUp()
    {
        towerLevel += 1;
        if (towerLevel == 2)
        {
            GameObject turretbase = Resources.Load("Turrets/Plasma/Base 1") as GameObject;
            transform.parent.GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretbarrel = Resources.Load("Turrets/Plasma/Turret 1") as GameObject;
            transform.GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
            GameObject barrel = Resources.Load("Turrets/Plasma/Barrel 1") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = barrel.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (towerLevel == 3)
        {
            GameObject turretbase = Resources.Load("Turrets/Plasma/Base 2") as GameObject;
            transform.parent.GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretbarrel = Resources.Load("Turrets/Plasma/Turret 2") as GameObject;
            transform.GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
            GameObject barrel = Resources.Load("Turrets/Plasma/Barrel 2") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = barrel.GetComponent<MeshFilter>().sharedMesh;
        }
        LevelUpgrades(1, 2, 0.05f, 0.5f);
    }

    public override int GetCost()
    {
        return towerCost[towerLevel];
    }
    public override int GetLevel()
    {
        return towerLevel;
    }
    public override int[] GetCostArray()
    {
        return towerCost;
    }
}
