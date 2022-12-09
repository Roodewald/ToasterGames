using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public abstract class WeaponBehavior : MonoBehaviour
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



		#endregion
	}
}

