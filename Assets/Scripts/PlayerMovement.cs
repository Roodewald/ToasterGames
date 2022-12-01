using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System;

public class PlayerMovement : NetworkBehaviour
{
	[SerializeField] private CharacterController controller;
	private NetworkVariable<DamageToClientData> DamageToClient = new NetworkVariable<DamageToClientData>(default,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

	public struct DamageToClientData: INetworkSerializable
	{
		public ulong damageOrigin;
		public ulong damageTarget;
		public int damageWeapon;
		public float damageDestination;
		public float damage;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref damageOrigin);
			serializer.SerializeValue(ref damageTarget);
			serializer.SerializeValue(ref damageWeapon);
			serializer.SerializeValue(ref damageDestination);
			serializer.SerializeValue(ref damage);
		}
	}


	float xRotation = 0f;
	
	public GameObject playerCamera;
	public float mouseSensetiviy = 100f;
	public float speed = 12f;

	public override void OnNetworkSpawn()
	{
		DamageToClient.OnValueChanged += OnSomeValueChanged;
		if (IsOwner)
		{
			playerCamera.SetActive(true);
		}
	}

	private void OnSomeValueChanged(DamageToClientData previousValue, DamageToClientData newValue)
	{
		Debug.Log($"Ты {OwnerClientId}, попали по{newValue.damageTarget}");
		Debug.Log($"Ой,ой,ой дружочек, пирожочек. По тебе попал игрок с ID: {newValue.damageOrigin}, с дистанции {newValue.damageDestination}, на сокрушительные {newValue.damage} демага!");
	}

	private void Update()
	{
		if (!IsOwner) return;

		Shoot();
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
	private void Shoot()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit);
			if (hit.transform.GetComponent<PlayerMovement>()!= null)
			{
				print("Я попал по" + hit.transform.GetComponent<PlayerMovement>().OwnerClientId);
				DamageToClient.Value = new DamageToClientData
				{
					damageOrigin = OwnerClientId,
					damageTarget = hit.transform.GetComponent<PlayerMovement>().OwnerClientId,
					damageWeapon = 0,
					damageDestination = Vector3.Distance(transform.position, hit.transform.position),
					damage = 15f
				};
			}
		}
	}
}
