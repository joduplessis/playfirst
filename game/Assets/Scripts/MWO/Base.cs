using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using SimpleJSON;
using System.Collections.Generic;
using DG.Tweening;

namespace MWO {
	public class Base : MonoBehaviour {

		public int x = 0;
		public int z = 0;

		[Header("Properties")]
		public int id;
		public int user;
		public int world;
		public string prefab;
		public string name;
		public int node;
		public int portal;
		public float progress;

		[Header("Tooltips")]
		public GameObject nodeTooltip;
		public GameObject portalTooltip;
		public GameObject pointsTooltip;
		public GameObject progressTooltip;

		[Header("Game time")]
		public bool placed;
		public int generatedPoints;			// Total current points generated
		public float pointsTicker; 			// Timer that keeps ticking for points

		private GameManager gm;

		private RaycastHit hit;
		private Vector3 screenPoint;
		private Vector3 offset;
		private Vector3 startingPosition;

		[Header("Building & generating")]
		private int build_points;
		private int build_time;
		private int generate_time;
		private int generate_points;

		private bool selected;
		private bool isNotBuiltYet;

		void Start () {}

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			// Assign the node object for the guide MOVABLE
			if (gm.activeNode != null && gm.activeNode != 0) {
				if (node == gm.activeNode) {
					gm.activeNodeObject = gameObject; 
				}
			}

			// Here we generate the points
			if (placed && progress >= 100 && pointsTicker != null) {
				pointsTicker += Time.deltaTime * generate_time;

				if (pointsTicker >= 100) {
					generatedPoints += Mathf.FloorToInt(generate_points);
					setPoints ();
					pointsTicker = 0;
				}
			}

			// Here we generate the build process
			if (progress != null && placed) {
				if (progress < 100) {
					progress += Time.deltaTime * build_time;

					if (progressTooltip != null) {
						progressTooltip.GetComponent<Movable.Progress> ().progress = progress;
					}
				} else {
					if (isNotBuiltYet) {	
						// Set it back so it only runs once
						isNotBuiltYet = false;

						// Assign the appropriate material
						setMaterial();

						// Set tooltips
						destroyTooltips ();
						setTooltips();

						// Play the animation
						shakeObject ();

						// Play the sound
						gm.sfxBuildingComplete.Play ();

						// Remap the roads
						gm.updateRoads ();

						// Save the object
						saveObject();
					}
				}
			}
		}
			
		// Touch actions

		void OnMouseUp() {
		}

		void OnMouseDown(){
			if (gm.IsPointerOverGameObject ()) {
				// If there's no selected object & the user owns this object
				if ((gm.userId == user) && (gm.selectedObject==null) && (gm.selectedOtherPlayerObject == null)) {
					gm.sfxObjectSelect.Play ();
					gm.selectedObject = gameObject;
					gm.hudSelectedToolsContainer.gameObject.SetActive (true);
					setSelectedMaterial ();

					if (placed) {
						if (selected) {
							selected = false;
						} else {
							selected = true;
						}

						if (selected) {
							setSelectedMaterial ();
						} else {
							if (progress < 100) {
								setBuildingMaterial ();
							} else {
								setNormalMaterial ();
							}
						}
					}

					screenPoint = gm.cameraMain.WorldToScreenPoint (gameObject.transform.position);
					startingPosition = gameObject.transform.position;
					offset = startingPosition - gm.cameraMain.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
					shakeObject ();
				}

				// If there's no other player selected object and the user doesn't this object
				if ((gm.selectedOtherPlayerObject == null) && (gm.userId != user) && (gm.selectedObject==null)) {
					gm.selectedOtherPlayerObject = gameObject;
					setSelectedMaterial ();
					shakeObject ();
					gm.sfxObjectSelect.Play ();
				}
			}
		}
	
		// Once off setup of the object
			
