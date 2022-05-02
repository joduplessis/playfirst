using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PopupNodes {
	public class Mixed : MonoBehaviour {

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

			StartCoroutine(loadNodeMixedContent());
		}
			
		IEnumerator loadNodeMixedContent () {
			string Url = gm.api_path+"/nodes/content_mixed/" + id;

			Debug.Log (Url);
			Debug.Log ("webViewObject doesn't work with Unity 2017.x - needs fix");

			/*

			webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
			webViewObject.Init(
				cb: (msg) => {
					Debug.Log(string.Format("CallFromJS[{0}]", msg));
					// status.text = msg;
					// status.GetComponent<Animation>().Play();
				},
				err: (msg) => {
					Debug.Log(string.Format("CallOnError[{0}]", msg));
					// status.text = msg;
					// status.GetComponent<Animation>().Play();
				},
				ld: (msg) => {
					Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
					webViewObject.EvaluateJS(@"
						window.Unity = {
							call: function(msg) {
								var iframe = document.createElement('IFRAME');
								iframe.setAttribute('src', 'unity:' + msg);
								document.documentElement.appendChild(iframe);
								iframe.parentNode.removeChild(iframe);
								iframe = null;
							}
						}");
				},
				enableWKWebView: true);

			// The margins differ between editor & mobile screens
			if (gm.mobileControlMode) {
				webViewObject.SetMargins (60*2, 260*2, 60*2, 120*2);
			} else {
				webViewObject.SetMargins (60, 200, 60, 85);
			}

			// Display it
			webViewObject.SetVisibility(true);

			if (Url.StartsWith("http")) {
				webViewObject.LoadURL(Url.Replace(" ", "%20"));
			} else {
				var exts = new string[]{
					".jpg",
					".html"
				};

				foreach (var ext in exts) {
					var url = Url.Replace(".html", ext);
					var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
					var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
					byte[] result = null;

					if (src.Contains("://")) {
						var www = new WWW(src);
						yield return www;
						result = www.bytes;
					} else {
						result = System.IO.File.ReadAllBytes(src);
					}

					System.IO.File.WriteAllBytes(dst, result);

					if (ext == ".html") {
						webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
						break;
					}
				}

				//webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
			}


			webViewObject.EvaluateJS(
				"parent.$(function() {" +
				"   window.Unity = {" +
				"       call:function(msg) {" +
				"           parent.unityWebView.sendMessage('WebViewObject', msg)" +
				"       }" +
				"   };" +
				"});");
				
			*/

			yield break;
		}

		public void confirm() {
			gm.sfxNodeCorrect.Play ();
			gm.createAlert ("Congrats", "Well done for completing this module. Now find the next one to earn more points!");
			gm.addUserNode(id, path, points);
			gm.savePlayer ();

			// Disable the view
			webViewObject.SetVisibility(false);
			webViewObject.gameObject.SetActive (false);

			// Disable the window
			gameObject.SetActive (false);
		}

		public void cancel() {
			gameObject.SetActive (false);
			gm.sfxPopupClose.Play ();

			webViewObject.gameObject.SetActive (false);
		}
	}
}