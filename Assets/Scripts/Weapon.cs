using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

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

	

		[Tooltip("It is necessary for the client server architecture to work")]
		

		#endregion

		#region FIELDS
		private Camera playerCamera;
		private PlayerBehavior playerBehaviour;

		#endregion

		protected override void Awake()
		{
			playerBehaviour = GetComponentInParent<PlayerBehavior>();
			playerCamera = playerBehaviour.GetCameraWorld();
		}
	}
}

