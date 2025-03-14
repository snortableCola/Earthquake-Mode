using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DealOrNoDeal : Minigame
{
    public Button[] suitcases; // Assign your suitcase buttons in the inspector
    public Button selectButton; // Assign your select/confirm button in the inspector
    public Button exitButton; // Assign your exit button in the inspector
    public TMP_Text rewardText; // Assign a TextMeshPro text element to display the reward
    public Color highlightColor; // Assign a color for highlighting the selected suitcase
    public GameObject initialTextObject; // Reference to the GameObject containing the initial text
    public Player player; // Reference to the Player class

    private int[] rewards = { 0, 0, 4, -4 };
    private int selectedReward;
    private Button selectedSuitcase;
    private Color originalColor;

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

        // Call the StartGame method to playtest on start
        StartGame();
    }

    public override void StartGame()
    {
        // Show game panels
        PanelManager.Instance.ShowPanel("Deal Or No Deal", 0); // Show the first panel

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
        if (selectedSuitcase != null)
        {
            // Reset the previously selected suitcase color to its original state
            selectedSuitcase.image.color = originalColor;
        }

        // Store the original color of the clicked suitcase
        originalColor = clickedButton.image.color;

        // Highlight the selected suitcase
        clickedButton.image.color = highlightColor;

        selectedSuitcase = clickedButton;
        selectedReward = rewards[index];

        // Show the select button
        selectButton.gameObject.SetActive(true);
    }

    void OnSelectButtonClicked()
    {
        // Check references before proceeding
        if (player == null) Debug.LogError("Player reference is null!");
        if (rewardText == null) Debug.LogError("RewardText reference is null!");
        if (selectButton == null) Debug.LogError("SelectButton reference is null!");
        if (initialTextObject == null) Debug.LogError("InitialTextObject reference is null!");

        // Adjust the player's points based on the selected reward
        player.AdjustPoints(selectedReward);

        // Display the reward text
        rewardText.text = "You got: " + selectedReward + " points! Total Points: " + player.totalPoints;
        Debug.Log("Selected Reward: " + selectedReward);

        // Hide the initial text
        if (initialTextObject != null)
        {
            initialTextObject.SetActive(false);
        }

        // Make the select button inactive and the exit button active
        selectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
    }

    void OnEndMinigameButtonClicked()
    {
        // Reset the color of the selected suitcase to its original state
        if (selectedSuitcase != null)
        {
            selectedSuitcase.image.color = originalColor;
        }

        // Notify the manager that the minigame is complete
        MinigameManager.Instance.MinigameCompleted(selectedReward);

        // Hide game panels after selection
        PanelManager.Instance.HideAllPanels();
    }
}
