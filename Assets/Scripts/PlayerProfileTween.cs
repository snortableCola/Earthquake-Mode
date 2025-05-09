using UnityEngine;

public class PlayerProfileTween : MonoBehaviour
{
    [SerializeField] private RectTransform[] profiles;
    [SerializeField] private float moveDist;
    [SerializeField] private float moveTime;
    [SerializeField] private RectTransform[] destinations;
    private RectTransform[] startPoints;


    private void Start()
    {
        // Store the starting positions of the profiles
        startPoints = new RectTransform[profiles.Length];
        for (int i = 0; i < profiles.Length; i++)
        {
            startPoints[i] = profiles[i];
        }
    }

    private void OnNewTurnStarted(int playerIndex)
    {
        // use leantween to move the profile on the y axis that matches the player index based on movedist and movetime
        //LeanTween.moveLocalY(profiles[playerIndex].gameObject, destinations[playerIndex].position.y, moveTime).setEaseInOutSine();
        // profiles[playerIndex].anchoredPosition = new Vector2(profiles[playerIndex].anchoredPosition.x, moveDist);
    }

    private void OnTurnEnded(int playerIndex)
    {
        //profiles[playerIndex].anchoredPosition = new Vector2(profiles[playerIndex].anchoredPosition.x, -moveDist);
        //LeanTween.moveLocalY(profiles[playerIndex].gameObject, startPoints[playerIndex].position.y, moveTime).setEaseInOutSine();
        Debug.Log("Turn ended for player: " + playerIndex);

    }

    private void OnEnable()
    {
        GameManager.NewTurnStarted += OnNewTurnStarted;
        GameManager.TurnEnded += OnTurnEnded;
    }
    private void OnDisable()
    {
        GameManager.NewTurnStarted -= OnNewTurnStarted;
        GameManager.TurnEnded -= OnTurnEnded;
    }
}
