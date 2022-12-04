using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System;

public class UINetworkManager : MonoBehaviour
{
	[SerializeField] private Button serverButton;
	[SerializeField] private Button HostButton;
	[SerializeField] private Button clientButton;
	[SerializeField] private TMP_InputField IP;
	[SerializeField] private TMP_InputField port;
	[SerializeField] private Button applyButton;
	[SerializeField] private UnityTransport unityTransport;

	private void Awake()
	{
		
		serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
		HostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
		clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
	}
	public void applyIpAndPort()
	{
		unityTransport.ConnectionData.Address = IP.text;
		unityTransport.ConnectionData.Port = Convert.ToUInt16(port.text);
	}
}
