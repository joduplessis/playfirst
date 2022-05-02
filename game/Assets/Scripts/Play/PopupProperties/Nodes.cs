using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

namespace PopupProperties {
	public class Nodes : MonoBehaviour {

		private GameManager gm;
		private float spacing = 50f;
		private bool nodesHaveLoaded = false;

		public GameObject nodeContainer;
		public GameObject nodeBlueprint;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {
		}

		public void show() {
			if (gm.selectedObject == null) {
				gm.createAlert ("Whoops", "Please select an object first");
			} else {
				gm.sfxPopupOpen.Play ();
				gameObject.SetActive (true);
				nodeBlueprint.gameObject.SetActive (false);

				if (!nodesHaveLoaded) {
					for (int c = 0; c < gm.nodes.Count; c++) {
						GameObject p = Instantiate (nodeBlueprint);

						p.gameObject.SetActive (true);
						p.gameObject.transform.SetParent (nodeContainer.gameObject.transform);
						p.gameObject.GetComponent<PopupProperties.Node> ().id = gm.nodes [c] ["id"].AsInt;
						p.gameObject.GetComponent<PopupProperties.Node> ().title = gm.nodes [c] ["title"];

						for (int d = 0; d < gm.paths.Count; d++) {
							if (gm.nodes [c] ["path"].AsInt == gm.paths [d] ["id"].AsInt) {
								p.gameObject.GetComponent<PopupProperties.Node> ().path = gm.paths [d] ["title"];
							}
						}

						p.gameObject.GetComponent<PopupProperties.Node> ().init ();
					}


					gm.resizeScrollableContent (nodeContainer, nodeBlueprint, gm.nodes.Count, "portait");

					nodesHaveLoaded = true;
				}
			}
		}

		public void hide() {
			gameObject.SetActive (false); 
			gm.sfxPopupClose.Play ();
		}

		public void removeNode() {
			gm.selectedObject.GetComponent<MWO.Base> ().setNode (0);
			hide ();
		}
	}
}
