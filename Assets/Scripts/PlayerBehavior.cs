using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public abstract class PlayerBehavior : NetworkBehaviour
	{
		public abstract Camera GetCameraWorld();
	}
}
