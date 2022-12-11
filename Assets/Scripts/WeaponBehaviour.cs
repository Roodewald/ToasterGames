using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public abstract class WeaponBehaviour : MonoBehaviour
	{
		#region UNITY
		protected virtual void Awake() { }

		protected virtual void Start() { }

		protected virtual void Update() { }

		protected virtual void LateUpdate() { }

		#endregion

		#region GETTERS

		/// Returns the reload audio clip.
		//public abstract AudioClip GetAudioClipReload();

		/// Returns the reload empty audio clip.
		//public abstract AudioClip GetAudioClipReloadEmpty();

		/// Returns the full Magazine
		public abstract bool IsFull();

		/// If weapon have ammunation to Shoot
		public abstract bool HasAmmunition();

		///If weapon automatic
		public abstract bool IsAutomatic();

		///How fast weapon can shoot
		public abstract float GetRateOfFire();

		/// Get weapon animator
		public abstract Animator GetAnimator();

		#endregion

		#region METHODS

		public abstract void Reload();
		public abstract void Fire();

		#endregion
	}
}

