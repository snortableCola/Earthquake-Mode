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
    [SerializeField] private GameObject _shopPanelGameObject; // Reference to the ShopPanel GameObject
    [SerializeField] private GameObject _diceButton;
    [SerializeField] private GameObject _shopButton;
    [SerializeField] private GameObject _instructionButton;
    [SerializeField] public GameObject _victimSelectButton;
    [SerializeField] public GameObject _methodSelectButton;
    [SerializeField] public GameObject _consequenceButton;
    [SerializeField] public GameObject _ceoSelectButton;
    [SerializeField] public GameObject _flipButton;
    [SerializeField] public GameObject _oilPassButton;
    [SerializeField] public GameObject _ceoFireButton;
    private ShopPanel _shopPanelScript; // Reference to the ShopPanel script

    private GameObject _activePanel;
	private Minigame _startingMinigame;
    

    private void Awake()
    {
        Instance = this;
        _startMinigameButton.onClick.AddListener(OnStartMinigameButtonClicked);
        if (_shopPanelGameObject != null)
        {
            _shopPanelGameObject.SetActive(false);
            _shopPanelScript = _shopPanelGameObject.GetComponent<ShopPanel>();

            if (_shopPanelScript == null)
            {
                Debug.LogError("The assigned ShopPanel GameObject does not have a ShopPanel script attached!");
            }
        }
    }

	private void OnStartMinigameButtonClicked()
	{
		ShowPanel(_startingMinigame.InitialPanel, _startingMinigame._startButton);
		_startingMinigame.StartGame(); // Start the minigame
	}

	public void ShowInstructionPanel(Minigame minigame)
	{
		_startingMinigame = minigame;

		_instructionText.text = minigame.Instructions;
		_gameNameText.text = minigame.name;

		ShowPanel(_instructionPanel, _instructionButton);
	}

	public void ShowMovementUI() => ShowPanel(_movementUI, _diceButton);

    public void ShowShop(Player player)
    {
        if (_shopPanelScript != null)
        {
            _shopPanelScript.OpenShop(player); // Pass the player to the ShopPanel
            ShowPanel(_shopPanelGameObject, _shopButton);
        }
        else
        {
            Debug.LogError("ShopPanel is not assigned in the PanelManager!");
        }
    }

    public void CloseShop()
    {
        _shopPanelGameObject.SetActive(false);
    
    }
    public void ShowPanel(GameObject panel, GameObject firstSelected)
	{
		if (_activePanel) _activePanel.SetActive(false);
		_activePanel = panel;
        GameManager.Instance.CurrentPlayer.multiplayerEventSystem.SetSelectedGameObject(firstSelected);
        if (panel) panel.SetActive(true);
	}
}