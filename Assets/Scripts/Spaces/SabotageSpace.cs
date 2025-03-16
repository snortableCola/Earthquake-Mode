using UnityEngine;

public class SabotageSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = true;

	public override void RespondToPlayer(Player player)
	{
		Debug.Log($"{player} landed on a sabotage space.");
	}
}
