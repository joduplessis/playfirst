using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using System.Net.Sockets;

namespace MWO {
	public class Player : MonoBehaviour {

		[SerializeField] private Animator m_animator;
		[SerializeField] private Rigidbody m_rigidBody;

		public float m_currentV = 0;
		public float m_currentH = 0;
		private float m_moveSpeed = 10;
		private float m_turnSpeed = 200;
		private float m_jumpForce = 5;
		private readonly float m_interpolation = 10;
		private readonly float m_walkScale = 0.33f;
		private readonly float m_backwardsWalkScale = 0.16f;
		private readonly float m_backwardRunScale = 0.66f;
		private float v;
		private float h;

		public bool m_wasGrounded;
		public  bool m_isGrounded;

		private Vector3 m_currentDirection = Vector3.zero;
		private float m_jumpTimeStamp = 0;
		private float m_minJumpInterval = 0.25f;

		private List<Collider> m_collisions = new List<Collider>();

		private Vector3 previousPosition;
		private float previousRotation;
		private float previousWalkingSpeed;
		private GameManager gm;
		public GameObject playnameTooltip;
		public string playname;
		public int id;

		private void OnCollisionEnter(Collision collision) {
			ContactPoint[] contactPoints = collision.contacts;

			for(int i = 0; i < contactPoints.Length; i++)
			{
				if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
				{
					if (!m_collisions.Contains(collision.collider)) {
						m_collisions.Add(collision.collider);
					}
					m_isGrounded = true;
				}
			}
		}

		private void OnCollisionStay(Collision collision) {
			ContactPoint[] contactPoints = collision.contacts;
			bool validSurfaceNormal = false;
			for (int i = 0; i < contactPoints.Length; i++)
			{
				if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
				{
					validSurfaceNormal = true; break;
				}
			}

			if(validSurfaceNormal)
			{
				m_isGrounded = true;
				if (!m_collisions.Contains(collision.collider))
				{
					m_collisions.Add(collision.collider);
				}
			} else
			{
				if (m_collisions.Contains(collision.collider))
				{
					m_collisions.Remove(collision.collider);
				}
				if (m_collisions.Count == 0) { m_isGrounded = false; }
			}
		}

		private void OnCollisionExit(Collision collision) {
			if(m_collisions.Contains(collision.collider)) { m_collisions.Remove(collision.collider);  }
			if (m_collisions.Count == 0) { m_isGrounded = false; }
		}
			
		void Awake () {
			gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager> ();
		}

		void Update () {
			if (id == gm.userId) {

				// This is for no controls
				if (!gm.mobileControlMode) {
					if (gm.editMode) {
						m_moveSpeed = 100;
					} else {
						m_moveSpeed = 20;
					}

					v = Input.GetAxis ("Vertical");
					h = Input.GetAxis ("Horizontal");
				}

				m_animator.SetBool ("Grounded", m_isGrounded);

				bool walk = Input.GetKey (KeyCode.LeftShift);

				if (v < 0) {
					if (walk) { 
						v *= m_backwardsWalkScale; 
					} else { 
						v *= m_backwardRunScale; 
					}
				} else if (walk) { 
					v *= m_walkScale; 
				}

				m_currentV = Mathf.Lerp (m_currentV, v, Time.deltaTime * m_interpolation);
				m_currentH = Mathf.Lerp (m_currentH, h, Time.deltaTime * m_interpolation);

				transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
				transform.Rotate (0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

				setAnimationSpeed (m_currentV);

				// Jumping 

				bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

				if (jumpCooldownOver && m_isGrounded && Input.GetKey (KeyCode.Space)) {
					m_jumpTimeStamp = Time.time;
					m_rigidBody.AddForce (Vector3.up * m_jumpForce, ForceMode.Impulse);
				}

				if (!m_wasGrounded && m_isGrounded) {
					m_animator.SetTrigger ("Land");
				}
				if (!m_isGrounded && m_wasGrounded) {
					m_animator.SetTrigger ("Jump");
				}

				m_wasGrounded = m_isGrounded;

				// Network - previous position let's us not clog the server for static data
				if (gameObject.transform.position != previousPosition || gameObject.transform.rotation.y != previousRotation) {
					previousPosition = gameObject.transform.position;
					previousRotation = gameObject.transform.rotation.y;
					previousWalkingSpeed = m_currentV;

					if (gm.world != null) {
						gm.broadcast ("player_movement," + gm.userId + "," + gm.world ["id"] + "," + previousPosition.x + "," + previousPosition.y + "," + previousPosition.z + "," + transform.localRotation.eulerAngles.y + "," + previousWalkingSpeed + "," + gm.userAvatar + "," + gm.userPlayname);
					}
				}
			} else {
				m_animator.SetBool ("Grounded", m_isGrounded);
			}
		}

		public void setAnimationSpeed(float s) {
			m_animator.SetFloat ("MoveSpeed", s);
		}

		public void GoForward() { v = 1; }
		public void GoBackward() { v = -1; }
		public void TurnLeft() { h = -1; }
		public void TurnRight() { h = 1; }
		public void StopLeftAndRight() { h = 0; }
		public void StopForwardAndBack() { v = 0; }
		public void Jump() {
			m_jumpTimeStamp = Time.time;
			m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
		}

		public void init(int uid, string up) {
			playname = up;
			id = uid;

			if (gm.userId != uid) {
				GameObject t = Instantiate (gm.playerTooltipBlueprint);
				t.gameObject.transform.SetParent (gm.movableTooltipContainer.gameObject.transform);
				t.gameObject.transform.position = gm.cameraMain.WorldToScreenPoint (transform.position);
				t.gameObject.GetComponent<Movable.Player> ().player = gameObject;
				t.gameObject.GetComponent<Movable.Player> ().playname.text = playname;

				playnameTooltip = t.gameObject;
			}
		}

	}
}