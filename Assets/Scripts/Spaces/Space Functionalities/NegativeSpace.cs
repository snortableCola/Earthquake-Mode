using System.Collections;
using UnityEngine;

public class NegativeSpace : SpaceBehavior
{
	public GameObject redHUD; 
	public float hudMessageDuration = 2f;
    private IEnumerator FlashHudMessage()
    {
        redHUD.gameObject.SetActive(true);
        yield return new WaitForSeconds(hudMessageDuration);
        redHUD.gameObject.SetActive(false);
    }
    public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} landed on a negative space.");
        StartCoroutine(FlashHudMessage());

		yield break;
	}
}
