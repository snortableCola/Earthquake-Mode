using UnityEngine;

[RequireComponent(typeof(Space))]
public class BonusSpace : SpaceLandedBehavior
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
		Debug.Log($"Will give {player} 3 free {biome} resources.");

		_disasterManager.IncrementBiomeDisaster(biome);
	}
}
