using System.Collections;
using UnityEngine;

public class OilSpace : SpaceBehavior
{
	[SerializeField] private DisasterManager _disasterManager;

	public override bool EndsTurn { get; } = true;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player} landed on an oil space.");

		_disasterManager.IncrementDisaster(DisasterManager.DisasterType.Earthquake, player);

		yield break;
	}
}