		public void init() {
			// Get the build time
			for (int c = 0; c < gm.assets.Count; c++) {
				string prefabName = gm.assets [c] ["prefab"];

				if (prefabName == prefab) {
					build_time = gm.assets [c] ["build_time"].AsInt;
					build_points = gm.assets [c] ["build_points"].AsInt;
					generate_points = gm.assets [c] ["generate_points"].AsInt;
					generate_time = gm.assets [c] ["generate_points"].AsInt;
				}
			}

			// Not selected
			selected = false;
			generatedPoints = 0;
			pointsTicker = 0;

			// If this object is not built yet
			if (progress < 100) {
				isNotBuiltYet = true;
			} else {
				isNotBuiltYet = false;
			}
				
			// If it's not placed
			if (!placed) {
				gm.selectedObject = gameObject;
				gm.objectToPlace = gameObject;
				gm.hudSelectedToolsContainer.gameObject.SetActive (true);
				gm.createNotice ("Place your object!");
				selected = true;
			}

			// Set the material & tooltips
			setMaterial();
			destroyTooltips ();
			setTooltips();
		}
			
		// General actions

		public void deselect() {
			if (!placed) {
				placed = true;

				setTooltips ();
			}

			selected = false;
			gm.selectedObject = null;
			gm.selectedOtherPlayerObject = null;
			gm.objectToPlace = null;
			gm.hudSelectedToolsContainer.gameObject.SetActive (false);

			setMaterial ();
		}

		public void collectPoints() {
			if (generatedPoints!=null && generatedPoints!=0) {
				gm.sfxMoneyCollect.Play ();
				gm.createNotice ("Points collected");
				gm.userPoints += generatedPoints;
				generatedPoints = 0;
				pointsTicker = 0;
				Destroy (pointsTooltip);
				pointsTooltip = null;
			} else {
				gm.createNotice("No points yet!");
			}

		}
	
		public void save() {
			StartCoroutine (saveMwo());
		}
			
		IEnumerator saveMwo() {
			WWWForm form = new WWWForm();
			form.AddField ("id", id+"");
			form.AddField ("user", user);
			form.AddField("world", world);
			form.AddField("prefab", prefab);
			form.AddField("name", name);
			form.AddField("x", transform.position.x+"");
			form.AddField("y", transform.position.y+"");
			form.AddField("z", transform.position.z+"");
			form.AddField("rotation", transform.localRotation.eulerAngles.y+"");
			form.AddField("node", node+"");
			form.AddField("portal", portal+"");
			form.AddField("progress", progress+"");

			// Make the request
			WWW www = new WWW(gm.api_path+"/mwo/update", form);
			yield return www;

			// Deselect the material
			deselect();
		}
	
		// Materials

		public void setMaterial() {
			if (selected) {
				setSelectedMaterial ();
			} else {
				if (progress < 100) {
					setBuildingMaterial ();
				} else {
					setNormalMaterial ();
				}
			}
		}

		public void setSelectedMaterial() {
			foreach(Transform child in gameObject.transform) {
				if (child.gameObject.GetComponent<Renderer> () != null) {
					foreach (Material childMaterial in child.gameObject.GetComponent<Renderer> ().materials) {
						childMaterial.shader = gm.shaderSelected;
					}
				}
			}
		}

		public void setNormalMaterial() {
			foreach(Transform child in gameObject.transform) {
				if (child.gameObject.GetComponent<Renderer> () != null) {
					foreach (Material childMaterial in child.gameObject.GetComponent<Renderer> ().materials) {
						childMaterial.shader = gm.shaderNormal;
					}
				}
			}
		}

		public void setBuildingMaterial() {
			foreach(Transform child in gameObject.transform) {
				if (child.gameObject.GetComponent<Renderer> () != null) {
					foreach (Material childMaterial in child.gameObject.GetComponent<Renderer> ().materials) {
						childMaterial.shader = gm.shaderBuilding;
					}
				}
			}
		}

		// Tooltips

		public bool destroyTooltips() {
			if (nodeTooltip != null) {
				Destroy (nodeTooltip);
			}
			if (portalTooltip != null) {
				Destroy (portalTooltip);
			}
			if (pointsTooltip != null) {
				Destroy (pointsTooltip);
			}
			if (progressTooltip != null) {
				Destroy (progressTooltip);
			}

			return true;
		}

		public void setTooltips() {
			if (placed) {
				setProgress ();
				setNode (node);
				setPortal (portal);
				setPoints ();
			}
		}

