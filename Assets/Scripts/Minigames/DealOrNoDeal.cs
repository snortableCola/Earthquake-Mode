using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DealOrNoDeal : Minigame
{
	[SerializeField] private GameObject[] _panels;

    [SerializeField] private Button[] _suitcaseButtons;
	[SerializeField] private Button _selectButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private Color _highlightColor;
    [SerializeField] private GameObject _initialTextObject; // Reference to the GameObject containing the initial text

    private readonly int[] _possibleRewards = { 0, 0, 4, -4 };
    private int _selectedReward;
	private Button _selectedSuitcase;
    private Color _originalColor;

	public override string Instructions { get; } = "Four suitcases lie before you. One will give you +4 points, two are empty, and one will give you -4 points. The suitcases will be shuffled and you must choose wisely...";

	public override GameObject[] MinigamePanels => _panels;

	private void Awake()
	{
		for (int i = 0; i < _suitcaseButtons.Length; i++)
		{
			Button suitcase = _suitcaseButtons[i];
			int index = i;

			suitcase.onClick.AddListener(SuitcaseResponse);
			void SuitcaseResponse() => OnSuitcaseClicked(suitcase, index);
		}

		_selectButton.onClick.AddListener(OnSelectClicked);
		_exitButton.onClick.AddListener(OnExitClicked);

		_selectedSuitcase = _suitcaseButtons[0]; // "Default" selected suitcase is the first one
		_originalColor = _selectedSuitcase.image.color;
	}

	public override void StartGame()
	{
		Player player = GameManager.Instance.CurrentPlayer;
		Debug.Log($"Starting DealOrNoDeal for Player: {player.name}.");

        PanelManager.Instance.ShowPanel(this, 0); // Show the first panel (instructions)

        ShuffleSuitcases();

        _rewardText.text = "";
        _selectButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
		_initialTextObject.SetActive(true);
		foreach (Button suitcase in _suitcaseButtons)
		{
			suitcase.enabled = true;
		}
	}

	// Shuffle the rewards array
	private void ShuffleSuitcases()
    {
        for (int i = _possibleRewards.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
			(_possibleRewards[i], _possibleRewards[rnd]) = (_possibleRewards[rnd], _possibleRewards[i]);
		}
	}

	private void OnSuitcaseClicked(Button suitcase, int index)
	{
		_selectedSuitcase.image.color = _originalColor;

		_selectedSuitcase = suitcase;
		suitcase.image.color = _highlightColor;

		_selectedReward = _possibleRewards[index];
        _selectButton.gameObject.SetActive(true);
    }

	private void OnSelectClicked()
	{
		foreach (Button suitcase in _suitcaseButtons)
		{
			suitcase.enabled = false;
		}

		Player player = GameManager.Instance.CurrentPlayer;

		Debug.Log($"{name}, {GetInstanceID()}");
        Debug.Log($"Player {player.name} selected a suitcase with reward {_selectedReward}.");

        player.Points += _selectedReward;

        _rewardText.text = $"You got: {_selectedReward} points! Total Points: {player.Points}";
        Debug.Log($"Selected Reward: {_selectedReward}");
        Debug.Log($"Player {player.name} now has {player.Points} points after adjustment.");

		_initialTextObject.SetActive(false);
		_selectButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(true);
	}

	private void OnExitClicked()
	{
		// Reset the color of all suitcases to their original color
		foreach (var suitcase in _suitcaseButtons)
		{
			suitcase.image.color = _originalColor;
		}

		// Notify the manager that the minigame is complete
		MinigameManager.Instance.EndMinigame();
	}
}