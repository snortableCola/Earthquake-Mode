using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorporateRoulette : Minigame
{
	[SerializeField] private Button _spinButton;
	[SerializeField] private Button _fireButton;
	[SerializeField] private Button _exitButton;

    public TMP_Text GeneralText;
    private readonly int[] rewards = { 0, 0, -3, -4, 6, 4 };
    private int currentChamber = -1;

    public AudioSource audioSource;
    public AudioClip FiringSound;

	public override string Instructions { get; } = "Spin the chamber and take your shot. Some bullets earn you points (+6,+4), others hit you where it hurts (-3, -4 points), and a couple just click harmlessly. Will luck be on your side?";

	private void Awake()
	{
		_spinButton.onClick.AddListener(OnSpinClicked);
		_fireButton.onClick.AddListener(OnFireClicked);
		_exitButton.onClick.AddListener(OnExitClicked);
	}

	public override void StartGame()
	{
		Player player = GameManager.Instance.CurrentPlayer;
		Debug.Log($"Starting Corporate Roulette for player: {player.name} with {player.Coins} points.");

		_spinButton.gameObject.SetActive(true);
		_exitButton.gameObject.SetActive(false);
        _fireButton.gameObject.SetActive(false);

		GeneralText.text = "Spin the chamber to start.";

		currentChamber = -1;
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
        _fireButton.gameObject.SetActive(true);
        _spinButton.gameObject.SetActive(false);
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

        player.Coins += reward;
        Debug.Log($"Player {player.name} now has {player.Coins} points after the result.");

        GeneralText.text = message;

        _fireButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(true);
    }

    public void Cleanup()
    {
        // Reset button states
        _spinButton.gameObject.SetActive(true);
        _fireButton.gameObject.SetActive(false);
		_exitButton.gameObject.SetActive(false);

		// Reset text fields
		GeneralText.text = "Spin the chamber to start.";

        // Reset internal variables
        currentChamber = -1;

        Debug.Log("CorporateRoulette cleanup completed.");
    }

	public void OnExitClicked() => MinigameManager.Instance.EndMinigame();
}
