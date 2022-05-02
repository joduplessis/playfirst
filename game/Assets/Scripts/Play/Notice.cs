using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Notice : MonoBehaviour {

	public Text title;

	void Start () {
	}

	void Update () {
	}

	public void show() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-190, 0), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void hide() {
		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(190, 0), 1.0f, true).SetEase(Ease.InOutBack);
	}

	public void init(string text) {
		StartCoroutine(createNoticeEnum(text));
	}

	IEnumerator createNoticeEnum(string text) {
		title.text = text;
		show ();
		yield return new WaitForSeconds(3);
		hide ();
	}
}
