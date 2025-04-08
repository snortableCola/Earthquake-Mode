using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

	[SerializeField] private Minigame[] _singleplayerMinigames;
	[SerializeField] private Minigame[] _multiplayerMinigames;
    [SerializeField] private Image _hudMessage; // Reference to the HUD message image
    [SerializeField] private float _hudMessageDuration = 3f; // Duration for HUD message to display

	private Minigame _currentMinigame;
	private bool _isMultiplayer;

	public bool IsMinigameSequenceOngoing { get; private set; }

	private void Awake() => Instance = this;

	public void StartMinigame()
    {
		Debug.Log("Starting minigame: " + _currentMinigame.name);
		_currentMinigame.StartGame();
	}

    public void StartRandomSingleplayerMinigame()
	{
		IsMinigameSequenceOngoing = true;

        if (_singleplayerMinigames.Length == 0 || _isMultiplayer)
        {
            Debug.LogError("Cannot start singleplayer minigame in multiplayer mode or no singleplayer minigames defined.");
            return;
        }

        // Instantiate a new singleplayer minigame
        int randomIndex = Random.Range(0, _singleplayerMinigames.Length);
        _currentMinigame = _singleplayerMinigames[randomIndex];

        // Confirm the player is assigned
        Debug.Log($"Player {GameManager.Instance.CurrentPlayer.name} assigned to Minigame: {_currentMinigame.name}");

        // Show HUD message and instructions
        StartCoroutine(ShowHudMessageThenInstructions());
	}

	private IEnumerator ShowHudMessageThenInstructions()
	{
		// Display the HUD message
		if (_hudMessage != null)
		{
			_hudMessage.gameObject.SetActive(true); // Show the HUD message
			yield return new WaitForSeconds(_hudMessageDuration); // Wait for the HUD message duration
			_hudMessage.gameObject.SetActive(false);
		}

		PanelManager.Instance.ShowInstructionPanel(_currentMinigame);
	}

	public void EndMinigame()
    {
		PanelManager panelManager = PanelManager.Instance;

		panelManager.HideAllPanels();
		panelManager.ShowMovementUI();

		IsMinigameSequenceOngoing = false;
	}
}
