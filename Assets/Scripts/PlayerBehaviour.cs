using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public abstract class PlayerBehaviour : NetworkBehaviour
	{
		public abstract Camera GetCameraWorld();
	}
}
