using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public class Player : PlayerBehavior
	{
		[SerializeField] private CharacterController controller;

		float xRotation = 0f;

		public Camera playerCamera;
		public float mouseSensetiviy = 5f;
		public float speed = 12f;


		public override void OnNetworkSpawn()
		{
			if (IsOwner)
			{
				transform.position = new Vector3(Random.Range(5, 5), 0, Random.Range(5, 5));

				playerCamera.gameObject.SetActive(true);
			}
		}

		private void Update()
		{
			if (!IsOwner) return;
			Look();
			Move();
		}


		private void Look()
		{
			float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensetiviy;
			float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensetiviy;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);
		}
		private void Move()
		{
			float x = Input.GetAxisRaw("Horizontal");
			float z = Input.GetAxisRaw("Vertical");

			Vector3 move = transform.right * x + transform.forward * z;

			controller.Move(move.normalized * speed * Time.deltaTime);
		}
		public override Camera GetCameraWorld() => playerCamera;
	}

}
