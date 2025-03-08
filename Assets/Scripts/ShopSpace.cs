using UnityEngine;

public class ShopSpace : SpacePassedBehavior
{
	public override void ReactToPlayerPassing(Player player)
	{
		Debug.Log($"{player} passed a shop space!");
	}
}
