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

    private IEnumerator FlashHudMessage()
    {
        if (redHUD != null)
        {
            redHUD.gameObject.SetActive(true);
            yield return new WaitForSeconds(hudMessageDuration);
            redHUD.gameObject.SetActive(false);
        }

    }
    public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} landed on a negative space.");
        StartCoroutine(FlashHudMessage());

		yield break;
	}
}
