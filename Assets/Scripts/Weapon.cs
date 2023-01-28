using Unity.Netcode;
using UnityEngine;
using static ToasterGames.ShootingEverything.ClientServer;

namespace ToasterGames.ShootingEverything
{
	public class Weapon : WeaponBehaviour
	{
		#region SERIALIZEDFIELDS 

		[Header("Firing")]

		[Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
		[SerializeField]
		private bool automatic;

		[SerializeField]
		private bool IsProjectile = false;

		[Tooltip("How fast the projectiles are.")]
		[SerializeField]
		private float projectileImpulse = 400.0f;

		[Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
		[SerializeField]
		private int roundsPerMinutes = 200;

		[SerializeField]
		private int totalAmunition = 2;

		[SerializeField]
		private float damageWeapon = 10f;


		[Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
		[SerializeField]
		private float maximumDistance = 500.0f;

		[Header("Animation")]


		[Header("Resources")]
		[Tooltip("Casing Prefab.")]
		[SerializeField]
		private GameObject prefabCasing;

		[SerializeField]
		private Transform barrel;

		#endregion

		#region FIELDS
		private Camera playerCamera;
		private PlayerBehaviour playerBehaviour;
		private ClientServer clientServer;
		private NetworkObject networkObject;
		private DamageToClientData DamageToClient;
		private int ammunitionCurrent;
		private Animator weaponAnimator;
		private Inventory inventory;

		#endregion

		#region UNITY
		protected override void Awake()
		{
			inventory = GetComponentInParent<Inventory>();
			ammunitionCurrent = totalAmunition;
			weaponAnimator = GetComponent<Animator>();
			playerBehaviour = GetComponentInParent<PlayerBehaviour>();
			clientServer = GetComponentInParent<ClientServer>();
			networkObject = clientServer.GetNetworkObject();
			playerCamera = playerBehaviour.GetCameraWorld();
		}

		#endregion

		#region METHODS

		public override void Reload()
		{
			//Play Reload Animation.
			weaponAnimator.Play(HasAmmunition() ? "Reload" : "Reload Empty", 0, 0.0f);
			ammunitionCurrent = totalAmunition;
		}

		public override void Fire()
		{
			weaponAnimator.Play("Fire");
			RaycastHit hit;

			if (IsProjectile)
			{
				clientServer.SpawnProjectileServerRpc(barrel.transform.position, transform.rotation);
			}
			else
			{
				if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maximumDistance))
				{
					if (hit.transform.TryGetComponent(out Player otherPlayer))
					{
						DamageToClient = new DamageToClientData
						{
							damageOrigin = networkObject.OwnerClientId,
							damageTarget = otherPlayer.OwnerClientId,
							damageWeapon = inventory.GetEquippedIndex(),
							damageDestination = Vector3.Distance(transform.position, hit.transform.position),
							damage = damageWeapon
						};
						clientServer.ServerRpc(DamageToClient);
					}
				}
			}
			ammunitionCurrent--;
		}
		#endregion

		#region GETTERS

		public override bool IsFull() => ammunitionCurrent == totalAmunition;

		public override bool HasAmmunition() => ammunitionCurrent > 0;

		public override bool IsAutomatic() => automatic;

		public override float GetRateOfFire() => roundsPerMinutes;

		public override Animator GetAnimator() => weaponAnimator;

		#endregion
	}
}