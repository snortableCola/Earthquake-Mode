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
		SpinButton.onClick.AddListener(SpinChamber);
		FireButton.onClick.AddListener(FireChamber);
		ExitButton.onClick.AddListener(OnEndMinigameButtonClicked);

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
        Debug.Log($"{name}, {GetInstanceID()}");

		if (currentChamber < 0)
		{
			GeneralText.text = "Please spin the chamber first!";
            return;
		}

		// Play firing sound if available
		if (FiringSound != null)
		{
			audioSource.PlayOneShot(FiringSound);
		}

		int reward = rewards[currentChamber]; // Get the reward/penalty for the current chamber
		string message = reward switch
		{
			0 => "The chamber was empty. You're safe.",
			> 0 => $"You gained {reward} points!",
			_ => $"You lost {-reward} points.",
		};

		// Update player's points
		if (player == null)
		{
			Debug.LogError("Player reference is null! Cannot update points.");
            return;
		}

		player.AdjustPoints(reward);
		Debug.Log($"Player {player.name} now has {player.totalPoints} points after the result.");

		// Display the message to the user
		GeneralText.text = message;

		FireButton.gameObject.SetActive(false);
		ExitButton.gameObject.SetActive(true);
	}

    void OnEndMinigameButtonClicked()
    {
        // Notify the manager that the minigame is complete
        MinigameManager.Instance.MinigameCompleted(0); // Passing a 0 reward (adjust as necessary)

        // Hide game panels after completion
        panelManager.HideAllPanels();
    }
}
