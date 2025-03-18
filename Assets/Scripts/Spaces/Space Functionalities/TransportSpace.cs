using System.Collections;
using UnityEngine;

public class TransportSpace : SpaceBehavior
{
	[SerializeField] private Space _destination;

	public override bool EndsTurn { get; } = false;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} used a transport space.");

		PlayerMovement movement = player.Movement;
		yield return movement.JumpToSpaceCoroutine(_destination, false);
		movement.ResetMovementPath(_destination);
	}
}
