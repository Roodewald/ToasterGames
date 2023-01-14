using QFSW.QC;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{

	private Lobby hostLobby;
	private float heartbeatLobbyTimer;
	private string playerName;

	private async void Start()
	{
		await UnityServices.InitializeAsync();

		AuthenticationService.Instance.SignedIn += () =>
		{
			Debug.Log("signed in " + AuthenticationService.Instance.PlayerId);
		};
		await AuthenticationService.Instance.SignInAnonymouslyAsync();

		playerName = "Roodewald" + UnityEngine.Random.Range(10, 99);
		Debug.Log(playerName);
	}


	private void Update()
	{
		HandleLobbyHeartbeatAsync();
	}

	private async Task HandleLobbyHeartbeatAsync()
	{
		if (hostLobby != null)
		{
			heartbeatLobbyTimer -= Time.deltaTime;
			if (heartbeatLobbyTimer < 0f)
			{
				float heartbeatLobbyTimerMax = 15;
				heartbeatLobbyTimer = heartbeatLobbyTimerMax;

				await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
			}
		}
	}

	[Command]
	private async void CreateLobby()
	{
		try
		{
			string lobbyName = "MyLobby";
			int maxPlayers = 4;

			CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
			{
				IsPrivate = false,
				Player = GetPlayer()
			};

			Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

			hostLobby = lobby;


			Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.LobbyCode);

			PrintPlayers(hostLobby);

		}
		catch (LobbyServiceException e)
		{
			Debug.LogException(e);
		}
	}

	[Command]
	private async void ListLobbies()
	{
		try
		{
			QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
			{
				Count = 20,
				Filters = new List<QueryFilter>
				{
					new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0" ,QueryFilter.OpOptions.GT)
				},
				Order = new List<QueryOrder>
				{
					new QueryOrder(false, QueryOrder.FieldOptions.Created)
				}
			};
			QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

			Debug.Log("Lobbies found: " + queryResponse.Results.Count);
			foreach (Lobby lobby in queryResponse.Results)
			{
				Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
			}
		}
		catch (LobbyServiceException e)
		{
			Debug.LogException(e);
		}
	}

	[Command]
	private async void JoinLobby()
	{
		try
		{

			QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

			await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
		}
		catch (LobbyServiceException e)
		{
			Debug.LogException(e);
		}
	}

	[Command]
	private async void JoinLobbyByCode(string lobbyCode)
	{
		try
		{
			JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions()
			{
				Player = GetPlayer()
			};
			Lobby joinedLobby  = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);


			Debug.Log("Joined Lobby with code " + lobbyCode);
			PrintPlayers(joinedLobby);
		}
		catch (LobbyServiceException e)
		{
			Debug.LogException(e);
		}
	}

	[Command]
	private void PrintPlayers(Lobby lobby)
	{
		Debug.Log("Players in Lobby " + lobby.Name);
		foreach (Player player in lobby.Players)
		{
			Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
		}
	}

	private Player GetPlayer()
	{
		return new Player
		{
			Data = new Dictionary<string, PlayerDataObject>
					{
						{ "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
					}
		};
	}
}