		public void setPortal(int p) {
			portal = p;

			if (portalTooltip != null) {
				Destroy (portalTooltip);
			}

			if (p != 0 && p != null && progress >=100) {
				GameObject t = Instantiate (gm.portalTooltipBlueprint);

				// Set the ititial position
				t.gameObject.transform.position = gm.cameraMain.WorldToScreenPoint (transform.position);

				t.gameObject.SetActive (true);
				t.gameObject.transform.SetParent(gm.movableTooltipContainer.gameObject.transform);
				t.gameObject.GetComponent<Movable.Portal> ().mwo = gameObject;
				t.gameObject.GetComponent<Movable.Portal> ().id = portal;
				t.gameObject.GetComponent<Movable.Portal> ().init ();

				portalTooltip = t.gameObject;
			}
		} 

		public void setNode(int n) {
			node = n;

			// If the tooltip exists, remove it
			if (nodeTooltip != null) {
				Destroy (nodeTooltip);
			}

			if (node != 0 && node != null && progress >=100) {
				// Get the node
				for (int c = 0; c < gm.nodes.Count; c++) {
					if (gm.nodes [c] ["id"].AsInt == node) {
						GameObject t = Instantiate (gm.nodeTooltipBlueprint);

						t.gameObject.transform.position = gm.cameraMain.WorldToScreenPoint (transform.position);
						t.gameObject.SetActive (true);
						t.gameObject.transform.SetParent(gm.movableTooltipContainer.gameObject.transform);
						t.gameObject.GetComponent<Movable.Node> ().init(gameObject, node);

						nodeTooltip = t.gameObject;
					}
				}
			}
		}

		public void setPoints() {
			if (pointsTooltip != null) {
				Destroy (pointsTooltip);
			}

			if (gameObject.tag != "Road") {
				if (generatedPoints != 0 && generatedPoints != null && progress >= 100) {
					GameObject t = Instantiate (gm.pointsTooltipBlueprint);

					// Set the ititial position
					t.gameObject.transform.position = gm.cameraMain.WorldToScreenPoint (transform.position);

					t.gameObject.SetActive (true);
					t.gameObject.transform.SetParent(gm.movableTooltipContainer.gameObject.transform);
					t.gameObject.GetComponent<Movable.Points> ().mwo = gameObject;
					t.gameObject.GetComponent<Movable.Points> ().points = generatedPoints;
					t.gameObject.GetComponent<Movable.Points> ().init ();

					pointsTooltip = t.gameObject;
				}
			}
		}

		public void setProgress() {
			if (progressTooltip != null) {
				Destroy (progressTooltip);
			}

			if (progress < 100) {
				GameObject t = Instantiate (gm.progressTooltipBlueprint);

				// Set the ititial position
				t.gameObject.transform.position = gm.cameraMain.WorldToScreenPoint (transform.position);

				t.gameObject.SetActive (true);
				t.gameObject.transform.SetParent(gm.movableTooltipContainer.gameObject.transform);
				t.gameObject.GetComponent<Movable.Progress> ().mwo = gameObject;
				t.gameObject.GetComponent<Movable.Progress> ().progress = progress;

				progressTooltip = t.gameObject;
			}
		}

		// General

		public void shakeObject() {
			transform.DOShakeScale(0.5f, 1f, 10, 45f, true);
		}
	
		public void saveObject() {
			StartCoroutine (saveObjectSync ());
		}

		IEnumerator saveObjectSync() {
			WWWForm form = new WWWForm();
			form.AddField ("id", id+"");
			form.AddField ("user", user+"");
			form.AddField ("world", world+"");
			form.AddField ("prefab", prefab+"");
			form.AddField ("name", name+"");
			form.AddField ("x", gameObject.transform.position.x+"");
			form.AddField ("y", gameObject.transform.position.y+"");
			form.AddField ("z", gameObject.transform.position.z+"");
			form.AddField ("rotation", gameObject.transform.localRotation.eulerAngles.y+"");
			form.AddField ("node", node+"");
			form.AddField ("portal", portal+"");
			form.AddField ("progress", progress+"");

			// Make the request
			WWW www = new WWW(gm.api_path+"/mwo/update", form);
			yield return www;
		}
	}
}
