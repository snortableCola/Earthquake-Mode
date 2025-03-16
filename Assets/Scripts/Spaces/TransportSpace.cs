using System.Collections;
using UnityEngine;

public class TransportSpace : SpaceBehavior
{
	[SerializeField] private Space _destination;

	public override bool EndsTurn { get; } = false;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player} used a transport space.");

		yield return player.JumpToSpaceCoroutine(_destination, false);

		player.Movement.Space = _destination;
		player.Movement.Path = new() { _destination };
	}
}
