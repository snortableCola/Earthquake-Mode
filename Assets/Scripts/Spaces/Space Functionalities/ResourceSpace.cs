using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
  
    private Space _space;
    private Image _blueHUD;
    public float hudMessageDuration = 2f;

    private void Awake()
    {
        _space = GetComponent<Space>();
        // Dynamically find the image by name or tag
        _blueHUD = Resources.FindObjectsOfTypeAll<Image>()
                       .FirstOrDefault(img => img.gameObject.CompareTag("BLUEHUD"));

        if (_blueHUD == null)
        {
            Debug.LogError("HUD Image not found! Make sure the tag is correctly set and object exists.");
        }

    }

   
    public override IEnumerator RespondToPlayer(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name} landed on a {biome} resource space.");
       _blueHUD.gameObject.SetActive(true);
        yield return new WaitForSeconds(hudMessageDuration);
        _blueHUD.gameObject.SetActive(false); 
        player.Points += 3;

		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
    }
    public override IEnumerator WaitForHudCompletion()
    {
        while (_blueHUD != null && _blueHUD.gameObject.activeSelf)
        {
            yield return null; // Wait until the red HUD is no longer active
        }
    }
}
