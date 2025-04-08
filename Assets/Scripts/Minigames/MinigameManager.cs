using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    public bool IsMinigameSequenceOngoing { get; private set; }

    public GameObject[] singlePlayerMinigames;
    public GameObject[] multiplayerMinigames;
    public bool isMultiplayer;
    public PanelManager panelManager;
    private Minigame currentMinigame;
    public Image hudMessage; // Reference to the HUD message image
    public float hudMessageDuration = 3f; // Duration for HUD message to display

    void Awake()
    {
        // Singleton pattern to ensure only one instance of MinigameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
   

    private IEnumerator ShowHudMessageThenInstructions(Minigame minigame)
    {
        // Display the HUD message
        if (hudMessage != null)
        {
            hudMessage.gameObject.SetActive(true); // Show the HUD message
        }

        // Wait for the HUD message duration
        yield return new WaitForSeconds(hudMessageDuration);

        // Hide the HUD message
        if (hudMessage != null)
        {
            hudMessage.gameObject.SetActive(false);
        }

        if (panelManager != null)
        {
            panelManager.ShowInstructionPanel(minigame);
        }
        else
        {
            Debug.LogWarning("PanelManager is not assigned. Skipping instruction panel display.");
        }
    }
   public void LoadRandomMinigame()
    {
        // Ensure the minigame array is valid
        if (singlePlayerMinigames == null || singlePlayerMinigames.Length == 0)
        {
            Debug.LogError("No singleplayer minigames are defined or the array is empty.");
            return;
        }

        // Clean up the current minigame instance
        if (currentMinigame != null)
        {
            Destroy(currentMinigame.gameObject);
            currentMinigame = null;
        }

        // Randomly select and instantiate a minigame
        int randomIndex = Random.Range(0, singlePlayerMinigames.Length);
        currentMinigame = Instantiate(singlePlayerMinigames[randomIndex]).GetComponent<Minigame>();

        if (currentMinigame == null)
        {
            Debug.LogError("Failed to instantiate the minigame.");
            return;
        }

        // Start the process of showing HUD message, instructions, and the minigame
        string minigameName = currentMinigame.name;
        Debug.Log($"Loaded Minigame: {minigameName}");

        StartCoroutine(ShowHudThenInstructionsAndStart(currentMinigame));
    }
    private IEnumerator ShowHudThenInstructionsAndStart(Minigame minigame)
    {
        // Step 1: Display the HUD message
        if (hudMessage != null)
        {
            hudMessage.gameObject.SetActive(true);
            Debug.Log($"Displaying HUD message for: {minigame.name}");
        }

        // Wait for HUD message duration
        yield return new WaitForSeconds(hudMessageDuration);

        // Hide HUD message
        if (hudMessage != null)
        {
            hudMessage.gameObject.SetActive(false);
        }

        // Step 2: Show the instruction panel
        if (panelManager != null)
        {
            Debug.Log($"Showing instructions panel for: {minigame.name}");
            panelManager.ShowInstructionPanel(minigame);
        }
        else
        {
            Debug.LogWarning("PanelManager not assigned. Skipping instructions.");
        }

        // Wait briefly before starting the minigame (optional)
        yield return new WaitForSeconds(1f);

        // Step 3: Start the minigame
        Debug.Log($"Starting minigame: {minigame.name}");
        currentMinigame.StartGame();
    }
    public void StartMinigame(string minigameName)
    {
		if (currentMinigame == null)
		{
			Debug.LogError("Current minigame is null when trying to start: " + minigameName);
            return;
		}

		Debug.Log("Starting minigame: " + minigameName);
		currentMinigame.StartGame();
	}

    public void StartMinigameForPlayer(Player player)
	{
		IsMinigameSequenceOngoing = true;

		if (player == null)
        {
            Debug.LogError("Player passed to StartMinigameForPlayer is null! Cannot proceed.");
            return;
        }

        if (singlePlayerMinigames == null || singlePlayerMinigames.Length == 0 || isMultiplayer)
        {
            Debug.LogError("Cannot start singleplayer minigame in multiplayer mode or no singleplayer minigames defined.");
            return;
        }

        // Clean up previous minigame
        if (currentMinigame != null)
        {
            currentMinigame.Cleanup();
            Destroy(currentMinigame.gameObject);
            currentMinigame = null;
        }
        else
        {
            Debug.LogWarning("No current minigame to clean up.");
        }

        // Instantiate a new singleplayer minigame
        int randomIndex = Random.Range(0, singlePlayerMinigames.Length);
        currentMinigame = Instantiate(singlePlayerMinigames[randomIndex]).GetComponent<Minigame>();

        if (currentMinigame == null)
        {
            Debug.LogError("Failed to instantiate the minigame. Aborting start.");
            return;
        }

        // Confirm the player is assigned
        Debug.Log($"Player {player.name} assigned to Minigame: {currentMinigame.name}");

        // Show HUD message and instructions
        StartCoroutine(ShowHudMessageThenInstructions(currentMinigame));
    }

    public void EndCurrentMinigame()
    {
        if (currentMinigame != null)
        {
            panelManager.HideAllPanels();
            panelManager.ShowMovementUI(); 
            Destroy(currentMinigame.gameObject);
            currentMinigame = null;
		}
		IsMinigameSequenceOngoing = false;
	}
}
