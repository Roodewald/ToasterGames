using System.Collections;
using System.Collections.Generic;
using ToasterGames.ShootingEverything;
using Unity.Netcode;
using UnityEngine;
using static ToasterGames.ShootingEverything.ClientServer;

public class ToastProjectile : MonoBehaviour
{
	private DamageToClientData DamageToClient;
	public Rigidbody rb;
	private void Start()
	{
		rb.GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * 8, ForceMode.Impulse);
		rb.AddForce(transform.up* 4, ForceMode.Impulse);

		
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.TryGetComponent(out Player otherPlayer))
		{
			DamageToClient = new DamageToClientData
			{
				damageOrigin = 0,
				damageTarget = 0,
				damageWeapon = 0,
				damageDestination = Vector3.Distance(transform.position, transform.position),
				damage = 15
			};
			ClientServer.instance.ServerRpc(DamageToClient);
		}
	}
}
