using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace ToasterGames.ShootingEverything
{
	public abstract class PlayerBehaviour : NetworkBehaviour
	{
		public abstract Camera GetCameraWorld();
	}
}
