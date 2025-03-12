using UnityEngine;

public class NegativeSpace : SpaceLandedBehavior
{
	public override void ReactToPlayerLanding(Player player)
	{
		Debug.Log($"{player} landed on a negative space.");
	}
}
