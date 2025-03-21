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

        // Show the instruction panel using the PanelManager
        panelManager.ShowInstructionPanel(minigameName);
    }
  //  public void LoadRandomMinigame()
  //  {
  //      if (currentMinigame != null)
  //      {
  //          Destroy(currentMinigame);
  //          panelManager.HideAllPanels();
  //      }

		//GameObject[] minigames = isMultiplayer ? multiplayerMinigames : singlePlayerMinigames;

		//int randomIndex = Random.Range(0, minigames.Length);
		//Minigame selectedMinigame = Instantiate(minigames[randomIndex]).GetComponent<Minigame>();
		//string minigameName = selectedMinigame.name;

		//currentMinigame = selectedMinigame;
  //      Debug.Log("Loaded minigame: " + minigameName);
  //      panelManager.ShowInstructionPanel(minigameName); // Show instruction panel based on minigame name
  //  }

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
		if (singlePlayerMinigames.Length == 0 || isMultiplayer)
		{
			Debug.LogError("Cannot start singleplayer minigame in multiplayer mode or no singleplayer minigames defined.");
            return;
		}

		if (currentMinigame != null)
		{
			Destroy(currentMinigame); // Destroy previous minigame
			panelManager.HideAllPanels(); // Ensure a clean slate
		}

		currentPlayer = player; // Store the current player

		// Load a random singleplayer minigame
		int randomIndex = Random.Range(0, singlePlayerMinigames.Length);
		currentMinigame = Instantiate(singlePlayerMinigames[randomIndex]).GetComponent<Minigame>();
		string minigameName = singlePlayerMinigames[randomIndex].name;

		Debug.Log($"{player.name} is starting the minigame: {minigameName}");

		// Pass the player object to the minigame

		currentMinigame.SetPlayer(player);
		StartCoroutine(ShowHudMessageThenInstructions(minigameName));
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
