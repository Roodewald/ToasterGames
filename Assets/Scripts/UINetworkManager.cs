using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkManager : MonoBehaviour
{
	[SerializeField] private Button serverButton;
	[SerializeField] private Button HostButton;
	[SerializeField] private Button clientButton;

	private void Awake()
	{
		serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
		HostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
		clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
	}
}
