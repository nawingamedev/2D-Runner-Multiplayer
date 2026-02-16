using Unity.Services.Lobbies;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Netcode;

public class LobbyHeartbeat : MonoBehaviour
{
    void Start()
    {
        if (!NetworkManager.Singleton.IsHost) return;
        StartLobby();
    }
    async void StartLobby()
    {
        while (true)
        {
            if (LobbyBrowserManager.Instance != null &&
                LobbyBrowserManager.Instance.currentLobby != null)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(
                    LobbyBrowserManager.Instance.currentLobby.Id);
            }
            await Task.Delay(15000);
        }
    }
}
