using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Space))]
public class BonusSpace : SpaceBehavior
{
	private Space _space;

	public void Awake()
	{
		_space = GetComponent<Space>();
	}
	public override IEnumerator RespondToPlayer(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name} landed on a {biome} bonus space.");

		DisasterManager.Instance.IncrementBiomeDisaster(biome, player);

		yield break;
	}
}
