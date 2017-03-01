using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OnScreenJoystick : MonoBehaviour {

	private Image joystickForeground;
	private Image joystickBackground;
	private Vector3 inputAxis;

	// Use this for initialization
	void Start () {
		joystickBackground = GetComponent<Image> ();
		joystickForeground = transform.GetChild(0).GetComponent<Image>();
		inputAxis = new Vector3(0.0f, 0.0f, 0.0f);
	}

	// Update is called once per frame
	void Update () {
		if (!joystickForeground.GetComponent<RectTransform>().anchoredPosition.Equals(Vector3.zero)) {
			gameObject.GetComponent<RectTransform>().parent.gameObject.GetComponent<TouchScreenCameraController>().SetMovementInputAxis(inputAxis.y, inputAxis.x);
		}
	}
		
	public void Drag() {
		#if UNITY_ANDROID
			Touch touch = Input.GetTouch(0);
			Vector3 touchPosition = new Vector3 (touch.position.x, touch.position.y, 1);
		#else
			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		#endif

		Vector3 backgroundPosition = joystickBackground.GetComponent<RectTransform>().position;

		float distance = Vector2.Distance(touchPosition, joystickBackground.GetComponent<RectTransform>().position); //How far was the joystick moved?
		float backgroundRadius = joystickBackground.GetComponent<RectTransform>().rect.height * 0.5f; //Radius of the background.
		Vector3 direction = (touchPosition - gameObject.GetComponent<RectTransform>().position); //Direction of the joystick.
		direction.z = 0.0f;
		direction = direction.normalized;

		//Limit the distance.
		distance = Mathf.Min(distance, backgroundRadius);
		joystickForeground.GetComponent<RectTransform>().position = gameObject.transform.position + (direction * distance);

		inputAxis = direction * (distance / backgroundRadius);
	}

	public void PointerUp() {
		//Reset the joystick's position,
		joystickForeground.rectTransform.anchoredPosition = new Vector3 (0, 0, 0);
		inputAxis = new Vector3(0.0f, 0.0f, 0.0f);
	}

}