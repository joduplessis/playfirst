using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SimpleJSON;
using System;
using System.IO;
using System.Net.Sockets;
using System.IO;
using DG.Tweening;

namespace PopupNodes {
	public class Images : MonoBehaviour {

		private GameManager gm;

		public int points;
		public int id;
		public string title;
		public int path;
		public string type;
		public string content ;

		private List<GameObject> slides;
		private int position;
		private bool forward;

		[Header("UI")]
		public Text uiPoints;
		public Text uiPath;
		public Text uiTitle;
		public GameObject uiContent;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Awake() {
			position = 0;
			slides = new List<GameObject> ();
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

			StartCoroutine(loadNodeImagesContent());
		}

		IEnumerator loadNodeImagesContent () {
			string[] imagesArray = content.Split (',');

			for (int c=0; c < imagesArray.Length; c++) {
				string[] filenameArray = imagesArray [c].Split ('/');
				string filenameSanitized = filenameArray[filenameArray.Length-1];
				string url = "file://" + Application.persistentDataPath + "/" + filenameSanitized;

				var www = new WWW(url); 
				yield return www.texture;

				// Create game Object
				GameObject si = new GameObject("Slide");
				Sprite sprite = new Sprite();     
				sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero); 

				si.transform.SetParent(uiContent.gameObject.transform);
				si.AddComponent<RectTransform> ();
				si.AddComponent<Image> ();
				si.gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				si.GetComponent<RectTransform> ().localScale = new Vector3 (1.0f, 1.0f, 1.0f);

				// Make it transparent
				Color ac = si.GetComponent<Image> ().color;
				ac.a = 0;
				si.GetComponent<Image> ().color = ac;

				// Set the rect properties
				si.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
				si.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
				si.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
				si.GetComponent<RectTransform> ().localPosition = Vector3.zero;

				// Set the image
				si.GetComponent<Image> ().sprite = sprite;

				// Add them to the array
				slides.Add(si);
			}  

			// Set the marker to forward


			// Init the slideshow
			StartCoroutine (changeSlide ());

			yield return true;
		}

		public void setForward() {
			gm.createNotice ("Playing forward");
			forward = true;

		}

		public void setBackward() {
			gm.createNotice ("Playing backward");
			forward = false;
		}
			
		IEnumerator changeSlide() {
			float time = 2.0f;
			float delay = 4.0f;

			if (forward) { position++; } else { position--; }

			if (position == slides.Count) { position = 0; }
			if (position < 0) { position = slides.Count - 1; }

			int next = position;

			if (forward) { next++; } else { next--; }

			if (next == slides.Count) { next = 0; }
			if (next < 0) { next = slides.Count - 1; }

			slides [position].GetComponent<Image> ().DOFade (0.0f, time);
			slides [next].GetComponent<Image> ().DOFade (1.0f, time);

			yield return new WaitForSeconds(time+delay);

			StartCoroutine (changeSlide ());
		}

		public void confirm() {
			gm.sfxNodeCorrect.Play ();
			gm.createAlert ("Congrats", "Well done for completing this module. Now find the next one to earn more points!");
			gm.addUserNode(id, path, points);
			gm.savePlayer ();
			gameObject.SetActive (false);
		}

		public void cancel() {
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();
		}
	}
}
