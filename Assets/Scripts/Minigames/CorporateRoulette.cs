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
        if (MinigameManager.Instance.testMode)
        {
            Debug.Log("Test Mode: Starting Corporate Roulette without a Player.");
            player = MinigameManager.Instance.fakeplayer;

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
        if (player == null)
        {
            Debug.LogError("Player is null in CorporateRoulette! Ensure SetPlayer is called before starting.");
            return;
        }
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
    public override void SetPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogError("SetPlayer called with a null Player in CorporateRoulette.");
            return;
        }

        base.SetPlayer(player); // Call the base class method
        Debug.Log($"Player {player.name} assigned to CorporateRoulette.");
    }
    void FireChamber()
    {
        if (MinigameManager.Instance.testMode)
        {
            Debug.Log("Test Mode: Skipping player point adjustments.");
            
        }
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
    public override void Cleanup()
    {
        base.Cleanup(); // Call shared cleanup logic from Minigame

        // Remove button listeners specific to CorporateRoulette
        SpinButton.onClick.RemoveAllListeners();
        FireButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        // Reset buttons to their default state
        SpinButton.gameObject.SetActive(true);
        FireButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);

        // Reset button text
        TMP_Text spinButtonText = SpinButton.GetComponentInChildren<TMP_Text>();
        if (spinButtonText != null)
        {
            spinButtonText.text = "Spin";
        }

        Debug.Log("CorporateRoulette cleanup completed.");
    }

    void OnEndMinigameButtonClicked()
    {
        // Notify the manager that the minigame is complete
        MinigameManager.Instance.MinigameCompleted(0); // Passing a 0 reward (adjust as necessary)

        // Hide game panels after completion
        panelManager.HideAllPanels();
    }
}
