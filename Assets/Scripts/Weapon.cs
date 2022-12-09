using Unity.Netcode;
using UnityEngine;
using static ToasterGames.ShootingEverything.ClientServer;

namespace ToasterGames.ShootingEverything
{
	public class Weapon : WeaponBehavior
	{
		#region FIELDS SERIALIZED

		[Header("Firing")]

		[Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
		[SerializeField]
		private bool automatic;

		[Tooltip("How fast the projectiles are.")]
		[SerializeField]
		private float projectileImpulse = 400.0f;

		[Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
		[SerializeField]
		private int roundsPerMinutes = 200;

		[Tooltip("Mask of things recognized when firing.")]
		[SerializeField]
		private LayerMask mask;

		[Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
		[SerializeField]
		private float maximumDistance = 500.0f;

		[Header("Animation")]

		[Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
		[SerializeField]
		private Transform socketEjection;

		[Header("Resources")]
		[Tooltip("Casing Prefab.")]
		[SerializeField]
		private GameObject prefabCasing;


		#endregion

		#region FIELDS
		private Camera playerCamera;
		private PlayerBehavior playerBehaviour;
		private ClientServer clientServer;
		private NetworkObject networkObject;
		private DamageToClientData DamageToClient;

		#endregion

		#region UNITY
		protected override void Awake()
		{
			playerBehaviour = GetComponentInParent<PlayerBehavior>();
			clientServer = GetComponentInParent<ClientServer>();
			networkObject = clientServer.GetNetworkObject();
			playerCamera = playerBehaviour.GetCameraWorld();
		}
		protected override void Update()
		{
			Shoot();
		}
		#endregion

		#region METHODS

		private void Shoot()
		{
			
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
				{
					if (hit.transform.TryGetComponent(out Player otherPlayer))
					{
						DamageToClient = new DamageToClientData
						{
							damageOrigin = networkObject.OwnerClientId,
							damageTarget = otherPlayer.OwnerClientId,
							damageWeapon = 0,
							damageDestination = Vector3.Distance(transform.position, hit.transform.position),
							damage = 15f
						};
						clientServer.ServerRpc(DamageToClient);
					}
				}
			}
		}
		#endregion
	}
}

