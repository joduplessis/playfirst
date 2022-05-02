using UnityEngine;
using System.Collections;
using SimpleJSON;

namespace PopupMain {
	public class Badges : MonoBehaviour {

		private GameManager gm;

		public GameObject badgeContainer;
		public GameObject badgeBlueprint;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {
		}

		public void show() {
			gameObject.SetActive (true);
			badgeBlueprint.gameObject.SetActive (false);
			populateBadgeList ();
		}

		public void populateBadgeList() {
			StartCoroutine (getBadges ());
		}

		IEnumerator getBadges() {
			for (int c = 0; c < gm.userBadges.Count; c++) {
				GameObject p = Instantiate (badgeBlueprint);

				p.gameObject.SetActive (true);
				p.gameObject.transform.SetParent(badgeContainer.gameObject.transform);
				p.gameObject.GetComponent<PopupMain.Badge> ().id = gm.userBadges [c];
				p.gameObject.GetComponent<PopupMain.Badge> ().init ();
			}

			gm.resizeScrollableContent (badgeContainer, badgeBlueprint, gm.userBadges.Count, "portrait");

			yield return null;
		}

		public void hide() {
			emptyBadgesList ();
		}

		public void emptyBadgesList() {
			StartCoroutine (emptyBadgesListDelay ());
		}

		IEnumerator emptyBadgesListDelay() {
			foreach (Transform child in badgeContainer.transform) {
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