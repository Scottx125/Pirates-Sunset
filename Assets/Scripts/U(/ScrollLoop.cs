using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLoop : MonoBehaviour
{
    public Scrollbar scrollbar;
    private bool isLooping = false;
    private float prevScrollbarValue;
    private float scrollSpeed;

    void Start()
    {
        if (scrollbar == null)
        {
            scrollbar = GetComponent<Scrollbar>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverScrollbar())
            {
                StartLooping();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopLooping();
        }

        if (isLooping)
        {
            CalculateScrollSpeed();
            LoopScrollbar();
        }

        prevScrollbarValue = scrollbar.value;
    }

    bool IsMouseOverScrollbar()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform scrollRect = scrollbar.GetComponent<RectTransform>();
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, mousePosition, null, out localMousePosition);
        Rect scrollbarRect = scrollRect.rect;
        return scrollbarRect.Contains(localMousePosition);
    }

    void StartLooping()
    {
        isLooping = true;
        prevScrollbarValue = scrollbar.value;
    }

    void StopLooping()
    {
        isLooping = false;
    }

    void CalculateScrollSpeed()
    {
        float deltaValue = Mathf.Abs(scrollbar.value - prevScrollbarValue);
        float deltaTime = Time.deltaTime;
        if (deltaTime > 0)
        {
            scrollSpeed = deltaValue / deltaTime;
        }
    }

    void LoopScrollbar()
    {
        scrollbar.value += scrollSpeed * Time.deltaTime;

        if (scrollbar.value >= 1.0f)
        {
            scrollbar.value = 0f;
        }
    }
}