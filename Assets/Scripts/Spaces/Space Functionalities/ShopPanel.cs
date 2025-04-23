using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ShopPanel: MonoBehaviour
{
    [SerializeField] private Button[] itemButtons; // Buttons for the shop UI
    [SerializeField] private List<Item> allItems = new List<Item>(); // List of all items
    private List<Item> displayedItems = new List<Item>(); // Currently displayed items

    [SerializeField] private HeliEvac heliEvac;
    [SerializeField] private WasteDump wasteDump;
    [SerializeField] private TravelPlan travelPlan;
    [SerializeField] private PrivateJet privateJet;
    [SerializeField] private SpaceSwap spaceSwap;

    private Player currentPlayer;

    [SerializeField] private TMP_Text shoptext; 

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
        currentPlayer = player;
        // Randomly select 3 unique items
        displayedItems = GenerateRandomItems();

        // Assign the selected items to the buttons
        AssignItemsToButtons();
        shoptext.text = "Welcome to my shop!";
        
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
            Item item = displayedItems[i];

            // Safely get child components
            Transform buttonTransform = itemButtons[i].transform;
            Image itemImage = buttonTransform.Find("ItemImage")?.GetComponent<Image>();
         
            if (itemImage == null)
            {
                Debug.LogError($"Button {i} is missing required UI components.");
                continue;
            }

            // Set item details
            itemImage.sprite = item.itemSprite; // Set the item's sprite
          

            // Configure button click
            itemButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
            itemButtons[i].onClick.AddListener(() => AddItemToPlayer(item)); // Add button click listener
        }
    }

    private void AddItemToPlayer(Item item)
    {
       
        if (currentPlayer == null)
        {
            Debug.LogError("Player reference is missing.");
            return;
        }

        // Check if the player can afford the item
        if (currentPlayer.Coins >= item.cost)
        {
            currentPlayer.Coins -= item.cost; // Deduct the cost
            Debug.Log($"Bought: {item.itemName} for {item.cost} coins.");
            currentPlayer.HeldItem = item; // Add the item to the player's inventory
            shoptext.text = "Thank you for shopping, come back soon!";
        }
        else
        {
            Debug.Log($"Not enough coins to buy {item.itemName}.");
            shoptext.text = "You do not have enough coins to buy that";


        }
    }
}
