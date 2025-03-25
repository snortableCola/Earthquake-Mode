using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance; // Singleton instance
    private Dictionary<string, string> minigameInstructions = new Dictionary<string, string>();
    private Dictionary<string, GameObject[]> minigamePanels = new Dictionary<string, GameObject[]>();
    private string currentMinigameName;

    public GameObject instructionPanel;
    public TMP_Text instructionText;
    public TMP_Text gameName; // Reference to the TMP_Text component for the minigame's name
    public Button startMinigameButton;
    public GameObject[] ceoGambitPanels;
    public GameObject[] dealOrNoDealPanels;
    public GameObject[] CRPanels; 
    //public TMP_Text rewardText; // Reference to the TMP_Text component for displaying the reward

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeMinigameInstructions();
        InitializeMinigamePanels();
        startMinigameButton.onClick.AddListener(OnStartMinigameButtonClicked); // Add listener
    }

    private void InitializeMinigameInstructions()
    {
        minigameInstructions.Add("CEO Gambit", "How confident are you in your coin-flipping skills? Bet on your hard-earned cash, call heads or tails, and flip the coin. Get it right to double your bet—get it wrong, and, well… hope you didn’t bet too much.");
        minigameInstructions.Add("Deal Or No Deal", "Four suitcases lie before you. One will give you +4 points, two are empty, and one will give you -4 points. The suitcases will be shuffled and you must choose wisely...");
        minigameInstructions.Add("Corporate Roulette", "Spin the chamber and take your shot. Some bullets earn you points (+6,+4), others hit you where it hurts (-3, -4 points), and a couple just click harmlessly. Will luck be on your side?");
        // Add more minigame instructions here
        Debug.Log("Instructions initialized");
    }

    private void InitializeMinigamePanels()
    {
        // Add panels for each minigame to the dictionary
        minigamePanels.Add("CEO Gambit", ceoGambitPanels);
        minigamePanels.Add("Deal Or No Deal", dealOrNoDealPanels);
        minigamePanels.Add("Corporate Roulette",CRPanels);
        foreach (var key in minigameInstructions.Keys)
        {
            Debug.Log("Minigame instruction key: " + key);
        }
        // Add more minigames and their corresponding panels here
        Debug.Log("Panels initialized");
    }

    public void ShowInstructionPanel(string minigameName)
    {
        HideAllPanels();
        currentMinigameName = minigameName; // Save the current minigame name
        Debug.Log("Attempting to show instructions for: " + minigameName);

		if (!minigameInstructions.ContainsKey(minigameName))
		{
			Debug.LogWarning("Minigame instructions not found for: " + minigameName);
            return;
		}

		Debug.Log("Setting instruction text and game name for: " + minigameName);
		instructionText.text = minigameInstructions[minigameName];
		gameName.text = minigameName; // Set the minigame name in the TMP_Text component
		instructionPanel.SetActive(true);
	}

    public void OnStartMinigameButtonClicked()
    {
        instructionPanel.SetActive(false); // Hide the instruction panel
		if (currentMinigameName == null)
		{
			Debug.LogWarning("No minigame name set to start.");
            return;
		}

		Debug.Log("Starting minigame: " + currentMinigameName);
		MinigameManager.Instance.StartMinigame(currentMinigameName); // Start the minigame
	}

    public void ShowPanel(string minigameName, int panelIndex)
    {
        HideAllPanels();
        if (minigamePanels.ContainsKey(minigameName) && panelIndex < minigamePanels[minigameName].Length)
        {
            minigamePanels[minigameName][panelIndex].SetActive(true);
        }
    }

    public void HideAllPanels()
    {
        foreach (var minigame in minigamePanels)
        {
            foreach (var panel in minigame.Value)
            {
                panel.SetActive(false);
            }
        }
    }

    public void EndMinigame(string minigameName)
    {
        HideAllPanels();
        // Add any additional logic for ending the minigame if needed
    }

    //public void DisplayReward(string message)
    //{
    //    rewardText.text = message;
    //    // Show the reward panel or update the UI as needed
    //}
}
