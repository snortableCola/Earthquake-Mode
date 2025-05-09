using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CeoGambit : Minigame
{
	[SerializeField] private GameObject _selectionPanel;
	[SerializeField] private GameObject _flippingPanel;

	public TMP_Text pointsDisplay;
    public TMP_InputField pointsInput;
    public TMP_Text resultText;
    public Button headsButton;
    public Button tailsButton;
    public Button flipButton;
    public Button doneButton;
    public Button selectButton;
    public Button exitButton;

    private int points;
    private bool betOnHeads;

    public Image coinDisplay;
    public Sprite tailsImage;
    public List<Sprite> headsImages;
    public AudioSource audioSource;
    public AudioClip CoinSound;

	private PanelManager _panelManager;

    public override string Instructions { get; } = "How confident are you in your coin-flipping skills? Bet on your hard-earned cash, call heads or tails, and flip the coin. Get it right to double your bet—get it wrong, and, well… hope you didn’t bet too much.";

	private void Awake()
	{
		headsButton.onClick.AddListener(() => SelectHeads(true));
		tailsButton.onClick.AddListener(() => SelectHeads(false));
		selectButton.onClick.AddListener(SelectBet);
		flipButton.onClick.AddListener(FlipCoin);
		doneButton.onClick.AddListener(PlaceBet);
		exitButton.onClick.AddListener(ExitMinigame);
		pointsInput.contentType = TMP_InputField.ContentType.IntegerNumber;
		pointsInput.onValueChanged.AddListener(ValidateBet);
	}

	public override void StartGame()
    {
		Player player = GameManager.Instance.CurrentPlayer;
		Debug.Log($"Starting CEO Gambit for Player: {player.name}.");

        UpdatePointsDisplay();

		pointsInput.text = "1";
		selectButton.gameObject.SetActive(false);
		flipButton.gameObject.SetActive(true);
		exitButton.gameObject.SetActive(false);
		headsButton.image.color = Color.white;
		tailsButton.image.color = Color.white;
		resultText.text = "Choose Heads or Tails";

		Debug.Log(player.name);
    }

	private void ValidateBet(string input)
	{
		Player player = GameManager.Instance.CurrentPlayer;

		if (!int.TryParse(input, out points) || points < 1)
		{
			pointsInput.text = "1";
		}
		else if (points > player.Coins)
		{
			pointsInput.text = player.Coins.ToString();
		}
	}

    private void PlaceBet()
	{
		Player player = GameManager.Instance.CurrentPlayer;

		Debug.Log($"{name}, {GetInstanceID()}");
        Debug.Log("PlaceBet method called.");

        if (int.TryParse(pointsInput.text, out points) && points > 0 && points <= player.Coins)
		{
			PanelManager.Instance.ShowPanel(_selectionPanel, _panelManager._ceoSelectButton); // Show the selection panel for CeoGambit
		}
        else
        {
            pointsDisplay.text = "Please enter a valid number of points within your available points. Points: " + player.Coins;
        }
    }

	private void SelectHeads(bool isHeads)
    {
        betOnHeads = isHeads;
        headsButton.image.color = isHeads ? Color.green : Color.white;
        tailsButton.image.color = isHeads ? Color.white : Color.green;
		selectButton.gameObject.SetActive(true);
	}

	private void SelectBet() => PanelManager.Instance.ShowPanel(_flippingPanel, _panelManager._flipButton);

	private void FlipCoin()
	{
		Player player = GameManager.Instance.CurrentPlayer;

		if (CoinSound != null)
		{
			audioSource.PlayOneShot(CoinSound);
		}

		bool coinLandedHeads = Random.value > 0.5f;
		if (coinLandedHeads)
		{
			int randomIndex = Random.Range(0, headsImages.Count);
			coinDisplay.sprite = headsImages[randomIndex];
			Debug.Log($"Coin landed on Heads. Sprite: {headsImages[randomIndex].name}");
		}
        else
        {
			coinDisplay.sprite = tailsImage;
			Debug.Log($"Coin landed on Tails. Sprite: {tailsImage.name}");
		}

        bool playerWon = coinLandedHeads == betOnHeads;

        if (playerWon) player.Coins += points;
        else player.Coins -= points;

        resultText.text = $"You {(playerWon ? "won!" : "lost.")} Coin landed on {(coinLandedHeads ? "Heads" : "Tails")}. Points: {player.Coins}";

		UpdatePointsDisplay();
		flipButton.gameObject.SetActive(false);
		exitButton.gameObject.SetActive(true);
	}

	private void ExitMinigame() => MinigameManager.Instance.EndMinigame();

	private void UpdatePointsDisplay()
	{
		Player player = GameManager.Instance.CurrentPlayer;

		pointsDisplay.text = "Points: " + player.Coins.ToString();
        Debug.Log("Updating points: " + player.Coins);
    }
}