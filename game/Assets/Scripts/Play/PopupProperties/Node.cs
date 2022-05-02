using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PopupProperties {
	public class Node : MonoBehaviour {

		public int id;
		public string path;
		public string title;

		public Text uiTitle;
		public Text uiPath;
		public Text uiCheck;

		private GameManager gm;

		void Start () {
		}		

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (gm.selectedObject != null) {
				if (gm.selectedObject.GetComponent<MWO.Base> ().node == id) {
					uiCheck.gameObject.SetActive (true);
				} else {
					uiCheck.gameObject.SetActive (false);
				}
			}
		}

		public void init() {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			uiTitle.text = title;
			uiPath.text = path;
		}

		public void setNode() {
			gm.sfxSet.Play ();
			gm.selectedObject.GetComponent<MWO.Base> ().setNode(id);
		}
	}
}