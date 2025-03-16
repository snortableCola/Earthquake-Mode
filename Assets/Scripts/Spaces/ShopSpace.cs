using UnityEngine;

public class ShopSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = false;

	public override void RespondToPlayer(Player player)
	{
		Debug.Log($"{player} passed a shop space.");
	}
}
