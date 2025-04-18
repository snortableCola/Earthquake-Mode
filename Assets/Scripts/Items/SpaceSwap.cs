using System.Collections;
using UnityEngine;

public class SpaceSwap : Item
{
	public override IEnumerator BeUsedBy(Player player)
	{
		player.UsedItem = this;

		Player[] players = GameManager.Instance.Players;

		// Randomly selected player, can be given UI for player choice
		int targetIndex = Random.Range(0, players.Length - 1);
		Player target = players[targetIndex];
		if (target == player) target = players[^1];

		Space start = player.GetComponentInParent<Space>();
		Space end = target.GetComponentInParent<Space>();

		yield return player.Movement.MoveToSpaceCoroutine(end);
		yield return target.Movement.MoveToSpaceCoroutine(start);

		DisasterManager.Instance.UpdateDisasterInfo();
	}
}
