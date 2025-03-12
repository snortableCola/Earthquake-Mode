using UnityEngine;

[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceLandedBehavior
{
	private Space _space;
	[SerializeField] private DisasterManager _disasterManager;

	public void Awake()
	{
		_space = GetComponent<Space>();
	}
	public override void ReactToPlayerLanding(Player player)
	{
		Space.BoardBiome biome = _space.Biome;
		Debug.Log($"{player} landed on a {biome} resource space.");

		_disasterManager.IncrementBiomeDisaster(biome, player);
	}
}
