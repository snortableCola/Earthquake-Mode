using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; 

public class NegativeSpace : SpaceBehavior
{
	public Image redHUD; 
	public float hudMessageDuration = 2f;
    private void Awake()
    {
        // Dynamically find the image by name or tag
        redHUD = Resources.FindObjectsOfTypeAll<Image>()
                       .FirstOrDefault(img => img.gameObject.CompareTag("REDHUD"));

        if (redHUD == null)
        {
            Debug.LogError("HUD Image not found! Make sure the tag is correctly set and object exists.");
        }


    }

    public override IEnumerator WaitForHudCompletion()
    {
        while (redHUD != null && redHUD.gameObject.activeSelf)
        {
            yield return null; // Wait until the red HUD is no longer active
        }
    }
    public override IEnumerator RespondToPlayerEnd(Player player)
	{
		Debug.Log($"{player.name} landed on a negative space.");
        // Show the red HUD
        redHUD.gameObject.SetActive(true);
        yield return new WaitForSeconds(hudMessageDuration);

        // Hide the red HUD
        redHUD.gameObject.SetActive(false);
        player.Coins -= 3;

        yield break;
	}
}
