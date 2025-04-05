using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorporateRoulette : Minigame
{
    public Button SpinButton;
    public Button FireButton;
    public Button ExitButton;
    public TMP_Text GeneralText;
    private int[] rewards = { 0, 0, -3, -4, 6, 4 };
    private int currentChamber = -1; // Tracks the selected chamber

    public AudioSource audioSource;
    public AudioClip FiringSound;
    public PanelManager panelManager;
    public override void SetPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogError("SetPlayer called with a null Player in CR.");
            return;
        }

        base.SetPlayer(player);
        Debug.Log($"Player {player.name} assigned to CR");
    }
    void Start()
    {
        Debug.Log("CorporateRoulette Start method called.");

        // Add listeners to the buttons
        SpinButton.onClick.AddListener(SpinChamber);
        FireButton.onClick.AddListener(FireChamber);
        ExitButton.onClick.AddListener(OnEndMinigameButtonClicked);
        // Check if buttons are interactable
        Debug.Log($"SpinButton interactable: {SpinButton.interactable}");
        Debug.Log($"FireButton interactable: {FireButton.interactable}");
        Debug.Log($"ExitButton interactable: {ExitButton.interactable}");
    }
    void Awake()
    {
        Debug.Log("CorporateRoulette Awake method called.");
    }
    public override void StartGame()
    {
      if (Player == null)
        {
            Debug.LogError("Player is null in CorporateRoulette! Ensure SetPlayer is called before starting.");
            return;
        }
        else
        {
            Debug.Log($"Starting Corporate Roulette for Player: {Player.name}.");
        }

        // Show the initial panel for Corporate Roulette
        if (panelManager != null)
        {
            panelManager.ShowPanel("Corporate Roulette", 0);
        }
        else
        {
            Debug.LogError("PanelManager is not assigned in CorporateRoulette!");
        }

        ExitButton.gameObject.SetActive(false);
        FireButton.gameObject.SetActive(false);
        Debug.Log($"Starting Corporate Roulette for player: {Player.name} with {Player.totalPoints} points.");
    }

    public void SpinChamber()
    {
        Debug.Log("SpinChamber() initiated");
        for (int i = rewards.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = rewards[i];
            rewards[i] = rewards[randomIndex];
            rewards[randomIndex] = temp;
        }
       
        currentChamber = Random.Range(0, rewards.Length);
        GeneralText.text = "The chamber has stopped spinning...";
        FireButton.gameObject.SetActive(true);
        SpinButton.gameObject.SetActive(false);
        Debug.Log("SpinChamber method completed.");
    }

    
    public void FireChamber()
    {
        

        if (currentChamber < 0)
        {
            GeneralText.text = "Please spin the chamber first!";
            return;
        }

        if (FiringSound != null)
        {
            audioSource.PlayOneShot(FiringSound);
        }

        int reward = rewards[currentChamber];
        string message = reward switch
        {
            0 => "The chamber was empty. You're safe.",
            > 0 => $"You gained {reward} points!",
            _ => $"You lost {-reward} points.",
        };

        if (Player == null)
        {
            Debug.LogError("Player reference is null! Cannot update points.");
            return;
        }

        Player.AdjustPoints(reward);
        Debug.Log($"Player {Player.name} now has {Player.totalPoints} points after the result.");

        GeneralText.text = message;

        FireButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(true);
    }

    public override void Cleanup()
    {
        base.Cleanup();

        // Remove all listeners from buttons
        SpinButton.onClick.RemoveAllListeners();
        FireButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        // Reset button states
        SpinButton.gameObject.SetActive(true);
        FireButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);

        // Reset text fields
        GeneralText.text = "Spin the chamber to start.";

        // Reset internal variables
        currentChamber = -1;

        // Reset button text if needed
        TMP_Text spinButtonText = SpinButton.GetComponentInChildren<TMP_Text>();
        if (spinButtonText != null)
        {
            spinButtonText.text = "Spin";
        }

        Debug.Log("CorporateRoulette cleanup completed.");
    }

  public   void OnEndMinigameButtonClicked()
    {
        MinigameManager.Instance.EndCurrentMinigame();
        panelManager.HideAllPanels();
        panelManager.ShowMovementUI();
    }
}
