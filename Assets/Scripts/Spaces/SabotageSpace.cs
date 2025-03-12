using UnityEngine;

public class SabotageSpace : SpaceLandedBehavior
{
	public override void ReactToPlayerLanding(Player player)
	{
		Debug.Log($"{player} landed on a sabotage space.");
	}
}
