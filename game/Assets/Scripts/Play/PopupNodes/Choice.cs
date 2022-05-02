using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PopupNodes {
	public class Choice : MonoBehaviour {

		public bool correctAnswer;
		public Text title;
		public GameObject parentWindow;

		void Start () {
			
		}

		void Awake() {
		}

		void Update () {
		}


		public void init() {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			gameObject.GetComponent<RectTransform>().localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}

		public void isCorrect() {
			if (correctAnswer) {
				parentWindow.GetComponent<PopupNodes.Choices> ().correctAnswer ();
			} else {
				parentWindow.GetComponent<PopupNodes.Choices> ().incorrectAnswer ();
			}
		}
	}
}
