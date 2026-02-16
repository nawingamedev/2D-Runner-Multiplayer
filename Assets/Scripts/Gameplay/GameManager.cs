using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public PlayerNetwork.PlayerId playerId;
    public static GameManager instance;
    public event EventHandler OnGameStartEvent;
    public event Action<bool> OnGameOverEvent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    public override void OnNetworkSpawn()
    {
        PlayerNetwork.OnGameOverEvent += OnPlayerGameOver;
        PlayerNetwork.OnSetPlayerId += SetPlayerID;
        GameOverState.OnRestart += OnRestart;
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnNetworkConnectedCallBack;
        }
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;
        NetworkManager.Singleton.OnServerStopped += OnServerStopped;
    }
    void OnNetworkConnectedCallBack(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            TriggerGameStartRpc();
        }
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    void TriggerGameStartRpc()
    {
        OnGameStartEvent?.Invoke(this,EventArgs.Empty);
        UIStateMachine.instance.ChangeState(uiStateName.GamePlayState);
    }
    [Rpc(SendTo.ClientsAndHost)]
    void WinnerRpc(PlayerNetwork.PlayerId winnerId)
    {
        UIStateMachine.instance.ChangeState(uiStateName.GameOverState);
        OnGameOverEvent?.Invoke(playerId == winnerId);
    }
    [Rpc(SendTo.Server)]
    void RestartRpc()
    {
        TriggerGameStartRpc();
    }
    
    void SetPlayerID(PlayerNetwork.PlayerId _playerId)
    {
        playerId = _playerId;
    }
    void OnPlayerGameOver(PlayerNetwork.PlayerId id)
    {
        WinnerRpc(id);
    }
    void OnRestart()
    {
        RestartRpc();
    }
    void OnDisconnect(ulong id)
    {
        UIStateMachine.instance.ChangeState(uiStateName.NetworkState);
        NetworkManager.Singleton.Shutdown();
    }
    void OnServerStopped(bool isHost)
    {
        UIStateMachine.instance.ChangeState(uiStateName.NetworkState);
        NetworkManager.Singleton.Shutdown();
    }
}
