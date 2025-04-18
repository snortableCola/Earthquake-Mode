using System.Collections;
using UnityEngine;

public class SabotageSpace : SpaceBehavior
{
	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		Debug.Log($"{player.name} landed on a sabotage space.");

		yield break;
	}
}
