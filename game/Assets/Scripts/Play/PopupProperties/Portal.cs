using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PopupProperties {
	public class Portal : MonoBehaviour {

		public int id;
		public string title;

		public Text uiTitle;
		public Text uiCheck;

		private GameManager gm;

		void Start () {
		}		

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (gm.selectedObject != null) {
				if (gm.selectedObject.GetComponent<MWO.Base> ().portal == id) {
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
		}

		public void setPortal() {
			gm.sfxSet.Play ();
			gm.selectedObject.GetComponent<MWO.Base> ().setPortal(id);
		}
	}
}