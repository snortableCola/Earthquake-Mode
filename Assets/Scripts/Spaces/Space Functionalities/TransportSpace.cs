using System.Collections;
using UnityEngine;

public class TransportSpace : SpaceBehavior
{
	[SerializeField] private Space _destination;
	public Space Destination => _destination;

	public override bool HasPassingBehavior { get; } = true;

	public override IEnumerator RespondToPlayerPassing(Player player)
	{
		yield return player.Movement.MoveToSpaceCoroutine(_destination);
		player.Movement.ResetMovementPath();
		DisasterManager.Instance.UpdateDisasterInfo();
	}

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		yield return player.Movement.MoveToSpaceCoroutine(_destination);
		Debug.Log("Player landed on transport space, will act as resource space for destination.");
		yield return GeneralResourceSpace.Instance.RespondToPlayerEnd(player);
	}

	public override IEnumerator WaitForHudCompletion() => GeneralResourceSpace.Instance.WaitForHudCompletion();
}
