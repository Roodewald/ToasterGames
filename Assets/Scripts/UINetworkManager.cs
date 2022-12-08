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
	//[SerializeField] private TMP_Text ping;




	private void Awake()
	{
		
		serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
		HostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
		clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
	}
	private void Update()
	{
		//ping.text = NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(0).ToString();
	}
	public void applyIpAndPort()
	{
		unityTransport.ConnectionData.Address = IP.text;
		unityTransport.ConnectionData.Port = Convert.ToUInt16(port.text);
	}
}
