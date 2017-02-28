using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAttackCrystal : AIAttack {

	public GameObject tritanCrystal;
	private int tritanCrystalLayerMask;
	private bool touchingCrystal;
	private AIMovement aiMovement;

	[SerializeField]
	private List<Vector3> waypoints; //our waypoints.
	[SerializeField]
	private int targetWaypointIndex; //Where we wanna end up.
	[SerializeField]
	private int currentWaypointIndex; //Where we are.	
	[SerializeField]
	private bool reachedEndWaypoint; //Have we reached the end?

	[SerializeField]
	private float waypointsRadius;
	private float stuckTime; //If we can't mvoe for too long, we're stuck.
	private float stuckCountdownTimer;

	bool isBoss;

	// Use this for initialization
	void Start () {
		InitValues();
		tritanCrystalLayerMask  = LayerMask.GetMask("Tritan Crystal");
		touchingCrystal = false;
		aiMovement = gameObject.GetComponent<AIMovement>();

		GenerateWaypoints();
		stuckTime = 1.0f;
		stuckCountdownTimer  = stuckTime;
	}
	
	// Update is called once per frame
	void Update () {		
		UpdateCountdownTimer();

		//Don't bother attacking if there's nothing to attack.
		if (tritanCrystal == null || tritanCrystal.activeSelf == false) {
			attacking = false;
			return;
		}

		if (reachedEndWaypoint && touchingCrystal) {
			Vector3 directionToCrystal = tritanCrystal.gameObject.transform.position - gameObject.transform.position;
			directionToCrystal.y = 0.0f;
			aiMovement.RotateTowards(directionToCrystal); //Face the crystal.
			if (attackCountdownTimer <= 0.0f) {
				if (Attack()) {
					attacking = true;
					attackCountdownTimer = 1.0f / attackRate;
				}
			} else {
				attacking = false;
			}
		} else {
			MoveTowardsCrystal();			
		}	
	}

	void OnDisable() {
		attacking = false;
	}

	void OnCollisionEnter(Collision _collisionInfo) {
		if (_collisionInfo.gameObject == tritanCrystal) {
			touchingCrystal = true; //We're already touching the crystal. No need to move towards to anymore.
		}
	}

	void OnCollisionExit(Collision _collisionInfo) {
		if (_collisionInfo.gameObject == tritanCrystal) {
			touchingCrystal = false; //We're not touching the crystal anymore. Move towards it.
		}
	}

	private void GenerateWaypoints() {		
		if (waypoints == null) {
			waypoints = new List<Vector3>();
		}
		waypoints.Clear();


		int numWaypoints = Random.Range(12, 18); //Random it to have even lesser crowding.
		float angle = 360.0f / (float)numWaypoints;
		if (Mathf.Abs(waypointsRadius) < 2.0f) {
			waypointsRadius = 2.0f;
		}

		for (int i = 0; i < numWaypoints; ++i) {
			float currentAngle = angle * (float)i * Mathf.Deg2Rad;
			Vector3 offset = new Vector3(Mathf.Rad2Deg * Mathf.Cos(currentAngle), 0, Mathf.Rad2Deg * Mathf.Sin(currentAngle));
			Vector3 waypoint = tritanCrystal.gameObject.transform.position + (offset.normalized * waypointsRadius);
			waypoints.Add(waypoint);
		}

		reachedEndWaypoint = false;
		currentWaypointIndex = -1;
		targetWaypointIndex = Random.Range(0, waypoints.Count);
	}

	private void MoveTowardsCrystal() {
		//Which waypoint are we closest to?
		if (currentWaypointIndex < 0) {
			Vector3 closestWaypoint = waypoints[0];
			float closestDistanceSquared = (closestWaypoint - gameObject.transform.position).sqrMagnitude;
			currentWaypointIndex = 0;

			for (int i = 1; i < waypoints.Count; ++i) {
				float distanceSquared = (waypoints[i] - gameObject.transform.position).sqrMagnitude;
				if (distanceSquared < closestDistanceSquared) {
					closestDistanceSquared = distanceSquared;
					closestWaypoint = waypoints[i];
					currentWaypointIndex = i;
				}
			}
		}
		
		//Look at the crystal.
		Vector3 destination;
		if (reachedEndWaypoint) {
			destination = tritanCrystal.transform.position;
		} else {
			destination = waypoints[currentWaypointIndex];
		}
		Vector3 directionToCrystal = destination - gameObject.transform.position;
		directionToCrystal.y = 0.0f; //Disregard the height difference.

		aiMovement.RotateTowards(directionToCrystal); //Face the crystal.
		aiMovement.Move(directionToCrystal);

		//Once we reach a waypoint, move on to the next one.
		if (directionToCrystal.sqrMagnitude < 0.3f * 0.3f) {
			//We've already reached the last waypoint. Walk directly to the crystal.
			if (currentWaypointIndex == targetWaypointIndex) {
				reachedEndWaypoint = true;
			} else {
				currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
			}
		}

		if (stuckCountdownTimer <= 0.0f) {
			print("Stuck");
			reachedEndWaypoint = true;
		} else {
			if (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 0.005) {
				stuckCountdownTimer -= Time.deltaTime;
			} else {
				stuckCountdownTimer = stuckTime;
			}
		}
	}

	//Returns if we've manage to attack anything.
	public override bool Attack() {
		bool result = false;
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, tritanCrystalLayerMask);
		for (int i = 0; i < hitColliders.Length; ++i) {
			//If it's not a Tritan Crystal, don't hurt it.
			/*if (hitColliders[i].gameObject.CompareTag("Tritan Crystal") == false) {
				continue;
			}*/
			//If it's not facing us, also don't hurt it.
			if (Vector3.Dot(gameObject.GetComponent<Transform>().forward, hitColliders[i].gameObject.GetComponent<Transform>().position - gameObject.transform.position) < 0.0f) {
				continue;
			}
			DealDamage(hitColliders[i].gameObject.GetComponent<Health>());
			result = true;
		}		
		return result;
	}

	public bool IsDone() {
		if (tritanCrystal == null || tritanCrystal.GetComponent<Health>() == null || tritanCrystal.GetComponent<Health>().IsDead()) {
			return true;
		}
		return false;
	}

	public void Reset() {
		GenerateWaypoints();
		stuckCountdownTimer = stuckTime;
	}

}