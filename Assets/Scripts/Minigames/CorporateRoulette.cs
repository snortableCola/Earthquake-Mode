using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorporateRoulette : Minigame
{
	[SerializeField] private GameObject[] _panels;

	public Button SpinButton;
    public Button FireButton;
    public Button ExitButton;
    public TMP_Text GeneralText;
    private readonly int[] rewards = { 0, 0, -3, -4, 6, 4 };
    private int currentChamber = -1; // Tracks the selected chamber

    public AudioSource audioSource;
    public AudioClip FiringSound;

	public override string Instructions { get; } = "Spin the chamber and take your shot. Some bullets earn you points (+6,+4), others hit you where it hurts (-3, -4 points), and a couple just click harmlessly. Will luck be on your side?";

	public override GameObject[] MinigamePanels => _panels;

	private void Awake()
	{
		// Add listeners to the buttons
		SpinButton.onClick.AddListener(OnSpinClicked);
		FireButton.onClick.AddListener(OnFireClicked);
		ExitButton.onClick.AddListener(OnExitClicked);
	}

	public override void StartGame()
	{
		Debug.Log("CorporateRoulette Start method called.");

		Player player = GameManager.Instance.CurrentPlayer;

		Debug.Log($"Starting Corporate Roulette for player: {player.name} with {player.Points} points.");

		PanelManager.Instance.ShowPanel(this, 0);

		ExitButton.gameObject.SetActive(false);
        FireButton.gameObject.SetActive(false);
    }

    public void OnSpinClicked()
    {
        Debug.Log("SpinChamber() initiated");
        for (int i = rewards.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
			(rewards[randomIndex], rewards[i]) = (rewards[i], rewards[randomIndex]);
		}

		currentChamber = Random.Range(0, rewards.Length);
        GeneralText.text = "The chamber has stopped spinning...";
        FireButton.gameObject.SetActive(true);
        SpinButton.gameObject.SetActive(false);
        Debug.Log("SpinChamber method completed.");
    }

    
    public void OnFireClicked()
	{
		Player player = GameManager.Instance.CurrentPlayer;

		if (currentChamber < 0)
        {
            GeneralText.text = "Please spin the chamber first!";
            return;
        }

        if (FiringSound != null)
        {
            audioSource.PlayOneShot(FiringSound);
        }

        int reward = rewards[currentChamber];
        string message = reward switch
        {
            0 => "The chamber was empty. You're safe.",
            > 0 => $"You gained {reward} points!",
            _ => $"You lost {-reward} points.",
        };

        player.Points += reward;
        Debug.Log($"Player {player.name} now has {player.Points} points after the result.");

        GeneralText.text = message;

        FireButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(true);
    }

    public void Cleanup()
    {
		Debug.Log($"Cleaning up minigame: {name} {GetInstanceID()}");

		// Hide any related UI
		PanelManager.Instance.HideAllMinigamePanels(this);

		// Remove all listeners from buttons
		SpinButton.onClick.RemoveAllListeners();
        FireButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        // Reset button states
        SpinButton.gameObject.SetActive(true);
        FireButton.gameObject.SetActive(false);
		ExitButton.gameObject.SetActive(false);

		// Reset text fields
		GeneralText.text = "Spin the chamber to start.";

        // Reset internal variables
        currentChamber = -1;

        // Reset button text if needed
        TMP_Text spinButtonText = SpinButton.GetComponentInChildren<TMP_Text>();
        if (spinButtonText != null)
        {
            spinButtonText.text = "Spin";
        }

        Debug.Log("CorporateRoulette cleanup completed.");
    }

    public void OnExitClicked()
    {
        MinigameManager.Instance.EndMinigame();

        PanelManager panelManager = PanelManager.Instance;
        panelManager.HideAllMinigamePanels(this);
        panelManager.ShowMovementUI();
    }
}
