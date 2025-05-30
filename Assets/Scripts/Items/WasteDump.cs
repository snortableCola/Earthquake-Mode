using System.Collections;
using UnityEngine;

public class WasteDump : Item
{
	private void Awake()
	{
		cost = 10;
		itemName = "Waste Dump";
	}
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;

		Biome biome = (Biome) Random.Range(0, 3); // Biome/disaster currently chosen at random
		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
		DisasterManager.Instance.UpdateDisasterInfo();
	}
}
