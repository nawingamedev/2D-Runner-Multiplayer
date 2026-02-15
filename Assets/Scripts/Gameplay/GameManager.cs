using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public event EventHandler OnGameStartEvent;
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
}
