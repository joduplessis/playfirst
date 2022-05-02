using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Logout : MonoBehaviour {

	private GameManager gm;

	void Start () {
		gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
	}		

	public void show() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void hide() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -700), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void logout() {
		hide ();
		gm.logout ();
	}

	void Update () {
	}
}
