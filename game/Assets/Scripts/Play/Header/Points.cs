using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Header {
	public class Points : MonoBehaviour {

		private GameManager gm;

		public Text uiPoints;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (gm.userPoints != null) {
				uiPoints.text = gm.userPoints.ToString();
			}
		}
	}
}