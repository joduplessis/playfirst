using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class NodeDetail : MonoBehaviour {

	public Text path;
	public Text node;
	public Text points;
	public GameObject nodeMixed;	
	public GameObject nodeChoices;
	public GameObject nodeImages;
	public GameObject nodeVideo;

	private GameManager gm;
	private int id;

	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
	}		
		
	void Update () {}

	private int getPathForNode(int n) {
		int p = -1;
		for (int c = 0; c < gm.nodes.Count; c++) {
			if (gm.nodes [c] ["id"].AsInt == id) {
				p = gm.nodes [c] ["path"].AsInt;
			}
		}
		return p;
	}

	private int requirementPathForPath(int p) {
		int rp = -1;
		for (int pa = 0; pa < gm.paths.Count; pa++) {
			if (p == gm.paths [pa] ["id"].AsInt) {
				if (gm.paths [pa] ["requirement"].AsInt!=null) {
					rp = gm.paths [pa] ["requirement"].AsInt;
				}
			}
		}
		return rp;
	}

	public void launchNode() {
		hide (); 

		bool userHasAllRequiredPaths = true;
		int pathForThisNode = getPathForNode (id);
		int requirementPathForThisPath = requirementPathForPath(pathForThisNode);

		// If there is no requirement path
		if (requirementPathForThisPath != -1) {
			for (int n = 0; n < gm.nodes.Count; n++) {
				if (gm.nodes [n] ["path"].AsInt == requirementPathForThisPath) {
					if (gm.userNodes.IndexOf (gm.nodes [n] ["id"].AsInt) == -1) {
						userHasAllRequiredPaths = false;
					}
				}
			}
		}

		if (userHasAllRequiredPaths) {
			for (int c = 0; c < gm.nodes.Count; c++) {
				if (gm.nodes [c] ["id"].AsInt == id) {
					if (gm.userNodes.IndexOf (id) == -1) {

						string t = gm.nodes [c] ["type"];

						if (t == "mixed") {
							nodeMixed.GetComponent<PopupNodes.Mixed> ().id = gm.nodes [c] ["id"].AsInt;
							nodeMixed.GetComponent<PopupNodes.Mixed> ().points = gm.nodes [c] ["points"].AsInt;
							nodeMixed.GetComponent<PopupNodes.Mixed> ().path = gm.nodes [c] ["path"].AsInt;
							nodeMixed.GetComponent<PopupNodes.Mixed> ().title = gm.nodes [c] ["title"];
							nodeMixed.GetComponent<PopupNodes.Mixed> ().type = gm.nodes [c] ["type"];
							nodeMixed.GetComponent<PopupNodes.Mixed> ().content = gm.nodes [c] ["content"];

							nodeMixed.GetComponent<PopupNodes.Mixed> ().init ();
						}

						if (t == "choices") {
							nodeChoices.GetComponent<PopupNodes.Choices> ().id = gm.nodes [c] ["id"].AsInt;
							nodeChoices.GetComponent<PopupNodes.Choices> ().points = gm.nodes [c] ["points"].AsInt;
							nodeChoices.GetComponent<PopupNodes.Choices> ().path = gm.nodes [c] ["path"].AsInt;
							nodeChoices.GetComponent<PopupNodes.Choices> ().title = gm.nodes [c] ["title"];
							nodeChoices.GetComponent<PopupNodes.Choices> ().type = gm.nodes [c] ["type"];
							nodeChoices.GetComponent<PopupNodes.Choices> ().content = gm.nodes [c] ["content"];

							nodeChoices.GetComponent<PopupNodes.Choices> ().init ();
						}

						if (t == "images") {
							nodeImages.GetComponent<PopupNodes.Images> ().id = gm.nodes [c] ["id"].AsInt;
							nodeImages.GetComponent<PopupNodes.Images> ().points = gm.nodes [c] ["points"].AsInt;
							nodeImages.GetComponent<PopupNodes.Images> ().path = gm.nodes [c] ["path"].AsInt;
							nodeImages.GetComponent<PopupNodes.Images> ().title = gm.nodes [c] ["title"];
							nodeImages.GetComponent<PopupNodes.Images> ().type = gm.nodes [c] ["type"];
							nodeImages.GetComponent<PopupNodes.Images> ().content = gm.nodes [c] ["content"];

							nodeImages.GetComponent<PopupNodes.Images> ().init ();
						}

						if (t == "video") {
							nodeVideo.GetComponent<PopupNodes.Video> ().id = gm.nodes [c] ["id"].AsInt;
							nodeVideo.GetComponent<PopupNodes.Video> ().points = gm.nodes [c] ["points"].AsInt;
							nodeVideo.GetComponent<PopupNodes.Video> ().path = gm.nodes [c] ["path"].AsInt;
							nodeVideo.GetComponent<PopupNodes.Video> ().title = gm.nodes [c] ["title"];
							nodeVideo.GetComponent<PopupNodes.Video> ().type = gm.nodes [c] ["type"];
							nodeVideo.GetComponent<PopupNodes.Video> ().content = gm.nodes [c] ["content"];

							nodeVideo.GetComponent<PopupNodes.Video> ().init ();
						}

						if (t == "wind") {
							PlayerPrefs.SetInt("node", gm.nodes [c] ["id"].AsInt);
							PlayerPrefs.SetInt("path", gm.nodes [c] ["path"].AsInt);
							PlayerPrefs.SetInt ("points", gm.nodes [c] ["points"].AsInt);
							PlayerPrefs.SetFloat("speed", float.Parse (gm.nodes [c] ["content"]));

							gm.loadWindMiniGame ();
						}
					} else {
						gm.createAlert ("Well done", "You've already completed this module!");
					}
				}
			}
		} else {
			gm.createAlert ("Whoops", "You can't do this mission yet!");
		}
	}

	public void show() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void hide() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -700), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void init(int nid) {
		gm.sfxNodeSelect.Play ();

		id = nid;

		// Get the node text
		for (int c = 0; c < gm.nodes.Count; c++) {
			if (gm.nodes [c] ["id"].AsInt == id) {
				string title = gm.nodes [c] ["title"];

				if (title.Length > 50) {
					node.text = title.Substring(0, 50)+" ...";
				} else {
					node.text = title;
				}

				points.text = gm.nodes [c] ["points"];

				// Get the path for the node
				for (int d = 0; d < gm.paths.Count; d++) {
					if (gm.paths [d] ["id"].AsInt == gm.nodes [c] ["path"].AsInt) {
						path.text = gm.paths [d] ["title"];
					}
				}	
			}
		}

		show ();
	}

}
