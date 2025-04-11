using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SetupScreen : MonoBehaviour
{
    private bool isGamepadConnected = false;
    private Gamepad[] connectedGamepads = new Gamepad[4];
    [SerializeField] private TextMeshProUGUI[] controllerTextBoxes = new TextMeshProUGUI[4];

    private void Start()
    {
        controllerTextBoxes[4].gameObject.SetActive(false); // Hide all text boxes at the start
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
                if (controllerTextBoxes[i] != null)
                {
                    Debug.Log($"Controller {i + 1} connected: {connectedGamepads[i].displayName}");
                    controllerTextBoxes[i].text = $"Controller {i + 1}: Connected";
                    controllerTextBoxes[i].gameObject.SetActive(true);
                }
            }
            else
            {
                connectedGamepads[i] = null; // Clear unused slots
                if (controllerTextBoxes[i] != null)
                {
                    Debug.Log($"Controller {i + 1} disconnected: {connectedGamepads[i].displayName}");
                    
                    controllerTextBoxes[i].gameObject.SetActive(false);
                }
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
