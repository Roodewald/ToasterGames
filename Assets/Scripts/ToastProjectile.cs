using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastProjectile : MonoBehaviour
{
	public Rigidbody rb;
	private void Awake()
	{
		rb.GetComponent<Rigidbody>();
		rb.velocity = rb.transform.forward * 10f;
	}
}
