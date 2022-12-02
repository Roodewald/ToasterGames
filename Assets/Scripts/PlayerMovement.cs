using UnityEngine;
using Unity.Netcode;
using Unity.Collections.LowLevel.Unsafe;

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

	private void Start()
	{
		//Cursor.lockState = CursorLockMode.Locked;
	}

	public override void OnNetworkSpawn()
	{
		if (IsOwner)
		{
			transform.position = new Vector3(Random.Range(5,5),0,Random.Range(5,5));

			playerCamera.SetActive(true);
		}
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
			if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
			{
				if (hit.transform.TryGetComponent(out PlayerMovement otherPlayerMovement))
				{
					DamageToClient.Value = new DamageToClientData
					{
						damageOrigin = OwnerClientId,
						damageTarget = otherPlayerMovement.OwnerClientId,
						damageWeapon = 0,
						damageDestination = Vector3.Distance(transform.position, hit.transform.position),
						damage = 15f
					};
					ServerRpc(DamageToClient.Value);
				}
			}
		}
	}
	[ServerRpc]
	private void ServerRpc(DamageToClientData data)
	{
		ClientRpcParams clientRpcParams = new ClientRpcParams
		{
			Send = new ClientRpcSendParams
			{
				TargetClientIds = new ulong[] { data.damageTarget }
			}
		};

		TestClientRpc(data, clientRpcParams);
	}

	[ClientRpc]
	private void TestClientRpc(DamageToClientData data, ClientRpcParams clientRpcParams = default)
	{
		Debug.Log($"Ой,ой,ой дружочек, пирожочек. По тебе попал игрок с ID: {data.damageOrigin}, с дистанции {data.damageDestination}, на сокрушительные {data.damage} демага!");
	}
}
