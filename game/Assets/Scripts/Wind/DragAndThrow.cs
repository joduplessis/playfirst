using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SimpleJSON;
using System;
using System.IO;
using System.Net.Sockets;
using System.IO;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DragAndThrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	Vector3 pointA;
	Vector3 pointB;
	Vector3 differenceVector;
	float lineWidth = 20f;
	float angle;
	bool dragging = false;
	public Image line;
	public Camera camera;
	public GameObject can;
	public GameObject bin;
	private GameManager gm;
	private AudioSource gma;
	private float speed;
	private float points;

	void Start () {
		pointA = line.GetComponent<RectTransform> ().position;
	}

	void Awake() {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
	}

	void Update () {
		if (dragging) {
			pointB = Input.mousePosition;
			differenceVector = pointB - pointA;
			angle = Mathf.Atan2 (differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;

			line.GetComponent<RectTransform> ().pivot = new Vector2 (0, 0.5f);
			line.GetComponent<RectTransform> ().position = pointA;
			line.GetComponent<RectTransform> ().rotation = Quaternion.Euler (0, 0, angle);
			line.GetComponent<RectTransform> ().DOSizeDelta (new Vector2 (differenceVector.magnitude, lineWidth), 0.1f, true);

			// Move the can
			Vector3 p = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane + 1));
			can.transform.position = p;
		}
	}

	public void OnDrag(PointerEventData eventData) {}
	public void OnPointerEnter(PointerEventData eventData) {}
	public void OnPointerExit(PointerEventData eventData) {}
	public void OnPointerClick(PointerEventData eventData) {}
	public void OnPointerDown(PointerEventData eventData) { dragging = true; }
	public void OnPointerUp(PointerEventData eventData) { 
		dragging = false; 

		float heightHalf = Screen.height / 2;
		float widthHalf = Screen.width / 2;
		float distanceBetweenPoints = Vector2.Distance(new Vector3(Input.mousePosition.x, Input.mousePosition.y), new Vector2(widthHalf, heightHalf));

		line.GetComponent<RectTransform> ().DOSizeDelta (new Vector2 (0, lineWidth), 0.1f, true).SetEase(Ease.InBounce);
		can.GetComponent<Rigidbody>().AddForce(PlayerPrefs.GetFloat("speed")/10, 5, 2, ForceMode.Impulse);
	}

	public void cancel() {
		gm.unloadWindMiniGame ();
	}
}
