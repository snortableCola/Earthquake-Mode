using UnityEngine;

public class NegativeSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = true;

	public override void RespondToPlayer(Player player)
	{
		Debug.Log($"{player} landed on a negative space.");
	}
}
