using UnityEngine;

public class TransportSpace : SpacePassedBehavior
{
	[SerializeField] private TransportSpace _destination;

	public override void ReactToPlayerPassing(Player player)
	{
		Debug.Log($"{player} passed a transport space, and could choose to go to {_destination}");
	}
}
