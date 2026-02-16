using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MovementUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public static Action<float> OnTouchMove;

    [Header("Sensitivity")]
    [SerializeField] float deadZone = 30f;     
    [SerializeField] float maxSwipeDistance = 300f; 
    [SerializeField] float sensitivity = 1f;   

    public bool isHolding;
    private float startPosX;
    private float currentDir;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        startPosX = eventData.position.x;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        float delta = eventData.position.x - startPosX;

        if (Mathf.Abs(delta) < deadZone)
        {
            currentDir = 0;
            return;
        }

        // Normalize movement (-1 to 1)
        float normalized = Mathf.Clamp(delta / maxSwipeDistance, -1f, 1f);
        currentDir = normalized * sensitivity;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        currentDir = 0;
    }

    void Update()
    {
        if(!isHolding) currentDir = 0;
        OnTouchMove?.Invoke(currentDir);
    }
}
