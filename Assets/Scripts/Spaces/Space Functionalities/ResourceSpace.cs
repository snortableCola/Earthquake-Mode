using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
	private Space _space;
    //public Image BlueHUD; //HUD that tells the player they gained 3 coins 
    //public float hudMessageDuration = 2f;

    private void Awake()
    {
        _space = GetComponent<Space>();
        // Dynamically find the image by name or tag
        //BlueHUD = Resources.FindObjectsOfTypeAll<Image>()
        //               .FirstOrDefault(img => img.gameObject.CompareTag("BLUEHUD"));

        //if (BlueHUD == null)
        //{
        //    Debug.LogError("HUD Image not found! Make sure the tag is correctly set and object exists.");
        //}


    }

    //private IEnumerator FlashHudMessage()
    //{
    //    if (BlueHUD != null)
    //    {
    //        BlueHUD.gameObject.SetActive(true);
    //        yield return new WaitForSeconds(hudMessageDuration);
    //        BlueHUD.gameObject.SetActive(false);
    //    }

    //}
  

	public override IEnumerator RespondToPlayer(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name} landed on a {biome} resource space.");
        Debug.Log($"Player Name: {player.name}");
        Debug.Log($"Player Points: {player.totalPoints}");
        //flashes message when player lands on the space 
        //StartCoroutine(FlashHudMessage());


        // This is when a random singleplayer minigame should happen.
        if (player == null)
        {
            Debug.LogError("Player is null in RespondToPlayer!");
            yield break;
        }

        Debug.Log($"{player.name} landed on a resource space.");

        if (MinigameManager.Instance != null)
        {
            Debug.Log($"Sending {player.name} to MinigameManager.");

            MinigameManager.Instance.StartMinigameForPlayer(player);
        }
        else
        {
            Debug.LogError("MinigameManager instance not found.");
        }

        yield break;
    }
}
