using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItemUI : MonoBehaviour
{
    public TextMeshProUGUI lobbyName;
    public Button joinButton;
    private Lobby lobbyData;

    public void Setup(Lobby lobby)
    {
        lobbyData = lobby;
        lobbyName.text = lobby.Name + $" ({lobby.Players.Count}/2)";

        joinButton.onClick.AddListener(() =>
        {
            LobbyBrowserManager.Instance.JoinLobby(lobbyData);
        });
    }
}
