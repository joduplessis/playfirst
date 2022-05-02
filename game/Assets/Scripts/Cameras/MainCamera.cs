using UnityEngine;
using System.Collections;
namespace Cameras {
	public class MainCamera : MonoBehaviour {

		public float perspectiveZoomSpeed = 0.5f; 
		public float orthoZoomSpeed = 0.5f;  

		private GameManager gm;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}
		

		void Update () {
			/*
			if (Input.touchCount == 2) {
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			
				if (gameObject.GetComponent<Camera>().orthographic) {
					gameObject.GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
					gameObject.GetComponent<Camera>().orthographicSize = Mathf.Max(gm.cameraMain.orthographicSize, 0.1f);
				} else {
					gameObject.GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
					gameObject.GetComponent<Camera>().fieldOfView = Mathf.Clamp(gm.cameraMain.fieldOfView, 0.1f, 179.9f);
				}
			}


			// Duplicate functionality here to the above - combine at some points
			if (gm.cameraMain.orthographicSize < 15f) { gameObject.GetComponent<Camera>().orthographicSize = 15f; }
			if (gm.cameraMain.orthographicSize > 100f) { gameObject.GetComponent<Camera>().orthographicSize = 100f; }
			if (Input.GetAxis("Mouse ScrollWheel") > 0f) { gameObject.GetComponent<Camera>().orthographicSize--; }
			if (Input.GetAxis("Mouse ScrollWheel") < 0f) { gameObject.GetComponent<Camera>().orthographicSize++; }
			*/

			// If there is a selected object
			if ((gm.selectedObject != null) || (gm.selectedOtherPlayerObject != null)) {
				followSelection ();
			} else {
				if (gm.avatarObject != null) {
					followAvatar ();
				}
			}

		}
			
		private void followAvatar() {
			float cameraX = gm.avatarObject.transform.position.x - gameObject.GetComponent<Camera>().orthographicSize;
			float cameraZ = gm.avatarObject.transform.position.z - gameObject.GetComponent<Camera>().orthographicSize;
			float cameraY = gameObject.GetComponent<Camera>().orthographicSize*1.5f;

			Vector3 cameraOrigin = transform.position;
			Vector3 cameraDestination = new Vector3 (cameraX, cameraY, cameraZ);

			transform.position = Vector3.Lerp(cameraOrigin, cameraDestination, 5.0f * Time.deltaTime);
		}

		private void followSelection() {
			if (gm.selectedObject != null) {
				float cameraX = gm.selectedObject.transform.position.x - gameObject.GetComponent<Camera> ().orthographicSize;
				float cameraZ = gm.selectedObject.transform.position.z - gameObject.GetComponent<Camera> ().orthographicSize;
				float cameraY = gameObject.GetComponent<Camera> ().orthographicSize * 1.1f; // Usually 1.5 - compensates for the UI

				Vector3 cameraOrigin = transform.position;
				Vector3 cameraDestination = new Vector3 (cameraX, cameraY, cameraZ);

				transform.position = Vector3.MoveTowards (cameraOrigin, cameraDestination, 5.0f * Time.deltaTime);
			}

			if (gm.selectedOtherPlayerObject != null) {
				float cameraX = gm.selectedOtherPlayerObject.transform.position.x - gameObject.GetComponent<Camera> ().orthographicSize;
				float cameraZ = gm.selectedOtherPlayerObject.transform.position.z - gameObject.GetComponent<Camera> ().orthographicSize;
				float cameraY = gameObject.GetComponent<Camera> ().orthographicSize * 1.1f; // Usually 1.5 - compensates for the UI

				Vector3 cameraOrigin = transform.position;
				Vector3 cameraDestination = new Vector3 (cameraX, cameraY, cameraZ);

				transform.position = Vector3.Lerp (cameraOrigin, cameraDestination, 5.0f * Time.deltaTime);
			}
		}
				
	}
}
