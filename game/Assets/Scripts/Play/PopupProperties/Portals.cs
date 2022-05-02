using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

namespace PopupProperties {
	public class Portals : MonoBehaviour {

		private GameManager gm;

		public GameObject portalContainer;
		public GameObject portalBlueprint;

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
				portalBlueprint.gameObject.SetActive (false);
				populateWorldList ();
			}
		}

		public void populateWorldList() {
			StartCoroutine (getWorlds ());
		}

		IEnumerator getWorlds() {
			WWW www = new WWW(gm.api_path+"/worlds");
			yield return www;

			JSONNode portals = JSON.Parse(www.text);

			for (int c = 0; c < portals.Count; c++) {
				GameObject p = Instantiate (portalBlueprint);

				p.gameObject.SetActive (true);
				p.gameObject.transform.SetParent(portalContainer.gameObject.transform);
				p.gameObject.GetComponent<PopupProperties.Portal> ().id = portals [c] ["id"].AsInt;
				p.gameObject.GetComponent<PopupProperties.Portal> ().title = portals [c] ["title"];
				p.gameObject.GetComponent<PopupProperties.Portal> ().init ();
			}


			gm.resizeScrollableContent (portalContainer, portalBlueprint, portals.Count, "portait");
		}

		public void hide() {
			emptyWorldList ();
		}

		public void emptyWorldList() {
			StartCoroutine (emptyWorldListDelay ());
		}

		IEnumerator emptyWorldListDelay() {
			yield return new WaitForSeconds(1f);
			foreach (Transform child in portalContainer.transform) {
				if (child.gameObject.activeSelf) {
					GameObject.Destroy(child.gameObject);
				}
			}	
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();
		}

		public void removePortal() {
			gm.selectedObject.GetComponent<MWO.Base> ().setPortal (0);
			hide ();
		}
	}
}
