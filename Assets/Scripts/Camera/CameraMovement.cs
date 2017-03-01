using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public enum CAMERA_MODE {
		BUILDING,
		FREE,
	}
	public CAMERA_MODE currentMode;
	public bool lockCameraMode;

	public enum CAMERA_DIRECTION {
		FORWARD,
		RIGHT,
		BACKWARD,
		LEFT,
	}
	private CAMERA_DIRECTION currentDirection;
	private float rotationSpeed;
	private Vector3 moveVector;

	public Vector3 minimumBounds, maximumBounds;

	// Use this for initialization
	void Start () {
		currentMode = CAMERA_MODE.BUILDING;
		currentDirection = CAMERA_DIRECTION.FORWARD;
		rotationSpeed = 20.0f;
		moveVector = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 forward = Vector3.forward;
		Vector3 right = Vector3.right;
		Vector3 up = Vector3.up;

		float yRotation = 0.0f;
		float xRotation = 0.0f;
		float zRotation = 0.0f;

		if (currentMode == CAMERA_MODE.FREE) {
			xRotation = 30.0f;
		} else {
			xRotation = 90.0f;
		}

		switch (currentDirection) {
			case CAMERA_DIRECTION.FORWARD:
				yRotation = 0.0f;
				forward = Vector3.forward;
				right = Vector3.right;
				break;
			case CAMERA_DIRECTION.RIGHT:
				yRotation = 90.0f;
				forward = Vector3.right;
				right = Vector3.back;
				break;
			case CAMERA_DIRECTION.BACKWARD:
				yRotation = 180.0f;
				forward = Vector3.back;
				right = Vector3.left;
				break;
			case CAMERA_DIRECTION.LEFT:
				yRotation = 270.0f;
				forward = Vector3.left;
				right = Vector3.forward;
				break;			
			default:
				print("Invalid Camera Direction.");
				break;
		}

		//Rotate
		Quaternion rotation = new Quaternion();
		Vector3 rotationEuler = new Vector3(xRotation, yRotation, zRotation);
		rotation.eulerAngles = rotationEuler;
		gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Mathf.Clamp(rotationSpeed * Time.deltaTime, 0.0f, 1.0f));

		//Move
		gameObject.transform.position += forward * moveVector.z;
		gameObject.transform.position += right * moveVector.x;
		gameObject.transform.position += up * moveVector.y;
		moveVector = new Vector3(0.0f, 0.0f, 0.0f); //Reset it.

		LimitPositionToBoundary();
	}

	private void LimitPositionToBoundary() {
		Vector3 position = new Vector3();
		position.x = Mathf.Clamp(gameObject.transform.position.x, minimumBounds.x, maximumBounds.x);
		position.y = Mathf.Clamp(gameObject.transform.position.y, minimumBounds.y, maximumBounds.y);
		position.z = Mathf.Clamp(gameObject.transform.position.z, minimumBounds.z, maximumBounds.z);
		gameObject.transform.position = position;
	}

	public void RotateLeft() {
		//Such inelegant code. Oh well.
		if (currentDirection == CAMERA_DIRECTION.FORWARD) {
			currentDirection = CAMERA_DIRECTION.LEFT;
		} else {
			currentDirection = (CAMERA_DIRECTION)((int)currentDirection - 1);
		}
	}

	public void RotateRight() {
		//I might say this is slightly better, but I would probably be lying to myself.
		currentDirection = (CAMERA_DIRECTION)(((int)currentDirection + 1) % System.Enum.GetValues(typeof(CAMERA_DIRECTION)).Length);
	}

	public void CycleMode() {
		if (lockCameraMode == true) {
			return;
		}
		currentMode = (CAMERA_MODE)(((int)currentMode + 1) % System.Enum.GetValues(typeof(CAMERA_MODE)).Length);
	}

	public void Move(float _forward, float _right, float _up) {
		moveVector.x += _right;
		moveVector.y += _up;
		moveVector.z += _forward;
	}

}