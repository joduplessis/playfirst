using UnityEngine;
using System.Collections;
using SimpleJSON;


namespace PopupMain {
	public class Leaderboard : MonoBehaviour {

		private GameManager gm;

		public GameObject leaderContainer;
		public GameObject leaderBlueprint;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {
		}

		public void show() {
			gm.sfxPopupOpen.Play ();
			gameObject.SetActive (true); 
			leaderBlueprint.gameObject.SetActive (false);
			populateActivityList ();
		}

		public void populateActivityList() {
			StartCoroutine (getActivity ());
		}

		IEnumerator getActivity() {
			WWW www = new WWW(gm.api_path+"/leaderboard");
			yield return www;

			JSONNode feed = JSON.Parse(www.text);

			for (int c = 0; c < feed.Count; c++) {
				GameObject p = Instantiate (leaderBlueprint);

				p.gameObject.SetActive (true);
				p.gameObject.transform.SetParent(leaderContainer.gameObject.transform);
				p.gameObject.GetComponent<PopupMain.Leader> ().position.text = (c+1)+"";
				p.gameObject.GetComponent<PopupMain.Leader> ().user.text = feed [c] ["playname"];
				p.gameObject.GetComponent<PopupMain.Leader> ().points.text = feed [c] ["points"]+" points";
				p.gameObject.GetComponent<PopupMain.Leader> ().loadImage (feed [c] ["image"]);
				p.gameObject.GetComponent<PopupMain.Leader> ().init ();

				if (c == 0) {
					p.gameObject.GetComponent<PopupMain.Leader> ().leader.gameObject.SetActive (true);
				} else {
					p.gameObject.GetComponent<PopupMain.Leader> ().leader.gameObject.SetActive (false);
				}
			}

			gm.resizeScrollableContent (leaderContainer, leaderBlueprint, feed.Count, "portait");
		}

		public void hide() {
			emptyActivityList ();
		}

		public void emptyActivityList() {
			StartCoroutine (emptyActivityListDelay ());
		}

		IEnumerator emptyActivityListDelay() {
			foreach (Transform child in leaderContainer.transform) {
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