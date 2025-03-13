using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CeoGambit : Minigame
{
    public TMP_Text pointsDisplay;
    public TMP_InputField pointsInput;
    public TMP_Text resultText;
    public Button headsButton;
    public Button tailsButton;
    public Button flipButton;
    public Button doneButton;
    public Button selectButton;
    public Button exitButton; // Reference to the Exit button
    public Player player;
    public PanelManager panelManager;

    private int points;
    private bool betOnHeads;

    public override void StartGame()
    {
        Debug.Log("start game method called");
        UpdatePointsDisplay();
        headsButton.onClick.AddListener(() => SelectHeads(true));
        tailsButton.onClick.AddListener(() => SelectHeads(false));
        selectButton.onClick.AddListener(SelectBet);
        flipButton.onClick.AddListener(FlipCoin);
        doneButton.onClick.AddListener(PlaceBet);
        exitButton.onClick.AddListener(ExitMinigame); // Add listener to the exit button

        pointsInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        pointsInput.onValueChanged.AddListener(ValidateBet);

        // Initially, make the exit button inactive
        exitButton.gameObject.SetActive(false);

        // Show the initial panel for CEO Gambit
        panelManager.ShowPanel("Ceo Gambit", 0);
    }

    void ValidateBet(string input)
    {
        if (int.TryParse(input, out points))
        {
            if (points > player.totalPoints)
            {
                pointsInput.text = player.totalPoints.ToString();
            }
            else if (points < 0)
            {
                pointsInput.text = "0";
            }
        }
        else
        {
            pointsInput.text = "0";
        }
    }

    public void PlaceBet()
    {
        Debug.Log("PlaceBet method called");
        if (int.TryParse(pointsInput.text, out points) && points > 0 && points <= player.totalPoints)
        {
            Debug.Log("Placing Bet with points: " + points);
            panelManager.ShowPanel("Ceo Gambit", 2); // Show the selection panel for CeoGambit
        }
        else
        {
            pointsDisplay.text = "Please enter a valid number of points within your available points. Points: " + player.totalPoints;
        }
    }

    void SelectHeads(bool isHeads)
    {
        betOnHeads = isHeads;
        headsButton.image.color = isHeads ? Color.green : Color.white;
        tailsButton.image.color = isHeads ? Color.white : Color.green;
    }

    void SelectBet()
    {
        if (headsButton.image.color == Color.green || tailsButton.image.color == Color.green)
        {
            panelManager.ShowPanel("Ceo Gambit", 3); // Show the flip panel for CeoGambit
        }
        else
        {
            resultText.text = "Please select Heads or Tails.";
        }
    }

    void FlipCoin()
    {
        if (points > 0 && points <= player.totalPoints)
        {
            bool coinLandedHeads = Random.value > 0.5f;

            if (coinLandedHeads == betOnHeads)
            {
                player.AdjustPoints(points);
                resultText.text = "You won! Coin landed on " + (coinLandedHeads ? "Heads" : "Tails") + ". Points: " + player.totalPoints;
            }
            else
            {
                player.AdjustPoints(-points);
                resultText.text = "You lost. Coin landed on " + (coinLandedHeads ? "Heads" : "Tails") + ". Points: " + player.totalPoints;
            }

            UpdatePointsDisplay();

            // Make the flip button inactive and the exit button active
            flipButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Please enter a valid number of points within your available points.";
            panelManager.ShowPanel("Ceo Gambit", 1); // Show the betting panel for CeoGambit
        }
    }

    public void ExitMinigame()
    {
        Debug.Log("Exiting minigame");

        // Notify the manager that the minigame is complete
        MinigameManager.Instance.MinigameCompleted(0);

        panelManager.EndMinigame("Ceo Gambit");
    }

    void UpdatePointsDisplay()
    {
        pointsDisplay.text = "Points: " + player.totalPoints.ToString();
        Debug.Log("Updating points: " + player.totalPoints);
    }
}
