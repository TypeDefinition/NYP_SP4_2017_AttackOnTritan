using UnityEngine;
using System.Collections;


public class TurretScript : MonoBehaviour
{

    public enum ATTSTYLE
    {
        NEAREST_FIRST = 0,
        FURTHEST_FIRST,
        TOTAL_STYLES
    }

    protected int towerLevel; 
    public int maxTowerLevel;

    public int minAttackDamage;          
    protected int maxAttackDamage;          
    protected float timer;                  
    protected float attackSpeed;            
    protected float proximity;              
    protected ATTSTYLE attackStyle;         
    protected Quaternion originalRotation;

    public virtual int GetLevel()
    {
        return towerLevel; 
    }
    public virtual int GetCost()
    {
        return 0;
    }
    public virtual int[] GetCostArray()
    {
        return null;
    }

    public float Proximity
    {
        get { return proximity; }
        set {  proximity = value; }
    }
    public ATTSTYLE Style
    {
        get { return attackStyle; }
        set { attackStyle = value; }
    }
    public int Level
    {
        set { towerLevel = value; }
        get { return towerLevel; }
    }
    public int MaxLevel {
        get { return maxTowerLevel; }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        towerLevel = 1;
        maxTowerLevel = 3;
        timer = 0;
        attackStyle = ATTSTYLE.NEAREST_FIRST;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (EnemiesInAttackRadius() == null)
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > attackSpeed)
            {
                Attacking(EnemiesInAttackRadius());
                timer = 0;
            }
        }
    }

    // Checks if there are any colliders within the radius/proximity of where the Tower is
    protected virtual Collider[] EnemiesInAttackRadius()
    {
        // Return an array with all colliders touching or inside the sphere
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, proximity, LayerMask.GetMask("Monster"));

        // If there are no enemy colliders
        if(enemiesColliders.Length <= 0)
            return null;

        return enemiesColliders;
    }

    // Different attacks based on the type specified
    protected virtual void Attacking(Collider[] enemies)
    {
        
    }

    protected virtual void LevelUpgrades(int min, int max, float attspd, float prox)
    {
        minAttackDamage += min;
        maxAttackDamage += max;
        if (attackSpeed - attspd > 0)
            attackSpeed -= attspd;
        proximity += prox;
    }

    public virtual void LevelUp()
    {

    }
}
