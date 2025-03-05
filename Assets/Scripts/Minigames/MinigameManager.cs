using UnityEngine;

public class MinigameManager : MonoBehaviour


{
    public GameObject[] singlePlayerMinigames;
    public GameObject[] multiplayerMinigames;
    public bool isMultiplayer;

    private GameObject currentMinigame;
    public void LoadRandomMinigame()
    {
        if (currentMinigame != null)
        {
            Destroy(currentMinigame);
        }

        int randomIndex;
        GameObject selectedMinigame;

        if (isMultiplayer)
        {
            randomIndex = Random.Range(0, multiplayerMinigames.Length);
            selectedMinigame = Instantiate(multiplayerMinigames[randomIndex]);
        }
        else
        {
            randomIndex = Random.Range(0, singlePlayerMinigames.Length);
            selectedMinigame = Instantiate(singlePlayerMinigames[randomIndex]);
        }

        currentMinigame = selectedMinigame;
        selectedMinigame.GetComponent<Minigame>().StartGame();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
