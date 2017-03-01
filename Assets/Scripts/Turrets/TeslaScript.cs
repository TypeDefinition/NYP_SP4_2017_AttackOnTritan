using UnityEngine;
using System.Collections;

public class TeslaScript : TurretScript {

    private GameObject target;
    private Vector3 direction;

    private GameObject lightning;
    private GameObject lightning2;

    [SerializeField]
    private int[] towerCost;

    void Awake()
    {
        proximity = 6f;
    }
    // Use this for initialization
    protected override void Start()
    {
        //onGrid; 
        base.Start();
        minAttackDamage = 5;
        maxAttackDamage = 8;
        attackSpeed = 0.25f;
        proximity = 6f;
        direction = new Vector3(0, 0, 0);
        lightning = transform.GetChild(0).gameObject;
        lightning2 = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target)
        {
            if (!lightning.activeInHierarchy)
                lightning.SetActive(true);
            if (!lightning2.activeInHierarchy)
                lightning2.SetActive(true);

            direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction.normalized);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, direction.magnitude * 0.28f);
            Health mobHealth = target.transform.GetComponent<Health>();
            if (mobHealth.GetCurrentHealth() <= 0)
            {
                target = null;
            }
        }
        else
        {
            if (lightning.activeInHierarchy)
                lightning.SetActive(false);
            if (lightning2.activeInHierarchy)
                lightning2.SetActive(false);
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
                            if(mobHealth.GetCurrentHealth() > 0)
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
            GameObject turretbase = Resources.Load("Turrets/Tesla/Tesla 1") as GameObject;
            print(transform.parent.parent.name);
            transform.parent.parent.GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (towerLevel == 3)
        {
            GameObject turretbase = Resources.Load("Turrets/Tesla/Tesla 2") as GameObject;
            transform.parent.parent.GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
        }
        LevelUpgrades(3, 7, 0.08f, 0f);
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
    public override float GetProximity()
    {
        return proximity;
    }
}
