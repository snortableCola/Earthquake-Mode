using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
	public override bool EndsTurn { get; } = true;

	private Space _space;

	public void Awake()
	{
		_space = GetComponent<Space>();
	}

	public override IEnumerator RespondToPlayer(Player player)
	{
		Space.BoardBiome biome = _space.Biome;
		Debug.Log($"{player} landed on a {biome} resource space.");

		DisasterManager.Instance.IncrementBiomeDisaster(biome, player);

		yield break;
	}
}
