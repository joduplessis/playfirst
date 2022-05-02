using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Movable {
	public class Player : MonoBehaviour {

		private GameManager gm;

		public GameObject player;
		public Text playname;

		void Awake () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		// Update is called once per frame
		void Update () {
			if (player != null) {
				gameObject.transform.localScale = new Vector3 (1, 1, 1);

				Vector3 playerPosition = gm.cameraMain.WorldToScreenPoint (player.transform.position);

				// Move the pointer around
				Vector3 origin = gameObject.transform.position;
				Vector3 destination = new Vector3 (playerPosition.x, playerPosition.y + 135, playerPosition.z);

				gameObject.transform.position = Vector3.Lerp (origin, destination, 3.0f * Time.deltaTime);
			}
		}
	}
}
