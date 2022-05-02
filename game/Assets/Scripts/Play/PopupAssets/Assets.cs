using UnityEngine;
using System.Collections;

namespace PopupAssets {
	public class Assets : MonoBehaviour {

		private GameManager gm;
		private float spacing = 200f;
		private bool assetsHaveLoaded = false;

		public GameObject assetContainer;
		public GameObject assetBlueprint;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {
		}

		public void show() {
			gameObject.SetActive (true);
			gm.sfxPopupOpen.Play ();

			if (!assetsHaveLoaded) {
				for (int c = 0; c < gm.assets.Count; c++) {
					GameObject p = Instantiate (assetBlueprint); 

					p.gameObject.transform.SetParent(assetContainer.gameObject.transform);
					p.gameObject.GetComponent<PopupAssets.Asset> ().id = gm.assets [c] ["id"].AsInt;
					p.gameObject.GetComponent<PopupAssets.Asset> ().points = gm.assets [c] ["build_points"].AsInt;
					p.gameObject.GetComponent<PopupAssets.Asset> ().prefab = gm.assets [c] ["prefab"];
					p.gameObject.GetComponent<PopupAssets.Asset> ().name = gm.assets [c] ["name"];
					p.gameObject.GetComponent<PopupAssets.Asset> ().image = gm.assets [c] ["image"];
					p.gameObject.GetComponent<PopupAssets.Asset> ().init ();
				}

				gm.resizeScrollableContent (assetContainer, assetBlueprint, gm.assets.Count, "landscape");

				assetsHaveLoaded = true;
			}

			assetBlueprint.gameObject.SetActive (false);
		}

		public void hide() {
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();
		}
	}
}