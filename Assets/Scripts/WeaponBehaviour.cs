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

		//returns the have amunation
		public abstract bool IsFull();
		public abstract bool HasAmmunition();
		public abstract bool IsAutomatic();
		public abstract float GetRateOfFire();

		#endregion

		#region METHODS

		public abstract void Reload();
		public abstract void Fire();

		#endregion
	}
}

