using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Header {
	public class CurrentPath : MonoBehaviour {

		private GameManager gm;

		public Text uiNode;
		public Text uiPath;

		public GameObject node;
		public GameObject path;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
		}

		public void show() {
			node.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-0.5f, -69), 0.75f, true).SetEase (Ease.OutBack);
			path.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (0.5f, -169), 1.0f, true).SetEase (Ease.OutBack);
		}

		public void hide() {
			node.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (0, -280), 1.25f, true).SetEase (Ease.OutBack);
			path.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (0, -280), 0.75f, true).SetEase (Ease.OutBack);
		}
	}
}