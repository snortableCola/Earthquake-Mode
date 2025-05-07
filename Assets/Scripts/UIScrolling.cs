using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class UIScrollingBackground : MonoBehaviour
{
    public RectTransform background;
    public float scrollSpeed = 50f;

    void Update()
    {
        background.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        
        if (background.anchoredPosition.x <= -background.rect.width / 2)
        {
            background.anchoredPosition = new Vector2(0, background.anchoredPosition.y);
        }
    }
}
