using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class RelayManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hostJoinCode;
    [SerializeField] TMP_InputField clientJoinCode;
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(HostRelay);
        clientButton.onClick.AddListener(() => JoinRelay(clientJoinCode.text));
    }

    async void HostRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        hostJoinCode.text = "Join Code: " + joinCode;
        Debug.Log("Relay Join Code: " + joinCode);
        var relayData = AllocationUtils.ToRelayServerData(allocation,"dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);

        NetworkManager.Singleton.StartHost();
    }

    async void JoinRelay(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var relayData = AllocationUtils.ToRelayServerData(allocation,"dtls");
        transport.SetRelayServerData(relayData);

        NetworkManager.Singleton.StartClient();
    }
}
