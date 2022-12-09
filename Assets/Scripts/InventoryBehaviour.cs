using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public abstract class InventoryBehaviour : MonoBehaviour
	{
		#region GETTERS

		/// Returns the index that is before the current index. Very helpful in order to figure out
		/// what the next weapon to equip is.
		public abstract int GetLastIndex();

		/// Returns the next index after the currently equipped one. Very helpful in order to figure out
		/// what the next weapon to equip is.
		public abstract int GetNextIndex();

		/// Returns the currently equipped WeaponBehaviour.
		public abstract WeaponBehaviour GetEquipped();

		/// Returns the currently equipped index. Meaning the index in the weapon array of the equipped weapon.
		public abstract int GetEquippedIndex();

		#endregion

		#region METHODS

		/// Init. This function is called when the game starts. We don't use Awake or Start because we need the
		/// <param name="equippedAtStart">Inventory index of the weapon we want to equip when the game starts.</param>
		public abstract void Init(int equippedAtStart = 0);


		/// Equips a Weapon.
		/// <param name="index">Index of the weapon to equip.</param>
		public abstract WeaponBehaviour Equip(int index);

		#endregion
	}
}

