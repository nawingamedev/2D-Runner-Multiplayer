using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class JumpSlideUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Action OnJump;
    public static Action OnSlide;

    [Header("Swipe Settings")]
    [SerializeField] float swipeThreshold = 120f;   
    [SerializeField] float maxSwipeTime = 0.4f;     

    private Vector2 startPos;
    private float startTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        startTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 endPos = eventData.position;
        float swipeTime = Time.time - startTime;
        Vector2 delta = endPos - startPos;

        if (swipeTime > maxSwipeTime)
            return;

        if (delta.y > swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
        {
            Debug.Log("Nawin JUMP SWIPE");
            OnJump?.Invoke();
        }
        else if (delta.y < -swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
        {
            Debug.Log("Nawin SLIDE SWIPE");
            OnSlide?.Invoke();
        }
    }
}
