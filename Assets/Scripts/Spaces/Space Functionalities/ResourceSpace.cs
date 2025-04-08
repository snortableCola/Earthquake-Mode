using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; 


[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
	private Space _space;
    public Image BLUEHud;
    public float hudMessageDuration = 2f;

    private void Awake()
    {
        _space = GetComponent<Space>();
        // Dynamically find the image by name or tag
        BLUEHud = Resources.FindObjectsOfTypeAll<Image>()
                       .FirstOrDefault(img => img.gameObject.CompareTag("BLUEHUD"));

        if (BLUEHud == null)
        {
            Debug.LogError("HUD Image not found! Make sure the tag is correctly set and object exists.");
        }

    }

    private IEnumerator FlashHudMessage()
    {
        if (BLUEHud != null)
        {
            BLUEHud.gameObject.SetActive(true);
            yield return new WaitForSeconds(hudMessageDuration);
            BLUEHud.gameObject.SetActive(false);
        }

    }
    public override IEnumerator RespondToPlayer(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name} landed on a {biome} resource space.");
        StartCoroutine(FlashHudMessage());
        player.Points += 3;

		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
    }
}
