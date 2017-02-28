using UnityEngine;
using System.Collections;

public class AIBehaviour_Boss : MonoBehaviour {

	public enum AI_STATE_BOSS {
		FOLLOW_PATH,
		ATTACK_WALL,
		ATTACK_CRYSTAL,
		DEAD,
		NUM_AI_STATE_BOSS,
	}

	private MonoBehaviour[] states;
	private AI_STATE_BOSS currentState;

	private bool stop;

	void Awake() {
		currentState = AI_STATE_BOSS.FOLLOW_PATH;
	}

	// Use this for initialization
	void Start () {
		stop = false;
		states = new MonoBehaviour[(uint)AI_STATE_BOSS.NUM_AI_STATE_BOSS];
		states[(uint)AI_STATE_BOSS.FOLLOW_PATH] = gameObject.GetComponent<AIFollowPath_Boss>();
		states[(uint)AI_STATE_BOSS.ATTACK_WALL] = gameObject.GetComponent<AIAttackWall>();
		states[(uint)AI_STATE_BOSS.ATTACK_CRYSTAL] = gameObject.GetComponent<AIAttackCrystal>();
		states[(uint)AI_STATE_BOSS.DEAD] = gameObject.GetComponent<AIDeath>();
	}
	
	// Update is called once per frame
	void Update () {
		if (stop) {
			return;
		}

		//We only want to enable the currentState.
		if (states[(uint)currentState].enabled == false) {
			states[(uint)currentState].enabled = true;
		}

		//Disable the other states.
		for (AI_STATE_BOSS i = 0; i < AI_STATE_BOSS.NUM_AI_STATE_BOSS; ++i) {
			if (i != currentState) {
				states[(uint)i].enabled = false;
			}
		}

		switch (currentState) {
			case AI_STATE_BOSS.FOLLOW_PATH:
				{
					//If we reached the end, then attack the crystal.
					AIFollowPath_Boss moveState = (AIFollowPath_Boss)states[(uint)currentState];
					if (moveState.IsDone()) {
						currentState = AI_STATE_BOSS.ATTACK_CRYSTAL;
					} else {
						GameObject wall = moveState.GetWall();
						if (wall != null && wall.activeSelf == true) {
							currentState = AI_STATE_BOSS.ATTACK_WALL;
							AIAttackWall attackWallState = (AIAttackWall)states[(uint)AI_STATE_BOSS.ATTACK_WALL];
							attackWallState.wall = wall;
						}
					}
				}
				break;
			case AI_STATE_BOSS.ATTACK_WALL:
				{
					AIAttackWall attackWallState = (AIAttackWall)states[(uint)AI_STATE_BOSS.ATTACK_WALL];
					if (attackWallState.IsDone()) {
						currentState = AI_STATE_BOSS.FOLLOW_PATH;
					}
				}
				break;
			case AI_STATE_BOSS.ATTACK_CRYSTAL:
				AIAttackCrystal attackState = (AIAttackCrystal)states[(uint)currentState];
				if (attackState.IsDone()) {
					stop = true;
				}
				break;
			case AI_STATE_BOSS.DEAD:
				AIDeath deadState = (AIDeath)states[(uint)currentState];
				if (deadState.IsDone()) {
					stop = true;
					//gameObject.SetActive(false);
					GameObject.Destroy(gameObject);
				}
				break;
			default:
				print("Invalid AIBehaviour State!");
				break;
		}

		//No matter what, dead is dead.
		if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0) {
			currentState = AI_STATE_BOSS.DEAD;
		}
	}
}
