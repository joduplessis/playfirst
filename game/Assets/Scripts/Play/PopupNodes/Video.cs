using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using System;
using DG.Tweening;

namespace PopupNodes {
	public class Video : MonoBehaviour {

		private GameManager gm;
		private MovieTexture movieTexture;

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
		public RawImage uiVideo;
		public GameObject uiLoading;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			gameObject.SetActive (false);
		}

		void Awake() {
			uiLoading.gameObject.SetActive (true);
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

			// Load video
			StartCoroutine(loadNodeVideoContent());
		}

		IEnumerator loadNodeVideoContent () {
			string url = gm.file_path + "/" + content;			

			if (gm.mobileControlMode) {
				gm.createAlert ("Whoops", "Please use the desktop game to complete this video exercise!");

				yield return true;
			} else {
				uiLoading.gameObject.SetActive (true);

				WWW www = new WWW (url);
				yield return www;

				// Remove the loader
				uiLoading.gameObject.SetActive (false);

				// Assign the raw image the movie texture - yes this throws an error - it's a bug in Unity
				movieTexture = (MovieTexture)www.GetMovieTexture();
				uiVideo.texture = movieTexture;

				// Play the audio
				GetComponent<AudioSource> ().clip = movieTexture.audioClip;

				// Stop & start the video again
				replay ();
			}
		}

		public void replay() {
			if (!gm.mobileControlMode) {
				movieTexture.Stop ();
				movieTexture.Play ();

				// Play the audio
				GetComponent<AudioSource> ().Stop ();
				GetComponent<AudioSource> ().Play ();

				// Lower the volume
				gm.GetComponent<AudioSource> ().DOFade (0.1f, 3.0f);
			}
		}

		public void play() {
			if (!gm.mobileControlMode) {
				movieTexture.Play ();
				GetComponent<AudioSource> ().Play ();
			}
		}

		public void pause() {
			if (!gm.mobileControlMode) {
				movieTexture.Pause ();
				GetComponent<AudioSource> ().Pause ();
			}
		}

		public void confirm() {
			gm.sfxNodeCorrect.Play ();
			gm.createAlert ("Congrats", "Well done for completing this module. Now find the next one to earn more points!");
			gm.addUserNode(id, path, points);
			gm.savePlayer ();

			// Lift the volume
			gm.GetComponent<AudioSource>().DOFade(0.5f, 1.0f);

			// Disable the window
			gameObject.SetActive (false);
		}

		public void cancel() {
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();

			// Lift the volume
			gm.GetComponent<AudioSource>().DOFade(1.0f, 1.0f);
		}
	}
}