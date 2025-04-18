using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class ShopSpace : SpaceBehavior
{
	public override bool HasPassingBehavior { get; } = true;
	private PanelManager panelManager;

	public override IEnumerator RespondToPlayerPassing(Player player)
	{
		Debug.Log($"{player.name} passed a shop space.");

		yield break;
		
	}

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		
		Debug.Log($"{player.name} landed on a shop space.");
		panelManager.ShowShop();
		

		yield break;
	}
}
