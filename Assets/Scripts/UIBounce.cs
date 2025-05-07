using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundBounce : MonoBehaviour
{
    public RectTransform background;
    public Image backgroundImage;
    public float speed = 50f;
    public Vector2 direction = new Vector2(1f, 1f); // Diagonal direction
    public float maxOffset = 100f; // Control the bounce distance
    public float fadeRange = 50f;
    public bool enableFade = true;

    private Vector2 startPos;
    private Vector2 currentDirection;
    private float traveled = 0f;

    void Start()
    {
        startPos = background.anchoredPosition;
        currentDirection = direction.normalized;
    }

    void Update()
    {
        // Smooth bounce easing
        float easedSpeed = speed * Mathf.Sin((traveled / maxOffset) * Mathf.PI);

        Vector2 move = currentDirection * easedSpeed * Time.deltaTime;
        background.anchoredPosition += move;
        traveled += move.magnitude;

        // Reverse direction once maxOffset is reached
        if (traveled >= maxOffset)
        {
            currentDirection = -currentDirection;
            traveled = 0f;
        }

        // Optional fading at edges
        if (enableFade && backgroundImage != null)
        {
            float alpha = 1f;

            if (traveled > maxOffset - fadeRange)
                alpha = Mathf.Lerp(1f, 0.5f, (traveled - (maxOffset - fadeRange)) / fadeRange);
            else if (traveled < fadeRange)
                alpha = Mathf.Lerp(0.5f, 1f, traveled / fadeRange);

            backgroundImage.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
