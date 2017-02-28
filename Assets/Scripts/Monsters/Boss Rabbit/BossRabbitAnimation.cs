using UnityEngine;
using System.Collections;

public class BossRabbitAnimation : MonoBehaviour {

	private Animator animator;
	int velocityRatioHash;
	int attackHash;
	int attack2Hash;
	int healthHash;
	bool isAlive;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
		velocityRatioHash = Animator.StringToHash("Velocity Ratio");
		attackHash = Animator.StringToHash("Attack");
		attack2Hash = Animator.StringToHash("Attack 2");
		healthHash = Animator.StringToHash("Health");
		isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
		int currentHealth = gameObject.GetComponent<Health>().GetCurrentHealth();
		animator.SetInteger(healthHash, currentHealth);
		if (!gameObject.GetComponent<Health>().IsAlive() && isAlive) {
			animator.SetTrigger("Die");
		}
		isAlive = gameObject.GetComponent<Health>().IsAlive();
		if (!isAlive) {
			return;
		}

		//Velocity Ratio
		Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
		velocity.y = 0.0f;
		float velocityRatio = velocity.magnitude / gameObject.GetComponent<AIMovement>().GetMaxMovementSpeed();
		animator.SetFloat(velocityRatioHash, velocityRatio);

		//Attack
		if (gameObject.GetComponent<AIAttackCrystal>().IsAttacking()) {
			animator.SetTrigger(attackHash);
		} else if (gameObject.GetComponent<AIAttackWall>().IsAttacking()) {
			animator.SetTrigger(attack2Hash);
		}
	}

}