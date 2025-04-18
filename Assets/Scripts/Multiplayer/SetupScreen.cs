using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class SetupScreen : MonoBehaviour
{
    private bool isGamepadConnected = false;
    private Gamepad[] connectedGamepads = new Gamepad[4];
    GameObject playerPrefab; // Reference to the player prefab
    [SerializeField] private TextMeshProUGUI[] controllerTextBoxes = new TextMeshProUGUI[4];
    private bool[] inputReceived = new bool[4];
    public int[] activationOrder = new int[4]; // Tracks the activation order of text boxes  
    private int nextAvailableTextBox = 0; // Tracks the next available text box index  
    public bool[] isReady = new bool[4];

    [SerializeField] private Transform[] spawnPoints = new Transform[4]; // Spawn points for players  
    private PlayerInputManager playerInputManager; // Reference to PlayerInputManager
    private void Start()
    {
        playerInputManager.playerPrefab = playerPrefab;
        for (int i = 0; i < controllerTextBoxes.Length; i++)
        {
            if (controllerTextBoxes[i] != null)
            {
                controllerTextBoxes[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Check if any gamepad is connected  
        isGamepadConnected = Gamepad.all.Count > 0;

        // Assign gamepads based on input received  
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Gamepad currentGamepad = Gamepad.all[i];

            // Check if the gamepad has made an input and is not already assigned  
            if (currentGamepad != null && !connectedGamepads.Contains(currentGamepad))
            {
                if (currentGamepad.allControls.Any(control => control.IsPressed()))
                {
                    int availableIndex = System.Array.IndexOf(inputReceived, false);
                    if (availableIndex != -1)
                    {
                        inputReceived[availableIndex] = true;
                        connectedGamepads[availableIndex] = currentGamepad;

                        if (nextAvailableTextBox < controllerTextBoxes.Length && controllerTextBoxes[nextAvailableTextBox] != null)
                        {
                            Debug.Log($"Controller {availableIndex + 1} connected and input detected: {currentGamepad.displayName}");
                            controllerTextBoxes[nextAvailableTextBox].text = $"Controller {availableIndex + 1}: Connected";
                            controllerTextBoxes[nextAvailableTextBox].gameObject.SetActive(true);
                            activationOrder[availableIndex] = nextAvailableTextBox;

                            // Spawn player prefab at the corresponding spawn point  
                            if (playerInputManager.playerPrefab != null && spawnPoints[availableIndex] != null)
                            {
                                Instantiate(playerPrefab, spawnPoints[availableIndex].position, spawnPoints[availableIndex].rotation);
                                
                            }

                            nextAvailableTextBox++;
                        }
                    }
                }
            }
        }

        // Handle disconnection  
        for (int i = 0; i < connectedGamepads.Length; i++)
        {
            if (connectedGamepads[i] != null && !Gamepad.all.Contains(connectedGamepads[i]))
            {
                int textBoxIndex = activationOrder[i];
                if (controllerTextBoxes[textBoxIndex] != null)
                {
                    Debug.Log($"Controller {i + 1} disconnected.");
                    controllerTextBoxes[textBoxIndex].gameObject.SetActive(false);
                }

                connectedGamepads[i] = null;
                inputReceived[i] = false;
            }
        }
    }

    public bool IsGamepadConnected()
    {
        return isGamepadConnected;
    }

    public Gamepad GetGamepad(int index)
    {
        if (index >= 0 && index < connectedGamepads.Length)
        {
            return connectedGamepads[index];
        }
        return null;
    }
}
