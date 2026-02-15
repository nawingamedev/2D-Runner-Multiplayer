using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] SpriteRenderer playerVisuals;
    [SerializeField] Color playerRed;
    [SerializeField] Color playerBlue;
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 8f;

    Rigidbody2D rb;

    public enum PlayerId { Red, Blue }

    public NetworkVariable<PlayerId> playerColor =
        new(PlayerId.Red,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public static Action<Transform> OnSetCameraTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnNetworkSpawn()
    {
        GameManager.instance.OnGameStartEvent += OnGameStart;
        if (IsServer)
        {
            AssignColor();
        }
    }

    void OnGameStart(object sender,EventArgs e)
    {
        playerColor.OnValueChanged += OnPlayerColorChanged;

        if (IsServer)
        {
            SetSpawn();
        }

        UpdateColor();

        if (IsOwner)
            OnSetCameraTarget?.Invoke(transform);
    }

    void AssignColor()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 1)
            playerColor.Value = PlayerId.Red;
        else
            playerColor.Value = PlayerId.Blue;
    }

    void SetSpawn()
    {
        if (playerColor.Value == PlayerId.Red)
            transform.position = new Vector3(5, 2);
        else
            transform.position = new Vector3(8, 2);
    }

    void OnPlayerColorChanged(PlayerId oldVal, PlayerId newVal)
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        playerVisuals.color = (playerColor.Value == PlayerId.Red) ? playerRed : playerBlue;
    }

    void Update()
    {
        if (!IsOwner) return;

        float dir = Input.GetAxis("Horizontal");
        MoveServerRpc(dir);

        if (Input.GetKeyDown(KeyCode.Space))
            JumpServerRpc();
    }

    [Rpc(SendTo.Server)]
    void MoveServerRpc(float dir)
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
    }

    [Rpc(SendTo.Server)]
    void JumpServerRpc()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
