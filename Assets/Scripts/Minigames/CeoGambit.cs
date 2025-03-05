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
    public Button doneButton; // Reference to the Done button
    public Button selectButton; // Reference to the Select button
    public Button ExitButton; 
    public Player player;
    public PanelManager panelManager; // Reference to the PanelManager script

    private int points;
    private bool betOnHeads;
    void Start()
    {
        StartGame();
    }
    public override void StartGame()
    {
        Debug.Log("start game method called");
        UpdatePointsDisplay();
        headsButton.onClick.AddListener(() => SelectHeads(true));
        tailsButton.onClick.AddListener(() => SelectHeads(false));
        selectButton.onClick.AddListener(SelectBet); // Set up the Select button
        flipButton.onClick.AddListener(FlipCoin);
        doneButton.onClick.AddListener(PlaceBet); // Set up the Done button

        // Limit the input field to only accept numbers
        pointsInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        pointsInput.onValueChanged.AddListener(ValidateBet);
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
            panelManager.ShowSelectionPanel(); // Move to the selection panel
           
        }
        else
        {
            pointsDisplay.text = "Please enter a valid number of points within your available points." + "Points:" + player.totalPoints;
        }
    }

    void SelectHeads(bool isHeads)
    {
        betOnHeads = isHeads;
        headsButton.image.color = isHeads ? Color.green : Color.white; // Highlight the selected button
        tailsButton.image.color = isHeads ? Color.white : Color.green; // Reset the other button
    }

    void SelectBet()
    {
        if (headsButton.image.color == Color.green || tailsButton.image.color == Color.green)
        {
            panelManager.ShowFlipPanel(); // Move to the flip panel
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
            UpdatePointsDisplay();
            // Disable flip button and show exit button
            flipButton.gameObject.SetActive(false);
            ExitButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Please enter a valid number of points within your available points.";
            panelManager.ShowBettingPanel(); // Return to the betting panel if input is invalid
        }
    }
   public void ExitMinigame()
    {
        // Logic to exit the minigame (e.g., return to main menu)
        Debug.Log("Exiting minigame");
        panelManager.EndCG();
    }

    void UpdatePointsDisplay()
    {
        pointsDisplay.text = "Points: " + player.totalPoints.ToString();
        Debug.Log("Updating points: " + player.totalPoints); 
    }
}
