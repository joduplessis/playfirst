using UnityEngine;
using System.Collections;
using SimpleJSON;


namespace PopupMain {
	public class Activity : MonoBehaviour {

		private GameManager gm;

		public GameObject activeContainer;
		public GameObject activeBlueprint;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {
		}

		public void show() {
			gameObject.SetActive (true); 
			activeBlueprint.gameObject.SetActive (false);
			populateActivityList ();
			gm.sfxPopupOpen.Play ();
		}

		public void populateActivityList() {
			StartCoroutine (getActivity ());
		}

		IEnumerator getActivity() {
			WWW www = new WWW(gm.api_path+"/activity");
			yield return www;

			JSONNode feed = JSON.Parse(www.text);

			for (int c = 0; c < feed.Count; c++) {
				GameObject p = Instantiate (activeBlueprint);

				p.gameObject.SetActive (true);
				p.gameObject.transform.SetParent(activeContainer.gameObject.transform);
				p.gameObject.GetComponent<PopupMain.Active> ().status.text = feed [c] ["status"];
				p.gameObject.GetComponent<PopupMain.Active> ().user.text = feed [c] ["playname"];
				p.gameObject.GetComponent<PopupMain.Active> ().time.text = feed [c] ["ago"];
				p.gameObject.GetComponent<PopupMain.Active> ().loadImage (feed [c] ["image"]);
				p.gameObject.GetComponent<PopupMain.Active> ().init ();
			}

			gm.resizeScrollableContent (activeContainer, activeBlueprint, feed.Count, "portait");
		}

		public void hide() {
			emptyActivityList ();
		}

		public void emptyActivityList() {
			StartCoroutine (emptyActivityListDelay ());
		}

		IEnumerator emptyActivityListDelay() {
			foreach (Transform child in activeContainer.transform) {
				if (child.gameObject.activeSelf) {
					GameObject.Destroy(child.gameObject);
				}
			}	
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();
			yield return null;
		}
	}
}