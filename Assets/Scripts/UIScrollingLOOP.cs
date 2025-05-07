using UnityEngine;
using UnityEngine.UI;

public class UIScrollingLoop : MonoBehaviour
{
    public RectTransform background1;
    public RectTransform background2;
    public Image image1;
    public Image image2;

    public float scrollSpeedX = 50f;
    public float scrollSpeedY = 50f;
    public float fadeSpeed = 0.5f;

    private float imageWidth;
    private float imageHeight;

    void Start()
    {
        imageWidth = background1.rect.width;
        imageHeight = background1.rect.height;

        image1.color = new Color(1f, 1f, 1f, 1f);
        image2.color = new Color(1f, 1f, 1f, 0.8f);
    }

    void Update()
    {
        Vector2 move = new Vector2(scrollSpeedX, scrollSpeedY) * Time.deltaTime;

        background1.anchoredPosition += move;
        background2.anchoredPosition += move;

        float distanceX = background1.anchoredPosition.x - background2.anchoredPosition.x;
        float distanceY = background1.anchoredPosition.y - background2.anchoredPosition.y;

        if (Mathf.Abs(distanceX) >= imageWidth || Mathf.Abs(distanceY) >= imageHeight)
        {
            // Find the one that's behind and move it ahead
            if (background1.anchoredPosition.x > background2.anchoredPosition.x ||
                background1.anchoredPosition.y > background2.anchoredPosition.y)
            {
                background2.anchoredPosition = background1.anchoredPosition - new Vector2(imageWidth, imageHeight);
            }
            else
            {
                background1.anchoredPosition = background2.anchoredPosition - new Vector2(imageWidth, imageHeight);
            }

            // Swap alphas for crossfade
            float tempAlpha1 = image1.color.a;
            float tempAlpha2 = image2.color.a;

            image1.color = new Color(1f, 1f, 1f, tempAlpha2);
            image2.color = new Color(1f, 1f, 1f, tempAlpha1);
        }

        // Smoothly fade toward target alpha
        float targetAlpha1 = image1.color.a < image2.color.a ? 0.8f : 1f;
        float targetAlpha2 = image2.color.a < image1.color.a ? 0.8f : 1f;

        image1.color = Color.Lerp(image1.color, new Color(1f, 1f, 1f, targetAlpha1), fadeSpeed * Time.deltaTime);
        image2.color = Color.Lerp(image2.color, new Color(1f, 1f, 1f, targetAlpha2), fadeSpeed * Time.deltaTime);
    }
}
