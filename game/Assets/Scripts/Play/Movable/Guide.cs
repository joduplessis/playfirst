using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Movable {
	public class Guide : MonoBehaviour {

		private GameManager gm;
		private bool isInView = false;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		public void show() {
			gameObject.SetActive (true);
		}

		public void hide() {
			gameObject.SetActive (false);
		}

		void Update () {
			isInView = true;

			// If there is an object to track
			if (gm.activeNodeObject!=null) {
				Vector3 objectivePosition = gm.cameraMain.WorldToScreenPoint (gm.activeNodeObject.transform.position);

				float objectivePositionX = objectivePosition.x;
				float objectivePositionY = objectivePosition.y;
				float paddingX = 40;
				float paddingY = 40;

				if (objectivePositionX > (Screen.width - paddingX)) {
					objectivePositionX = (Screen.width - paddingX);
					isInView = false;
				}

				if (objectivePositionX < paddingX) {
					objectivePositionX = paddingX;
					isInView = false;
				}

				if (objectivePositionY > (Screen.height - paddingY)) {
					objectivePositionY = (Screen.height - paddingY);
					isInView = false;
				}

				if (objectivePositionY < paddingY) {
					objectivePositionY = paddingY;
					isInView = false;
				}

				// Move the pointer around
				Vector3 origin = gameObject.transform.position;
				Vector3 destination = new Vector3 (objectivePositionX, objectivePositionY, objectivePosition.z);

				gameObject.transform.position = Vector3.Lerp (origin, destination, 5.0f * Time.deltaTime);

				// Adjust the size and pointer
				if (isInView) {
					gameObject.transform.position = new Vector3 (20000, 20000, 20000); // Off the screen
				} else {
					gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);

					// Handle the rotation
					Vector2 one = new Vector2 (objectivePosition.x, objectivePosition.y) - new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
					Vector2 two = new Vector2 (0, 1);
					float angleInRadians = Mathf.Atan2 (two.y, two.x) - Mathf.Atan2 (one.y, one.x);
				}
			} else {
				gameObject.transform.position = new Vector3 (20000, 20000, 20000); // Off the screen
			}
		}

		private float RadianToDegree(float angle){
			double d = angle * (180.0 / Mathf.PI);
			float f = (float)d;
			return f ;
		}
			
	}
}
