using System.Collections;
using UnityEngine;

public class ShopSpace : SpaceBehavior
{
	public override bool HasPassingBehavior { get; } = true;

	public override IEnumerator RespondToPlayerPassing(Player player)
	{
		Debug.Log($"{player.name} passed a shop space.");

		yield break;
	}

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		Debug.Log($"{player.name} landed on a shop space.");

		yield break;
	}
}
