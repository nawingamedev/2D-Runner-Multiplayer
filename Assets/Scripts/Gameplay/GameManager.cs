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
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnNetworkConnectedCallBack;
        }
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
        Debug.Log($"Nawin ServerRpc winner is {winnerId} {playerId == winnerId}");
        UIStateMachine.instance.ChangeState(uiStateName.GameOverState);
        OnGameOverEvent?.Invoke(playerId == winnerId);
    }
    void SetPlayerID(PlayerNetwork.PlayerId _playerId)
    {
        playerId = _playerId;
        Debug.Log("Nawin My PlayerID "+playerId);
    }
    void OnPlayerGameOver(PlayerNetwork.PlayerId id)
    {
        Debug.Log($"Nawin OnPlayerGameOver {id} is Winner");
        WinnerRpc(id);
    }
}
