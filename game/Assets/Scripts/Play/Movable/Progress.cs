using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Movable {
	public class Progress : MonoBehaviour {

		private GameManager gm;
		public GameObject mwo;
		public float progress;
		public GameObject foreground;
		public GameObject background;

		void Start () {
		}		

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (mwo != null) {
				gameObject.transform.localScale = new Vector3 (1, 1, 1);

				Vector3 p = gm.cameraMain.WorldToScreenPoint (mwo.transform.position);
				Vector3 origin = gameObject.transform.position;
				Vector3 destination = new Vector3 (p.x, p.y - 75, p.z);

				gameObject.transform.position = Vector3.Lerp (origin, destination, 3.0f * Time.deltaTime);

				foreground.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (progress, foreground.gameObject.GetComponent<RectTransform> ().sizeDelta.y);
			}
		}
	}
}
