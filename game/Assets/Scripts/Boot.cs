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

public class Boot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public GameObject background;
	public GameObject playCanvas;
	public GameObject playPanel;
	public GameObject loginPanel;
	public Text status;
	public InputField playname;
	public InputField email;
	public InputField password;

	public GameObject spawn;
	public GameObject ground;

	private GameManager gm;
	private int avatarPosition = 0;
	private float dragStartPosition;
	private int startWorld = -1;
		
	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

		gm.cameraMain.gameObject.SetActive (false);
		gm.cameraBoot.gameObject.SetActive (true);

		playCanvas.gameObject.SetActive (false);

		// Remove the dark background
		if (gm.editMode) {
			bootStart ();
		}
	}

	public void bootStart() {
		Color myColor = new Color();
		ColorUtility.TryParseHtmlString("#242A31", out myColor);

		if (gm.editMode) {
			bootLogin();
		}
	}

	public void bootLogin() {
		StartCoroutine(initLogin());
	}

	IEnumerator initLogin() {
		// Make the login form request
		WWWForm form = new WWWForm();
		form.AddField("email", "jo@playfirst.co.za");
		form.AddField("password", "Dupljohan78!");
		//form.AddField("email", "admin@playfirst.co.za");
		//form.AddField("password", "admin");

		// Make the request
		status.text = "Logging in...";
		WWW www = new WWW(gm.api_path+"/login", form);
		yield return www;

		// Get the user data
		status.text = "Storing user data...";
		JSONNode data = JSON.Parse(www.text);

		gm.userId = data [0]["id"].AsInt;
		gm.userFullname = data [0]["fullname"];
		gm.userPlayname = data [0]["playname"];
		gm.userEmail = data [0]["email"];
		gm.userPassword = data [0]["password"];
		gm.userPoints = data [0]["points"].AsInt;
		gm.userRole = data [0]["role"].AsInt;
		gm.userAvatar = data [0]["avatar"];

		// Update the UI text for the playname
		playname.text = gm.userPlayname;

		// Get the array position of the users avatar
		avatarPosition = findPositionInStringArray (gm.avatars, gm.userAvatar);

		// Split the user badges
		string apiUserNodes = data [0]["nodes"];
		string apiUserPaths = data [0]["paths"];
		string apiUserBadges = data [0]["badges"];

		// Iterate over each one and add them to a list
		if (apiUserPaths != null && apiUserPaths != "") {
			string[] b = apiUserPaths.Split(',');
			foreach (string s in b) {
				gm.userPaths.Add(int.Parse(s));
			}
		}

		if (apiUserNodes != null && apiUserNodes != "") {
			string[] b = apiUserNodes.Split(',');
			foreach (string s in b) {
				gm.userNodes.Add(int.Parse(s));
			}
		}

		if (apiUserBadges != null && apiUserBadges != "") {
			string[] b = apiUserBadges.Split(',');
			foreach (string s in b) {
				gm.userBadges.Add(int.Parse(s));
			}
		}

		// Make the requests for the API
		status.text = "Loading guides...";
		WWW wwwGuides = new WWW(gm.api_path+"/library/guides");
		yield return wwwGuides;

		status.text = "Loading badges...";
		WWW wwwBadges = new WWW(gm.api_path+"/library/badges");
		yield return wwwBadges;

		status.text = "Loading nodes...";
		WWW wwwNodes = new WWW(gm.api_path+"/library/nodes");
		yield return wwwNodes;

		status.text = "Loading paths...";
		WWW wwwPaths = new WWW(gm.api_path+"/library/paths");
		yield return wwwPaths;

		status.text = "Loading assets...";
		WWW wwwAssets = new WWW(gm.api_path+"/library/assets");
		yield return wwwAssets;

		status.text = "Loading worlds...";
		WWW wwwWorlds = new WWW(gm.api_path+"/library/worlds");
		yield return wwwWorlds;

		// Caching the images
		status.text = "Loading images...";
		WWW wwwImages = new WWW(gm.api_path+"/library/images");
		yield return wwwImages;

		status.text = "Caching images...";
		JSONNode images = JSON.Parse(wwwImages.text);

		for (int c = 0; c < images.Count; c++) {
			string url = gm.file_path + "/image/thumbnail?image=" + images [c];
			WWW w = new WWW(url);  
			yield return w;        

			if(w.bytes != null) {
				string filenameFromApi = images [c];
				string[] filenameArray = filenameFromApi.Split ('/');
				string filenameSanitized = filenameArray[filenameArray.Length-1];
				string dataWritePath = Application.persistentDataPath + "/" + filenameSanitized;

				System.IO.File.WriteAllBytes(dataWritePath, w.bytes);
			}
		}

		// Log
		gm.log("login", 0);

		// Assign the data to the Game Manager
		gm.badges = JSON.Parse(wwwBadges.text);
		gm.nodes = JSON.Parse(wwwNodes.text);
		gm.paths = JSON.Parse(wwwPaths.text);
		gm.assets = JSON.Parse(wwwAssets.text);
		gm.worlds = JSON.Parse(wwwWorlds.text);
		gm.guides = JSON.Parse(wwwGuides.text);

		// Get the start world
		status.text = "Getting start world...";
		WWW wwwWorldStart = new WWW(gm.api_path+"/worlds/start");
		yield return wwwWorldStart;

		// Get the level
		JSONNode startWorldApi = JSON.Parse(wwwWorldStart.text);
		startWorld = startWorldApi ["id"].AsInt;

		// Reset the player array
		gm.multiplayers = new List<GameObject> ();

		// Connecting to server
		status.text = "Connecting to server...";

		try {
			gm.theSocket = new TcpClient(gm.server_host, gm.server_port);
			gm.theStream = gm.theSocket.GetStream();
			gm.theWriter = new StreamWriter(gm.theStream);
			gm.theReader = new StreamReader(gm.theStream);

			gm.theWriter.AutoFlush = true;

			gm.socket_ready = true;
		} catch (Exception e) {
			Debug.Log("Boot socket error: " + e);
		}

		// Place the avatar
		placeAvatar ();

		gm.sfxMapLoad.Play();
		
		loginPanel.gameObject.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (313.25f, -1422), 0.75f, true).SetEase (Ease.OutQuart);
		playPanel.gameObject.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (313.25f, -422), 0.75f, true).SetEase (Ease.OutQuart).OnComplete (() => {
			if (gm.editMode) {
				bootGame();
			}
		});	
	}

	private void placeAvatar() {
		Destroy(gm.avatarObject);

		// Make is null
		gm.avatarObject = null;

		// Load the new one
		GameObject ob = Resources.Load ("Avatars/"+gm.avatars[avatarPosition]) as GameObject;
		GameObject instantiatedObject = Instantiate(ob); 

		// Assign the new one to the game
		gm.avatarObject = instantiatedObject;

		// Assign it the correct layer
		foreach (Transform child in gm.avatarObject.transform) { 
			child.gameObject.layer = LayerMask.NameToLayer("Character");
		}

		// Place it
		gm.avatarObject.transform.position = new Vector3 (spawn.gameObject.transform.position.x, spawn.gameObject.transform.position.y, spawn.gameObject.transform.position.z);
		gm.avatarObject.transform.rotation = Quaternion.Euler (0, 0, 0);
		gm.avatarObject.GetComponent<MWO.Player>().init(gm.userId, gm.userPlayname);
	}

	void Update () {}

	public void OnDrag(PointerEventData eventData) {}
	public void OnPointerEnter(PointerEventData eventData) {}
	public void OnPointerExit(PointerEventData eventData) {}
	public void OnPointerClick(PointerEventData eventData) {}
	public void OnPointerDown(PointerEventData eventData) { dragStartPosition = Input.mousePosition.x; }
	public void OnPointerUp(PointerEventData eventData) { 
		// Calculate the difference
		float difference = dragStartPosition - Input.mousePosition.x;

		if (difference < 0)
			difference = difference * -1;

		// If it's substantial
		if (difference > 10f) {
			if (dragStartPosition >= Input.mousePosition.x) {
				avatarPosition--;
				if (avatarPosition == -1) {
					avatarPosition = gm.avatars.Length-1;
				}
				placeAvatar ();
			} else {
				avatarPosition++;
				if (avatarPosition == gm.avatars.Length) {
					avatarPosition = 0;
				}
				placeAvatar ();
			}
		}

		gm.sfxObjectRotate.Play();
	}
		
	public void bootGame() {	
		// Remove the background
		background.gameObject.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-400, 0), 0.75f, true).SetEase (Ease.OutQuart).OnComplete (() => {
			gm.sfxLogin.Play();

			// Store the new playname
			gm.userPlayname = playname.text;

			// Show the Panel
			playCanvas.gameObject.SetActive (true);

			// Place the HUD camera
			gm.cameraHud.transform.SetParent(gm.avatarObject.transform);
			//gm.cameraMain.transform.SetParent(gm.avatarObject.transform);

			// Switch camera
			gm.cameraMain.gameObject.SetActive (true);
			gm.cameraBoot.gameObject.SetActive (false);

			// Reset the status
			status.text = "";

			// Position the player
			gm.setWorld(startWorld) ;

			// Disable the box collider for the character stage
			ground.GetComponent<MeshCollider>().enabled = false;

			// Save the player for testing
			gm.savePlayer ();

			// Disable the booting UI
			gameObject.SetActive(false);
		});	
	}
		
	public int findPositionInStringArray(string[] haystack, string needle) {
		int position = 0;
		for (int count=0; count<haystack.Length; count++) { 
			if (haystack[count] == needle) {
				position = count;
			}
		}
		return position;
	}

	public void quit() {
		Application.Quit();
	}
}
