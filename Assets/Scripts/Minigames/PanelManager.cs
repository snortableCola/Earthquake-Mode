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

    public void ShowShop(Player player)
    {
        if (_shopPanelScript != null)
        {
            _shopPanelScript.OpenShop(player); // Pass the player to the ShopPanel
            ShowPanel(_shopPanelGameObject);
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
    public void ShowPanel(GameObject panel)
	{
		if (_activePanel) _activePanel.SetActive(false);
		_activePanel = panel;
		if (panel) panel.SetActive(true);
	}
}