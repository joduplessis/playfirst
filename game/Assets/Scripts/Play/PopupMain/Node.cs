using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PopupMain {
	public class Node : MonoBehaviour {

		public int id;
		public Text title;
		public GameObject complete;
		public GameObject incomplete;
		public GameObject current;

		private GameManager gm;

		void Start () {
		}		

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			current.gameObject.SetActive (false);
		}

		void Update () {
			if (gm != null) {
				// If the user has completed this node
				if (gm.userNodes.IndexOf (id) == -1) {
					incomplete.gameObject.SetActive (true);
					complete.gameObject.SetActive (false);
				} else {
					incomplete.gameObject.SetActive (false);
					complete.gameObject.SetActive (true);
				}

				// If this is the current node
				if (gm.activeNode == id) {
					current.gameObject.SetActive (true);
				} else {
					current.gameObject.SetActive (false);
				}
			}
		}

		public void init() {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}
	}
}