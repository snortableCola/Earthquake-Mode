using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;


public class SetupScreen : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab; // Reference to the player prefab
    [SerializeField] private TextMeshProUGUI[] controllerTextBoxes = new TextMeshProUGUI[4];
    [SerializeField] private Transform[] spawnPoints = new Transform[4]; // Spawn points for players
	[SerializeField] private PlayerInputManager playerInputManager; // Reference to PlayerInputManager

	private readonly Gamepad[] _connectedGamepads = new Gamepad[4];
	private readonly bool[] _inputReceived = new bool[4];
	private readonly int[] _activationOrder = new int[4]; // Tracks the activation order of text boxes

	private int nextAvailableTextBox = 0; // Tracks the next available text box index

	private void Start()
    {
        playerInputManager.playerPrefab = playerPrefab;
        foreach (TextMeshProUGUI controllerTextBox in controllerTextBoxes)
        {
            if (controllerTextBox != null)
            {
				controllerTextBox.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Assign gamepads based on received inputs
        foreach (Gamepad currentGamepad in Gamepad.all)
		{
			// Confirm the gamepad exists and is not already assigned  
			if (currentGamepad == null || _connectedGamepads.Contains(currentGamepad))
			{
				continue;
			}

            // Confirm the gamepad is sending any button input
			if (!currentGamepad.allControls.Any(control => control.IsPressed()))
			{
				continue;
			}

            // Confirm there is an opening available for the gamepad
			int availableIndex = System.Array.IndexOf(_inputReceived, false);
			if (availableIndex == -1)
			{
				continue;
			}

            // Store the gamepad and mark its opening as taken
			_inputReceived[availableIndex] = true;
			_connectedGamepads[availableIndex] = currentGamepad;

			if (nextAvailableTextBox >= controllerTextBoxes.Length || controllerTextBoxes[nextAvailableTextBox] == null)
			{
				continue;
			}

			Debug.Log($"Controller {availableIndex + 1} connected and input detected: {currentGamepad.displayName}");
			controllerTextBoxes[nextAvailableTextBox].text = $"Controller {availableIndex + 1}: Connected";
			controllerTextBoxes[nextAvailableTextBox].gameObject.SetActive(true);
			_activationOrder[availableIndex] = nextAvailableTextBox;

			// Spawn player prefab at the corresponding spawn point  
			if (playerInputManager.playerPrefab != null && spawnPoints[availableIndex] != null)
			{
				Instantiate(playerPrefab, spawnPoints[availableIndex].position, spawnPoints[availableIndex].rotation);
			}

			nextAvailableTextBox++;
		}

		// Handle disconnection  
		for (int i = 0; i < _connectedGamepads.Length; i++)
		{
			Gamepad gamepad = _connectedGamepads[i];
			if (gamepad == null || Gamepad.all.Contains(gamepad))
			{
				continue;
			}

			int textBoxIndex = _activationOrder[i];
			if (controllerTextBoxes[textBoxIndex] != null)
			{
				Debug.Log($"Controller {i + 1} disconnected.");
				controllerTextBoxes[textBoxIndex].gameObject.SetActive(false);
			}

			_connectedGamepads[i] = null;
			_inputReceived[i] = false;
		}
	}

	public bool IsGamepadConnected() => Gamepad.all.Count > 0;

	public Gamepad GetGamepad(int index)
    {
        if (index >= 0 && index < _connectedGamepads.Length)
        {
            return _connectedGamepads[index];
        }
        return null;
    }
}
