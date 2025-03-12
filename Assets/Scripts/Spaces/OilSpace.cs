using UnityEngine;

public class OilSpace : SpaceLandedBehavior
{
	[SerializeField] private DisasterManager _disasterManager;
	public override void ReactToPlayerLanding(Player player)
	{
		Debug.Log($"{player} landed on an oil space.");

		_disasterManager.IncrementDisaster(DisasterManager.DisasterType.Earthquake, player);
	}
}