using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class ShopSpace : SpaceBehavior
{
	public override bool HasPassingBehavior { get; } = true;
	private PanelManager panelManager;
    [SerializeField] private GameObject ViewShopPanel; // Reference to the "View Shop?" UI panel
    [SerializeField] private Button yesButton;        // Reference to the Yes button
    [SerializeField] private Button noButton;
    private Player currentPlayer;

    private void Awake()
    {
        // Dynamically find the "View Shop?" panel in the scene

        ViewShopPanel = System.Array.Find(Resources.FindObjectsOfTypeAll<GameObject>(),
        obj => obj.CompareTag("Shop Panel"));

        if (ViewShopPanel == null)
        {
            Debug.LogError("ViewShopPanel could not be found in the scene!");
            return;
        }

        // Ensure the panel is initially disabled
        ViewShopPanel.SetActive(false);

        // Find the Yes and No buttons as children of the ViewShopPanel
        Transform panelTransform = ViewShopPanel.transform;
        yesButton = panelTransform.Find("YesButton")?.GetComponent<Button>();
        noButton = panelTransform.Find("NoButton")?.GetComponent<Button>();

        if (yesButton == null || noButton == null)
        {
            Debug.LogError("YesButton or NoButton could not be found as children of ViewShopPanel!");
        }

        // Cache the reference to the PanelManager
        panelManager = FindFirstObjectByType<PanelManager>();
        if (panelManager == null)
        {
            Debug.LogError("PanelManager could not be found in the scene!");
        }
    }

    public override IEnumerator RespondToPlayerPassing(Player player)
	{
		Debug.Log($"{player.name} passed a shop space.");

        ShowShopPrompt(player);

        // Wait for the player's decision (Yes or No)
        while (ViewShopPanel.activeSelf)
        {
            yield return null; // Wait until the player makes a choice
        }

        Debug.Log($"{player.name} has finished interacting with the shop space.");
        yield break;

    }

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		
		Debug.Log($"{player.name} landed on a shop space.");
        
		panelManager.ShowShop(player);
		

		yield break;
	}
    private void ShowShopPrompt(Player player)
    {
        currentPlayer = player; // Track the player interacting with the shop

        // Activate the "View Shop?" panel
        if (ViewShopPanel != null)
        {
            ViewShopPanel.SetActive(true);

            // Assign button click listeners
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(OnYesButtonClicked);

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(OnNoButtonClicked);
        }
    }

    private void OnYesButtonClicked()
    {
        // Open the shop using PanelManager and pass the current player
        if (panelManager != null && currentPlayer != null)
        {
            panelManager.ShowShop(currentPlayer);
        }

        // Hide the "View Shop?" panel
        HideShopPrompt();
    }

    private void OnNoButtonClicked()
    {
        // Simply hide the "View Shop?" panel and let the player continue
        HideShopPrompt();
    }

    private void HideShopPrompt()
    {
        // Deactivate the "View Shop?" panel
        if (ViewShopPanel != null)
        {
            ViewShopPanel.SetActive(false);
        }

        // Clear the current player reference
        currentPlayer = null;
    }
}
