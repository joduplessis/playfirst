using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

namespace PopupAssets {
	public class Asset : MonoBehaviour {

		private GameManager gm;

		public int id;
		public string image;
		public string prefab;
		public string name;
		public int points;

		public Text uiName;
		public Text uiPoints;
		public Image uiImage;

		void Start () {
		}		

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
		}

		public void placeAsset() {
			if (points > gm.userPoints) {
				gm.createNotice ("Not enough points");
			} else {
				gm.userPoints = gm.userPoints - points;
				StartCoroutine (initPlaceAsset ());
			}
		}

		IEnumerator initPlaceAsset() {
			WWWForm form = new WWWForm();
			form.AddField ("user", gm.userId);
			form.AddField("world", gm.world["id"]);
			form.AddField("prefab", prefab);
			form.AddField("name", name);
			form.AddField("x", gm.avatarObject.transform.position.x-15+"");
			form.AddField("y", 0+"");
			form.AddField("z", gm.avatarObject.transform.position.z+"");
			form.AddField("rotation", 0+"");
			form.AddField("node", 0+"");
			form.AddField("portal", 0+"");
			form.AddField("progress", 0+"");

			// Make the request
			WWW www = new WWW(gm.api_path+"/mwo/add", form);
			yield return www;

			// Get the new ID
			int mwoId = int.Parse(www.text);

			// Broadcast
			gm.broadcast("object_add,"+gm.userId+","+mwoId+","+gm.world["id"]);

			// Place the object
			gm.createMWOAsset (mwoId, gm.userId, gm.world["id"].AsInt, prefab, name, gm.avatarObject.transform.position.x-15, 0, gm.avatarObject.transform.position.z, 0, 0, 0, 0, false);

			// Play the start sound
			gm.sfxBuildingStart.Play ();
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

		public void init() {
			uiName.text = name;
			uiPoints.text = points+"";
			loadImage (image);
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}
	}
}
