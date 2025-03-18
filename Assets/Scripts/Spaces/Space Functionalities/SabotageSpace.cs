using System.Collections;
using UnityEngine;

public class SabotageSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = true;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} landed on a sabotage space.");

		yield break;
	}
}
