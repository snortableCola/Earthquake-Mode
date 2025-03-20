using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class CorporateRoulette : Minigame
{
    public Button SpinButton;
    public Button FireButton;
    public Button ExitButton;
    public TMP_Text GeneralText;
    private int[] rewards = {0, 0,-3,-4,6,4};
    private int currentChamber = -1; // Tracks the selected chamber

    public AudioSource audioSource;
    public AudioClip FiringSound; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpinButton.onClick.AddListener(SpinChamber);
        FireButton.onClick.AddListener(FireChamber);
        ExitButton.onClick.AddListener(OnEndMiniGameButtonClicked);
    }
    public override void StartGame()
    {
        PanelManager.Instance.ShowPanel("Corporate Roulette",0);
        ExitButton.gameObject.SetActive(false);
        FireButton.gameObject.SetActive(false);

    }
    void SpinChamber()
    {
        // Shuffle rewards array
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
    }
    void FireChamber()
    {

        if (currentChamber >= 0)
        {
            if (FiringSound != null)
            {
                audioSource.PlayOneShot(FiringSound);
            }
            int reward = rewards[currentChamber];
            string message;

            // Customize messages based on reward value
            if (reward == 0)
            {
                message = "The chamber was empty. You're safe.";
            }
            else if (reward > 0)
            {
                message = $"You gained {reward} points!";
            }
            else
            {
                message = $"You lost {-reward} points.";
            }

            GeneralText.text = message;
            FireButton.gameObject.SetActive(false);
            ExitButton.gameObject.SetActive(true);
        }
        else
        {
            GeneralText.text = "Please spin the chamber first!";
        }
    }
    void OnEndMiniGameButtonClicked()
    {
        // Notify the manager that the minigame is complete
        MinigameManager.Instance.MinigameCompleted(2);

        // Hide game panels after selection
        PanelManager.Instance.HideAllPanels();

    }
}
