using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;


public class LobbyBrowserManager : MonoBehaviour
{
    public static LobbyBrowserManager Instance;
    public Lobby currentLobby;

    async void Awake()
    {
        if (Instance == null) Instance = this;
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby(string _lobbyName)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>()
            {
                { "RelayCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
            }
        };

        currentLobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName + Random.Range(1000,9999), 2, options);

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var relayData = AllocationUtils.ToRelayServerData(allocation,"dtls");
        transport.SetRelayServerData(relayData);
        NetworkManager.Singleton.StartHost();

        Debug.Log("Lobby Created: " + currentLobby.Name);
    }

    public async Task<List<Lobby>> GetLobbyList()
    {
        QueryLobbiesOptions options = new QueryLobbiesOptions
        {
            Count = 10
        };

        QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(options);
        return response.Results;
    }


    public async void JoinLobby(Lobby lobby)
    {
        currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);

        string joinCode = currentLobby.Data["RelayCode"].Value;
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var relayData = AllocationUtils.ToRelayServerData(allocation,"dtls");
        transport.SetRelayServerData(relayData);
        NetworkManager.Singleton.StartClient();

        Debug.Log("Joined Lobby: " + currentLobby.Name);
    }
}
