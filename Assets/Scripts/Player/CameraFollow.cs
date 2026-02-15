using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float xOffset = 3f;
    [SerializeField] private float fixedY = 0f;

    void OnEnable()
    {
        PlayerNetwork.OnSetCameraTarget += SetTarget;
    }
    void OnDisable()
    {
        PlayerNetwork.OnSetCameraTarget -= SetTarget;
    }
    void Update()
    {
        if (!target) return;

        Vector3 pos = transform.position;
        pos.x = target.position.x + xOffset;
        pos.y = target.position.y;
        transform.position = pos;
    }
    void SetTarget(Transform player)
    {
        target = player;
    }
}
