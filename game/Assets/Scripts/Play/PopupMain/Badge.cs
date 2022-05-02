using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace PopupMain {
	public class Badge : MonoBehaviour {

		private GameManager gm;

		public int id;

		public Image uiImage;
		public Text uiTitle;
		public Text uiDescription;

		// Use this for initialization
		void Start () {
			
		}
		
		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
		}

		public void init() {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			for (int c = 0; c < gm.badges.Count; c++) {
				if (gm.badges [c] ["id"].AsInt == id) {
					uiTitle.text = gm.badges [c] ["title"];
					uiDescription.text = gm.badges [c] ["description"];

					loadImage (gm.badges [c] ["image"]);
				}
			}
		}

		public void loadImage(string img) {
			StartCoroutine(loadImageCache (img));
		}

		IEnumerator loadImageCache(string img) {

			string[] filenameArray = img.Split ('/');
			string filenameSanitized = filenameArray[filenameArray.Length-1];

			var www = new WWW("file://" + Application.persistentDataPath + "/"+filenameSanitized); 
			yield return www.texture;

			Sprite sprite = new Sprite();     
			sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero); 

			uiImage.sprite = sprite;
			uiImage.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(uiImage.gameObject.GetComponent<RectTransform> ().sizeDelta.x, uiImage.gameObject.GetComponent<RectTransform> ().sizeDelta.y);
		}

	}
}
