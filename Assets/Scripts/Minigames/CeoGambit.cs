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
    public Button exitButton;
    public PanelManager panelManager;

    private int points;
    private bool betOnHeads;

    public Image coinDisplay;
    public Sprite tailsImage;
    public List<Sprite> headsImages;
    public AudioSource audioSource;
    public AudioClip CoinSound;

    public override void StartGame()
    {
		Player player = GameManager.Instance.CurrentPlayer;

		if (player == null)
        {
            Debug.LogError("Player is null in CEO Gambit! Ensure SetPlayer is called before starting.");
            return;
        }
        else
        {
            Debug.Log($"Starting CEO Gambit for Player: {player.name}.");
        }

        // Show the initial panel for CEO Gambit
        if (panelManager != null)
        {
            panelManager.ShowPanel("CEO Gambit", 0);
        }
        else
        {
            Debug.LogError("PanelManager is not assigned in CeoGambit!");
        }

        Debug.Log("StartGame method called.");
        UpdatePointsDisplay();
        headsButton.onClick.AddListener(() => SelectHeads(true));
        tailsButton.onClick.AddListener(() => SelectHeads(false));
        selectButton.onClick.AddListener(SelectBet);
        flipButton.onClick.AddListener(FlipCoin);
        doneButton.onClick.AddListener(PlaceBet);
        exitButton.onClick.AddListener(ExitMinigame);

        pointsInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        pointsInput.onValueChanged.AddListener(ValidateBet);

        exitButton.gameObject.SetActive(false);

        Debug.Log(player.name);
    }

    void ValidateBet(string input)
	{
		Player player = GameManager.Instance.CurrentPlayer;

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
		Player player = GameManager.Instance.CurrentPlayer;

		Debug.Log($"{name}, {GetInstanceID()}");
        Debug.Log("PlaceBet method called.");
        if (int.TryParse(pointsInput.text, out points) && points > 0 && points <= player.totalPoints)
        {
            if (panelManager != null)
            {
                panelManager.ShowPanel("CEO Gambit", 1);
            }
            else
            {
                Debug.LogError("PanelManager is not assigned in CeoGambit!");
            }
        }
        if (int.TryParse(pointsInput.text, out points) && points > 0 && points <= player.totalPoints)
        {
            Debug.Log("Placing bet with points: " + points);
            if (panelManager != null)
            {
                panelManager.ShowPanel("CEO Gambit", 1); // Show the selection panel for CeoGambit
            }
            else
            {
                Debug.LogError("PanelManager is not assigned in CeoGambit!");
            }
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
            if (panelManager != null)
            {
                panelManager.ShowPanel("CEO Gambit", 2); // Show the flip panel for CeoGambit
            }
            else
            {
                Debug.LogError("PanelManager is not assigned in CeoGambit!");
            }
        }
        else
        {
            resultText.text = "Please select Heads or Tails.";
        }
    }

    void FlipCoin()
	{
		Player player = GameManager.Instance.CurrentPlayer;

		Debug.Log("FlipCoin method called.");
        if (points > 0 && points <= player.totalPoints)
        {
            bool coinLandedHeads = Random.value > 0.5f;
            if (CoinSound != null)
            {
                audioSource.PlayOneShot(CoinSound);
            }

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

                if (betOnHeads)
                {
                    player.AdjustPoints(points);
                    resultText.text = $"You won! Coin landed on Heads. Points: {player.totalPoints}";
                }
                else
                {
                    player.AdjustPoints(-points);
                    resultText.text = $"You lost. Coin landed on Heads. Points: {player.totalPoints}";
                }
            }
            else
            {
                coinDisplay.sprite = tailsImage;
                Debug.Log($"Coin landed on Tails. Sprite: {tailsImage.name}");

                if (!betOnHeads)
                {
                    player.AdjustPoints(points);
                    resultText.text = $"You won! Coin landed on Tails. Points: {player.totalPoints}";
                }
                else
                {
                    player.AdjustPoints(-points);
                    resultText.text = $"You lost. Coin landed on Tails. Points: {player.totalPoints}";
                }
            }

            UpdatePointsDisplay();
            flipButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Please enter a valid number of points within your available points.";
            if (panelManager != null)
            {
                panelManager.ShowPanel("CEO Gambit", 1);
            }
            else
            {
                Debug.LogError("PanelManager is not assigned in CeoGambit!");
            }
        }
    }

    public override void Cleanup()
    {
        base.Cleanup();

        // Remove all listeners from buttons
        headsButton.onClick.RemoveAllListeners();
        tailsButton.onClick.RemoveAllListeners();
        selectButton.onClick.RemoveAllListeners();
        flipButton.onClick.RemoveAllListeners();
        doneButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        // Reset input field state and clear previous input
        pointsInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        pointsInput.text = string.Empty; // Clear any previous input
        pointsInput.onValueChanged.RemoveAllListeners(); // Remove previous listeners
        pointsInput.onValueChanged.AddListener(ValidateBet);

        // Hide exit button
        exitButton.gameObject.SetActive(false);

        // Reset any other UI elements to their default states
        headsButton.gameObject.SetActive(true);
        tailsButton.gameObject.SetActive(true);
        selectButton.gameObject.SetActive(false);
        flipButton.gameObject.SetActive(false);
        doneButton.gameObject.SetActive(false);

        // Reset text fields or other UI elements if needed
        resultText.text = "Choose Heads or Tails";

        // Reset any internal variables used in the minigame
        betOnHeads = false;

        Debug.Log("Cleanup completed and UI reset for the new player.");
    }

    public void ExitMinigame()
    {
        Debug.Log("Exiting minigame.");
       
        MinigameManager.Instance.EndCurrentMinigame();
        panelManager.ShowMovementUI(); 
    }

    void UpdatePointsDisplay()
	{
		Player player = GameManager.Instance.CurrentPlayer;

		pointsDisplay.text = "Points: " + player.totalPoints.ToString();
        Debug.Log("Updating points: " + player.totalPoints);
    }
}