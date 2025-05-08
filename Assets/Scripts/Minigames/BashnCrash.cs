using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class BashnCrash : Minigame
{
    [Header("Player Settings")]
    public Slider[] playerHealthbars;
    public Button[] playerButtons;
    public Button exitButton;

    [Header("Game Settings")]
    public float maxHealth = 1;
    public float damagePerMash = 0.05f;
    public float mashCooldown = 0.01f; //prevents spamming
    [SerializeField] public TMP_Text winnerText;
    [SerializeField] public TMP_Text BashText;

    [Header("Building Settings")]
    public Image[] buildingImages; // Images representing each player's building
    public Sprite[] player1Sprites; // Sprites for Player 1's building states [0: Full Health, 1: Damaged]
    public Sprite[] player2Sprites; // Sprites for Player 2's building states [0: Full Health, 1: Damaged]
    public Sprite[] player3Sprites; // Sprites for Player 3's building states [0: Full Health, 1: Damaged]
    public Sprite[] player4Sprites; // Sprites for Player 4's building states [0: Full Health, 1: Da


    private bool[] hasFinished; // Tracks if a player has finished (health bar reached 0)
    private float[] lastMashTime; // Tracks the last mash time for each player
    private bool _gameFinished = true;
    public override string Instructions { get; } = "Smash your competition's HQ to the ground!! Mash your X/A button on your controller like your career depends on it. Each mash deals damage to the HQ. The first to destroy the enemy HQ wins 5 corruption coins!";
    private PlayerMovement _playerMovement;
	private void Start()
	{
		int playerCount = playerHealthbars.Length;
		for (int i = 0; i < playerCount; i++)
		{
			int playerIndex = i; // Capture index for lambda
			playerButtons[i].onClick.AddListener(() => HandleMash(playerIndex));
            exitButton.onClick.AddListener(ExitGame);
		}
	}

	public override void StartGame()
    {
		_gameFinished = false;
        exitButton.gameObject.SetActive(false);
		int playerCount = playerHealthbars.Length;
        hasFinished = new bool[playerCount];
        lastMashTime = new float[playerCount];

        // Initialize health bars and buttons
        for (int i = 0; i < playerCount; i++)
        {
            playerHealthbars[i].maxValue = maxHealth;
            playerHealthbars[i].value = maxHealth;
        }

        winnerText.gameObject.SetActive(false); // Hide winner text at the start
        UpdateAllBuildingImages(); // Initialize building images
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameFinished) return;

        // Check for button mashing input from gamepads
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (i >= playerHealthbars.Length) break; // Limit to the number of players

            var gamepad = Gamepad.all[i];
            if (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame) // Assuming "buttonSouth" is the mash button
            {
                HandleMash(i);
            }
        }

        // Keyboard support for testing (optional)
        if (Keyboard.current.spaceKey.wasPressedThisFrame) // Simulate Player 0 mash
        {
            HandleMash(0);
        }
    }
    private void UpdateBuildingImage(int playerIndex)
    {
        float currentHealth = playerHealthbars[playerIndex].value;

        // Determine the appropriate sprite array for the player
        Sprite[] playerSprites = GetPlayerSprites(playerIndex);

        if (currentHealth > maxHealth * 0.5f) // Full health state
        {
            buildingImages[playerIndex].sprite = playerSprites[0]; // Full health sprite
        }
        else if (currentHealth > 0) // Damaged state
        {
            buildingImages[playerIndex].sprite = playerSprites[1]; // Damaged sprite
        }
        else // Destroyed state
        {
            buildingImages[playerIndex].gameObject.SetActive(false); // Hide the building
        }
    }

    private Sprite[] GetPlayerSprites(int playerIndex)
    {
        // Return the correct sprite set based on the player index
        switch (playerIndex)
        {
            case 0: return player1Sprites;
            case 1: return player2Sprites;
            case 2: return player3Sprites;
            case 3: return player4Sprites;
            default: return null;
        }
    }

    private void UpdateAllBuildingImages()
    {
        for (int i = 0; i < buildingImages.Length; i++)
        {
            UpdateBuildingImage(i);
        }
    }

    private void HandleMash(int playerIndex)
    {
        if (_gameFinished || hasFinished[playerIndex] || Time.time - lastMashTime[playerIndex] < mashCooldown) return;

        // Reduce health
        playerHealthbars[playerIndex].value -= damagePerMash;

        // Update the building image for this player
        UpdateBuildingImage(playerIndex);

        // Check for winner
        if (playerHealthbars[playerIndex].value <= 0)
        {
            hasFinished[playerIndex] = true;
            
            EndGame(playerIndex);
        }

        lastMashTime[playerIndex] = Time.time;
    }
   

    public void EndGame(int winningPlayer)
    {
        _gameFinished = true;
        BashText.gameObject.SetActive(false);
        winnerText.text = $"Player {winningPlayer + 1} Wins! +3 corruption coins";
       

        winnerText.gameObject.SetActive(true);

       
        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(true);
           

        }
        // Disable buttons
        foreach (var button in playerButtons)
        {
            button.interactable = false;
		}
		
	}
    private void ExitGame()
    {
        Debug.Log("Exit button clicked. Exiting the minigame.");
        MinigameManager.Instance.EndMinigame();
        Cleanup();
    }
	public void Cleanup()
    {
        // Reset health bars and re-enable buttons
        for (int i = 0; i < playerHealthbars.Length; i++)
        {
            playerHealthbars[i].value = maxHealth;
            playerButtons[i].interactable = true;
            hasFinished[i] = false;

            // Reset building images
            buildingImages[i].gameObject.SetActive(true);
            buildingImages[i].sprite = GetPlayerSprites(i)[0]; // Reset to full health sprite
        }

        winnerText.gameObject.SetActive(false); // Hide winner text
        BashText.gameObject.SetActive(true);
    }
}
