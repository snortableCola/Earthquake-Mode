using System.Collections;
using UnityEngine;

public class NegativeSpace : SpaceBehavior
{
	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} landed on a negative space.");

		yield break;
	}
}
