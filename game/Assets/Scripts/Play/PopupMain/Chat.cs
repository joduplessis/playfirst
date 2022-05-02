using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using System;

namespace PopupMain {
	public class Chat : MonoBehaviour {

		private GameManager gm;

		public Text chat;
		public Text user;
		public Image avatar;
		public Text time;
		public string avatarString;
		public Int32 unixTimestamp;

		// Use this for initialization
		void Start () {
		}

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void init() {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			time.text = System.DateTime.Now.ToString("h:mm:ss tt");
			unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			StartCoroutine(loadImageEnum (avatarString));
		}

		private IEnumerator loadImageEnum (string img) {
			WWW www = new WWW(gm.file_path+"/avatars/"+img+".png");  
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
