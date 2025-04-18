using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Space))]
public class BonusSpace : SpaceBehavior
{
	private Space _space;

	private void Awake()
	{
		_space = GetComponent<Space>();
	}

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		Biome biome = _space.Biome;
		Debug.Log($"{player.name}, with {player.Points}, landed on a {biome} bonus space. Sending to minigame manager.");

		MinigameManager.Instance.StartRandomSingleplayerMinigame();
		yield return new WaitUntil(() => !MinigameManager.Instance.IsMinigameSequenceOngoing);

		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
	}
}
