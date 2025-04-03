using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
	private Space _space;
    private void Awake()
    {
        _space = GetComponent<Space>();
    }
  

	public override IEnumerator RespondToPlayer(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name} landed on a {biome} resource space.");
		player.totalPoints += 3;

		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
    }
}
