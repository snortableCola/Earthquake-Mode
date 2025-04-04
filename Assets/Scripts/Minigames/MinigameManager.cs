using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    public GameObject[] singlePlayerMinigames;
    public GameObject[] multiplayerMinigames;
    public bool isMultiplayer;
    public PanelManager panelManager;
    private Minigame currentMinigame;
    private Player currentPlayer; // Track the current player playing the minigame
    public Image hudMessage; // Reference to the HUD message image
    public float hudMessageDuration = 3f; // Duration for HUD message to display
    

    protected Player player;

    public virtual void SetPlayer(Player player)
    {
      
      

        if (this.player != null)
        {
            Debug.Log($"Player {this.player.name} assigned with {this.player.totalPoints} points.");
        }
        else
        {
            Debug.Log("No player assigned.");
        }
    }

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
   

    private IEnumerator ShowHudMessageThenInstructions(string minigameName)
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
            panelManager.ShowInstructionPanel(minigameName);
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
        string minigameName = singlePlayerMinigames[randomIndex].name;
        Debug.Log($"Loaded Minigame: {minigameName}");

        StartCoroutine(ShowHudThenInstructionsAndStart(minigameName));
    }
    private IEnumerator ShowHudThenInstructionsAndStart(string minigameName)
    {
        // Step 1: Display the HUD message
        if (hudMessage != null)
        {
            hudMessage.gameObject.SetActive(true);
            Debug.Log($"Displaying HUD message for: {minigameName}");
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
            Debug.Log($"Showing instructions panel for: {minigameName}");
            panelManager.ShowInstructionPanel(minigameName);
        }
        else
        {
            Debug.LogWarning("PanelManager not assigned. Skipping instructions.");
        }

        // Wait briefly before starting the minigame (optional)
        yield return new WaitForSeconds(1f);

        // Step 3: Start the minigame
        Debug.Log($"Starting minigame: {minigameName}");
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

        // Assign the player to the minigame
        currentMinigame.SetPlayer(player);

        // Confirm the player is assigned
        Debug.Log($"Player {player.name} assigned to Minigame: {currentMinigame.name}");

        // Show HUD message and instructions
        string minigameName = singlePlayerMinigames[randomIndex].name;
        StartCoroutine(ShowHudMessageThenInstructions(minigameName));
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
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
    }
}
