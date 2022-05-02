using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using EpPathFinding.cs;
using DG.Tweening;

namespace MWO {
	public class Car : MonoBehaviour {

		private GameManager gm;
		private int currentJourneyGridNode;
		private List<GridPos> route;
		private float dist;
		private Vector3 current; 
		private Vector3 next;
		private float grounded = 1.0f;

		public GridPos travellingTo;
		public GridPos travellingFrom;

		public GameObject[] cars = new GameObject[7];
		
		void Start () {}

		private void OnCollisionEnter(Collision collision) {
			if (collision.gameObject.name == "Car(Clone)") {
				gm.createRandomCar ();
				gm.deleteObjectSilently (gameObject);
			}
		}

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			// Select a random car mesh
			if (cars [0] != null) {
				cars [UnityEngine.Random.Range(0, cars.Length)].SetActive (true);
			}

			// Decide which route to base it on
			switch (UnityEngine.Random.Range (1, 4)) {
			case 1: 
				route = gm.journeyGoingLeftGrid;
				break;
			case 2:
				route = gm.journeyGoingRightGrid;
				break;
			case 3:
				route = gm.journeyGoingUpGrid;
				break;
			case 4:
				route = gm.journeyGoingDownGrid;
				break;
			}

			// So that's not null
			travellingTo = new GridPos (0, 0);
			travellingFrom = new GridPos (0, 0);

			// If there are too many cars - failsafe for the Enum running on repeat in GM
			if (GameObject.FindGameObjectsWithTag("Car").Length > route.Count) {
				gm.deleteObjectSilently (gameObject);
			} else {
				// Find a block tile to place the car on initially
				bool foundAnOpenRoadTile = false;
				int failsafe_counter = 0;
				while (!foundAnOpenRoadTile) {
					failsafe_counter++;
					currentJourneyGridNode = UnityEngine.Random.Range (0, route.Count-1);

					int desiredX = route [currentJourneyGridNode].x;
					int desiredY = route [currentJourneyGridNode].y;

					GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

					if (cars.Length == 0) {
						foundAnOpenRoadTile = true;
					} else if (failsafe_counter == 10) {
						foundAnOpenRoadTile = true;
					} else {
						foreach (GameObject car in cars) {
							if (car.GetComponent<MWO.Car> ().travellingTo.x != desiredX && 
								car.GetComponent<MWO.Car> ().travellingTo.y != desiredY &&
								car.GetComponent<MWO.Car> ().travellingFrom.x != desiredX && 
								car.GetComponent<MWO.Car> ().travellingFrom.y != desiredY ) {
								foundAnOpenRoadTile = true;

								break;
							}
						}
					}
				}
			}
		}

		public void createJourney() {
			StartCoroutine (createJourneySync ());
		} 

		IEnumerator createJourneySync() {
			for (int r = currentJourneyGridNode; r < route.Count-1; r++) {
				// Start point
				float sx = gm.convertTileToCoordinate (route [r].x);
				float sz = gm.convertTileToCoordinate (route [r].y);
				Vector3 start = new Vector3 (sx, grounded, sz);

				// End point
				int er = r+1;
				float ex = gm.convertTileToCoordinate (route [er].x);
				float ez = gm.convertTileToCoordinate (route [er].y);
				Vector3 end = new Vector3 (ex, grounded, ez);

				// Assign this to the public variable so other cars can check
				travellingFrom = new GridPos(route[r].x, route[r].y);
				travellingTo = new GridPos(route[er].x, route[er].y);

				// Calculate the distance & time 
				float speed = 30.0f;
				float dis = Vector3.Distance (start, end);
				float time = dis / speed;

				// Move the car
				gameObject.transform.position = start;
				transform.DOLookAt (end, 0.25f);//.SetDelay(time-0.25f);
				gameObject.transform.DOMove (end, time).SetEase (Ease.Linear);
				yield return new WaitForSeconds(time);
			}

			currentJourneyGridNode = 0;
			createJourney ();
		}
	}
}