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
        if (player == null)
        {
            Debug.LogError("Player is null in RespondToPlayer!");
            yield break;
        }

        Debug.Log($"{player.name} landed on a resource space.");

        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigameForPlayer(player);
        }
        else
        {
            Debug.LogError("MinigameManager instance not found.");
        }

        yield break;
    }
}
