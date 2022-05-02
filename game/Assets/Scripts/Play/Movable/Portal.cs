using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections;
using SimpleJSON;

namespace Movable {
	public class Portal : MonoBehaviour {

		public GameObject mwo;
		public int id;
		public Text uiTitle;

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
				Vector3 destination = new Vector3 (p.x, p.y + 100, p.z);

				gameObject.transform.position = Vector3.Lerp (origin, destination, 3.0f * Time.deltaTime);
			}
		}

		public void init() {
			StartCoroutine (getWorlds ());
		}

		IEnumerator getWorlds() {
			WWW www = new WWW(gm.api_path+"/worlds");
			yield return www;

			JSONNode worlds = JSON.Parse(www.text);

			for (int c = 0; c < worlds.Count; c++) {
				if (worlds [c] ["id"].AsInt == id) {
					uiTitle.text = worlds [c] ["title"];
				}
			}
		}

		public void launchPortal() {
			gm.setWorld (id);
		}
	}
}