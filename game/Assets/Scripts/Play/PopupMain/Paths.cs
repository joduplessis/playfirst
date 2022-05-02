using UnityEngine;
using System.Collections;

namespace PopupMain {
	public class Paths : MonoBehaviour {
		private GameManager gm;
		private bool pathsHaveLoaded = false;

		public GameObject pathContainer;
		public GameObject pathBlueprint;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {
		}

		public void show() {
			gm.sfxPopupOpen.Play ();
			pathBlueprint.gameObject.SetActive (false);
			gameObject.SetActive (true);

			if (!pathsHaveLoaded) {
				for (int c = 0; c < gm.paths.Count; c++) {

					GameObject p = Instantiate (pathBlueprint);

					p.gameObject.SetActive (true);
					p.gameObject.transform.SetParent (pathContainer.gameObject.transform);
					p.gameObject.GetComponent<PopupMain.Path> ().id = gm.paths [c] ["id"].AsInt;
					p.gameObject.GetComponent<PopupMain.Path> ().title = gm.paths [c] ["title"];
					p.gameObject.GetComponent<PopupMain.Path> ().description = gm.paths [c] ["description"];
					p.gameObject.GetComponent<PopupMain.Path> ().init ();
				}

				gm.resizeScrollableContent (pathContainer, pathBlueprint, gm.paths.Count, "landscape");

				pathsHaveLoaded = true;
			}
		}

		public void hide() {
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();
		}
	}
}
