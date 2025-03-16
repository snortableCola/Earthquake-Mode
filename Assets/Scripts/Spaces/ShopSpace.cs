using System.Collections;
using UnityEngine;

public class ShopSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = false;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player} passed a shop space.");

		yield break;
	}
}
