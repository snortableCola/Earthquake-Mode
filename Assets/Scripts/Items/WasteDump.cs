using System.Collections;
using UnityEngine;

public class WasteDump : MonoBehaviour, IItem
{
	public IEnumerator GetUsedBy(Player player)
	{
		Biome biome = (Biome) Random.Range(0, 3); // Biome/disaster currently chosen at random
		return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
	}
}
