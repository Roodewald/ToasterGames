using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using System;
using Mirror.SimpleWeb;

namespace ToasterGames.ShootingEverything
{
	public class UINetworkManager : MonoBehaviour
	{
		#region SerializeField

		[SerializeField] private Button serverButton;
		[SerializeField] private Button HostButton;
		[SerializeField] private Button clientButton;
		[SerializeField] private TMP_InputField IP;
		[SerializeField] private TMP_InputField port;
		[SerializeField] private Button applyButton;
		[SerializeField] private SimpleWebTransport webTransport;
		[SerializeField] private TMP_Text ping;
		[SerializeField] private TMP_Text version;

		#endregion

		private void Awake()
		{
			serverButton.onClick.AddListener(() => NetworkManager.singleton.StartServer());
			HostButton.onClick.AddListener(() => NetworkManager.singleton.StartHost());
			clientButton.onClick.AddListener(() => NetworkManager.singleton.StartClient());
			version.text = Application.version;
		}
		private void Update()
		{
			ping.text = $"Ping: {Math.Round(NetworkTime.rtt * 1000)}ms";
		}
		public void applyIpAndPort()
		{
			NetworkManager.singleton.networkAddress = IP.text;
			webTransport.port = Convert.ToUInt16(port.text);
		}
	}
}