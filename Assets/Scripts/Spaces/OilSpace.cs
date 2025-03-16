using UnityEngine;

public class OilSpace : SpaceBehavior
{
	[SerializeField] private DisasterManager _disasterManager;

	public override bool EndsTurn { get; } = true;

	public override void RespondToPlayer(Player player)
	{
		Debug.Log($"{player} landed on an oil space.");

		_disasterManager.IncrementDisaster(DisasterManager.DisasterType.Earthquake, player);
	}
}