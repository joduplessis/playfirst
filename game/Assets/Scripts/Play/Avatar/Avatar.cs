using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace Avatar {
	public class Avatar : MonoBehaviour {

		private GameManager gm;

		public Text uiWorld;
		public Text uiPoints;
		public GameObject play;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (gm.world != null) {
				string worldTitle = gm.world ["title"];
				uiWorld.text = worldTitle.ToUpper();
			}

			if (gm.userPoints != null) {
				uiPoints.text = gm.userPoints + "";
			}

			InvokeRepeating ("checkForNewLeader", 2.0f, 60.0f);
		}

		private void checkForNewLeader() {
			if (play.gameObject.activeSelf) {
				StartCoroutine (checkForNewLeaderApi ());
			}
		}

		IEnumerator checkForNewLeaderApi() {
			WWW www = new WWW(gm.api_path+"/leaderboard/position/"+gm.userId);
			yield return www;
		}
	}
}