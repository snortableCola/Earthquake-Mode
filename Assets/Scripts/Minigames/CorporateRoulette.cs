using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorporateRoulette : Minigame
{
    public Button SpinButton;
    public Button FireButton;
    public Button ExitButton;
    public TMP_Text GeneralText;
    private int[] rewards = { 0, 0, -3, -4, 6, 4 };
    private int currentChamber = -1; // Tracks the selected chamber

    public AudioSource audioSource;
    public AudioClip FiringSound;
    public PanelManager panelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add listeners to the buttons
        SpinButton.onClick.AddListener(SpinChamber);
        FireButton.onClick.AddListener(FireChamber);
        ExitButton.onClick.AddListener(OnEndMinigameButtonClicked);
    }

    public override void StartGame()
    {
        // Show the initial panel for Corporate Roulette
        panelManager.ShowPanel("Corporate Roulette", 0);
        ExitButton.gameObject.SetActive(false);
        FireButton.gameObject.SetActive(false);
        Debug.Log($"Starting Corporate Roulette for player: {player.name} with {player.totalPoints} points.");
    }

    void SpinChamber()
    {
        // Shuffle the rewards array
        for (int i = rewards.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = rewards[i];
            rewards[i] = rewards[randomIndex];
            rewards[randomIndex] = temp;
        }

        // Set the current chamber randomly
        currentChamber = Random.Range(0, rewards.Length);
        GeneralText.text = "The chamber has stopped spinning...";
        FireButton.gameObject.SetActive(true);
        SpinButton.gameObject.SetActive(false);
    }

    void FireChamber()
    {
        if (currentChamber >= 0)
        {
            // Play firing sound if available
            if (FiringSound != null)
            {
                audioSource.PlayOneShot(FiringSound);
            }

            int reward = rewards[currentChamber]; // Get the reward/penalty for the current chamber
            string message;

            // Customize messages based on the reward value
            if (reward == 0)
            {
                message = "The chamber was empty. You're safe.";
            }
            else if (reward > 0)
            {
                message = $"You gained {reward} points!";
            }
            else
            {
                message = $"You lost {-reward} points.";
            }

            // Update player's points
            if (player != null)
            {
                player.AdjustPoints(reward);
                Debug.Log($"Player {player.name} now has {player.totalPoints} points after the result.");
            }
            else
            {
                Debug.LogError("Player reference is null! Cannot update points.");
            }

            // Display the message to the user
            GeneralText.text = message;

            FireButton.gameObject.SetActive(false);
            ExitButton.gameObject.SetActive(true);
        }
        else
        {
            GeneralText.text = "Please spin the chamber first!";
        }
    }

    void OnEndMinigameButtonClicked()
    {
        // Notify the manager that the minigame is complete
        MinigameManager.Instance.MinigameCompleted(0); // Passing a 0 reward (adjust as necessary)

        // Hide game panels after completion
        panelManager.HideAllPanels();
    }
}
