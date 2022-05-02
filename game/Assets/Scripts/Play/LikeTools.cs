using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LikeTools : MonoBehaviour {

	private GameManager gm;

	public Text heading;

	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
	}

	void Update () {
		if (gm.selectedOtherPlayerObject != null && gm!=null) {
			Vector3 mwoPosition = gm.cameraMain.WorldToScreenPoint (gm.selectedOtherPlayerObject.transform.position);
			Vector3 origin = gameObject.transform.position;
			Vector3 destination = new Vector3 (mwoPosition.x, mwoPosition.y - 175, mwoPosition.z);

			gameObject.transform.position = Vector3.Lerp (origin, destination, 15.0f * Time.deltaTime);

			heading.text = gm.selectedOtherPlayerObject.GetComponent<MWO.Base> ().name;
		} else {
			gameObject.transform.position = new Vector3 (20000, 20000, 20000); // Off the screen
		}
	}

	public void deselectSelectedObject() {
		gm.selectedOtherPlayerObject.GetComponent<MWO.Base> ().deselect ();
	}

	public void show() {
		gameObject.SetActive (true);
	}

	public void hide() {
		gameObject.SetActive (false);
	}

	public void like() {
		gm.createNotice ("You liked this!");
		gm.log("like", gm.selectedOtherPlayerObject.GetComponent<MWO.Base>().id);
		gm.selectedOtherPlayerObject.GetComponent<MWO.Base> ().deselect ();
	}
		
}
