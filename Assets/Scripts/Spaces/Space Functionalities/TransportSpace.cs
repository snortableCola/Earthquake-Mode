using System.Collections;
using UnityEngine;

public class TransportSpace : SpaceBehavior
{
	[SerializeField] private Space _destination;
	public Space Destination => _destination;

	public override bool EndsTurn { get; } = false;

	public override IEnumerator RespondToPlayer(Player player)
	{
		Debug.Log($"{player.name} used a transport space.");

		PlayerMovement movement = player.Movement;
		yield return movement.MoveToSpaceCoroutine(_destination);
		movement.ResetMovementPath();
	}
}
