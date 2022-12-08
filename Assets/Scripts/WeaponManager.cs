using System.Collections;

using Unity.Netcode;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
	[SerializeField] private Animator toasterHands;
	public GameObject playerCamera;
	public GameObject toastPrefab;
	public Transform toasterSpawner;

	private NetworkVariable<DamageToClientData> DamageToClient = new NetworkVariable<DamageToClientData>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

	public struct DamageToClientData : INetworkSerializable
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


	private void Update()
	{
		if (!IsOwner) return;

		ThrowGranade();
		//Shoot();

	}

	private void Shoot()
	{
		if (Input.GetMouseButtonDown(0))
		{
			toasterHands.SetBool("SetShoot", true);

			RaycastHit hit;
			if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
			{
				if (hit.transform.TryGetComponent(out PlayerManager otherPlayerMovement))
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
			StartCoroutine(ExampleCoroutine());

		}
	}

	IEnumerator ExampleCoroutine()
	{
		yield return new WaitForSeconds(0.2f);
		toasterHands.SetBool("SetShoot", false);
	}


	private void ThrowGranade()
	{
		if (Input.GetMouseButtonDown(0))
		{
			toasterHands.SetBool("SetShoot", true);
			GameObject toast = Instantiate(toastPrefab, toasterSpawner.position, transform.rotation);
			toast.GetComponent<NetworkObject>().Spawn();



			StartCoroutine(ExampleCoroutine());
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
		Debug.Log($"��,��,�� ��������, ���������. �� ���� ����� ����� � ID: {data.damageOrigin}, � ��������� {data.damageDestination}, �� �������������� {data.damage} ������!");
	}
}
