using Unity.Services.Lobbies;
using UnityEngine;
using System.Threading.Tasks;

public class LobbyHeartbeat : MonoBehaviour
{
    async void Start()
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
