using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public enum CAMERA_MODE {
		BUILDING,
		FREE,
	}
	public CAMERA_MODE currentMode;
	public bool lockMode;

	public enum CAMERA_DIRECTION {
		FORWARD,
		RIGHT,
		BACKWARD,
		LEFT,
	}
	private CAMERA_DIRECTION currentDirection;
	private float rotationSpeed;
	private float movementSpeed;

	public Vector3 minimumBounds, maximumBounds;

	// Use this for initialization
	void Start () {
		currentMode = CAMERA_MODE.BUILDING;
		currentDirection = CAMERA_DIRECTION.FORWARD;
		rotationSpeed = 20.0f;
		movementSpeed = 50.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (lockMode == false && Input.GetButtonDown("Change Camera Mode")) {
			currentMode = (CAMERA_MODE)(((int)currentMode + 1) % System.Enum.GetValues(typeof(CAMERA_MODE)).Length);
		}

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

		if (Input.GetButtonDown("Camera Rotate Left")) {
			//Such inelegant code. Oh well.
			if (currentDirection == CAMERA_DIRECTION.FORWARD) {
				currentDirection = CAMERA_DIRECTION.LEFT;
			} else {
				currentDirection = (CAMERA_DIRECTION)((int)currentDirection - 1);
			}
		} else if (Input.GetButtonDown("Camera Rotate Right")) {
			//I might say this is slightly better, but I would probably be lying to myself.
			currentDirection = (CAMERA_DIRECTION)(((int)currentDirection + 1) % System.Enum.GetValues(typeof(CAMERA_DIRECTION)).Length);
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

		Quaternion rotation = new Quaternion();
		Vector3 rotationEuler = new Vector3(xRotation, yRotation, zRotation);
		rotation.eulerAngles = rotationEuler;
		gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Mathf.Clamp(rotationSpeed * Time.deltaTime, 0.0f, 1.0f));

		gameObject.transform.position += forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
		gameObject.transform.position += right * movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
		gameObject.transform.position += up * movementSpeed * Time.deltaTime * Input.GetAxis("Camera Height");

		Vector3 position = new Vector3();
		position.x = Mathf.Clamp(gameObject.transform.position.x, minimumBounds.x, maximumBounds.x);
		position.y = Mathf.Clamp(gameObject.transform.position.y, minimumBounds.y, maximumBounds.y);
		position.z = Mathf.Clamp(gameObject.transform.position.z, minimumBounds.z, maximumBounds.z);
		gameObject.transform.position = position;
	}

}