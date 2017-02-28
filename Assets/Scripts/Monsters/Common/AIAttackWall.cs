using UnityEngine;
using System.Collections;

public class AIAttackWall : AIAttack {

	public GameObject wall;
	private AIMovement aiMovement;

	// Use this for initialization
	void Start () {
		InitValues();
		aiMovement = gameObject.GetComponent<AIMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCountdownTimer();

		if (IsDone()) {
			return;
		}

		Vector3 moveDirection = wall.transform.position - gameObject.transform.position;
		moveDirection.y = 0.0f;
		aiMovement.Move(moveDirection);

		if (attackCountdownTimer <= 0.0f) {
			if (Attack()) {
				attacking = true;
				attackCountdownTimer = 1.0f / attackRate;
			}
		} else {
			attacking = false;
		}
	}

	void OnDisable() {
		attacking = false;
	}

	public virtual bool Attack() {
		if (wall != null && wall.activeSelf == true) {
			//If it's not facing us, also don't hurt it.
			if (Vector3.Dot(gameObject.GetComponent<Transform>().forward, wall.GetComponent<Transform>().position - gameObject.transform.position) > 0.0f) {
				DealDamage(wall.GetComponent<Health>());
				return true;
			}
		}

		return false;
	}

	public virtual bool IsDone() {
		if (wall == null || wall.activeSelf == false) {
			return true;
		}

		return false;
	}

}