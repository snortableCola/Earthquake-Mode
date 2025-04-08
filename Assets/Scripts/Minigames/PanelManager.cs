using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; } // Singleton instance
    private Dictionary<Type, string> minigameInstructions = new();
    private Dictionary<Type, GameObject[]> minigamePanels = new();
    private string currentMinigameName;
    public GameObject MovementUI; 
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
        minigameInstructions.Add(typeof(CeoGambit), "How confident are you in your coin-flipping skills? Bet on your hard-earned cash, call heads or tails, and flip the coin. Get it right to double your bet—get it wrong, and, well… hope you didn’t bet too much.");
        minigameInstructions.Add(typeof(DealOrNoDeal), "Four suitcases lie before you. One will give you +4 points, two are empty, and one will give you -4 points. The suitcases will be shuffled and you must choose wisely...");
        minigameInstructions.Add(typeof(CorporateRoulette), "Spin the chamber and take your shot. Some bullets earn you points (+6,+4), others hit you where it hurts (-3, -4 points), and a couple just click harmlessly. Will luck be on your side?");
        // Add more minigame instructions here
        Debug.Log("Instructions initialized");
    }

    private void InitializeMinigamePanels()
    {
        // Add panels for each minigame to the dictionary
        minigamePanels.Add(typeof(CeoGambit), ceoGambitPanels);
        minigamePanels.Add(typeof(DealOrNoDeal), dealOrNoDealPanels);
        minigamePanels.Add(typeof(CorporateRoulette), CRPanels);
        foreach (var key in minigameInstructions.Keys)
        {
            Debug.Log("Minigame instruction key: " + key);
        }
        // Add more minigames and their corresponding panels here
        Debug.Log("Panels initialized");
    }

    public void ShowInstructionPanel(Minigame minigame)
    {
        MovementUI.SetActive(false);
        HideAllPanels();
        currentMinigameName = minigame.name; // Save the current minigame name
        Debug.Log("Attempting to show instructions for: " + currentMinigameName);

        Type minigameType = minigame.GetType();
		if (!minigameInstructions.ContainsKey(minigameType))
		{
			Debug.LogWarning("Minigame instructions not found for: " + currentMinigameName);
            return;
		}

		Debug.Log("Setting instruction text and game name for: " + currentMinigameName);
		instructionText.text = minigameInstructions[minigameType];
		gameName.text = currentMinigameName; // Set the minigame name in the TMP_Text component
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

		MinigameManager.Instance.StartMinigame(); // Start the minigame
	}

    public void ShowPanel(Minigame minigame, int panelIndex)
    {
        HideAllPanels();

        Type minigameType = minigame.GetType();
        if (minigamePanels.TryGetValue(minigameType, out GameObject[] panels) && panelIndex < panels.Length)
        {
            panels[panelIndex].SetActive(true);
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
    public void ShowMovementUI()
    {
        MovementUI.SetActive(true);
    
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
