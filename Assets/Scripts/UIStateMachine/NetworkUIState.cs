using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIState : UIBaseStates
{
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    void Awake()
    {
        stateName = uiStateName.NetworkState;
        //hostButton.onClick.AddListener(HostOnClick);
        //clientButton.onClick.AddListener(ClientOnClick);
    }
    public override void EnterState()
    {
        gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
    }

    public override uiStateName GetUiStateName()
    {
        return stateName;
    }
    void HostOnClick()
    {
        NetworkManager.Singleton.StartHost();
    }
    void ClientOnClick()
    {
        UnityTransport transport = (UnityTransport) NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        //transport.ConnectionData.Address = "192.168.1.39";
        NetworkManager.Singleton.StartClient();
    }
}
