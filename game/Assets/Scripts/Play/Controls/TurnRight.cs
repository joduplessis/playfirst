﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SimpleJSON;
using System;

namespace Controls {
	public class TurnRight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

		private GameManager gm;

		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {}
		public void OnDrag(PointerEventData eventData) {}
		public void OnPointerEnter(PointerEventData eventData) {}
		public void OnPointerExit(PointerEventData eventData) {}
		public void OnPointerClick(PointerEventData eventData) {}
		public void OnPointerDown(PointerEventData eventData) { transform.localScale = new Vector3 (1.2f,1.2f,1.2f); gm.avatarObject.GetComponent<MWO.Player> ().TurnRight (); }
		public void OnPointerUp(PointerEventData eventData) { transform.localScale = new Vector3 (1,1,1); gm.avatarObject.GetComponent<MWO.Player> ().StopLeftAndRight (); }
	}
}