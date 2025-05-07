using UnityEngine;
using UnityEngine.UI;

public class MoneyScroll : MonoBehaviour
{
    public RawImage moneyImage;
    public Vector2 scrollSpeed = new Vector2(0.1f, 0.1f); // X and Y scroll speed
    private Vector2 offset;

    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        moneyImage.uvRect = new Rect(offset, moneyImage.uvRect.size);
    }
}
