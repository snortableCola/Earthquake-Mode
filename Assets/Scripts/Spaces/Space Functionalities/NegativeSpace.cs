using System.Collections;
using UnityEngine;

public class NegativeSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = true;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player} landed on a negative space.");

		yield break;
	}
}
