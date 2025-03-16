using UnityEngine;

public class TransportSpace : SpaceBehavior
{
	[SerializeField] private TransportSpace _destination;

	public override bool EndsTurn { get; } = false;

	public override void RespondToPlayer(Player player)
	{
		Debug.Log($"{player} passed a transport space.");
	}
}
