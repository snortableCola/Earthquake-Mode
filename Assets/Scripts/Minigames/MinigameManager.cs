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
    public bool isTesting; // Enable this for playtesting 
    private GameObject currentMinigame;
    private Minigame minigameComponent;
    private Player currentPlayer; // Track the current player playing the minigame
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
    void Start()
    {
        if (isTesting)
        {
            // Automatically load a random minigame for playtesting
            LoadRandomMinigame();
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

        // Show the instruction panel using the PanelManager
        panelManager.ShowInstructionPanel(minigameName);
    }
    public void LoadRandomMinigame()
    {
        if (currentMinigame != null)
        {
            Destroy(currentMinigame);
            panelManager.HideAllPanels();
        }

        int randomIndex;
        GameObject selectedMinigame;
        string minigameName;

        if (isMultiplayer)
        {
            randomIndex = Random.Range(0, multiplayerMinigames.Length);
            selectedMinigame = Instantiate(multiplayerMinigames[randomIndex]);
            minigameName = multiplayerMinigames[randomIndex].name;
        }
        else
        {
            randomIndex = Random.Range(0, singlePlayerMinigames.Length);
            selectedMinigame = Instantiate(singlePlayerMinigames[randomIndex]);
            minigameName = singlePlayerMinigames[randomIndex].name;
        }

        currentMinigame = selectedMinigame;
        Debug.Log("Loaded minigame: " + minigameName);
        panelManager.ShowInstructionPanel(minigameName); // Show instruction panel based on minigame name
    }

    public void StartMinigame(string minigameName)
    {
        if (currentMinigame != null)
        {
            minigameComponent = currentMinigame.GetComponent<Minigame>();
            if (minigameComponent != null)
            {
                Debug.Log("Starting minigame: " + minigameName);
                minigameComponent.StartGame();
            }
            else
            {
                Debug.LogError("Minigame component not found in: " + minigameName);
            }
        }
        else
        {
            Debug.LogError("Current minigame is null when trying to start: " + minigameName);
        }
    }

    public void StartMinigameForPlayer(Player player)
    {
        if (singlePlayerMinigames.Length > 0 && !isMultiplayer)
        {
            if (currentMinigame != null)
            {
                Destroy(currentMinigame); // Destroy previous minigame
                panelManager.HideAllPanels(); // Ensure a clean slate
            }

            currentPlayer = player; // Store the current player

            // Load a random singleplayer minigame
            int randomIndex = Random.Range(0, singlePlayerMinigames.Length);
            currentMinigame = Instantiate(singlePlayerMinigames[randomIndex]);
            string minigameName = singlePlayerMinigames[randomIndex].name;

            Debug.Log($"{player.name} is starting the minigame: {minigameName}");

            // Pass the player object to the minigame
            minigameComponent = currentMinigame.GetComponent<Minigame>();
            if (minigameComponent != null)
            {
                minigameComponent.SetPlayer(player);
            }
            else
            {
                Debug.LogError($"Minigame component not found in {minigameName}");
            }

            // Show instruction panel for the minigame
            panelManager.ShowInstructionPanel(minigameName);
        }
        else
        {
            Debug.LogError("Cannot start singleplayer minigame in multiplayer mode or no singleplayer minigames defined.");
        }
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void MinigameCompleted(int reward)
    {
        Debug.Log("Minigame Completed. Reward: " + reward);
        if (currentPlayer != null)
        {
            currentPlayer.AdjustPoints(reward); // Update player's points
            Debug.Log($"{currentPlayer.name} now has {currentPlayer.totalPoints} points.");
        }

        // Clean up for the next minigame
        if (currentMinigame != null)
        {
            Destroy(currentMinigame);
        }
        panelManager.HideAllPanels();
    }
}
