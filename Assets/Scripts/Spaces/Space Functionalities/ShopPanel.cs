using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class ShopPanel: MonoBehaviour
{
  
    [SerializeField] private Button[] itemButtons; // Buttons for the shop UI
    [SerializeField] private List<Item> allItems = new List<Item>(); // List of all items
    private List<Item> displayedItems = new List<Item>(); // Currently displayed items
    [SerializeField] private Button LeaveButton; 

    [SerializeField] private HeliEvac heliEvac;
    [SerializeField] private WasteDump wasteDump;
    [SerializeField] private TravelPlan travelPlan;
    [SerializeField] private PrivateJet privateJet;
    [SerializeField] private SpaceSwap spaceSwap;

    private Player currentPlayer;
    private PanelManager panelManager;

    [SerializeField] private TMP_Text shoptext;
    public bool isShopOpen { get; private set; } = false;
    public static ShopPanel instance { get; private set; } // Singleton instance

    private void Awake()
    {
        instance = this;
        panelManager = FindFirstObjectByType<PanelManager>();
        if (panelManager == null)
        {
            Debug.LogError("PanelManager could not be found in the scene!");
        }
    }
    private void Start()
    {
        // Populate the list of all items
        allItems.Add(heliEvac);
        allItems.Add(wasteDump);
        allItems.Add(travelPlan);
        allItems.Add(privateJet);
        allItems.Add(spaceSwap);

      
    }

    public void OpenShop(Player player)
    {
        isShopOpen = true;
        currentPlayer = player;
        // Randomly select 3 unique items
        displayedItems = GenerateRandomItems();

        // Assign the selected items to the buttons
        AssignItemsToButtons();
        shoptext.text = "Welcome to my shop!";
        LeaveButton.onClick.AddListener(OnLeaveButtonClicked);
    }

    private List<Item> GenerateRandomItems()
    {
        List<Item> tempItems = new List<Item>(allItems);
        List<Item> selectedItems = new List<Item>();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            int randomIndex = Random.Range(0, tempItems.Count);
            selectedItems.Add(tempItems[randomIndex]);
            tempItems.RemoveAt(randomIndex); // Remove the selected item to avoid duplication
        }

        return selectedItems;
    }

    private void AssignItemsToButtons()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            // Ensure the button exists in the array
            if (itemButtons[i] == null)
            {
                Debug.LogError($"Button {i} in the itemButtons array is null.");
                continue;
            }

            // Get the corresponding item
            Item item = displayedItems[i];

            // Get the Image component directly from the button
            Image buttonImage = itemButtons[i].GetComponent<Image>();

            if (buttonImage == null)
            {
                Debug.LogError($"Button {i} is missing an Image component.");
                continue;
            }

            // Assign the item's sprite to the button's Image component
            buttonImage.sprite = item.itemSprite;

            // Configure button click listener
            itemButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
            itemButtons[i].onClick.AddListener(() => AddItemToPlayer(item)); // Add button click listener
        }
        //for (int i = 0; i < itemButtons.Length; i++)
        //{
        //    Item item = displayedItems[i];

        //    // Safely get child components
        //    Transform buttonTransform = itemButtons[i].transform;
        //    Image itemImage = buttonTransform.Find("ItemImage")?.GetComponent<Image>();

        //    if (itemImage == null)
        //    {
        //        Debug.LogError($"Button {i} is missing required UI components.");
        //        continue;
        //    }

        //    // Set item details
        //    itemImage.sprite = item.itemSprite; // Set the item's sprite


        //    // Configure button click
        //    itemButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
        //    itemButtons[i].onClick.AddListener(() => AddItemToPlayer(item)); // Add button click listener
        //}
    }
    private void OnLeaveButtonClicked()
    {
        StartCoroutine(LeaveShop());
       
    }
    private IEnumerator LeaveShop()
    {
        // Update the shop text
        shoptext.text = "Thank you for shopping!";

        // Wait for 3 seconds (or any desired duration)
        yield return new WaitForSeconds(3f);

        // Execute the remaining lines
        panelManager.CloseShop();
        isShopOpen = false;
        ResetShopUI();
        LeaveButton.onClick.RemoveAllListeners();
    }

    private void ResetShopUI()
    {
        // Clear item buttons
        foreach (Button itemButton in itemButtons)
        {
            Image itemImage = itemButton.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = null; // Remove the sprite
            }

            itemButton.onClick.RemoveAllListeners(); // Clear previous listeners
        }

      

        Debug.Log("Shop UI has been reset.");
    }

    private void AddItemToPlayer(Item item)
    {
       
        if (currentPlayer == null)
        {
            Debug.LogError("Player reference is missing.");
            return;
        }
        // TODO make a close shop method to end the shop interaction for players, refactor this 

        // Check if the player can afford the item
        if (currentPlayer.Coins >= item.cost)
        {
            for (int i = 0; i < currentPlayer.HeldItems.Length; i++)
            {
                if (currentPlayer.HeldItems[i] == null) // Find empty slot
                {
                    currentPlayer.Coins -= item.cost; // Deduct the cost
                    currentPlayer.HeldItems[i] = item; // Add item to the slot
                    Debug.Log($"Added {item.itemName} to slot {i}.");
                    shoptext.text = $"nice you bought a {item.itemName}!";
                    return;
                }
                else
                { 
                //add code that opens up the use item panel and prompts the player to discard an item 

                }
            }
           
           
            
        }
        else
        {
            Debug.Log($"Not enough coins to buy {item.itemName}.");
            shoptext.text = "You do not have enough coins to buy that";


        }
    }
   
}
