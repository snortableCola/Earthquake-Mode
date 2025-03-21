using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public PanelManager panelManager;

    private int points;
    private bool betOnHeads;

    public Image coinDisplay; // Reference to the UI Image for displaying results
    public Sprite tailsImage; // The single image for tails
    public List<Sprite> headsImages;
    public AudioSource audioSource;
    public AudioClip CoinSound;

    public override void StartGame()
    {
        if (MinigameManager.Instance.testMode)
        {
            Debug.Log("Test Mode: Starting Corporate Roulette without a Player.");
            player = MinigameManager.Instance.fakeplayer;
            player.totalPoints = MinigameManager.Instance.fakeplayer.totalPoints;
            
        }
        else if (player == null)
        {
            Debug.LogError("Player is null in CorporateRoulette! Ensure SetPlayer is called before starting.");
            return;
        }
        else
        {
            Debug.Log($"Starting Corporate Roulette for Player: {player.name}.");
        }
        // Show the initial panel for CEO Gambit
        panelManager.ShowPanel("Ceo Gambit", 0);
        Debug.Log("StartGame method called.");
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

		Debug.Log(player.name);
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
        Debug.Log($"{name}, {GetInstanceID()}");
		Debug.Log("PlaceBet method called.");
        if (MinigameManager.Instance.testMode && int.TryParse(pointsInput.text, out points) && points > 0 && points <= MinigameManager.Instance.fakeplayer.totalPoints)
        {
           
            panelManager.ShowPanel("Ceo Gambit", 1);
        }
        if (int.TryParse(pointsInput.text, out points) && points > 0 && points <= MinigameManager.Instance.fakeplayer.totalPoints)
        {
            Debug.Log("Placing bet with points: " + points);
            panelManager.ShowPanel("Ceo Gambit", 1); // Show the selection panel for CeoGambit
        }
        else
        {
            pointsDisplay.text = "Please enter a valid number of points within your available points. Points: " + MinigameManager.Instance.fakeplayer.totalPoints;
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
            panelManager.ShowPanel("Ceo Gambit", 2); // Show the flip panel for CeoGambit
        }
        else
        {
            resultText.text = "Please select Heads or Tails.";
        }
    }

    void FlipCoin()
    {
       
        Debug.Log("FlipCoin method called.");
        if (points > 0 && points <= player.totalPoints)
        {
            bool coinLandedHeads = Random.value > 0.5f; // Determine heads or tails
            if (CoinSound != null)
            {
                audioSource.PlayOneShot(CoinSound);
            }

            // Set the correct image and debug log for heads or tails
            if (coinLandedHeads)
            {
                if (headsImages.Count > 0)
                {
                    int randomIndex = Random.Range(0, headsImages.Count);
                    coinDisplay.sprite = headsImages[randomIndex];
                    Debug.Log($"Coin landed on Heads. Sprite: {headsImages[randomIndex].name}");
                }
                else
                {
                    Debug.LogError("HeadsImages list is empty! Please assign sprites in the Inspector.");
                }

                // Player bet correctly (landed on heads and bet on heads)
                if (betOnHeads)
                {
                    player.AdjustPoints(points);
                    resultText.text = $"You won! Coin landed on Heads. Points: {player.totalPoints}";
                }
                else
                {
                    // Player bet incorrectly (landed on heads but bet on tails)
                    player.AdjustPoints(-points);
                    resultText.text = $"You lost. Coin landed on Heads. Points: {player.totalPoints}";
                }
            }
            else
            {
                // Landed on Tails
                coinDisplay.sprite = tailsImage;
                Debug.Log($"Coin landed on Tails. Sprite: {tailsImage.name}");

                // Player bet correctly (landed on tails and bet on tails)
                if (!betOnHeads)
                {
                    player.AdjustPoints(points);
                    resultText.text = $"You won! Coin landed on Tails. Points: {player.totalPoints}";
                }
                else
                {
                    // Player bet incorrectly (landed on tails but bet on heads)
                    player.AdjustPoints(-points);
                    resultText.text = $"You lost. Coin landed on Tails. Points: {player.totalPoints}";
                }
            }

            // Update UI and game state
            UpdatePointsDisplay();
            flipButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Please enter a valid number of points within your available points.";
            panelManager.ShowPanel("Ceo Gambit", 1);
        }
    }
    public override void Cleanup()
    {
        headsButton.onClick.RemoveAllListeners();
        tailsButton.onClick.RemoveAllListeners();
        selectButton.onClick.RemoveAllListeners();  
        flipButton.onClick.RemoveAllListeners();
        doneButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        pointsInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        pointsInput.onValueChanged.AddListener(ValidateBet);

        // Initially, make the exit button inactive
        exitButton.gameObject.SetActive(false);
    }

    public void ExitMinigame()
    {
        Debug.Log("Exiting minigame.");

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