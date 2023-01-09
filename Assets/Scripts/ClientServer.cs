using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public class ClientServer : NetworkBehaviour
	{
		#region FIELDS

		private NetworkTransform networkTransform;

		private SyncList<DamageToClientData> DamageToClient = new SyncList<DamageToClientData>();

		public struct DamageToClientData
		{
			public ulong damageOrigin;
			public ulong damageTarget;
			public int damageWeapon;
			public float damageDestination;
			public float damage;

		}
		#endregion

		#region UNITY
		private void Awake()
		{
			networkTransform = GetComponent<NetworkTransform>();
		}
		#endregion

		#region RPC

		[Command]
		public void ServerRpc(DamageToClientData data)
		{
			ApplyDamageClientRpc(data);
		}

		[ClientRpc]
		private void ApplyDamageClientRpc(DamageToClientData data)
		{
			Debug.Log($"Damage Form: {data.damageOrigin}, TO:{data.damageTarget}  Weapon ID{data.damageWeapon} DiSTANCE {data.damageDestination}, DAMAGE {data.damage} демага!");
		}
		#endregion

		#region GETERS
		public NetworkTransform GetNetworkObject() => networkTransform;
		#endregion
	}
}

