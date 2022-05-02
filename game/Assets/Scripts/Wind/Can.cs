using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Can : MonoBehaviour {

	int counter;
	private GameManager gm;

	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
	}

	void Update () {
	}

	void OnCollisionEnter(Collision collision) {

	}

	void OnCollisionStay(Collision collision) {
		ContactPoint[] contactPoints = collision.contacts;

		for (int i = 0; i < contactPoints.Length; i++) {
			if (contactPoints [i].otherCollider.name == "Bin") {
				counter++;

				if (counter == 100) {
					gm.sfxNodeCorrect.Play ();

					// Remove
					gm.unloadWindMiniGame();

					gm.createAlert ("Congrats", "Well done for completing this module. Now find the next one to earn more points!");
					gm.addUserNode(PlayerPrefs.GetInt("node"), PlayerPrefs.GetInt("path"), PlayerPrefs.GetInt ("points"));
					gm.savePlayer ();
				}
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		counter = 0;
	}
}
