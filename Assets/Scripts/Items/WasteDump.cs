using System.Collections;
using UnityEngine;

public class WasteDump : MonoBehaviour, IItem
{
	public IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;

		Biome biome = (Biome) Random.Range(0, 3); // Biome/disaster currently chosen at random
		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
	}
}
