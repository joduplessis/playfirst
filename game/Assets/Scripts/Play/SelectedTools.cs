using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedTools : MonoBehaviour {

	private GameManager gm;

	public Text heading;

	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
	}

	void Update () {
		if (gm.selectedObject != null && gm!=null) {
			Vector3 mwoPosition = gm.cameraMain.WorldToScreenPoint (gm.selectedObject.transform.position);
			Vector3 origin = gameObject.transform.position;
			Vector3 destination = new Vector3 (mwoPosition.x, mwoPosition.y - 175, mwoPosition.z);

			gameObject.transform.position = Vector3.Lerp (origin, destination, 15.0f * Time.deltaTime);

			heading.text = gm.selectedObject.GetComponent<MWO.Base> ().name;
		} else {
			gameObject.transform.position = new Vector3 (20000, 20000, 20000); // Off the screen
		}
	}

	public void deselectSelectedObject() {
		gm.selectedObject.GetComponent<MWO.Base> ().save ();
	}

	public void deleteSelectedObject() {
		StartCoroutine (deleteSelectedObjectApi ());
	}

	IEnumerator deleteSelectedObjectApi() {
		WWWForm form = new WWWForm();
		form.AddField ("id", gm.selectedObject.GetComponent<MWO.Base>().id+"");

		WWW www = new WWW(gm.api_path+"/mwo/delete", form);
		yield return www;

		// Kill all the UI tooltips
		bool tooltipsDestroyed = gm.selectedObject.GetComponent<MWO.Base> ().destroyTooltips ();
		yield return tooltipsDestroyed;

		// Emit this to the network
		gm.broadcast("object_destroy,"+gm.userId+","+gm.selectedObject.GetComponent<MWO.Base>().id+","+gm.world["id"]);

		// Kill the object
		gm.deleteObject (gm.selectedObject);

		// Reset the selected object
		gm.selectedObject = null;

		// Remap the roads
		gm.updateRoads ();

		// Hide this
		hide ();
	}

	public void rotateObject() {
		gm.sfxObjectRotate.Play ();
		float angle = gm.selectedObject.transform.localRotation.eulerAngles.y+90f;
		gm.selectedObject.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
	}

	public void show() {
		gameObject.SetActive (true);
	}

	public void hide() {
		gameObject.SetActive (false);
	}
		
	public void collect() {
		gm.selectedObject.GetComponent<MWO.Base> ().collectPoints ();
	}
}
