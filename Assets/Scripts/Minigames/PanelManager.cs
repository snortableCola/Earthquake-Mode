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

	private GameObject _activePanel;
	private Minigame _startingMinigame;

	private void Awake()
	{
		Instance = this;
		_activePanel = _movementUI;
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

	public void ShowPanel(GameObject panel)
	{
		_activePanel.SetActive(false);
		_activePanel = panel;
		panel.SetActive(true);
	}
}