using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MWO {
	public class Tile : MonoBehaviour {
		private GameManager gm;


		public float y;
		public float x;

		void Start () {}

		void Awake() {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
		}

		public void OnMouseDown() {
			// Debug.Log ("(Tile) x: " + x + ", y: " + y);

			if (gm.objectToPlace != null && gm.IsPointerOverGameObject()) {
				Color myColor = new Color();
				ColorUtility.TryParseHtmlString("#aa0000", out myColor);

				gameObject.GetComponent<Renderer> ().material.color = myColor;
				gm.objectToPlace.transform.DOMove(gameObject.transform.position, 0.25f, true).SetEase (Ease.InBack);
			}
		}

		public void OnMouseUp() {
			if (gm.objectToPlace != null && gm.IsPointerOverGameObject ()) {
				Color myColor = new Color ();
				ColorUtility.TryParseHtmlString ("#5B722F", out myColor);

				gameObject.GetComponent<Renderer> ().material.color = myColor;
			}		
		}

		public void OnMouseOver() {

		}
	}
}
