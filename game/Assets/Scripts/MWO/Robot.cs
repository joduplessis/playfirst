using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SimpleJSON;

public class Robot : MonoBehaviour {

	public bool paused;
	private GameManager gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();			

		paused = true;
	}

	public void shakeObject() {
		transform.DOShakeScale(0.5f, 1f, 10, 45f, true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused) {
			RaycastHit hit;
			bool foundTile = false;
			bool playerIsInFront = false;

			Vector3 current = new Vector3 (gameObject.transform.position.x, 5, gameObject.transform.position.z);

			Vector3 aheadWorld = new Vector3 (0, -5, 15);
			Vector3 leftWorld = new Vector3 (-15, -5, 0);
			Vector3 rightWorld = new Vector3 (15, -5, 0);
			Vector3 behindWorld = new Vector3 (0, -5, -15);

			Vector3 ahead = gameObject.transform.TransformDirection (aheadWorld);
			Vector3 left = gameObject.transform.TransformDirection (leftWorld);
			Vector3 right = gameObject.transform.TransformDirection (rightWorld);
			Vector3 behind = gameObject.transform.TransformDirection (behindWorld);

			// Debug.DrawRay(current, ahead);
			// Debug.DrawRay(current, left);
			// Debug.DrawRay(current, right);
			// Debug.DrawRay(current, behind);

			// If there is road ahead
			if (Physics.Raycast (current, ahead, out hit)) {
				if (!foundTile && isTile (hit.collider.gameObject)) {
					transform.LookAt (hit.collider.gameObject.transform.position);
					foundTile = true;
				}
			}

			// If there is road to the left
			if (Physics.Raycast (current, left, out hit)) {
				if (!foundTile && isTile (hit.collider.gameObject)) {
					transform.LookAt (hit.collider.gameObject.transform.position);
					foundTile = true;
				}
			}

			// If there is road to the right
			if (Physics.Raycast (current, right, out hit)) {
				if (!foundTile && isTile (hit.collider.gameObject)) {
					transform.LookAt (hit.collider.gameObject.transform.position);
					foundTile = true;
				}
			}

			// If there is road behind
			if (Physics.Raycast (current, behind, out hit)) {
				if (!foundTile && isTile (hit.collider.gameObject)) {
					transform.LookAt (hit.collider.gameObject.transform.position);
					foundTile = true;
				}
			}

			// Look at the road
			if (foundTile) {
				gameObject.transform.position += gameObject.transform.TransformDirection (Vector3.forward) * 10.0f * Time.deltaTime;
			}
		}
	}


	private bool isTile(GameObject g) {
		if (g.tag == "Tile") {
			return true;
		} else {
			return false;
		}
	}

	void OnMouseDown() {
		StartCoroutine (unpause ());
		shakeObject ();
		paused = true;
		float max = gm.guides.Count - 1;
		int r = (int) Random.Range (0f, max) ;

		JSONNode guide = gm.guides [r];

		gm.createAlert (gm.getNodeTitle(guide ["node"].AsInt), guide["text"]);
	}

	IEnumerator unpause() {
		yield return new WaitForSeconds (3);
		paused = false;
	}
}
