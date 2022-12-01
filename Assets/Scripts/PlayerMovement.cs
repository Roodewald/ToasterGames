using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerMovement : NetworkBehaviour
{
	[SerializeField] private CharacterController controller;
	float xRotation = 0f;
	
	public GameObject playerCamera;
	public float mouseSensetiviy = 100f;
	public float speed = 12f;

	public override void OnNetworkSpawn()
	{
		if (IsOwner)
		{
			playerCamera.SetActive(true);
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
}
