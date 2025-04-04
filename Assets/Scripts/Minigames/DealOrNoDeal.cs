using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DealOrNoDeal : Minigame
{
    public Button[] suitcases;
    public Button selectButton;
    public Button exitButton;
    public TMP_Text rewardText;
    public Color highlightColor;
    public GameObject initialTextObject; // Reference to the GameObject containing the initial text
    public PanelManager panelManager;

    private int[] rewards = { 0, 0, 4, -4 };
    private int selectedReward;
    private Button selectedSuitcase;
    private Color originalColor;
    public override void SetPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogError("SetPlayer called with a null Player in DOND.");
            return;
        }

        base.SetPlayer(player);
        Debug.Log($"Player {player.name} assigned to DOND");
    }
    void Start()
    {
        // Add listeners to the suitcase buttons
        for (int i = 0; i < suitcases.Length; i++)
        {
            int index = i; // Prevent closure issue in listener
            suitcases[index].onClick.AddListener(() => OnSuitcaseClicked(suitcases[index], index));
        }

        // Add listeners to the buttons
        selectButton.onClick.AddListener(OnSelectButtonClicked);
        exitButton.onClick.AddListener(OnEndMinigameButtonClicked);
    }

    public override void StartGame()
    {
        if (Player == null)
        {
            Debug.LogError("Player is null in DealOrNoDeal! Ensure SetPlayer is called before starting.");
            return;
        }
        else
        {
            Debug.Log($"Starting DealOrNoDeal for Player: {Player.name}.");
        }

        for (int i = 0; i < suitcases.Length; i++)
        {
            int index = i; // Prevent closure issue in listener
            suitcases[index].onClick.AddListener(() => OnSuitcaseClicked(suitcases[index], index));
        }

        // Add listeners to the buttons
        selectButton.onClick.AddListener(OnSelectButtonClicked);
        exitButton.onClick.AddListener(OnEndMinigameButtonClicked);

        // Show game panels
        panelManager.ShowPanel("Deal Or No Deal", 0); // Show the first panel

        // Shuffle the suitcases
        ShuffleSuitcases();

        // Reset the UI
        rewardText.text = "";
        selectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false); // Ensure exit button is initially inactive
        selectedSuitcase = null;

        // Show the initial text
        if (initialTextObject != null)
        {
            initialTextObject.SetActive(true);
        }
    }

    void ShuffleSuitcases()
    {
        // Shuffle the rewards array
        for (int i = 0; i < rewards.Length; i++)
        {
            int rnd = Random.Range(0, rewards.Length);
            int temp = rewards[rnd];
            rewards[rnd] = rewards[i];
            rewards[i] = temp;
        }
    }

    void OnSuitcaseClicked(Button clickedButton, int index)
    {
        selectedSuitcase = clickedButton;
        selectedReward = rewards[index];

        // Show the select button
        selectButton.gameObject.SetActive(true);
    }

    void OnSelectButtonClicked()
    {
        Debug.Log($"{name}, {GetInstanceID()}");

        // Check references before proceeding
        if (Player == null)
        {
            Debug.LogError("Player reference is null in OnSelectButtonClicked!");
            return;
        }

        Debug.Log($"Player {Player.name} selected a suitcase with reward {selectedReward}.");

        // Adjust the player's points based on the selected reward
        Player.AdjustPoints(selectedReward);

        // Display the reward text
        rewardText.text = $"You got: {selectedReward} points! Total Points: {Player.totalPoints}";
        Debug.Log($"Selected Reward: {selectedReward}");
        Debug.Log($"Player {Player.name} now has {Player.totalPoints} points after adjustment.");

        // Hide the initial text
        if (initialTextObject != null)
        {
            initialTextObject.SetActive(false);
        }

        // Make the select button inactive and the exit button active
        selectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
    }

    public override void Cleanup()
    {
        base.Cleanup();

        // Remove all listeners from buttons
        foreach (var suitcase in suitcases)
        {
            suitcase.onClick.RemoveAllListeners();
        }
        selectButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        // Reset input fields and text
        rewardText.text = "";
        selectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false); // Ensure exit button is initially inactive
        selectedSuitcase = null;

        // Reset the color of all suitcases to their original color
        foreach (var suitcase in suitcases)
        {
            suitcase.image.color = originalColor;
        }

        // Show the initial text
        if (initialTextObject != null)
        {
            initialTextObject.SetActive(true);
        }
    }

    void OnEndMinigameButtonClicked()
    {
        // Reset the color of the selected suitcase to its original state
        if (selectedSuitcase != null)
        {
            selectedSuitcase.image.color = originalColor;
        }
        Cleanup();

        // Notify the manager that the minigame is complete
        MinigameManager.Instance.EndCurrentMinigame();

        // Hide game panels after selection
        panelManager.HideAllPanels();
        panelManager.ShowMovementUI(); 
    }
}