using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Alert : MonoBehaviour {

	public Text title;
	public Text description;


	void Start () {
	}

	void Update () {
	}

	public void show() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void hide() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -700), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void init(string heading, string text) {
		title.text = heading;
		description.text = text;
		show ();
	}
}
