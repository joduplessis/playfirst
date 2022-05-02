using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using System.Net.Sockets;
using DG.Tweening;
using EpPathFinding.cs;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[Header("Traffic routes")] 
	public List<GridPos> journeyGoingLeftGrid;
	public List<GridPos> journeyGoingRightGrid;
	public List<GridPos> journeyGoingUpGrid;
	public List<GridPos> journeyGoingDownGrid;

	[Header("Grid")]
	private int grid = 25;
	private float size = 20f;

	[Header("API & Editing")]
	public bool editMode = false;
	public bool mobileControlMode = false;
	public string api_path = "http://app.playfirst.co.za/api";
	public string file_path = "http://app.playfirst.co.za";

	[Header("Multiplayers")]
	public List<GameObject> multiplayers;

	[Header("Network")]
	public string server_host = "localhost";
	public int server_port = 7520;
	public bool socket_ready = false;
	public TcpClient theSocket;
	public NetworkStream theStream;
	public StreamWriter theWriter;
	public StreamReader theReader;

	[Header("Effects")]
	public GameObject explosion;

	[Header("Shader")]
	public Shader shaderNormal;
	public Shader shaderSelected;
	public Shader shaderBuilding;

	[Header("Library")]
	public JSONNode badges;
	public JSONNode nodes;
	public JSONNode assets;
	public JSONNode paths;
	public JSONNode worlds;
	public JSONNode guides;

	[Header("Current world")]
	public JSONNode world;

	[Header("User data")]
	public int userId;
	public string userFullname;
	public string userPlayname;
	public string userEmail;
	public string userPassword;
	public int userPoints;
	public int userRole;
	public string userAvatar;
	public List<int> userBadges;
	public List<int> userNodes;
	public List<int> userPaths;

	[Header("Cameras")]	
	public Camera cameraMain;
	public Camera cameraMinimap;
	public Camera cameraHud;
	public Camera cameraBoot;
	public GameObject cameraContainer;

	[Header("Active objects")]	
	public GameObject selectedObject;
	public GameObject selectedOtherPlayerObject;
	public GameObject activeNodeObject;
	public GameObject currentNodeObject;
	public GameObject avatarObject;
	public GameObject objectToPlace;
	public GameObject landscape;

	[Header("Paths & nodes")]
	public int activePath;
	public int activeNode;

	[Header("HUD elements")]
	public GameObject hudSelectedToolsContainer;
	public GameObject hudCurrentNodeContainer;
	public GameObject hudMinimapContainer;
	public GameObject hudLoadingContainer;
	public Text hudLoadingContainerStatus;
	public GameObject hudNodeContainer;
	public GameObject hudLogoutContainer;
	public GameObject hudChats;

	[Header("Movable UI blueprints")]
	public GameObject portalTooltipBlueprint;
	public GameObject nodeTooltipBlueprint;
	public GameObject pointsTooltipBlueprint;
	public GameObject playerTooltipBlueprint;
	public GameObject progressTooltipBlueprint;
	public GameObject guideTooltipBlueprint;
	public GameObject movableTooltipContainer;

	[Header("User notifications")]
	public GameObject popupAlert;
	public GameObject popupNotice;
	public GameObject popupBigNotice;

	[Header("Canvases")]
	public Canvas boot;
	public Canvas play;

	[Header("Avatar list")]
	public string[] avatars = new string[8] { "MaleSimpleMovement1", "MaleSimpleMovement2", "MaleSimpleMovement3", "MaleSimpleMovement4", "FemaleSimpleMovement1", "FemaleSimpleMovement2", "FemaleSimpleMovement3", "FemaleSimpleMovement4" };

	[Header("Sounds")]
	public AudioSource sfxObjectExplode;
	public AudioSource sfxObjectRotate;
	public AudioSource sfxObjectSelect;
	public AudioSource sfxNodeSelect;
	public AudioSource sfxLogin;
	public AudioSource sfxLogout;
	public AudioSource sfxMoneyCollect;
	public AudioSource sfxMapLoad;
	public AudioSource sfxPopupOpen;
	public AudioSource sfxPopupClose;
	public AudioSource sfxAlert;
	public AudioSource sfxNotice;
	public AudioSource sfxNodeCorrect;
	public AudioSource sfxPathComplete;
	public AudioSource sfxNodeIncorrect;
	public AudioSource sfxBadge;
	public AudioSource sfxBuildingStart;
	public AudioSource sfxBuildingComplete;
	public AudioSource sfxSet;
	public AudioSource sfxStart;

	void Start() {
		boot.gameObject.SetActive (true);
		toggleMinimap ();
		hudCurrentNodeContainer.GetComponent<Header.CurrentPath> ().hide ();
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update () {
		if (socket_ready) {
			// Reading the data
			if (theStream.DataAvailable) {
				Byte[] inStreamBytes = new Byte[theSocket.SendBufferSize];
				theStream.Read(inStreamBytes, 0, inStreamBytes.Length);

				processIncomingBroadcast(System.Text.Encoding.UTF8.GetString(inStreamBytes));
			}

			// If the connection is dropped
			if(!theStream.CanRead) {
				theWriter.Close();
				theReader.Close();
				theSocket.Close();

				socket_ready = false;
			}
		}
	}

	// World creation
		
	public void setWorld(int wid) {
		StartCoroutine (setWorldEnum (wid));
	} 
		
	IEnumerator setWorldEnum(int wid) {
		
		hudLoadingContainer.gameObject.SetActive (true);
		hudLoadingContainerStatus.text = "Cleaning world...";

		destroyStageObjects ("Car");
		destroyStageObjects ("Tile");
		destroyStageObjects ("Road");
		destroyStageObjects ("Building");

		hudLoadingContainerStatus.text = "Creating tiles...";

		// Re create tiles
		for (int r = 0; r < grid; r++) {
			for (int c = 0; c < grid; c++) {
				GameObject tileObject = Resources.Load ("tile") as GameObject;
				GameObject instantiatedTile = Instantiate (tileObject); 

				float x = convertTileToCoordinate (c);
				float y = -0.5f;
				float z = convertTileToCoordinate (r);

				instantiatedTile.transform.position = new Vector3 (x, y, z);

				instantiatedTile.GetComponent<MWO.Tile> ().y = r;
				instantiatedTile.GetComponent<MWO.Tile> ().x = c;
			}
		}

		yield return null;

		// Tell the user
		hudLoadingContainerStatus.text = "Building world...";

		// Set this world
		for (int w = 0; w < worlds.Count; w++) {
			if (worlds [w] ["id"].AsInt == wid) {
				world = worlds [w];
			}
		}

		// Reset the object trackers
		selectedObject = null;
		activeNodeObject = null;

		// Log
		log("world_enter", wid);

		// Get all the world objects
		WWW wwwMwo = new WWW(api_path+"/mwo/"+wid);
		yield return wwwMwo;

		JSONNode mwo = JSON.Parse (wwwMwo.text);

		// Create the MWO objects
		for (int c = 0; c < mwo.Count; c++) {
			int id = mwo [c] ["id"].AsInt;
			int user = mwo [c] ["user"].AsInt;
			int world = mwo [c] ["world"].AsInt;
			string prefab = mwo [c] ["prefab"];
			string name = mwo [c] ["name"];
			float x = mwo [c] ["x"].AsFloat;
			float y = mwo [c] ["y"].AsFloat;
			float z = mwo [c] ["z"].AsFloat;
			float rotation = mwo [c] ["rotation"].AsFloat;
			int node = mwo [c] ["node"].AsInt;
			int portal = mwo [c] ["portal"].AsInt;
			float progress = mwo [c] ["progress"].AsFloat;

			createMWOAsset (id, user, world, prefab, name, x, y, z, rotation, node, portal, progress, true);
		}
		yield return null;

		hudLoadingContainerStatus.text = "Placing you...";

		// Look for a place for the player
		bool foundPlayerPosition = false;

		if (!editMode) {
			while (!foundPlayerPosition) {
				float minX = ((grid * size) / 2 - size) * -1;
				float maxX = (grid * size) / 2 - size;
				float minY = ((grid * size) / 2 - size) * -1;
				float maxY = (grid * size) / 2 - size;
				float x = UnityEngine.Random.Range (minX, maxX);
				float y = UnityEngine.Random.Range (minY, maxY);

				RaycastHit hit;

				Vector3 positionAbove = new Vector3 (x, 10, y);
				Vector3 positionBelow = new Vector3 (x, -10, y);
				if (Physics.Raycast (positionAbove, positionBelow, out hit)) {
					if (hit.collider.gameObject.tag == "Tile") {
						foundPlayerPosition = true;

						avatarObject.gameObject.transform.position = new Vector3 (hit.collider.gameObject.transform.position.x, 10, hit.collider.gameObject.transform.position.z);
					} 
				}
			}
		}

		// Make it invisible again
		sfxMapLoad.Play ();

		// Map the roads
		updateRoads();

		// Hide the laoding screen
		hudLoadingContainer.gameObject.SetActive (false);

		// Emit to the network
		broadcast ("player_world_switch,"+userId+","+world["id"]+","+userAvatar+","+userPlayname);

		yield return null;
	}

	IEnumerator setObjectEnum(int oid) {
		// Get one object
		WWW wwwMwo = new WWW(api_path+"/mwo/single/"+oid);
		yield return wwwMwo;

		JSONNode mwo = JSON.Parse (wwwMwo.text);

		// Create the MWO objects
		for (int c = 0; c < mwo.Count; c++) {
			int id = mwo [c] ["id"].AsInt;
			int user = mwo [c] ["user"].AsInt;
			int world = mwo [c] ["world"].AsInt;
			string prefab = mwo [c] ["prefab"];
			string name = mwo [c] ["name"];
			float x = mwo [c] ["x"].AsFloat;
			float y = mwo [c] ["y"].AsFloat;
			float z = mwo [c] ["z"].AsFloat;
			float rotation = mwo [c] ["rotation"].AsFloat;
			int node = mwo [c] ["node"].AsInt;
			int portal = mwo [c] ["portal"].AsInt;
			float progress = mwo [c] ["progress"].AsFloat;

			createMWOAsset (id, user, world, prefab, name, x, y, z, rotation, node, portal, progress, true);
		}
		yield return null;
	}

	public void createMWOAsset(
		int id,
		int user,
		int world,
		string prefab,
		string name,
		float x,
		float y,
		float z,
		float rotation,
		int node,
		int portal,
		float progress,
		bool placed) {

		GameObject mwoObject = Resources.Load ("MWO/"+prefab) as GameObject;
		GameObject instantiatedMWO = Instantiate (mwoObject); 

		instantiatedMWO.transform.localRotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		instantiatedMWO.transform.position = new Vector3 (x, y, z);

		instantiatedMWO.GetComponent<MWO.Base> ().x = convertCoordinateToTile (x);
		instantiatedMWO.GetComponent<MWO.Base> ().z = convertCoordinateToTile (z);
		instantiatedMWO.GetComponent<MWO.Base> ().id = id;
		instantiatedMWO.GetComponent<MWO.Base> ().user = user;
		instantiatedMWO.GetComponent<MWO.Base> ().world = world;
		instantiatedMWO.GetComponent<MWO.Base> ().prefab = prefab;
		instantiatedMWO.GetComponent<MWO.Base> ().name = name;
		instantiatedMWO.GetComponent<MWO.Base> ().node = node;
		instantiatedMWO.GetComponent<MWO.Base> ().portal = portal;
		instantiatedMWO.GetComponent<MWO.Base> ().progress = progress;
		instantiatedMWO.GetComponent<MWO.Base> ().placed = placed;
		instantiatedMWO.GetComponent<MWO.Base> ().init ();
	}

	// Alerts
		
	public void createAlert(string title, string description) {
		sfxAlert.Play ();
		popupAlert.GetComponent<Alert> ().init(title, description);
	}

	public void createNotice(string title) {
		sfxNotice.Play ();
		popupNotice.GetComponent<Notice> ().init (title);
	}

	public void createBigNotice(string heading, string description) {
		sfxNotice.Play ();
		popupBigNotice.GetComponent<BigNotice> ().init (heading, description);
	}
		
	// Network

	private void processIncomingBroadcast(string message) {
		string[] parts = message.Split(',');

		if (parts.Length != 1) {
			if (parts [0] == "player_leave") {
				int position = -1;
				int pid = Int32.Parse (parts [1]);

				for (int c = 0; c < multiplayers.Count; c++) {
					if (multiplayers [c].GetComponent<MWO.Player> ().id == pid) {
						Destroy (multiplayers [c].GetComponent<MWO.Player> ().playnameTooltip);
						Destroy (multiplayers [c]);
						position = c;
					}
				}

				if (position != -1) {
					multiplayers.RemoveAt (position);
				}
			}
				
			if (parts [0] == "player_movement") {
				int mp_id = Int32.Parse (parts [1]);
				int mp_world = Int32.Parse (parts [2]);
				float mp_x = float.Parse (parts [3]);
				float mp_y = float.Parse (parts [4]);
				float mp_z = float.Parse (parts [5]);
				float mp_r = float.Parse (parts [6]);
				float mp_ws = float.Parse (parts [7]);
				string mp_avatar = parts [8];
				string mp_playname = parts [9];
				int current_world = Int32.Parse (world ["id"]);

				if (mp_id != userId) {

					// The intial position in the array
					int pos = -1;

					// Is the other player here
					for (int n = 0; n < multiplayers.Count; n++) {
						if (multiplayers [n].GetComponent<MWO.Player> ().id == mp_id) {
							pos = n;
						}
					}

					// If he is - then move him
					if (pos != -1) {
						multiplayers [pos].transform.position = new Vector3 (mp_x, mp_y, mp_z);
						multiplayers [pos].transform.rotation = Quaternion.Euler (0, mp_r, 0);
						multiplayers [pos].GetComponent<MWO.Player> ().setAnimationSpeed(mp_ws);
					}

					// if not, then add him if it's the right world
					if (pos == -1 && mp_world == current_world) {
						GameObject np = Resources.Load ("Avatars/" + mp_avatar) as GameObject;
						GameObject instantiatedObject = Instantiate (np); 

						instantiatedObject.GetComponent<MWO.Player> ().init (mp_id, mp_playname);
						multiplayers.Add (instantiatedObject);
					}
				}
			}

			if (parts [0] == "player_world_switch") {
				string switched_to_world = parts [2];
				int pid = Int32.Parse (parts [1]);

				// If it's not this user
				if (pid != userId) {

					// If they're not switching to this world, then remove them
					if (switched_to_world != world ["id"]) {
						int position = -1;

						for (int c = 0; c < multiplayers.Count; c++) {
							if (multiplayers [c].GetComponent<MWO.Player> ().id == pid) {
								Destroy (multiplayers [c].GetComponent<MWO.Player> ().playnameTooltip);
								Destroy (multiplayers [c]);
								position = c;

							}
						}

						if (position != -1) {
							multiplayers.RemoveAt (position);
						}
					}

					// If they're switching to this world, add them
					if (switched_to_world == world ["id"]) {
						string avatar = parts [3];
						string playname = parts [4];

						GameObject np = Resources.Load ("Avatars/" + avatar) as GameObject;
						GameObject instantiatedObject = Instantiate (np); 

						instantiatedObject.GetComponent<MWO.Player> ().init (pid, playname);
						multiplayers.Add (instantiatedObject);
					}
				}
			}

			if (parts [0] == "object_add") {
				int pid = Int32.Parse (parts [1]);
				int oid = Int32.Parse (parts [2]);
				string object_world = parts [3];

				if (pid != userId) {
					if (object_world == world ["id"]) {
						setObjectEnum (oid);
					}
				}
			}

			if (parts [0] == "object_destroy") {
				int pid = Int32.Parse (parts [1]);
				int oid = Int32.Parse (parts [2]);
				string object_world = parts [3];

				if (pid!=userId) {
					if (object_world == world ["id"]) {
						destroyBaseMwoObjectById (oid, "Building");
						destroyBaseMwoObjectById (oid, "Road");
					}
				}
			}
		
			if (parts [0] == "chat") {
				hudChats.gameObject.GetComponent<PopupMain.Chats> ().addChat (message);
			}
		}
	}

	public void broadcast(string theLine) {
		if (!socket_ready)
			return;

		theWriter.Write (theLine+"\r\n");
		theWriter.Flush ();	
	}

	void OnApplicationQuit() {
		broadcast ("player_leave,"+userId);
		broadcast ("end");
	}

	// Log

	public void log(string type, int target) {
		// StartCoroutine (logApi (type, target));
	}

	IEnumerator logApi(string type, int target) {
		WWWForm form = new WWWForm();
		form.AddField("user", userId);
		form.AddField("type", type);
		form.AddField("target", target);

		WWW www = new WWW(api_path+"/activity/log", form);
		yield return www;
	}
		
	// General

	public void deleteObject(GameObject go) {
		sfxObjectExplode.Play ();
		GameObject newexplosion = (GameObject)Instantiate(explosion,  go.transform.position,  go.transform.rotation);
		Destroy(go);
		Destroy(newexplosion, 3);
	}

	public void deleteObjectSilently(GameObject go) {
		Destroy(go);
	}

	public void toggleMinimap() {
		sfxPopupOpen.Play ();

		if (hudMinimapContainer.activeSelf) {
			hudMinimapContainer.SetActive (false);
			cameraMinimap.gameObject.SetActive (false);
		} else {
			hudMinimapContainer.SetActive (true);
			cameraMinimap.gameObject.SetActive (true);
		}
	}

	public void resizeScrollableContent(GameObject parent, GameObject child, int children, string orientation) {
		if (orientation == "landscape") {
			parent.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(children * child.gameObject.GetComponent<RectTransform> ().sizeDelta.x, 0);
		} else {
			parent.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(0, children * child.gameObject.GetComponent<RectTransform> ().sizeDelta.y);
		}
	}

	public void quit() {
		Application.Quit();
	}

	public void logout() {
		sfxLogout.Play();

		// Emit to the network
		broadcast ("player_world_switch,"+userId+",-1,"+userAvatar+","+userPlayname);

		Color myColor = new Color();
		ColorUtility.TryParseHtmlString("#E34D50", out myColor);

		boot.gameObject.SetActive (true);
		cameraHud.gameObject.transform.SetParent(cameraContainer.transform);

		boot.GetComponent<Boot> ().background.gameObject.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (307, 0), 0.75f, true).SetEase (Ease.InQuart).OnComplete (() => {
			Destroy(avatarObject);

			destroyStageObjects ("Car");
			destroyStageObjects ("Tile");
			destroyStageObjects ("Road");
			destroyStageObjects ("Building");

			boot.GetComponent<Boot>().ground.GetComponent<MeshCollider>().enabled = true;

			// Reset
			selectedObject = null;
			activeNodeObject = null;
			avatarObject = null;
			multiplayers = null;

			// Switch cameras
			cameraMain.gameObject.SetActive (false);
			cameraBoot.gameObject.SetActive (true);

			// Log it
			log("logout", 0);

			// Move the screens back
			boot.GetComponent<Boot>().playPanel.gameObject.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (313.25f, -1200f), 0.75f, true).SetEase (Ease.OutQuart);
			boot.GetComponent<Boot>().loginPanel.gameObject.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (313.25f, -422f), 0.75f, true).SetEase (Ease.OutQuart).OnComplete (() => {


			});	

		
		});
	}

	public void destroyBaseMwoObjectById(int id, string tag) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);

		for(var i = 0 ; i < gameObjects.Length ; i ++) {
			if (gameObjects [i].GetComponent<MWO.Base> () != null) {
				if (gameObjects [i].GetComponent<MWO.Base> ().id == id) {
					gameObjects [i].GetComponent<MWO.Base> ().destroyTooltips ();

					Destroy (gameObjects [i]);
				}
			}
		}
	}

	public void destroyStageObjects(string tag) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);

		for(var i = 0 ; i < gameObjects.Length ; i ++) {
			if (gameObjects [i].GetComponent<MWO.Base> () != null) {
				gameObjects [i].GetComponent<MWO.Base> ().destroyTooltips ();
			}

			Destroy (gameObjects [i]);
		}	
	}
		
	public bool IsPointerOverGameObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

		if (results.Count == 1 && results [0].gameObject.name == "Movable") {
			return true;
		} else {
			return false;
		}
	}

	// Grid, roads & traffic

	public float convertTileToCoordinate(int n) {
		return n * size;
	}

	public int convertCoordinateToTile(float c) {
		return Mathf.FloorToInt((c / size)) ;
	}

	public void createRandomCar() {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Road");

		// Load the car
		GameObject carObject = Resources.Load ("MIA/Car") as GameObject;
		GameObject instantiatedCar = Instantiate (carObject); 

		instantiatedCar.GetComponent<MWO.Car> ().createJourney ();
	}
		
	public void updateTraffic() {
		StartCoroutine (updateTrafficSync ());
	}

	IEnumerator updateTrafficSync() {
		// Look for all car objects
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Car");

		// And recreate journeys
		for(var i = 0 ; i < gameObjects.Length ; i ++) {
			gameObjects [i].GetComponent<MWO.Car> ().createJourney ();
		}

		// If there are no cars
		if (gameObjects.Length == 0) {
			
		}

		yield return true;
	}

	public void updateRoads() {
		StartCoroutine (updateRoadsSync ());
	}

	IEnumerator updateRoadsSync() {
		// Kill all the cars
		destroyStageObjects("Car");
		yield return true;

		// Notice
		createNotice("Re-routing traffic");

		// Find all the road tiles
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Road");

		// Calculate the route from the bottom to the top & then reverse
		{
			bool foundTheFirstRoadPoint = false;

			BaseGrid roadGrid = new StaticGrid(grid, grid);
			GridPos lastRoadPoint = new GridPos (0, 0);
			GridPos firstRoadPoint = new GridPos (0, 0);

			// Add the road points to the grid
			for(var i = 0 ; i < gameObjects.Length ; i ++) {
				int x = gameObjects[i].GetComponent<MWO.Base> ().x;
				int z = gameObjects[i].GetComponent<MWO.Base> ().z;

				roadGrid.SetWalkableAt(x, z, true);
			}
			yield return true;

			for (int r = 0; r < grid; r++) {
				for (int c = 0; c < grid; c++) {
					for (var i = 0; i < gameObjects.Length; i++) {
						int x = gameObjects [i].GetComponent<MWO.Base> ().x;
						int z = gameObjects [i].GetComponent<MWO.Base> ().z;

						if (x == c && z == r) {
							if (!foundTheFirstRoadPoint) {
								foundTheFirstRoadPoint = true; 

								firstRoadPoint = new GridPos (x, z);
								lastRoadPoint = new GridPos (x, z);
							} else {
								lastRoadPoint = new GridPos (x, z);
							}
						}
					}
				}
			}

			// Calculate the actual route
			JumpPointParam jpParam = new JumpPointParam(roadGrid, firstRoadPoint, lastRoadPoint, false, false, false); 
			List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
			yield return resultPathList;

			// Update the global journey list
			journeyGoingUpGrid = resultPathList;
			journeyGoingDownGrid = new List<GridPos> (resultPathList);
			journeyGoingDownGrid.Reverse ();

		}
			
		// Calculate the route from the left to right & then reverse
		{
			bool foundTheFirstRoadPoint = false;

			BaseGrid roadGrid = new StaticGrid(grid, grid);
			GridPos lastRoadPoint = new GridPos (0, 0);
			GridPos firstRoadPoint = new GridPos (0, 0);

			// Add the road points to the grid
			for(var i = 0 ; i < gameObjects.Length ; i ++) {
				int x = gameObjects[i].GetComponent<MWO.Base> ().x;
				int z = gameObjects[i].GetComponent<MWO.Base> ().z;

				roadGrid.SetWalkableAt(x, z, true);
			}
			yield return true;

			for (int c = 0; c < grid; c++) {
				for (int r = 0; r < grid; r++) {
					for (var i = 0; i < gameObjects.Length; i++) {
						int x = gameObjects [i].GetComponent<MWO.Base> ().x;
						int z = gameObjects [i].GetComponent<MWO.Base> ().z;

						if (x == c && z == r) {
							if (!foundTheFirstRoadPoint) {
								foundTheFirstRoadPoint = true; 

								firstRoadPoint = new GridPos (x, z);
								lastRoadPoint = new GridPos (x, z);
							} else {
								lastRoadPoint = new GridPos (x, z);
							}
						}
					}
				}
			}

			// Calculate the actual route
			JumpPointParam jpParam = new JumpPointParam(roadGrid, firstRoadPoint, lastRoadPoint, false, false, false); 
			List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
			yield return resultPathList;

			// Update the global journey list
			journeyGoingRightGrid = resultPathList;
			journeyGoingLeftGrid = new List<GridPos> (resultPathList);
			journeyGoingLeftGrid.Reverse ();
		}
			
		/*
		Debug.Log ("Going up start: "+journeyGoingUpGrid [0].x + "/" + journeyGoingUpGrid [0].y+" "+journeyGoingUpGrid [journeyGoingUpGrid.Count-1].x + "/" + journeyGoingUpGrid [journeyGoingUpGrid.Count-1].y);
		Debug.Log ("Going down start: "+journeyGoingDownGrid [0].x + "/" + journeyGoingDownGrid [0].y+" "+journeyGoingDownGrid [journeyGoingDownGrid.Count-1].x + "/" + journeyGoingDownGrid [journeyGoingDownGrid.Count-1].y);
		Debug.Log ("Going right start: "+journeyGoingRightGrid [0].x + "/" + journeyGoingRightGrid [0].y+" "+journeyGoingRightGrid [journeyGoingRightGrid.Count-1].x + "/" + journeyGoingRightGrid [journeyGoingRightGrid.Count-1].y);
		Debug.Log ("Going left start: "+journeyGoingLeftGrid [0].x + "/" + journeyGoingLeftGrid [0].y+" "+journeyGoingLeftGrid [journeyGoingLeftGrid.Count-1].x + "/" + journeyGoingLeftGrid [journeyGoingLeftGrid.Count-1].y);
		*/

		for (int c = 0; c < journeyGoingUpGrid.Count; c++) {
			createRandomCar ();
			yield return new WaitForSeconds (1);
		}

		// All done
		yield return true;
	}

	public GridPos generateRandomRoadPoint() {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Road");

		int r = Mathf.FloorToInt (UnityEngine.Random.Range (0, gameObjects.Length - 1));
		int x = convertCoordinateToTile (gameObjects [r].transform.position.x);
		int z = convertCoordinateToTile (gameObjects [r].transform.position.z);

		return new GridPos(x, z); 
	}

	// Player specific

	public void savePlayer() {
		// StartCoroutine (savePlayerSync ());
	}

	IEnumerator savePlayerSync() {
		// Save player 
		WWWForm form = new WWWForm();
		form.AddField ("id", userId+"");
		form.AddField ("fullname", userFullname+"");
		form.AddField ("playname", userPlayname+"");
		form.AddField ("email", userEmail+"");
		form.AddField ("password", userPassword+"");
		form.AddField ("points", userPoints+"");
		form.AddField ("role", userRole+"");
		form.AddField ("avatar", userAvatar+"");
		form.AddField ("badges", String.Join(",", userBadges.ToArray().Select(p=>p.ToString()).ToArray())+"");
		form.AddField ("nodes", String.Join(",", userNodes.ToArray().Select(p=>p.ToString()).ToArray())+"");
		form.AddField ("paths", String.Join(",", userPaths.ToArray().Select(p=>p.ToString()).ToArray())+"");

		// Make the request
		if (!editMode) {
			WWW www = new WWW (api_path + "/player/update", form);
			yield return www;
		} else {
			yield return true;
		}
	}
		
	public void lookForNextNodeInPath(int pid) {
		bool foundNode = false;
		bool foundActiveNodeObject = false;

		// Iterate over the nodes
		for (int c = 0; c < nodes.Count; c++) {
			int nodePathId = nodes [c] ["path"].AsInt;
			int nodeId = nodes [c] ["id"].AsInt;

			// Focus on nodes for this path
			if (pid == nodePathId) {
				if (userNodes.IndexOf (nodeId) == -1) {
					if (foundNode == false) {
						// Make this the active node
						activeNode = nodeId;
						activePath = pid;

						// We found one, stop looking
						foundNode = true;

						// Get the path title for this node & update the UI
						for (int d = 0; d < paths.Count; d++) {
							if (paths [d] ["id"].AsInt == activePath) {
								string upperCased = paths [d] ["title"];

								hudCurrentNodeContainer.GetComponent<Header.CurrentPath> ().uiPath.text = upperCased.ToUpper ();
							}
						}

						// Get the node title for this node & update the UI
						for (int f = 0; f < nodes.Count; f++) {
							if (nodes [f] ["id"].AsInt == activeNode) {
								hudCurrentNodeContainer.GetComponent<Header.CurrentPath> ().uiNode.text = nodes [f] ["title"];
							}
						}

						// Look for objects on stage that point to this node for BUILDINGS
						GameObject[] buildingObjects = GameObject.FindGameObjectsWithTag("Building");
						for (int o = 0; o < buildingObjects.Length; o++) {
							if (buildingObjects [o].GetComponent<MWO.Base> ().node == nodeId) {
								foundActiveNodeObject = true;
							}
						}
					}
				}
			}
		}

		// Tell the user their objective isn't here
		if (!foundActiveNodeObject) { 
			createAlert ("Exploration", "The next exercise is not on this level. Explore other levels to find it!");
		}

		// Decide what to do with the top left UI
		if (!foundNode) {
			hudCurrentNodeContainer.GetComponent<Header.CurrentPath> ().hide ();
			createBigNotice ("Congrats", "You've finished this mission! You've played well. Now move on to the next one.");
			sfxPathComplete.Play ();

			activePath = -1;
			activeNode = -1;
			activeNodeObject = null;
		} else {
			hudCurrentNodeContainer.GetComponent<Header.CurrentPath> ().show ();
		}
	}

	public void addUserNode(int nid, int pid, int points) {
		// Add the node
		userNodes.Add (nid);

		// Add the points
		userPoints = userPoints + points;

		// Add the path if it's not there
		if (userPaths.IndexOf (pid) == -1) {
			userPaths.Add (pid);
		}

		// Add the badge if not already
		for (int c = 0; c < badges.Count; c++) {
			if (badges [c] ["node"].AsInt == nid) {
				if (userBadges.IndexOf (badges [c] ["id"].AsInt) == -1) {
					userBadges.Add (badges [c] ["id"].AsInt);

					// Log
					log("badge", badges [c] ["id"].AsInt);

					createNotice(badges [c] ["title"]);

					// Points
					userPoints += badges [c] ["points"].AsInt;
				}
			}
		}

		// Log
		log("node_complete", nid);

		// Mark the UI of the current node object as checked or not
		currentNodeObject.GetComponent<MWO.Base> ().nodeTooltip.GetComponent<Movable.Node> ().markAsCompleted ();

		// Look for the next node
		lookForNextNodeInPath (pid);

		// Save the player
		savePlayer();
	}

	// Scene management

	public void unloadWindMiniGame() {
		SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("Wind"));

		// Lift the volume
		gameObject.GetComponent<AudioSource>().DOFade(0.5f, 1.0f);

		avatarObject.SetActive (true);
		play.gameObject.SetActive (true);
		landscape.SetActive (true);
	}

	public void loadWindMiniGame() {
		SceneManager.LoadScene ("Wind", LoadSceneMode.Additive);

		gameObject.GetComponent<AudioSource>().DOFade (0.1f, 3.0f);

		avatarObject.SetActive (false);
		play.gameObject.SetActive (false);
		landscape.SetActive (false);
	}

	public int getUserNodeCountForPath(int p) {
		int count = 0;

		for (int c = 0; c < nodes.Count; c++) {
			if (nodes [c] ["path"].AsInt == p) {
				if (userNodes.IndexOf (nodes [c] ["id"].AsInt) != -1) {
					count++;
				}
			}
		}

		return count;
	}

	public int getNodeCountForPath(int p) {
		int count = 0;

		for (int c = 0; c < nodes.Count; c++) {
			if (nodes [c] ["path"].AsInt == p) {
				count++;
			}
		}

		return count;
	}

	public string getNodeTitle(int n) {
		string t = "General";

		for (int c = 0; c < nodes.Count; c++) {
			if (nodes [c] ["id"].AsInt == n) {
				return nodes [c]["title"];
			}
		}

		return t;
	}
}
