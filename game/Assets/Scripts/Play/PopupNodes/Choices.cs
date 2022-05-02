using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SimpleJSON;
using System;

namespace PopupNodes {
	public class Choices : MonoBehaviour {

		private GameManager gm;

		public int points;
		public int id;
		public string title;
		public int path;
		public string type;
		public string content ;

		[Header("UI")]
		public Text uiPoints;
		public Text uiPath;
		public Text uiTitle;
		public GameObject uiContent;
		public GameObject choiceBlueprint;

		[Header("Webview")]
		WebViewObject webViewObject;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Update () {}

		public void init() {
			gm.sfxPopupOpen.Play ();
			gameObject.SetActive (true);

			uiPoints.text = points.ToString ();
			uiTitle.text = title;

			// Get the path for the node
			for (int c = 0; c < gm.paths.Count; c++) {
				if (gm.paths [c] ["id"].AsInt == path) {
					uiPath.text = gm.paths [c] ["title"];
				}
			}

			// Log
			gm.log("node_open", id);

			StartCoroutine(loadNodeChoicesContent());
		}

		IEnumerator loadNodeChoicesContent () {
			WWW www = new WWW(gm.api_path+"/nodes/content_choices/" + id);
			yield return www;

			choiceBlueprint.gameObject.SetActive (false);
			float spacing = choiceBlueprint.gameObject.GetComponent<RectTransform> ().sizeDelta.y + 10;

			JSONNode choicesJSON = JSON.Parse(www.text);
			string cj = choicesJSON ["content"];
			string[] choices = cj.Split ('_');


			for (int c=0; c < choices.Length; c++) {

				string[] choiceParts = choices[c].Split ('|');
				GameObject w = Instantiate (choiceBlueprint);

				w.gameObject.SetActive (true);
				w.gameObject.transform.SetParent(uiContent.gameObject.transform);
				w.gameObject.GetComponent<PopupNodes.Choice> ().title.text = choiceParts[0];
				w.gameObject.GetComponent<PopupNodes.Choice> ().init ();

				if (choiceParts[1]=="true") {
					w.gameObject.GetComponent<PopupNodes.Choice> ().correctAnswer = true;
				} else {
					w.gameObject.GetComponent<PopupNodes.Choice> ().correctAnswer = false;
				}

			}  

			gm.resizeScrollableContent (uiContent, choiceBlueprint, choices.Length, "portait");

		}

		public void correctAnswer() {
			gm.sfxNodeCorrect.Play ();
			gm.createAlert ("Congrats", "Well done for completing this module. Now find the next one to earn more points!");
			gm.addUserNode(id, path, points);
			gm.savePlayer ();

			// Clear the choices
			emptyChoiceList ();

			// Set this window to inactive
			gameObject.SetActive (false);
		}

		public void incorrectAnswer() {
			gm.sfxNodeIncorrect.Play ();
			gm.createBigNotice ("Sorry!", "That was not the correct answer, please try again.");
		}

		public void cancel() {
			emptyChoiceList ();
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();
		}

		public void emptyChoiceList() {
			foreach (Transform child in uiContent.transform) {
				if (child.gameObject.activeSelf) {
					GameObject.Destroy(child.gameObject);
				}
			}	
		}

	}
}