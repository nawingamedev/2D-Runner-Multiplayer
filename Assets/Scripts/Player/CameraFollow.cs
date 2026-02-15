using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float xOffset = 3f;
    [SerializeField] private float fixedY = 0f;

    private void Update()
    {
        if (!target) return;

        Vector3 pos = transform.position;
        pos.x = target.position.x + xOffset;
        pos.y = fixedY;
        transform.position = pos;
    }
}
