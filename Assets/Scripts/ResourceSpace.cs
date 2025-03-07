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
		Debug.Log($"Will make {player} play minigame for 1-2 {biome} resources.");

		_disasterManager.IncrementBiomeDisaster(biome);
	}
}
