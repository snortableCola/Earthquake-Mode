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
        if (selectedSuitcase != null)
        {
            // Reset the previously selected suitcase color to its original state
            selectedSuitcase.image.color = originalColor;
        }

        // Store the original color of the clicked suitcase
        if (clickedButton.image != null)
        {
            originalColor = clickedButton.image.color;
        }

        // Highlight the selected suitcase
        if (clickedButton.image != null)
        {
            clickedButton.image.color = highlightColor;
        }
        else
        {
            Debug.LogWarning("Clicked button does not have an Image component!");
        }

        selectedSuitcase = clickedButton;
        selectedReward = rewards[index];

        // Show the select button
        selectButton.gameObject.SetActive(true);
    }

    void OnSelectButtonClicked()
    {
        Debug.Log($"{name}, {GetInstanceID()}");

        // Check references before proceeding
        if (player == null)
        {
            Debug.LogError("Player reference is null!");
            return;
        }

        // Adjust the player's points based on the selected reward
        player.AdjustPoints(selectedReward);

        // Display the reward text
        rewardText.text = $"You got: {selectedReward} points! Total Points: {player.totalPoints}";
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
        panelManager.HideAllPanels();
    }
}
