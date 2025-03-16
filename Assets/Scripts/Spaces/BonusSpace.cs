using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Space))]
public class BonusSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = true;

	private Space _space;
	[SerializeField] private DisasterManager _disasterManager;

	public void Awake()
	{
		_space = GetComponent<Space>();
	}
	public override IEnumerator RespondToPlayer(Player player)
	{
		Space.BoardBiome biome = _space.Biome;
		Debug.Log($"{player} landed on a {biome} bonus space.");

		_disasterManager.IncrementBiomeDisaster(biome, player);

		yield break;
	}
}
