using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class NetworkUIState : UIBaseStates
{
    [SerializeField] GameObject lobbyItemPrefab;
    [SerializeField] Transform lobbyContent;
    [SerializeField] Button refreshBtn;
    [SerializeField] Button craeteRoomBtn;
    [SerializeField] TMP_InputField roomName;
    void Awake()
    {
        stateName = uiStateName.NetworkState;
        refreshBtn.onClick.AddListener(RefreshLobbyList);
        craeteRoomBtn.onClick.AddListener(CreateRoom);
    }
    public override void EnterState()
    {
        gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        foreach (Transform child in lobbyContent)
            Destroy(child.gameObject);
        gameObject.SetActive(false);
    }

    public override uiStateName GetUiStateName()
    {
        return stateName;
    }
    

    async void RefreshLobbyList()
    {
        foreach (Transform child in lobbyContent)
            Destroy(child.gameObject);

        List<Lobby> lobbies = await LobbyBrowserManager.Instance.GetLobbyList();

        foreach (Lobby lobby in lobbies)
        {
            GameObject item = Instantiate(lobbyItemPrefab, lobbyContent);
            item.GetComponent<LobbyItemUI>().Setup(lobby);
        }
    }
    void CreateRoom()
    {
        if(roomName.text == "" || roomName.text.IsNullOrEmpty()) return;
        LobbyBrowserManager.Instance.CreateLobby(roomName.text);
    }

}
