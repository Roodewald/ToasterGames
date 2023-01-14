using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public class ClientServer : NetworkBehaviour
	{
		#region FIELDS

		private NetworkObject networkObject;
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
		#endregion

		#region UNITY
		private void Awake()
		{
			networkObject = GetComponentInChildren<NetworkObject>();
		}
		#endregion

		#region RPC

		[ServerRpc]
		public void ServerRpc(DamageToClientData data)
		{
			ClientRpcParams clientRpcParams = new ClientRpcParams
			{
				Send = new ClientRpcSendParams
				{
					TargetClientIds = new ulong[] { data.damageTarget }
				}
			};

			ApplyDamageClientRpc(data, clientRpcParams);
		}
		
		[ClientRpc]
		private void ApplyDamageClientRpc(DamageToClientData data, ClientRpcParams clientRpcParams = default)
		{
			Debug.Log($"Ой,ой,ой дружочек, пирожочек. По тебе попал игрок с ID: {data.damageOrigin}, c оружия{data.damageOrigin} с дистанции {data.damageDestination}, на сокрушительные {data.damage} демага!");
		}
		#endregion

		#region GETERS
		public NetworkObject GetNetworkObject() => networkObject;
		#endregion
	}
}

