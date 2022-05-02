using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Movable {
	public class Node : MonoBehaviour {

		public int id;
		public GameObject mwo;
		public GameObject complete;
		public GameObject incomplete;
		public Text path;
		public Text node;

		private GameManager gm;

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
				Vector3 destination = new Vector3 (p.x, p.y + 175, p.z);

				gameObject.transform.position = Vector3.Lerp (origin, destination, 3.0f * Time.deltaTime);
			}
		}
			
		public void init(GameObject nmwo, int nid) {
			mwo = nmwo;
			id = nid;

			// Find the node & path
			for (int n = 0; n < gm.nodes.Count; n++) {
				if (gm.nodes [n] ["id"].AsInt == id) {
					string title = gm.nodes [n] ["title"];

					if (title.Length > 30) {
						node.text = title.Substring(0, 30)+" ...";
					} else {
						node.text = title;
					}

					for (int p = 0; p < gm.paths.Count; p++) {
						if (gm.paths [p] ["id"].AsInt == gm.nodes [n] ["path"].AsInt) {
							path.text = gm.paths [p] ["title"];
						}
					}
				}
			}

			markAsCompleted ();
		}

		public void launchNode() {
			for (int c = 0; c < gm.nodes.Count; c++) {
				if (gm.nodes [c] ["id"].AsInt == mwo.GetComponent<MWO.Base> ().node) {
					if (gm.userNodes.IndexOf (mwo.GetComponent<MWO.Base> ().node) == -1) {
						gm.hudNodeContainer.GetComponent<NodeDetail> ().init (id);
					} else {
						gm.createBigNotice ("Well done", "You've already completed this module!");
					}
				}
			}
				
			gm.currentNodeObject = mwo;
		}

		public void markAsCompleted() {
			if (gm.userNodes.IndexOf (mwo.GetComponent<MWO.Base> ().node) == -1) {
				complete.gameObject.SetActive (false);
				incomplete.gameObject.SetActive (true);
			} else {
				complete.gameObject.SetActive (true);
				incomplete.gameObject.SetActive (false);
			}
		}

	}
}