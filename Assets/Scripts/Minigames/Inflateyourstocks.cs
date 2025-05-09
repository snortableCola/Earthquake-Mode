using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Inflateyourstocks : Minigame
{
    [Header("Player Settings")]
    public Slider[] playerHealthbars; // Sliders representing progress for each player
    public Button[] playerButtons; // Buttons for each player
    public Button exitButton; // Exit button to leave the minigame

    [Header("Game Settings")]
    public float maxHealth = 1f; // Maximum value for the slider
    public float inflationPerMash = 0.05f; // How much the slider increases with each button mash
    public float mashCooldown = 0.1f; // Cooldown between button presses to prevent spamming
    public float gameDuration = 6f; // Duration of the game in seconds

    [SerializeField] private TMP_Text winnerText; // Text to display the winner
    [SerializeField] private TMP_Text inflateText; // Text to display the timer

    private bool[] hasFinished; // Tracks if a player has finished (reached max value)
    private float[] lastMashTime; // Tracks the last mash time for each player
    private bool _gameFinished = false; // Tracks if the game is finished
    private float _timeRemaining; // Tracks the remaining time

    public override string Instructions { get; } = "Desperately try to buy back as many of your company’s stocks as possible before the market closes. The more you button mash your A button or click on the buy button, the more shares you buy ,inflating your company's value—but only one exec can come out on top! ";

    public override void StartGame()
    {
        // Initialize timer and variables
        _timeRemaining = gameDuration;
        inflateText.gameObject.SetActive(true);
        winnerText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false); 

        int playerCount = playerHealthbars.Length;
        hasFinished = new bool[playerCount];
        lastMashTime = new float[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            playerHealthbars[i].maxValue = maxHealth;
            playerHealthbars[i].value = 0; // Start at 0
            int playerIndex = i; // Capture index for lambda
            playerButtons[i].onClick.AddListener(() => HandleMash(playerIndex)); // Assign button listeners
        }

        exitButton.onClick.AddListener(ExitGame); // Assign listener to the exit button
    }

    private void Update()
    {
        if (_gameFinished) return;

        // Decrease the timer
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            inflateText.text = $"Time To Inflate: {Mathf.CeilToInt(_timeRemaining)}s";
        }
        else
        {
            // Time is up, determine the winner
            DetermineWinner();
            return;
        }

        // Handle button mashing from gamepads
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (i >= playerHealthbars.Length) break; // Ensure we don't exceed the number of players

            var gamepad = Gamepad.all[i];
            if (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame) // Example: "A" button on gamepad
            {
                HandleMash(i);
            }
        }

        // Keyboard support for testing (e.g., spacebar for Player 1)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            HandleMash(0); // Player 1
        }
    }

    private void HandleMash(int playerIndex)
    {
        if (_gameFinished || hasFinished[playerIndex] || Time.time - lastMashTime[playerIndex] < mashCooldown) return;

        // Increase the slider value for the player
        playerHealthbars[playerIndex].value += inflationPerMash;

        // Check if this player has reached the max value
        if (playerHealthbars[playerIndex].value >= maxHealth)
        {
            hasFinished[playerIndex] = true;
            EndGame(playerIndex);
        }

        // Record the time of this mash
        lastMashTime[playerIndex] = Time.time;
    }

    private void DetermineWinner()
    {
        _gameFinished = true;
        inflateText.gameObject.SetActive(false);

        // Find the player closest to max value
        int winningPlayer = -1;
        float closestValue = 0f;

        for (int i = 0; i < playerHealthbars.Length; i++)
        {
            float playerValue = playerHealthbars[i].value;

            if (playerValue > closestValue)
            {
                closestValue = playerValue;
                winningPlayer = i;
            }
        }

        // Display winner or draw
        if (winningPlayer >= 0)
        {
            EndGame(winningPlayer);
        }
        else
        {
            winnerText.text = "It's a Draw!";
            winnerText.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
    }

    public void EndGame(int winningPlayer)
    {
        _gameFinished = true;

        inflateText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);

        // Display the winner text
        winnerText.text = $"Player {winningPlayer + 1} Wins! +3 Corruption Coins!";
        winnerText.gameObject.SetActive(true);

        // Award coins to the winning player
        GameManager.Instance.Players[winningPlayer].Coins += 3;

        Debug.Log($"Player {winningPlayer + 1} wins with {GameManager.Instance.Players[winningPlayer].Coins} coins!");

        // Enable the exit button
        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(true);
        }

        // Disable all player buttons
        foreach (var button in playerButtons)
        {
            button.interactable = false;
        }
    }

    private void ExitGame()
    {
        Debug.Log("Exit button clicked. Exiting the minigame.");
        MinigameManager.Instance.EndMinigame();
        Cleanup();
    }

    public void Cleanup()
    {
        // Reset health bars and re-enable buttons
        for (int i = 0; i < playerHealthbars.Length; i++)
        {
            playerHealthbars[i].value = 0; // Reset slider value

            playerButtons[i].interactable = true; // Re-enable buttons
            hasFinished[i] = false; // Reset finished state
        }

        inflateText.gameObject.SetActive(true);

        // Reset game state
        winnerText.gameObject.SetActive(false); // Hide winner text

        _gameFinished = false;
        _timeRemaining = gameDuration;
    }
}

