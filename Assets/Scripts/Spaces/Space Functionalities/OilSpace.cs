using System.Collections;
using UnityEngine;

public class OilSpace : SpaceBehavior
{
	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} landed on an oil space.");

		yield return DisasterManager.Instance.IncrementEarthquake();
	}
}