using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;


namespace PopupMain {
	public class Active : MonoBehaviour {

		public Image avatar;
		public Text status;
		public Text user;
		public Text time;

		void Start () {}

		void Update () {
		}

		void Awake() {
		}


		public void init() {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}

		public void loadImage(string img) {
			StartCoroutine(loadImageEnum(img));
		}

		private IEnumerator loadImageEnum (string img) {
			WWW www = new WWW(img);  
			yield return www;        

			if(www.texture != null) {
				Sprite sprite = new Sprite();     
				avatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero); 
				avatar.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(avatar.gameObject.GetComponent<RectTransform> ().sizeDelta.x, avatar.gameObject.GetComponent<RectTransform> ().sizeDelta.y);
			}

			yield return null;
		}
	}
}
