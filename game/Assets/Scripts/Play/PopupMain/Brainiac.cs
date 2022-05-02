using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class Brainiac : MonoBehaviour {

	private GameManager gm;

	public GameObject paths;
	public GameObject achievements;
	public GameObject activity;
	public GameObject leaderboard;
	public GameObject notice;

	public Text subheading;

	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();

		gameObject.SetActive (false);
	}

	void Awake() {
	}

	public void hide() {
		gameObject.SetActive (false);
		gm.sfxPopupClose.Play ();
	}
	
	public void show() {
		gameObject.SetActive (true); 
		gm.sfxPopupOpen.Play ();
		subheading.text = "";
	}

	public void showPaths() {
		showPanel ("paths");
	}

	public void showAchievements() {
		showPanel ("achievements");
	}

	public void showActivity() {
		showPanel ("activity");
	}

	public void showLeaderboard() {
		showPanel ("leaderboard");
	}

	public void showPanel(string panel) {
		subheading.text = "";
		notice.gameObject.SetActive (false);

		if (activity.gameObject.activeSelf && panel != "activity") {
			activity.gameObject.GetComponent<PopupMain.Activity> ().hide ();
		}
		if (paths.gameObject.activeSelf && panel != "paths") {
			paths.gameObject.GetComponent<PopupMain.Paths> ().hide ();
		}
		if (achievements.gameObject.activeSelf && panel != "achievements") {
			achievements.gameObject.GetComponent<PopupMain.Badges> ().hide ();
		}
		if (leaderboard.gameObject.activeSelf && panel != "leaderboard") {
			leaderboard.gameObject.GetComponent<PopupMain.Leaderboard> ().hide ();
		}

		switch (panel) {
		case "activity":
			activity.gameObject.GetComponent<PopupMain.Activity> ().show ();
			subheading.text = "Recent activity";
			break;
		case "achievements":
			achievements.gameObject.GetComponent<PopupMain.Badges> ().show ();
			subheading.text = "Achievements";
			break;
		case "paths":
			paths.gameObject.GetComponent<PopupMain.Paths> ().show ();
			subheading.text = "Courses";
			break;
		case "leaderboard":
			leaderboard.gameObject.GetComponent<PopupMain.Leaderboard> ().show ();
			subheading.text = "Leaderboard";
			break;
		}
	}
}
