using UnityEngine;
using System.Collections;

public class AxisAlignedBillboard : MonoBehaviour {

	public enum BILLBOARD_AXIS {
		X,
		Y,
		Z,
	}

	public BILLBOARD_AXIS billboardAxis;
	public Camera camera;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		if (camera == null) {
			camera = Camera.main;
			if (camera == null) {
				return;
			}
		}

		Vector3 direction = camera.transform.position - gameObject.transform.position;
		Quaternion rotation = Quaternion.LookRotation(direction);
		Vector3 rotationEulerAngles = rotation.eulerAngles;

		switch (billboardAxis) {
			case BILLBOARD_AXIS.X:
				rotationEulerAngles.y = 0.0f;
				rotationEulerAngles.z = 0.0f;
				break;
			case BILLBOARD_AXIS.Y:
				rotationEulerAngles.x = 0.0f;
				rotationEulerAngles.z = 0.0f;
				break;
			case BILLBOARD_AXIS.Z:
				rotationEulerAngles.x = 0.0f;
				rotationEulerAngles.y = 0.0f;
				break;
			default:
				print(gameObject.name + " Invalid Billboard Axis!");
				break;
		}
		
		rotation.eulerAngles = rotationEulerAngles;
		gameObject.transform.rotation = rotation;
	}
}
