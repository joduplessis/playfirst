using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using System.Net.Sockets;
using DG.Tweening;
using EpPathFinding.cs;
using System.Linq;
using UnityEngine.SceneManagement;

namespace PopupMain {
	public class Chats : MonoBehaviour {

		private GameManager gm;

		public GameObject chatBlueprint;
		public GameObject chatContainer;
		public InputField chatInput;
		public ScrollRect scrollRect;

		// Use this for initialization
		void Start () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();	
			chatBlueprint.gameObject.SetActive (false);		
		}

		public void sendChat() {
			gm.broadcast ("chat," + gm.userPlayname + "," + gm.userAvatar + "," + chatInput.text);
			chatInput.text = "";
		}

		public void show() {
			gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-240.5f, -480f), 0.5f, true).SetEase(Ease.InQuad);
		}

		public void hide() {
			gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(300f, -480f), 0.5f, true).SetEase(Ease.OutQuad);
		}

		public void addChat(string message) {
			string[] parts = message.Split(',');

			string playname = parts [1];
			string avatar = parts [2];
			string chat = parts [3];

			GameObject p = Instantiate (chatBlueprint);

			p.gameObject.SetActive (true);
			p.gameObject.transform.SetParent(chatContainer.gameObject.transform);
			p.gameObject.GetComponent<PopupMain.Chat> ().chat.text = chat;
			p.gameObject.GetComponent<PopupMain.Chat> ().avatarString = avatar;
			p.gameObject.GetComponent<PopupMain.Chat> ().user.text = playname;
			p.gameObject.GetComponent<PopupMain.Chat> ().init ();

			gm.resizeScrollableContent (chatContainer, chatBlueprint, chatContainer.transform.childCount, "portrait");
			scrollRect.gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
		}

		void Update () {
		}
	}
}