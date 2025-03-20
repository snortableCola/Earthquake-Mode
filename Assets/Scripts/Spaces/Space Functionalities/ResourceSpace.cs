using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
	private Space _space;

	public void Awake()
	{
		_space = GetComponent<Space>();
	}

	public override IEnumerator RespondToPlayer(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name} landed on a {biome} resource space.");

        // This is when a random singleplayer minigame should happen.
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigameForPlayer(player);
        }
        else
        {
            Debug.LogError("MinigameManager instance not found in the scene.");
        }
        DisasterManager.Instance.IncrementBiomeDisaster(biome, player);

		yield break;
	}
}
