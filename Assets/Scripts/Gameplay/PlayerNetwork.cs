using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] SpriteRenderer playerVisuals;
    [SerializeField] Color playerRed;
    [SerializeField] Color playerBlue;
    [Header("Movement Values")]
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float slideDuration = 3f;
    [Header("Sliding Visuals")]
    [SerializeField] Vector2 characterNormalScale,characterSlideScale;
    [SerializeField] Vector2 colloiderNormalOffset,colloiderSlideOffset;
    [SerializeField] Vector2 colloiderNormalSize,colloiderSlideSize;
    private bool isSliding;
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private float lastDir;
    private float sendTimer;
    private float direction;
    private Transform character;
    private BoxCollider2D boxCollider;

    Rigidbody2D rb;

    public enum PlayerId { Red, Blue }

    public NetworkVariable<PlayerId> playerColor =
        new(PlayerId.Red,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public static Action<Transform> OnSetCameraTarget;
    public static Action<PlayerId> OnSetPlayerId;
    public static Action<PlayerId> OnGameOverEvent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        character = transform.GetChild(0);
        playerVisuals.sortingOrder++;
    }
    public override void OnNetworkSpawn()
    {
        GameManager.instance.OnGameStartEvent += OnGameStart;
        MovementUI.OnTouchMove += GetTouchInput;
        JumpSlideUI.OnJump += JumpInput;
        JumpSlideUI.OnSlide += SlideInput;
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

        if (IsOwner){
            OnSetCameraTarget?.Invoke(transform);
            OnSetPlayerId?.Invoke(playerColor.Value);
        }
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
        float dir = direction;
        sendTimer += Time.deltaTime;

        if (Mathf.Abs(lastDir - dir) > 0.01f || sendTimer > 0.1f)
        {
            lastDir = dir;
            sendTimer = 0;
            MoveServerRpc(dir);
        }

    }
    void GetTouchInput(float dir)
    {
        direction = dir;
    }
    void JumpInput()
    {
        if(!IsOwner) return;
        JumpServerRpc();
    }
    void SlideInput()
    {
        if(!IsOwner) return;
        SlideServerRpc();
    }

    [Rpc(SendTo.Server)]
    void MoveServerRpc(float dir)
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
    }

    [Rpc(SendTo.Server)]
    void JumpServerRpc()
    {
        CheckGround();
        if (isGrounded && !isSliding){
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    [Rpc(SendTo.Server)]
    void SlideServerRpc()
    {
        CheckGround();
        if (isGrounded && !isSliding)
        {
            StartCoroutine(SlideRoutine());
            SlideClientRpc();
        }
    }
    [Rpc(SendTo.Everyone)]
    void SlideClientRpc()
    {
        if (!isSliding)
            StartCoroutine(SlideRoutine());
    }
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }
    private IEnumerator SlideRoutine()
    {
        isSliding = true;
        float elapse = 0;
        float t;
        while(elapse < 0.5f)
        {
            t = elapse/0.5f;
            character.localScale = Vector2.Lerp(characterNormalScale,characterSlideScale,t);
            boxCollider.size = Vector2.Lerp(colloiderNormalSize,colloiderSlideSize,t);
            boxCollider.offset = Vector2.Lerp(colloiderNormalOffset,colloiderSlideOffset,t);
            elapse += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(slideDuration);
        elapse = 0;
        while(elapse < 0.5f)
        {
            t = elapse/0.5f;
            character.localScale = Vector2.Lerp(characterSlideScale,characterNormalScale,t);
            boxCollider.size = Vector2.Lerp(colloiderSlideSize,colloiderNormalSize,t);
            boxCollider.offset = Vector2.Lerp(colloiderSlideOffset,colloiderNormalOffset,t);
            elapse += Time.deltaTime;
            yield return null;
        }
        isSliding = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!IsServer) return;
        if (collision.gameObject.CompareTag("Finish"))
        {
            OnGameOverEvent?.Invoke(playerColor.Value);
        }
    }
}
