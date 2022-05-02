using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigNotice : MonoBehaviour {

	public Text heading;
	public Text description;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void show() {
		gameObject.SetActive (true);
	}

	public void hide() {
		gameObject.SetActive (false);
	}

	public void init(string h, string d) {
		heading.text = h;
		description.text = d;
		show ();
	}
}
