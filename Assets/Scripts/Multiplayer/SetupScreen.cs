using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class SetupScreen : MonoBehaviour
{
    private bool isGamepadConnected = false;
    private Gamepad[] connectedGamepads = new Gamepad[4];
    [SerializeField] private TextMeshProUGUI[] controllerTextBoxes = new TextMeshProUGUI[4];
    private bool[] inputReceived = new bool[4];
    private int[] activationOrder = new int[4]; // Tracks the activation order of text boxes  
    private int nextAvailableTextBox = 0; // Tracks the next available text box index  

    private void Start()
    {
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

        // Assign up to four gamepads as separate input sources  
        for (int i = 0; i < connectedGamepads.Length; i++)
        {
            if (i < Gamepad.all.Count)
            {
                connectedGamepads[i] = Gamepad.all[i];

                // Check if the gamepad has made an input  
                if (connectedGamepads[i] != null && !inputReceived[i])
                {
                    if (connectedGamepads[i].allControls.Any(control => control.IsPressed()))
                    {
                        inputReceived[i] = true;

                        if (nextAvailableTextBox < controllerTextBoxes.Length && controllerTextBoxes[nextAvailableTextBox] != null)
                        {
                            Debug.Log($"Controller {i + 1} connected and input detected: {connectedGamepads[i].displayName}");
                            controllerTextBoxes[nextAvailableTextBox].text = $"Controller {i + 1}: Connected";
                            controllerTextBoxes[nextAvailableTextBox].gameObject.SetActive(true);
                            activationOrder[i] = nextAvailableTextBox;
                            nextAvailableTextBox++;
                        }
                    }
                }
            }
            else
            {
                // Handle disconnection  
                if (connectedGamepads[i] != null && inputReceived[i])
                {
                    int textBoxIndex = activationOrder[i];
                    if (controllerTextBoxes[textBoxIndex] != null)
                    {
                        Debug.Log($"Controller {i + 1} disconnected.");
                        controllerTextBoxes[textBoxIndex].gameObject.SetActive(false);
                    }
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
