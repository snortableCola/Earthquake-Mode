using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;
    public GameObject[] singlePlayerMinigames;
    public GameObject[] multiplayerMinigames;
    public bool isMultiplayer;
    public PanelManager panelManager;

    private GameObject currentMinigame;
    private Minigame minigameComponent;

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

    void Start()
    {
        LoadRandomMinigame();
    }
}
