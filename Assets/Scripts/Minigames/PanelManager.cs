using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; } // Singleton instance

	[SerializeField] private GameObject _movementUI;
	[SerializeField] private GameObject _instructionPanel;
	[SerializeField] private TMP_Text _instructionText;
	[SerializeField] private TMP_Text _gameNameText;
	[SerializeField] private Button _startMinigameButton;
	[SerializeField] private GameObject _shopPanel; 

	private GameObject _activePanel;
	private Minigame _startingMinigame;

	private void Awake()
	{
		Instance = this;
        _startMinigameButton.onClick.AddListener(OnStartMinigameButtonClicked);
	}

	private void OnStartMinigameButtonClicked()
	{
		ShowPanel(_startingMinigame.InitialPanel);
		_startingMinigame.StartGame(); // Start the minigame
	}

	public void ShowInstructionPanel(Minigame minigame)
	{
		_startingMinigame = minigame;

		_instructionText.text = minigame.Instructions;
		_gameNameText.text = minigame.name;

		ShowPanel(_instructionPanel);
	}

	public void ShowMovementUI() => ShowPanel(_movementUI);

	public void ShowShop() => ShowPanel(_shopPanel);

	public void ShowPanel(GameObject panel)
	{
		if (_activePanel) _activePanel.SetActive(false);
		_activePanel = panel;
		if (panel) panel.SetActive(true);
	}
}