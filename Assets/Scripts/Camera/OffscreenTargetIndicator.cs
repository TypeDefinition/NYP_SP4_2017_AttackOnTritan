using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class OffscreenTargetIndicator : MonoBehaviour {

	[Range(0, 1000)]
	public int maxIndicators;
	//What is the tag of the targets we are looking for.
	public string targetTag;
	public Image targetIndicatorPrefab;

	private GameObject[] targetIndicators;
	private Canvas canvas;
	private Camera camera;

	// Use this for initialization
	void Start () {	
		camera = Camera.main;
		canvas = gameObject.GetComponent<Canvas>();
		if (canvas != null && targetIndicatorPrefab != null) {
			targetIndicators = new GameObject[maxIndicators];
			for (int i = 0; i < targetIndicators.Length; ++i) {
				targetIndicators[i] = GameObject.Instantiate(targetIndicatorPrefab.gameObject);
				targetIndicators[i].gameObject.transform.SetParent(gameObject.transform);
				targetIndicators[i].SetActive(false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (canvas == null) {
			print(gameObject.name + " has no Canvas.");
			return;
		}
		if (targetIndicators == null) {
			print(gameObject.name + " has no targetIndicatorPrefab.");
			return;
		}

		camera = Camera.main;
		if (camera == null) {
			return;
		}

		//Set everything to false by default.
		for (int i = 0; i < targetIndicators.Length; ++i) {
			targetIndicators[i].SetActive(false);
		}

		GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
		//i is the iteration for the targets(GameObjects). indicatorIndex is the iteration for the indicators(Arrow Image).
		for (int targetIndex = 0, indicatorIndex = 0; targetIndex < targets.Length && indicatorIndex < targetIndicators.Length; ++targetIndex) {
			if (targets[targetIndex].activeSelf == false) {
				//It's not active.
				continue;
			}

			Vector2 viewportPosition = camera.WorldToViewportPoint(targets[targetIndex].GetComponent<Transform>().position);
			if (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f) {
				//Already on our screen.
				continue;
			}

			//This is to fix a bug where sometimes things behind us have the wrong viewport position.
			if (viewportPosition.y > 1.0f) {
				if (Vector3.Dot(camera.gameObject.transform.forward, targets[targetIndex].transform.position - camera.gameObject.transform.position) < 0.0f) {					
					//print("Dot Product: " + Vector3.Dot(gameObject.transform.forward, targets[targetIndex].transform.position - gameObject.transform.position));
					viewportPosition.y = -viewportPosition.y;
				}
			}

			Vector2 offset = new Vector2(targetIndicators[indicatorIndex].GetComponent<RectTransform>().rect.width, targetIndicators[indicatorIndex].GetComponent<RectTransform>().rect.height);
			offset *= 0.5f;

			Quaternion rotation = new Quaternion();
			Vector3 rotationEuler = new Vector3(0.0f, 0.0f, 0.0f);
			//Which way should the indicator face?
			if (viewportPosition.x > 1.0f) {
				rotationEuler.z = 0.0f;
				offset.x = -offset.x;
			} else if (viewportPosition.x < 0.0f) {
				rotationEuler.z = 180.0f;
			}
			if (viewportPosition.y > 1.0f) {
				rotationEuler.z = 90.0f;
				offset.y = -offset.y;
			} else if (viewportPosition.y < 0.0f) {
				rotationEuler.z = 270.0f;
			}
			rotation.eulerAngles = rotationEuler;
			targetIndicators[indicatorIndex].transform.rotation = rotation;

			viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.0f, 1.0f);
			viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.0f, 1.0f);

			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			targetIndicators[indicatorIndex].transform.position = new Vector3((viewportPosition.x * rectTransform.rect.width * canvas.scaleFactor) + offset.x, (viewportPosition.y * rectTransform.rect.height * canvas.scaleFactor) + offset.y, 0);

			targetIndicators[indicatorIndex].SetActive(true);
			++indicatorIndex;
		}
	}

}