using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras {
	public class MinimapCamera : MonoBehaviour {

		private GameManager gm;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (gm.avatarObject != null) {
				Vector3 cameraMinimapOrigin = transform.position;
				Vector3 cameraMinimapDestination = new Vector3 (gm.avatarObject.transform.position.x, 100, gm.avatarObject.transform.position.z);

				transform.position = Vector3.Lerp (cameraMinimapOrigin, cameraMinimapDestination, 5.0f * Time.deltaTime);
			}
		}
	}
}