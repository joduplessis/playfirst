using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DG.Tweening;

namespace PopupMain {
	public class Path : MonoBehaviour {
		
		private GameManager gm;
		private float spacing = 60f;
		private bool nodesHaveLoaded = false;

		[Header("Path")]
		public int id;
		public string title;
		public string description;

		[Header("UI")]
		public Text uiTitle;
		public Text uiDescription;

		[Header("Nodes")]
		public GameObject nodesPanel;
		public Text activeButtonText;
		public GameObject nodeBlueprint;
		public GameObject nodeContainer;
		public Text exercises;
		public GameObject progress;

		private int excercisesCompleted;
		private int totalExercises;
		private float excercisesCompletedPercentage;

		void Start () {}		

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

			nodesPanel.gameObject.SetActive (false);
		}
			
		void Update () {
			totalExercises = gm.getNodeCountForPath (id);

			if (totalExercises != 0) {
				excercisesCompleted = gm.getUserNodeCountForPath (id);
				excercisesCompletedPercentage = excercisesCompleted / totalExercises;

				exercises.text = (excercisesCompleted==1) ? excercisesCompleted + " exercise completed so far." : excercisesCompleted + " exercises completed so far.";
				progress.gameObject.GetComponent<RectTransform> ().localScale = new Vector3 (excercisesCompletedPercentage, 1.0f, 1.0f);
			} else {
				exercises.text = "No exercises for this course.";
				progress.gameObject.GetComponent<RectTransform> ().localScale = new Vector3 (0f, 1.0f, 1.0f);
			}
		}

		public void hideNodes() {
			nodesPanel.gameObject.SetActive (false);
		}

		public void showNodes() {
			foreach (Transform child in nodeContainer.transform) {
				if (child.gameObject.activeSelf && child.gameObject!=nodeBlueprint.gameObject) {
					GameObject.Destroy(child.gameObject);
				}
			}	

			nodesPanel.gameObject.SetActive (true);
			nodeBlueprint.gameObject.SetActive (false);
		
			gm.sfxPopupOpen.Play ();

			int inc = 0;

			for (int c = 0; c < gm.nodes.Count; c++) {
				int nodePathID = gm.nodes [c] ["path"].AsInt;
				int nodeID = gm.nodes [c] ["id"].AsInt;

				if (id == nodePathID) {
					GameObject n = Instantiate (nodeBlueprint);

					n.gameObject.SetActive (true);
					n.gameObject.transform.SetParent(nodeContainer.gameObject.transform);
					n.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, (inc * spacing * -1));
					n.gameObject.GetComponent<PopupMain.Node> ().title.text = gm.nodes [c] ["title"];
					n.gameObject.GetComponent<PopupMain.Node> ().id = nodeID;
					n.gameObject.GetComponent<PopupMain.Node> ().init ();

					inc++;
				}
			}

			gm.resizeScrollableContent (nodeContainer, nodeBlueprint, inc, "portrait");
		}

		public void makePathActive() {
			if (gm.userPaths.IndexOf (id) == -1) {
				// Assign the path to be active
				gm.activePath = id;

				// Get the node
				gm.lookForNextNodeInPath (id);

				// Log
				gm.log ("path_activate", id);
			}
		}
	
		public void init() {
			uiTitle.text = title;
			uiDescription.text = description;

			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}

	}
}